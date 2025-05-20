namespace Framework.Minigames.MinigameDefClasses;

public class PianoMinigame : MinigameDefBase
{
	bool Set = false;
	string[] Colors = { "red", "orange", "yellow", "green", "blue", "indigo", "violet" };
	public override string BackgroundImage { get; set; } = "minigame_assets/Music_Assets/Piano_Close_View.jpg";


	string[][] rounds = [["C3", "E3", "G3"], ["F3", "Gs3", "C4"], ["Ds3", "G3", "As3"], ["Cs3", "E3", "Gs3"]];
	int level = 1;
	int step = 1;


	public PianoMinigame()
	{
		LoadUI();
		DrawKeys();

	}


	public void LoadUI()
	{

		Ellipse Start = new()
		{
			CX = 810,
			CY = 475,
			RY = 80,
			RX = 105,
			Fill = "black",
			FillOpacity = 0,
			OnClick = StartRound
		};
		AddElement(Start);

		Image Quit = new()
		{
			ImagePath = "minigame_assets/Music_Assets/Quit.png",
			X = 25,
			Y = 25,
			Width = 100,
			Height = 100,
			OnClick = (args) => { 

                Finish(null, "Music-2"); }
		};
		AddElement(Quit);

		Text Welcome = new()
		{
			Id = "WelcomeMessage",
			InnerText = "Welcome to \"Musician Test\"!",
			X = 325,
			Y = 100,
			FontSize = 70,
			Fill = "white"
		};
		AddElement(Welcome);


		Text Intro = new()
		{
			Id = "Introduction",
			InnerText = "You're task is to repeat the major/minor you're going to hear, there're 4 rounds. Click on the button above to play the melody.",
			X = 300,
			Y = 575,
			FontSize = 20,
			Fill = "white"
		};
		AddElement(Intro);
		Update();
	}
	async public void MusicLoop()
	{
		foreach (string note in rounds[level - 1])
		{
			PlaySound(note);
			await Task.Delay(2000);
		}
		Update();
	}


	public void StartRound(EventArgs args)
	{
		//  DrawKeys(args);
		//  CountDown();
		if (level == 1)
		{
			// try statement made by laurin
			// solves a really simple error, so I think I am allowed to put it in
			try
			{
				Elements.KillId("WelcomeMessage");
				Elements.KillId("Introduction");
			}
			catch { }
		}
		MusicLoop();
	}

