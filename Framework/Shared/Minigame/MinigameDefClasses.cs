using System.Numerics;
using Framework.Minigames;
using Framework.Minigames.MinigameDefClasses;
using Microsoft.AspNetCore.Components;
using Inject = Microsoft.AspNetCore.Components.InjectAttribute;
using Microsoft.JSInterop;

using Microsoft.AspNetCore.Components.Web;
using Framework.Sound;
using Framework.Video;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Framework.Mouse;
using Framework.Keyboard;

namespace Framework.Minigames;


public class MiniTest : MinigameDefBase
{
	public override string BackgroundImage { get; set; } = "images/TestingImages/HM305_blackboard.jpg";

	public void Test(EventArgs e)
	{
		MouseEventArgs me = (MouseEventArgs)e;
		// Console.WriteLine($"X: {me.ClientX}, Y: {me.ClientY}");
		// Console.WriteLine("Test");
		Rect.X += 20;
		Console.WriteLine(Rect.X);
		// await Task.Delay(2000);
		Update();
		if (Rect.X > 300)
		{
			Finish(null, "HM305Beamer");
		}

	}

	[Element] public Rectangle Rect { get; set; }

	public MiniTest()
	{
		Rect = new()
		{
			X = 100,
			Y = 100,
			Width = 100,
			Height = 100,
			ZIndex = 1,
			Fill = "red",
			OnClick = Test
		};
	}
}

public class CodeTerminal : MinigameDefBase
{
	public override string BackgroundImage { get; set; } = "images/TestingImages/calculator.png";

	public string Code { get; set; } = "1234";

	public string CurrentText { get; set; } = "";

	[Element] public Rectangle Button0 { get; set; }
	[Element] public Rectangle Button1 { get; set; }
	[Element] public Rectangle Button2 { get; set; }
	[Element] public Rectangle Button3 { get; set; }
	[Element] public Rectangle Button4 { get; set; }
	[Element] public Rectangle Button5 { get; set; }
	[Element] public Rectangle Button6 { get; set; }
	[Element] public Rectangle Button7 { get; set; }
	[Element] public Rectangle Button8 { get; set; }
	[Element] public Rectangle Button9 { get; set; }

	[Element] public Text Text { get; set; }

	[Element] public Image Key { get; set; }

	public bool Collected { get; set; } = false;

	public void SetNumber(int num)
	{
		if (CurrentText.Length < 4)
		{
			CurrentText += num.ToString();
			Text.InnerText = CurrentText;
			Update();
		}
		if (CurrentText.Length >= 4)
		{
			if (CurrentText == Code && Collected == false)
			{
				Key.Visible = true;
			}
			else
			{
				CurrentText = "";
			}
		}
	}

	public async void CollectKey(EventArgs e)
	{
		GameState.AddItem("goldkey");
		Key.Visible = false;
		GameState.ToggleState("HM305DoorClosed.CodeTerminal");
		Collected = true;
		Update();
		await Task.Delay(2000);
		Finish([["Route", "HM305DoorClosed"], ["Sleep", "1000"], ["Route", "HM305"]]);
	}

