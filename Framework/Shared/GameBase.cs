using Framework.Slides;
using Microsoft.AspNetCore.Components;
using Framework.Game.Parameters;
using Framework.State;
using Framework.Sound;
using Framework.Toast;
using Blazored.Toast.Services;
using Blazored.Toast;
using Framework.Video;



namespace Framework.Game;

public partial class GameBase : ComponentBase
{
	[Inject]
	protected SlideService SlideService { get; set; } = null!;

	[Inject]
	protected GameState GameState { get; set; } = null!;

	[Inject]
	protected SoundService SoundService { get; set; } = null!;

	[Inject]
	protected IToastService ToastService { get; set; } = null!;

	[Inject]
	protected VideoService VideoService { get; set; } = null!;

	// // private readonly TaskCompletionSource<bool> _tcs = new();
	// // protected Task InitTask => _tcs.Task;

	protected string SlideId => Parameters.SlideId;

	protected SlideComponentParameters Parameters { get; set; } = null!;
	// protected Dictionary<string, object?> ParametersDictionary { get; set; } = null!;


	protected override void OnInitialized()
	{
		// string slideId = SlideService.GetStartSlideId();
		// string slideId = GameState.CurrentSlide;
		Parameters = new SlideComponentParameters()
		{
			SlideId = GameState.CurrentSlide,
			OnButtonClick = EventCallback.Factory.Create<List<List<string>>>(this, EvaluateActions)
		};
		// // _tcs.SetResult(true);
	}

	protected static void HandlePolygonClick(string thing)
	{
		Console.WriteLine(thing);
	}

	protected void ChangeSlide(string slideId)
	{
		/* 
		* This is extremely hacky, but it is very important
		* For some godforsaken reason, you cannot route to a minigame if you finish in one
		* Because a more normal "re-routing" approach like
		* (`EvaluateActions([["Route", "__relay__"], ["Route", slideId]])`) 
		* does not seem to work, you set the OnEnter of the __relay__ slide to route to the new slide,
		* then you route there.
		* __relay__ doesn't exist in the json, it gets created by the SlideService
		* Doing it like this makes it however dangerous to route to __relay__ outside this special case
		* so don't do it.
		*/
		if (
			SlideService.GetSlide(slideId).Type == "Minigame" &&
			SlideService.GetSlide(Parameters.SlideId).Type == "Minigame"
		)
		{
			SlideService.Slides["__relay__"].OnEnter = [["Route", slideId]];
			slideId = "__relay__";
		}

		// for debug, throw an exception if the slide does not exist
		if (Debug)
		{
			SlideService.GetSlide(slideId);
		}


		Parameters.SlideId = slideId;
		// still a bit hacky, but I guess
		GameState.CurrentSlide = slideId;
		// Console.WriteLine(SlideId);
		StateHasChanged();
	}

	protected async Task FinishMinigame(List<List<string>> actions)
	{
		await EvaluateActions(actions);
	}

	protected struct Block
	{
		public List<string> Stack { get; set; }
		public bool Skipping { get; set; }
		public string SkippingTo { get; set; }
	}

