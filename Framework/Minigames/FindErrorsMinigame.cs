using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace Framework.Minigames.MinigameDefClasses;

public class FindErrorsMinigame : MinigameDefBase
{
	public int errorsspotted = 0;
	public override string BackgroundImage { get; set; } = "minigame_assets/FindErrors_Rejda/IMG_2455.jpg";
	[Element]
	public Rectangle Rects { get; set; }
	public Rectangle newRect { get; set; }
	public Rectangle rectspot { get; set; }
	public GameObjectContainer<Rectangle> Errors { get; set; } = new();
	[Element] public Rectangle Error0 { get; set; }
	[Element] public Rectangle Error1 { get; set; }
	[Element] public Rectangle Error2 { get; set; }
	[Element] public Rectangle Error3 { get; set; }
	[Element] public Rectangle Error4 { get; set; }
	[Element] public Rectangle Error5 { get; set; }
	[Element] public Rectangle Error6 { get; set; }
	[Element] public Rectangle Error7 { get; set; }
	[Element] public Rectangle Error8 { get; set; }
	[Element] public Rectangle Error9 { get; set; }


	public override string DefaultRoute { get; set; } = "Art3";
	public FindErrorsMinigame()
	{
		BackButton.Visible = true;

		Rects = new()
		{
			// <edit by laurin> 
			X = 1818 * 1620 / 1920,
			Y = 200,
			// </edit by laurin>
			// <original>
			// X = 1818 * (3 / 2),
			// Y = 200 * (3 / 2),
			// </original>
			Width = 100,
			Height = 600,
			Fill = "red",
			OnClick = (args) => errorspage(args)
		};
	}