	public CodeTerminal()
	{
		int width = 72;
		int height = 42;

		string fill = "#ffff9380";

		Button7 = new()
		{
			X = 732,
			Y = 830,
			Width = width,
			Height = height,
			Fill = fill,
			OnClick = (args) => SetNumber(7),
		};
		Button8 = new()
		{
			X = 828,
			Y = 832,
			Width = width,
			Height = height,
			Fill = fill,
			OnClick = (args) => SetNumber(8),
		};
		Button9 = new()
		{
			X = 923,
			Y = 832,
			Width = width,
			Height = height,
			Fill = fill,
			OnClick = (args) => SetNumber(9),
		};
		Button4 = new()
		{
			X = 733,
			Y = 888,
			Width = width,
			Height = height,
			Fill = fill,
			OnClick = (args) => SetNumber(4),
		};
		Button5 = new()
		{
			X = 828,
			Y = 887,
			Width = width,
			Height = height,
			Fill = fill,
			OnClick = (args) => SetNumber(5),
		};
		Button6 = new()
		{
			X = 921,
			Y = 887,
			Width = width,
			Height = height,
			Fill = fill,
			OnClick = (args) => SetNumber(6),
		};
		Button1 = new()
		{
			X = 736,
			Y = 941,
			Width = width,
			Height = height,
			Fill = fill,
			OnClick = (args) => SetNumber(1),
		};
		Button2 = new()
		{
			X = 831,
			Y = 940,
			Width = width,
			Height = height,
			Fill = fill,
			OnClick = (args) => SetNumber(2),
		};
		Button3 = new()
		{
			X = 926,
			Y = 942,
			Width = width,
			Height = height,
			Fill = fill,
			OnClick = (args) => SetNumber(3),
		};
		Button0 = new()
		{
			X = 735,
			Y = 993,
			Width = width,
			Height = height,
			Fill = fill,
			OnClick = (args) => SetNumber(0),
		};
		Text = new()
		{
			InnerText = CurrentText,
			X = 620,
			Y = 490,
			FontSize = 100,
			Fill = "white"
		};
		Key = new()
		{
			X = 1400,
			Y = 230,
			Width = 450,
			OnClick = CollectKey,
			ImagePath = "InventoryImages/key2.png",
			Visible = false
		};
	}


}

public class KillTest : MinigameDefBase
{
	[Element]
	public Rectangle Generate { get; set; }

	[Element]
	public Rectangle Cancel { get; set; }

	public override string BackgroundImage { get; set; } = "images/TestingImages/HM3_hallwayE.jpg";

	public GameObjectContainer<Rectangle> Rects { get; } = new();

	public override async Task GameLoop(CancellationToken token)
	{
		while (true)
		{
			// if (token.IsCancellationRequested)
			// {
			// 	return;
			// }
			token.ThrowIfCancellationRequested();
			Rects.Transform((rect) => rect.Y += 5);
			Update();
			await Task.Delay(10, token);
		}

	}


	public Rectangle GenerateRect()
	{
		var rnd = new Random();
		int x = rnd.Next(100, 1900);
		int y = rnd.Next(100, 1000);

		string id = Guid.NewGuid().ToString("N");

		Rectangle r = new()
		{
			Id = id,
			X = x,
			Y = y,
			Width = 100,
			Height = 100,
			Fill = "red",
			OnClick = (args) => { Rects.KillId(id); Update(); }
		};

		return r;
	}

	public KillTest()
	{
		Generate = new()
		{
			X = 0,
			Y = 0,
			Width = 100,
			Height = 100,
			Fill = "blue",
			OnClick = (args) =>
			{
				var r = GenerateRect();
				Rects.Add(r);
				AddElement(r);
				Update();
			}
		};
		Cancel = new()
		{
			X = 0,
			Y = 100,
			Width = 100,
			Height = 100,
			Fill = "green",
			OnClick = (args) => { Cts.Cancel(); Console.WriteLine("Cancelled"); }
		};
	}

	public static void KillThing(SVGElement element)
	{
		element.Kill();
	}
}


public class LaurinsRain : MinigameDefBase
{
	// [Element] 
	// public Rectangle Rect {get; set;} = new(){ // With this method it is not possible to enable the OnClick event
	// 	X = 100,
	// 	Y = 100,
	// 	Width = 100,
	// 	Height = 100,
	// 	Fill = "red",	
	// 	};


	// [Element]
	// public Rectangle RectOnclick {get; set;}
	// public LaurinsRain(){ // With this, onclick is enabled => First make initialise all elements, then add properties in the constructor
	// 	RectOnclick = new(){
	// 		X = 200,
	// 		Y = 100,
	// 		Width = 100,
	// 		Height = 100,
	// 		Fill = "blue",
	// 		OnClick = (args) => {RectOnclick.Fill = "green"; Update();}
	// 	};
	// }

	public override string BackgroundImage { get; set; } = "images/TestingImages/HM3_hallwayN.jpg"; // Background Image
	public GameObjectContainer<Rectangle> Rects { get; } = new(); // Gameobject container that contains rectangles that are to be moved

	[Element]
	public Rectangle? Down { get; set; }

	[Element]
	public Rectangle? Quit { get; set; }

