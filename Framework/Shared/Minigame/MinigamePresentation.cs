


namespace Framework.Minigames.MinigameDefClasses;

public class MyMinigame : MinigameDefBase
{
	public override string BackgroundImage { get; set; } = "images/TestingImages/HM3_hallwayN.jpg";



	[Element]
	public Rectangle Rect { get; set; }

	public GameObjectContainer<Rectangle> Rects { get; set; } = new();

	public MyMinigame()
	{
		var id = Guid.NewGuid().ToString();
		Rect = new()
		{
			Id = id,
			X = 100,
			Y = 100,
			Width = 100,
			Height = 100,
			Fill = "red",
			OnClick = (args) => { Finish(null, "HM305"); }
		};

		for (int i = 100; i < 1900; i += 100)
		{
			id = Guid.NewGuid().ToString();
			var rect = new Rectangle()
			{
				Id = id,
				X = i,
				Y = 100,
				Width = 50,
				Height = 50,
				Fill = "blue",
				OnClick = (arts) => { GameState.AddItem("goldkey"); }
			};
			Rects.Add(rect);
			AddElement(rect);
		}


	}


}















































// 	public override string BackgroundImage { get; set; } = "images/TestingImages/HM3_hallwayN.jpg";



// 	[Element]
// 	public Rectangle Rect { get; set; }

// 	public void SayHello(EventArgs args)
// 	{
// 		Console.WriteLine("Hello World!");
// 	}

// 	public GameObjectContainer<Rectangle> Rects { get; set; } = new();

// 	public MyMinigame()
// 	{

// 		Rect = new()
// 		{
// 			X = 100,
// 			Y = 100,
// 			Width = 100,
// 			Height = 100,
// 			Fill = "red",
// 			OnClick = (args) => { Rects.Transform((r) => r.Visible = !r.Visible); Update(); }
// 		};


// 		for (int i = 100; i < 1900; i += 100)
// 		{
// 			var rect = new Rectangle()
// 			{
// 				X = i,
// 				Y = 100,
// 				Width = 50,
// 				Height = 50,
// 				Fill = "blue",
// 			};
// 			AddElement(rect);
// 			Rects.Add(rect);
// 		}
// 	}
// }














// public Polygon Polygon { get; set; }

// public GameObjectContainer<Rectangle> Rects { get; set; } = new();

// public MyMinigame()
// {
// 	// Polygon = new()
// 	// {
// 	// 	Points = "100,100 500,500 100,500",
// 	// 	Fill = "red",
// 	// 	OnClick = (eventArgs) => { Console.WriteLine("hello"); }
// 	// };
// 	for (int i = 100; i < 1920; i += 100)
// 	{
// 		var id = Guid.NewGuid().ToString("N");
// 		var rect = new Rectangle()
// 		{
// 			Id = id,
// 			X = i,
// 			Y = 100,
// 			Width = 50,
// 			Height = 50,
// 			Fill = "blue",
// 			OnClick = (args) => { Rects.Transform((rect) => rect.Y -= 50); Update(); }
// 		};
// 		AddElement(rect);
// 		Rects.Add(rect);
// 	}
// }

// public override async Task GameLoop(CancellationToken token)
// {
// 	while (true)
// 	{
// 		Rects.Transform((r) => { r.Y += 5; });
// 		Update();
// 		await Task.Delay(10, token);
// 	}
// }