	public void errorspage(EventArgs e)
	{
		BackgroundImage = "minigame_assets/FindErrors_Rejda/IMG_2457.jpg";
		Update();
		newRect = new()
		{
			// <edit by laurin>
			X = 0,
			Y = 200,
			// </edit by laurin>
			// <original>
			// X = 0,
			// Y = 200 * (3 / 2),
			// </original>
			Width = 100,
			Height = 600,
			Fill = "red",
			OnClick = (args) => originalpage(args)
		};
		AddElement(newRect);
		Rects.Kill();
		bool found = false;
		Error0 = new() //Box
		{
			// <edit by laurin>
			X = 1200 * 1620 / 1920,
			Y = 10,
			// </edit by laurin>
			// <original>			
			// X = 1200 * (3 / 2),
			// Y = 10 * (3 / 2),
			// </original>
			Width = 350,
			Height = 250,
			Fill = "transparent",
			OnClick = (args) => ChangeColor(args, Error0)

		};
		Errors.Add(Error0);
		AddElement(Error0);
		Update();
		Error1 = new() //blue zylinder
		{
			// <edit by laurin>
			X = 1000 * 1620 / 1920,
			Y = 350,
			// </edit by laurin>
			// <original>
			// X = 1000 * (3 / 2),
			// Y = 350 * (3 / 2),
			// </original>
			Width = 110,
			Height = 240,
			Fill = "transparent",
			OnClick = (args) => ChangeColor(args, Error1)

		};
		Errors.Add(Error1);
		AddElement(Error1);
		Update();
		Error2 = new() //purple strip
		{
			// <edit by laurin>
			X = 1450 * 1620 / 1920,
			Y = 360,
			// </edit by laurin>
			// <original>
			// X = 1450 * (3 / 2),
			// Y = 360 * (3 / 2),
			// </original>
			Width = 110,
			Height = 200,
			Fill = "transparent",
			OnClick = (args) => ChangeColor(args, Error2)

		};
		Errors.Add(Error2);
		AddElement(Error2);
		Update();
		Error3 = new()//spoons
		{
			// <edit by laurin>
			X = 1380 * 1620 / 1920,
			Y = 850,
			// </edit by laurin>
			// <original>
			// X = 1380 * (3 / 2),
			// Y = 850 * (3 / 2),
			// </original>
			Width = 150,
			Height = 250,
			Fill = "transparent",
			OnClick = (args) => ChangeColor(args, Error3)

		};
		Errors.Add(Error3);
		AddElement(Error3);
		Update();
		Error4 = new() // black lid
		{
			// <edit by laurin>
			X = 1260 * 1620 / 1920,
			Y = 510,
			// </edit by laurin>
			// <original>
			// X = 1260 * (3 / 2),
			// Y = 510 * (3 / 2),
			// </original>
			Width = 120,
			Height = 200,
			Fill = "transparent",
			OnClick = (args) => ChangeColor(args, Error4)

		};
		Errors.Add(Error4);
		AddElement(Error4);
		Update();
		Error5 = new()//B12 Box upside down
		{
			// <edit by laurin>
			X = 1180 * 1620 / 1920,
			Y = 620,
			// </edit by laurin>
			// <original>
			// X = 1180 * (3 / 2),
			// Y = 620 * (3 / 2),
			// </original>
			Width = 55,
			Height = 100,
			Fill = "transparent",
			OnClick = (args) => ChangeColor(args, Error5)

		};
		Errors.Add(Error5);
		AddElement(Error5);
		Update();
		Error6 = new() //blue lid upside down
		{
			// <edit by laurin>
			X = 792 * 1620 / 1920,
			Y = 390,
			// </edit by laurin>
			// <original>
			// X = 792 * (3 / 2),
			// Y = 390 * (3 / 2),
			// </original>
			Width = 100,
			Height = 180,
			Fill = "transparent",
			OnClick = (args) => ChangeColor(args, Error6)

		};
		Errors.Add(Error6);
		AddElement(Error6);
		Update();
		Error7 = new() //t�rkis lid moved
		{
			// <edit by laurin>
			X = 750 * 1620 / 1920,
			Y = 600,
			// </edit by laurin>
			// <original>
			// X = 750 * (3 / 2),
			// Y = 600 * (3 / 2),
			// </original>
			Width = 100,
			Height = 140,
			Fill = "transparent",
			OnClick = (args) => ChangeColor(args, Error7)

		};
		Errors.Add(Error7);
		AddElement(Error7);
		Update();
		Error8 = new() //cutting board
		{
			// <edit by laurin>
			X = 750 * 1620 / 1920,
			Y = 770,
			// </edit by laurin>
			// <original>
			// X = 750 * (3 / 2),
			// Y = 770 * (3 / 2),
			// </original>
			Width = 100,
			Height = 300,
			Fill = "transparent",
			OnClick = (args) => ChangeColor(args, Error8)

		};
		Errors.Add(Error8);
		AddElement(Error8);
		Update();
		Error9 = new()//missing 123
		{
			// <edit by laurin>
			X = 560 * 1620 / 1920,
			Y = 630,
			// </edit by laurin>
			// <original>
			// X = 560 * (3 / 2),
			// Y = 630 * (3 / 2),
			// </original>
			Width = 75,
			Height = 100,
			Fill = "transparent",
			OnClick = (args) => ChangeColor(args, Error9)

		};
		Errors.Add(Error9);
		AddElement(Error9);
		Update();
	}
	public void originalpage(EventArgs e)
	{
		BackgroundImage = "minigame_assets/FindErrors_Rejda/IMG_2455.jpg";
		Update();
		Rects = new()
		{
			// <edit by laurin>
			X = 1818 * 1620 / 1920,
			Y = 200,
			// </edit by laurin>
			// <original>
			// X = 1818,
			// Y = 200,
			// </original>
			Width = 100,
			Height = 600,
			Fill = "red",
			OnClick = (args) => errorspage(args)
		};
		AddElement(Rects);
		newRect.Kill();
		Update();
		//Error0.Kill();
	}
	public void ChangeColor(EventArgs e, Rectangle rect)
	{
		rect.Fill = "rgba(0,255,0,0.5)";
		Update();
		errorsspotted++;
		if (errorsspotted == 10)
		{
		    GameState.SetState("BG-Game.Complete", true);
			Finish(null, "Art3");
		}
	}
}