	public LaurinsRain()
	{
		Down = new()
		{ // Initialise button for spawning new rects
			X = 0,
			Y = 0,
			Width = 100,
			Height = 100,
			Fill = "green",
			OnClick = (args) => Rects.Transform((r) => { r.Y += 30; Update(); }) // When clicked, every rectangle in the container moves down by 30 pixels
		};
		Quit = new()
		{ // Initialise button for quitting the game
			X = 0,
			Y = 100,
			Width = 100,
			Height = 100,
			Fill = "red",
			OnClick = (args) => Finish(null, "test")
		};

		for (var i = 100; i <= 1920; i += 100)
		{ // Initialise row of elements at once
			var Rect = new Rectangle()
			{
				X = i + 20,
				Y = 0,
				Width = 50,
				Height = 50,
				Fill = "blue",
			};
			Rects.Add(Rect);
			AddElement(Rect);
		}
	}


}

public class AudioTest : MinigameDefBase
{


	[Element]
	public Rectangle Dingrect { get; set; }
	[Element]
	public Rectangle Track { get; set; }
	[Element]
	public Rectangle Sfx { get; set; }
	[Element]
	public Rectangle StopRect { get; set; }

	public override string BackgroundImage { get; set; } = "images/TestingImages/HM305_fromEntrance.jpg"; // Background Image

	public async Task PlayAudio(string path)
	{
		await SoundService.PlaySound(path); // Play sound 
	}

	public async Task PlayMusic(string path)
	{
		await SoundService.PlayMusic(path); // Play music 
	}
	public async Task StopMusic()
	{
		await SoundService.StopMusic(); // Stop music 
	}


	public AudioTest()
	{
		Dingrect = new()
		{ // button for playing first track
			X = 0,
			Y = 0,
			Width = 100,
			Height = 100,
			Fill = "green",
			OnClick = (args) => _ = PlayMusic("/audio/ambient-piano-loop-85bpm.wav") // When clicked, piano music is played
		};
		Track = new()
		{ // Button for playing second track
			X = 0,
			Y = 200,
			Width = 100,
			Height = 100,
			Fill = "blue",
			OnClick = (args) => _ = PlayMusic("/audio/doom-soundtrack.wav") // When clicked, the Backgroundtrack is played 
																			// OnClick = (args) => _ = StopMusic()
		};
		Sfx = new()
		{ // Button for playing sound effect 
			X = 0,
			Y = 400,
			Width = 100,
			Height = 100,
			Fill = "yellow",
			OnClick = (args) => _ = PlayAudio("/audio/ding.wav") // When clicked, a 'ding' sound is played
		};
		StopRect = new()
		{ // Button to stop the currenty playing track
			X = 0,
			Y = 600,
			Width = 100,
			Height = 100,
			Fill = "red",
			OnClick = (args) => _ = StopMusic() // When clicked, the music is stopped
		};
	}


}

public class VideoTest : MinigameDefBase
{
	public override string BackgroundImage { get; set; } = "images/TestingImages/HM3_hallwayN.jpg";

	[Element]
	public Rectangle PlaceLeft { get; set; }
	[Element]
	public Rectangle PlaceRight { get; set; }
	[Element]
	public Rectangle Play { get; set; }
	[Element]
	public Rectangle Pause { get; set; }
	[Element]
	public Rectangle Letfinish { get; set; }

	public async Task PlaceVideo(string x, string y, string height, string width, string src)
	{
		await VideoService.PlaceVideo(x, y, height, width, src); // Function to set up the video
	}

	public async Task PlayVideo()
	{
		await VideoService.PlayVideo(); // Function to play the video
	}

	public async Task PauseVideo()
	{
		await VideoService.PauseVideo(); // Function to pause the video
	}

	public async Task LetFinish()
	{
		await VideoService.LetFinish(); // Wait until the video finishes,
		Console.WriteLine("Finished"); // then do something
	}