	public void CheckKey(EventArgs args, string key)
	{
		if (key == rounds[level - 1][step - 1])
		{
			if (step == rounds[level - 1].Length)
			{
				PlaySound("S");
				Update();
				if (level >= rounds.Length)
				{
		            GameState.SetState("Music-Red.Complete", true);
					Finish(null, "Music-Red");
				}
				else
				{
					level++;
					step = 1;
				}
			}
			else
			{
				PlaySound(key);
				step++;
			}
		}
		else if (step == rounds[level - 1].Length)
		{
			PlaySound("W");
			step = 1;
		}
		else
		{
			PlaySound(key);
			step++;
		}
	}
	public void DrawKeys()
	{

		var C3 = new Polygon()
		{
			Id = "Key_C3",
			Points = [[581, 965], [607, 966], [606, 948], [612, 906], [602, 905], [618, 832], [602, 832], [579, 949], [580, 948], [579, 949]],
			Fill = Colors[0],
			FillOpacity = 0.2,
			OnClick = (args) => CheckKey(args, "C3")

		};
		AddElement(C3);

		var Cs3 = new Polygon()
		{
			Id = "Key_Cs3",
			Points = [[617, 905], [602, 905], [618, 832], [631, 833]],
			Fill = Colors[1],
			FillOpacity = 0.6,
			OnClick = (args) => CheckKey(args, "Cs3")

		};
		AddElement(Cs3);
		Update();


		var D3 = new Polygon()
		{
			Id = "Key_D3",
			Points = [[608, 965], [606, 949], [613, 905], [618, 904], [631, 833], [631, 832], [648, 833], [635, 906], [639, 905], [632, 951], [633, 965], [620, 966]],
			Fill = Colors[2],
			FillOpacity = 0.2,
			OnClick = (args) => CheckKey(args, "D3")


		};
		AddElement(D3);
		Update();


		var Ds3 = new Polygon()
		{
			Id = "Key_Ds3",
			Points = [[648, 905], [635, 905], [648, 832], [660, 833]],
			Fill = Colors[3],
			FillOpacity = 0.6,
			OnClick = (args) => CheckKey(args, "Ds3")

		};
		AddElement(Ds3);
		Update();


		var E3 = new Polygon()
		{
			Id = "Key_E3",
			Points = [[660, 965], [634, 965], [633, 949], [640, 904], [648, 905], [660, 832], [674, 833], [660, 949]],
			Fill = Colors[4],
			FillOpacity = 0.2,
			OnClick = (args) => CheckKey(args, "E3")
		};
		AddElement(E3);
		Update();


		var F3 = new Polygon()
		{
			Id = "Key_F3",
			Points = [[660, 949], [674, 832], [688, 832], [680, 907], [690, 907], [687, 949], [687, 965], [660, 965]],
			Fill = Colors[5],
			FillOpacity = 0.2,
			OnClick = (args) => CheckKey(args, "F3")

		};
		AddElement(F3);
		Update();


		var Fs3 = new Polygon()
		{
			Id = "Key_Fs3",
			Points = [[680, 906], [688, 832], [703, 833], [694, 905]],
			Fill = Colors[6],
			FillOpacity = 0.6,
			OnClick = (args) => CheckKey(args, "Fs3")

		};
		AddElement(Fs3);
		Update();

		var G3 = new Polygon()
		{
			Id = "Key_G3",
			Points = [[686, 965], [686, 950], [691, 907], [694, 907], [702, 833], [715, 833], [711, 907], [715, 907], [712, 950], [712, 950], [714, 966], [714, 966]],
			Fill = Colors[0],
			FillOpacity = 0.2,
			OnClick = (args) => CheckKey(args, "G3")

		};
		AddElement(G3);
		Update();

		var Gs3 = new Polygon()
		{
			Id = "Key_Gs3",
			Points = [[711, 905], [724, 905], [728, 833], [716, 833], [711, 906]],
			Fill = Colors[1],
			FillOpacity = 0.6,
			OnClick = (args) => CheckKey(args, "Gs3")

		};
		AddElement(Gs3);
		Update();

		var A3 = new Polygon()
		{
			Id = "Key_A3",
			Points = [[714, 966], [714, 950], [717, 907], [724, 907], [728, 833], [744, 833], [739, 907], [742, 907], [740, 951], [740, 965]],
			Fill = Colors[2],
			FillOpacity = 0.2,
			OnClick = (args) => CheckKey(args, "A3")

		};
		AddElement(A3);
		Update();

		var As3 = new Polygon()
		{
			Id = "Key_As3",
			Points = [[740, 907], [754, 907], [757, 833], [744, 833]],
			Fill = Colors[3],
			FillOpacity = 0.6,
			OnClick = (args) => CheckKey(args, "As3")

		};
		AddElement(As3);
		Update();

		var B3 = new Polygon()
		{
			Id = "Key_B3",
			Points = [[742, 967], [741, 949], [743, 906], [754, 906], [757, 832], [771, 832], [769, 950], [767, 967]],
			Fill = Colors[4],
			FillOpacity = 0.2,
			OnClick = (args) => CheckKey(args, "B3")

		};
		AddElement(B3);
		Update();

		var C4 = new Polygon()
		{
			Id = "Key_C4",
			Points = [[768, 949], [768, 967], [794, 967], [794, 950], [794, 906], [784, 906], [786, 832], [771, 833]],
			Fill = Colors[5],
			FillOpacity = 0.2,
			OnClick = (args) => CheckKey(args, "C4")

		};
		AddElement(C4);
		Update();

	}

	public void PlaySound(string Note)
	{

		SoundService.PlaySound("minigame_assets/Music_Assets/Sounds/" + Note + ".wav");

	}
}