	protected async Task EvaluateActions(List<List<string>> actions)
	{
		Block block = new()
		{
			Stack = []
		};

		// no foreach to be able to look back/ahead
		for (int i = 0; i < actions.Count; i++)
		{
			List<string> action = actions[i];

			if (block.Skipping)
			{
				if (block.SkippingTo == action[1] && action[0] == "EndBlock")
				{
					block.Skipping = false;
					// block.Stack.RemoveAt(block.Stack.Count - 1);
					// removed this cause it gets removed in the switch
					// block.Stack.Remove(block.Stack.Last());
					block.SkippingTo = "";
				}
				else
				{
					continue;
				}
			}

			switch (action[0])
			{
				case "Route":
					ChangeSlide(action[1]);
					break;
				case "AddItem":
					GameState.AddItem(action[1]);
					break;
				case "RemoveItem":
					GameState.RemoveItem(action[1]);
					break;
				case "SetGameState":
					switch (action[2])
					{
						case "true":
							GameState.SetState(action[1], true);
							break;
						case "false":
							GameState.SetState(action[1], false);
							break;
						case "toggle":
							GameState.ToggleState(action[1]);
							break;
						default:
							break;
					}
					break;

				case "RequireItem":
					// if the check is negated
					if (action[1].StartsWith('!'))
					{
						// remove leading "!"
						if (!GameState.CheckForItem(action[1][1..]))
						{
							// if it is true, continue with executing
							// Console.WriteLine($"Required Item: {action[1]}");
							break;
						}
					}
					// if the check if not negated
					else
					{
						if (GameState.CheckForItem(action[1]))
						{
							// if it is true, continue with executing
							// Console.WriteLine($"Required Item: {action[1]}");
							break;
						}
					}
					// if the checks have been false

					// check if there is a StartBlock before
					List<string> a;
					try
					{
						a = actions[i - 1];
					}
					catch (Exception)
					{
						// Require is first element, exit entirely
						return;
					}

					if (a[0] == "StartBlock")
					{
						// start skipping to corresponding EndBlock statement
						block.Skipping = true;
						block.SkippingTo = a[1];
						break;
					}
					else
					{
						return;
					}

				case "RequireGameState":
					// if the check is negated
					if (action[1].StartsWith('!'))
					{
						// remove leading "!"
						if (!GameState.GetState(action[1][1..]))
						{
							// if it is true, continue with executing
							// Console.WriteLine($"Required GameState: {action[1]}");
							break;
						}
					}
					// if the check if not negated
					else
					{
						if (GameState.GetState(action[1]))
						{
							// if it is true, continue with executing
							// Console.WriteLine($"Required GameState: {action[1]}");
							break;
						}
					}

					// if the checks have been false

					// check if there is a StartBlock before
					// stupid naming, but why not
					List<string> b;
					try
					{
						b = actions[i - 1];
					}
					catch (Exception)
					{
						// Require is first element, exit entirely
						return;
					}

					if (b[0] == "StartBlock")
					{
						// start skipping to corresponding EndBlock statement
						block.Skipping = true;
						block.SkippingTo = b[1];
						break;
					}
					else
					{
						return;
					}

				case "PlaySound":
					await SoundService.PlaySound(action[1]);
					break;

				case "PlayMusic":
					await SoundService.PlayMusic(action[1]);
					break;

				case "StopMusic":
					await SoundService.StopMusic();
					break;

				//* They kind of don't do anything on their own
				// case "StartBlock":
				// 	block.Stack.Add(action[1]);
				// 	// Console.WriteLine($"StartBlock {action[1]}");
				// 	break;
				// case "EndBlock":
				// 	block.Stack.Remove(block.Stack.Last());
				// 	// Console.WriteLine($"EndBlock {action[1]}");
				// 	break;

				case "Exit":
					// Console.WriteLine("return");
					return;

				case "Sleep":
					await Task.Delay(int.Parse(action[1]));
					break;

				case "ChangeRedBull":
					GameState.ChangeRedBull(int.Parse(action[1]));
					break;

				// bloody hell, what have I done
				case "RequireRedBull":
					// more elegant in a switch
					switch (action[1][0])
					{
						case '=' when GameState.RedBullCount == int.Parse(action[1][1..]):
							break;
						case '<' when GameState.RedBullCount < int.Parse(action[1][1..]):
							break;
						case '>' when GameState.RedBullCount > int.Parse(action[1][1..]):
							break;
						// just use the `when` here and discard the value of action[1][0]
						case var _ when action[1].ToString().Contains('-'):
							string[] range = action[1].Split('-');
							if (GameState.RedBullCount > int.Parse(range[0]) && GameState.RedBullCount < int.Parse(range[1]))
							{
								break;
							}
							else
							{
								goto Skip;
							}
						// one danger: if the string is invalid, it will go to skip, which could cause problems
						// but the SlidesVerifier should catch invalid strings, so it should be fine
						default:
							goto Skip;
					}
					//* Really important, don't forget to break here
					//* else it falls through to the "Skip" label and does (mostly) just return
					break;

				// I used goto, let's fucking go
				// go here if RequireRedBull condition is not satisfied 
				Skip:
					List<string> c;
					try
					{
						c = actions[i - 1];
					}
					catch (Exception)
					{
						// Require is first element, exit entirely
						return;
					}

					if (c[0] == "StartBlock")
					{
						// start skipping to corresponding EndBlock statement
						block.Skipping = true;
						block.SkippingTo = c[1];
						break;
					}
					else
					{
						return;
					}

				case "ShowMessage":
					ToastParameters parameters = new();
					parameters.Add(nameof(ToastMessage.Message), action[1]);
					ToastService.ShowToast<ToastMessage>(parameters);
					break;

				case "PlayVideo":
					List<string> coords = [.. action[1].Split(",")];
					await VideoService.PlaceVideo(coords[0], coords[1], coords[2], coords[3], action[2]);
					await VideoService.PlayVideo();
					await VideoService.LetFinish();
					await VideoService.PlaceVideo("0", "0", "0", "0", "");
					break;

				default:
					break;
			}
		}
	}
}