	public VideoTest()
	{
		PlaceLeft = new()
		{
			X = 0,
			Y = 0,
			Width = 100,
			Height = 100,
			Fill = "red",
			OnClick = (args) => _ = PlaceVideo("200", "200", "500", "500", "/videos/axel f.mp4")
		};
		PlaceRight = new()
		{
			X = 200,
			Y = 0,
			Width = 100,
			Height = 100,
			Fill = "blue",
			OnClick = (args) => _ = PlaceVideo("500", "200", "500", "500", "/videos/axel f.mp4")
		};
		Play = new()
		{
			X = 0,
			Y = 100,
			Width = 100,
			Height = 100,
			Fill = "yellow",
			OnClick = (args) => _ = PlayVideo()
		};
		Pause = new()
		{
			X = 0,
			Y = 200,
			Width = 100,
			Height = 100,
			Fill = "green",
			OnClick = (args) => _ = PauseVideo()
		};

		Letfinish = new()
		{
			X = 0,
			Y = 400,
			Width = 100,
			Height = 100,
			Fill = "violet",
			OnClick = (args) => _ = LetFinish()
		};


	}
}

public class ElementStyleTest : MinigameDefBase
{
	public override string BackgroundImage { get; set; } = "images/TestingImages/HM3_hallwayW.jpg";

	[Element]
	public Rectangle Rectangle { get; set; } = new()
	{
		X = 100,
		Y = 100,
		Width = 200,
		Height = 100,
		Fill = "red",
		CustomStyle = "fill: blue",
		OnClick = (args) => Console.WriteLine("click"),
		OnDoubleClick = (args) => Console.WriteLine("doubleclick"),
		OnMouseEnter = (args) => Console.WriteLine("enter"),
		OnMouseLeave = (args) => Console.WriteLine("leave"),
	};

	[Element]
	public Image Image { get; set; } = new()
	{
		ImagePath = "InventoryImages/SurfaceCharger.jpeg",
		X = 100,
		Y = 400,
		Width = 300,
		// Height = 100,
		Opacity = 0.9,
	};

	[Element]
	public Text Text { get; set; } = new()
	{
		InnerText = "Hello World",
		X = 500,
		Y = 500,
		FontSize = 100,
		FontFamily = "Comic Sans MS",
		TextLength = 1000,
		// StretchLetters = true,
		// Selectable = true,
	};

	[Element]
	public Polygon Polygon { get; set; } = new()
	{
		Points = [[500, 500], [1000, 500], [800, 700], [650, 600]],
		Fill = "white",
		Opacity = 0.5,
	};

	[Element]
	public Polyline Polyline { get; set; } = new()
	{
		Points = [[1000, 500], [1500, 500], [1300, 700], [1150, 600]],
		Stroke = "blue",
		StrokeWidth = 20,
		Opacity = 0.5,
		Fill = "none"
	};

	[Element]
	public Line Line { get; set; } = new()
	{
		X1 = 1500,
		X2 = 1700,
		Y1 = 0,
		Y2 = 1000,
		Stroke = "aquamarine",
		StrokeWidth = 400,
		StrokeOpacity = 0.2,
	};

	[Element]
	public Circle Circle { get; set; } = new()
	{
		CX = 2000,
		CY = 0,
		R = 750,
		Fill = "darkviolet",
		FillOpacity = 0.3,
		Stroke = "black",
		StrokeWidth = 20,

	};

	[Element]
	public Ellipse Ellipse { get; set; } = new()
	{
		CX = 1000,
		CY = 750,
		RX = 400,
		RY = 100,
		Fill = "goldenrod",
		FillOpacity = 0.7,
		Cursor = "pointer"
	};
}

public class ElementTest : MinigameDefBase
{

	public override string BackgroundImage { get; set; } = "images/TestingImages/HM3_hallwayN.jpg";

	[Element]
	public RawMarkup Markup { get; set; }

	[Element]
	public ForeignObject ForeignTest { get; set; }

	public CustomObject CustomTest { get; set; } = new()
	{
		CustomTagName = "div",
		Content = ["Lorem ipsum dolor, sit amet consectetur adipisicing elit. Quisquam, sunt at aperiam dolore voluptatem quidem itaque sed id tenetur praesentium, minus error rerum? Nobis excepturi nostrum explicabo? Fugit, neque voluptatibus!", new CustomObject() { CustomTagName = "h1", Content = ["hihihi"] }],
		Callbacks = new()
		{
			{"onclick", (e) => Console.WriteLine("hellooooojoij")}
		},
	};

	[Element]
	public Text Text { get; set; }

	[Element]
	public CustomObject Gradient { get; set; }


