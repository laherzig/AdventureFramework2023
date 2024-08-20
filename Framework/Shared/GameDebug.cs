using Framework.Slides.JsonClasses;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Framework.Game;

public partial class GameBase
{
	[Parameter]
	public bool Debug { get; set; }

	// honestly, this is hilarious, but it kinda makes sense I guess
	protected override async Task OnInitializedAsync()
	{
		if (Debug)
		{
			await InitDebug();
		}
	}

	[Inject]
	private IJSRuntime JSRuntime { get; set; } = null!;

	private DotNetObjectReference<GameBase> objRef = null!;

	public async Task InitDebug()
	{
		objRef = DotNetObjectReference.Create(this);
		await JSRuntime.InvokeVoidAsync("debug.init", objRef);
	}

	[JSInvokable]
	public async void Action(List<string> action)
	{
		try
		{
			await EvaluateActions([action]);
		}
		catch (Exception e)
		{
			Console.WriteLine("Error while executing Action: " + e.Message);
		}
	}

	[JSInvokable]
	public async void Actions(List<List<string>> actions)
	{
		try
		{
			await EvaluateActions(actions);
		}
		catch (Exception e)
		{
			Console.WriteLine("Error while executing Actions: " + e.Message);
		}
	}
	[JSInvokable]
	public void Update()
	{
		StateHasChanged();
	}

	[JSInvokable]
	public string GetCurrentSlide()
	{
		return GameState.CurrentSlide;
	}
	[JSInvokable]
	public JsonSlide GetCurrentSlideContent()
	{
		return SlideService.GetSlide(GameState.CurrentSlide);
	}
	[JSInvokable]
	public Dictionary<string, JsonButton> GetCurrentSlideButtons()
	{
		try
		{
			return SlideService.GetSlide(GameState.CurrentSlide).Buttons;
		}
		catch (Exception e)
		{
			Console.WriteLine("Error while getting buttons: " + e.Message);
			return [];
		}
	}
	[JSInvokable]
	public bool GetGameState(string id)
	{
		if (GameState.TryGetState(id, out bool value))
		{
			return value;
		}
		Console.WriteLine($"State {id} not found, returning true");
		return true;
	}

	// becuase StateHasChanged doesn't quite do the job,
	// we need to route to another slide and then back
	[JSInvokable]
	public async Task ReRoute()
	{
		var slide = GameState.CurrentSlide;
		Random rand = new();
		var randomKey = SlideService.Slides.ElementAt(rand.Next(SlideService.Slides.Count)).Key;
		while (randomKey == slide)
		{
			randomKey = SlideService.Slides.ElementAt(rand.Next(SlideService.Slides.Count)).Key;
		}
		if (randomKey != null)
		{
			await EvaluateActions([["Route", randomKey], ["Route", slide]]);
		}
		else
		{
			Console.WriteLine("Error while re-routing. I don't know why this happened.");
		}
	}

	[JSInvokable]
	public string[] GetItems()
	{
		return [.. GameState.GetItemObjects().Keys];
	}

	[JSInvokable]
	public int RedBull(int amount)
	{
		// package getredbullcount and changeredbull into one method
		// if you want to get amount, just pass 0
		GameState.ChangeRedBull(amount);
		return GameState.RedBullCount;
	}

	[JSInvokable]
	public string GetSaveString()
	{
		return GameState.GetSaveString();
	}

	[JSInvokable]
	public void LoadSaveString(string saveString)
	{
		GameState.SetFromSaveString(saveString);
	}
}