	public ElementTest()
	{
		Text = new()
		{
			FontFamily = "Comic Sans MS",
			ContentMode = true,
			X = 10,
			Y = 50,
			Content =
			[
				new Tspan() {InnerText = "muha", Fill = "red", OnClick = (e) => Console.WriteLine("hello") },
				"\n\noh noes"
			]
		};

		Markup = new()
		{
			Markup = @"
			<foreignObject x=""20"" y=""20"" width=""160"" height=""160"">
				<div xmlns=""http://www.w3.org/1999/xhtml"">
				Lorem ipsum dolor sit amet,
				consectetur adipiscing elit.
				Sed mollis mollis mi ut ultricies.Nullam magna ipsum,
				porta vel dui convallis,
				rutrum imperdiet eros.
				Aliquam erat volutpat.
				</div>
			</foreignObject>
			"
		};

		ForeignTest = new()
		{
			X = 500,
			Y = 50,
			Width = 500,
			Height = 500,
			CustomObject = CustomTest
		};
		Gradient = new()
		{
			Attributes = new()
			{
				{"id", "grad"},
				{"x1", "0%"},
				{"x2", "100%"},
				{"y1", "0%"},
				{"y2", "0%"},
			},
			Content = [
				new CustomObject()
				{
					CustomTagName = "stop",
					Attributes = new()
					{
						{"offset", "0%"},
						{"stop-color", "red"}
					}
				},
				new CustomObject()
				{
					CustomTagName = "stop",
					Attributes = new()
					{
						{"offset", "100%"},
						{"stop-color", "yellow"}
					}
				}
			],
			CustomTagName = "linearGradient"
		};
		AddElement(
			new Ellipse()
			{
				CX = 300,
				CY = 70,
				RX = 85,
				RY = 55,
				Fill = "url(#grad)"
			}
		);
	}
}

public class MouseServiceTest : MinigameDefBase
{

	public override string BackgroundImage { get; set; } = "images/TestingImages/HM3_hallwayN.jpg";
	public override async Task GameLoop(CancellationToken ct)
	{
		while (true)
		{
			var state = MouseService.MouseState;
			ct.ThrowIfCancellationRequested();
			Console.WriteLine($"X: {state.X}, Y: {state.Y}");
			await Task.Delay(50, ct);
		}
	}

	public MouseServiceTest()
	{
		AddElement(new Rectangle()
		{
			X = 100,
			Y = 100,
			Width = 100,
			Height = 100,
			Fill = "red",
			OnClick = async (e) => { await MouseService.SetDelay(-1); Console.WriteLine("Disabled"); },
		});
		AddElement(new Rectangle()
		{
			X = 100,
			Y = 200,
			Width = 100,
			Height = 100,
			Fill = "green",
			OnClick = async (e) => { await MouseService.SetDelay(0); Console.WriteLine("No Delay"); },
		});
		AddElement(new Rectangle()
		{
			X = 100,
			Y = 300,
			Width = 100,
			Height = 100,
			Fill = "blue",
			OnClick = async (e) => { await MouseService.SetDelay(500); Console.WriteLine("500ms"); },
		});
		AddElement(new Rectangle()
		{
			X = 100,
			Y = 500,
			Height = 100,
			Width = 100,
			Fill = "yellow",
			OnClick = async (e) => { var x = await MouseService.GetMouseStateAsync(); Console.WriteLine($"AsyncX: {x.X}, AsyncY: {x.Y}"); },
		});
		AddElement(new Rectangle()
		{
			X = 100,
			Y = 600,
			Height = 100,
			Width = 100,
			Fill = "purple",
			OnClick = async (e) =>
			{
				var b = (MouseEventArgs)e;
				var (x, y) = await MouseService.ConvertToSvgCoords(b.ClientX, b.ClientY);
				Console.WriteLine($"ConvertedX: {x}, ConvertedY: {y}");
			},
		});
	}

}

public class IOServicesTest : MinigameDefBase
{
	public override string BackgroundImage { get; set; } = "images/TestingImages/HM3_hallwayN.jpg";

	public override void OnKeyDown(object? sender, KeyEventArgs e)
	{
		Console.WriteLine($"Key: {e.Key}, Down: {e.Down}");
	}

	public override void OnKeyUp(object? sender, KeyEventArgs e)
	{
		Console.WriteLine($"Key: {e.Key}, Down: {e.Down}");
	}

	public override void OnMouseDown(object? sender, ClickEventArgs e)
	{
		Console.WriteLine($"Button: {e.Button}, Down: {e.Down}, X: {e.X}, Y: {e.Y}");
	}

	public override void OnMouseUp(object? sender, ClickEventArgs e)
	{
		Console.WriteLine($"Button: {e.Button}, Down: {e.Down}, X: {e.X}, Y: {e.Y}");
	}
}

// public class KeyboardStateTest : MinigameDefBase
// {
// 	public override string BackgroundImage { get; set; } = "images/TestingImages/HM3_hallwayN.jpg";

// 	// public Rectangle ToggleA { get; set; }

// 	// public Rectangle ShowA { get; set; }

// 	public Dictionary<string, bool> States { get; set; }


// 	public KeyboardStateTest()
// 	{
// 		// States = KeyboardService.GetStaticKeyboardState();

// 		AddElement(new Rectangle
// 		{
// 			X = 100,
// 			Y = 100,
// 			Width = 100,
// 			Height = 100,
// 			Fill = "red",
// 			OnClick = (e) => States["KeyA"] = !States["KeyA"],
// 		});

// 		AddElement(new Rectangle()
// 		{
// 			X = 100,
// 			Y = 200,
// 			Width = 100,
// 			Height = 100,
// 			Fill = "blue",
// 			OnClick = (e) => Console.WriteLine(KeyboardService.GetKeyState("KeyA")),
// 		});
// 		AddElement(new Rectangle()
// 		{
// 			X = 100,
// 			Y = 300,
// 			Width = 100,
// 			Height = 100,
// 			Fill = "green",
// 			OnClick = (e) => States = KeyboardService.GetKeyboardState(),
// 		});
// 	}
// }

public class DimitriDialogue : MinigameDefBase
{
	public override string BackgroundImage { get; set; } = "images/TestingImages/HM3_hallwayN.jpg"; // Background Image


	//DIALOGUE STUFF
	private Image QuitButton { get; set; }
	private Image ForwardButton { get; set; }

	private Dialogue dialogue;

	//MINIGAME STUFF
	[Element]
	public required Rectangle NpcHitBox { get; set; }

	List<List<string>> messages = [
		["player", "Hello"],
		["npc", "bring me something"],
		["player", "Hello"],
		["npc", "did you bring it?"],
		["npc", "this text is white"],
		["npc", "this text is white"],
		["npc", "this text is white"],
		["npc", "this text is white"],
		["npc", "this text is white"],
		["npc", "this text is white"],
		["npc", "this text is white"],
	];

	public DimitriDialogue()
	{
		dialogue = new Dialogue(messages);

		NpcHitBox = new()
		{
			X = 1500,
			Y = 1000,
			Width = 100,
			Height = 100,
			Fill = "green",
			OnClick = async (args) => { Console.WriteLine("Click On NPC"); await StartDialogueAsync(); }
		};
	}

	public async Task StartDialogueAsync()
	{
		bool quit = false;
		bool forward = false;

		//Create forward and quit button
		QuitButton = dialogue.DrawQuitButton();
		ForwardButton = dialogue.DrawForwardButton();

		AddElement(QuitButton);
		AddElement(ForwardButton);

		QuitButton.OnClick = (args) => { quit = true; Console.WriteLine("Quit"); };
		ForwardButton.OnClick = (args) => { forward = true; Console.WriteLine("Forward"); };

		Update();

		foreach (List<string> speech in messages)
		{
			GameObjectContainer<SVGElement> Bubble = dialogue.DrawSpeechBubble(speech[0], speech[1]);

			AddElementsInContainer(Bubble);

			Update();

			await WaitForConditionAsync(() => forward || quit);

			foreach (string key in Bubble.Keys)
			{
				Elements.Remove(key);
			}

			if (quit == true)
			{
				Update();
				break;
			}

			forward = false;

			Update();
		}
	}

	private async Task WaitForConditionAsync(Func<bool> condition)
	{
		while (!condition())
		{
			await Task.Delay(100); // Check the condition every 100 milliseconds
		}
	}
}