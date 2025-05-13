using System.Dynamic;
using System.Security.Cryptography.X509Certificates;

namespace Framework.Minigames.MinigameDefClasses;

public class LockpickingMinigame : MinigameDefBase
{
	[Element]
	public Image ImagePath { get; set; } = new();

	[Element]
	public Rectangle ragemode { get; set; } = new();
	[Element]
	public Rectangle ragetangle { get; set; } = new();
	[Element]
	public Rectangle PunchNumber { get; set; } = new();

	[Element]
	public Rectangle rectLock { get; set; } = new();

	[Element]
	public Rectangle rectconf { get; set; } = new();

	[Element]
	public Rectangle rect0 { get; set; } = new();
	[Element]
	public Rectangle rect01 { get; set; } = new();
	[Element]
	public Rectangle rect02 { get; set; } = new();

	[Element]
	public Rectangle rect1 { get; set; } = new();
	[Element]
	public Rectangle rect11 { get; set; } = new();
	[Element]
	public Rectangle rect12 { get; set; } = new();

	[Element]
	public Rectangle rect2 { get; set; } = new();
	[Element]
	public Rectangle rect21 { get; set; } = new();
	[Element]
	public Rectangle rect22 { get; set; } = new();

	[Element]
	public Rectangle rect3 { get; set; } = new();
	[Element]
	public Rectangle rect31 { get; set; } = new();
	[Element]
	public Rectangle rect32 { get; set; } = new();

	[Element]
	public Text TriesText { get; set; } = new();
	[Element]
	public Text TriesNumber { get; set; } = new();

	[Element]
	public Text RageNumber { get; set; } = new();

	[Element]
	public Text Number0 { get; set; } = new();
	[Element]
	public Text Number02 { get; set; } = new();
	[Element]
	public Text Number01 { get; set; } = new();

	[Element]
	public Text Number1 { get; set; } = new();
	[Element]
	public Text Number11 { get; set; } = new();
	[Element]
	public Text Number12 { get; set; } = new();

	[Element]
	public Text Number2 { get; set; } = new();
	[Element]
	public Text Number21 { get; set; } = new();
	[Element]
	public Text Number22 { get; set; } = new();

	[Element]
	public Text Number3 { get; set; } = new();
	[Element]
	public Text Number31 { get; set; } = new();
	[Element]
	public Text Number32 { get; set; } = new();

	[Element]
	public Text Confirm { get; set; } = new();


	public override string BackgroundImage { get; set; } = "minigame_assets/LockPick_assets/images/LockerVonVorne.png";

	public interface IGameObject
	{
		public string Id { get; set; }
		public int? ZIndex { get; set; }
		public bool Visible { get; set; }
		public void Kill();
		void Finish(List<List<string>>? actions, string? route = null);


	}
	public override string DefaultRoute { get; set; } = "Biologie21";
	public LockpickingMinigame()
	{
		BackButton.Visible = true;

		if (BackgroundImage == "minigame_assets/LockPick_assets/images/LockerVonVorne.png")
		{
			rectLock = new()
			{
				X = 650,
				Y = 450,
				Width = 250,
				Height = 500,
				Fill = "transparent",
				OnClick = (args) =>
				{
					if (BackgroundImage == "minigame_assets/LockPick_assets/images/LockerVonVorne.png")
					{
						BackgroundImage = "minigame_assets/LockPick_assets/images/LockerVonHinten.png";
						Update();
						rect0.Visible = false;
						rect01.Visible = false;
						rect02.Visible = false;
						rect1.Visible = false;
						rect11.Visible = false;
						rect12.Visible = false;
						rect2.Visible = false;
						rect21.Visible = false;
						rect22.Visible = false;
						rect3.Visible = false;
						rect31.Visible = false;
						rect32.Visible = false;
						Number0.Visible = false;
						Number01.Visible = false;
						Number02.Visible = false;
						Number1.Visible = false;
						Number11.Visible = false;
						Number12.Visible = false;
						Number2.Visible = false;
						Number21.Visible = false;
						Number22.Visible = false;
						Number3.Visible = false;
						Number31.Visible = false;
						Number32.Visible = false;
					}
					else if (BackgroundImage == "minigame_assets/LockPick_assets/images/LockerVonHinten.png")
					{
						BackgroundImage = "minigame_assets/LockPick_assets/images/LockerVonVorne.png";
						Update();
						rect0.Visible = true;
						rect01.Visible = true;
						rect02.Visible = true;
						rect1.Visible = true;
						rect11.Visible = true;
						rect12.Visible = true;
						rect2.Visible = true;
						rect21.Visible = true;
						rect22.Visible = true;
						rect3.Visible = true;
						rect31.Visible = true;
						rect32.Visible = true;
						Number0.Visible = true;
						Number01.Visible = true;
						Number02.Visible = true;
						Number1.Visible = true;
						Number11.Visible = true;
						Number12.Visible = true;
						Number2.Visible = true;
						Number21.Visible = true;
						Number22.Visible = true;
						Number3.Visible = true;
						Number31.Visible = true;
						Number32.Visible = true;
					}
					Update();
				}

			};

			rectconf = new()
			{
				X = 1390,
				Y = 950,
				Width = 225,
				Height = 80,
				Fill = "darkgreen",

			};
			PunchNumber = new()
			{
				X = 705,
				Y = 50,
				Width = 225,
				Height = 80,
				Fill = "darkred",

			};
			ragetangle = new()
			{
				X = 20,
				Y = 20,
				Width = 220,
				Height = 100,
				Fill = "darkred",

			};
			rect0 = new()
			{
				X = 957,
				Y = 572,
				Width = 35,
				Height = 40,
				Fill = "lightgrey",

			};
			rect01 = new()
			{
				X = 1010,
				Y = 570,
				Width = 25,
				Height = 35,
				Fill = "lightgrey",

			};
			rect02 = new()
			{
				X = 907,
				Y = 571,
				Width = 25,
				Height = 35,
				Fill = "lightgrey",

			};
			rect1 = new()
			{
				X = 957,
				Y = 652,
				Width = 35,
				Height = 40,
				Fill = "lightgrey",
			};
			rect11 = new()
			{
				X = 1010,
				Y = 650,
				Width = 25,
				Height = 35,
				Fill = "lightgrey",
			};
			rect12 = new()
			{
				X = 909,
				Y = 652,
				Width = 25,
				Height = 35,
				Fill = "lightgrey",
			};
			rect2 = new()
			{
				X = 960,
				Y = 737,
				Width = 35,
				Height = 40,
				Fill = "lightgrey",
			};
			rect21 = new()
			{
				X = 1010,
				Y = 734,
				Width = 25,
				Height = 35,
				Fill = "lightgrey",
			};
			rect22 = new()
			{
				X = 911,
				Y = 736,
				Width = 25,
				Height = 35,
				Fill = "lightgrey",
			};
			rect3 = new()
			{
				X = 960,
				Y = 815,
				Width = 35,
				Height = 40,
				Fill = "lightgrey",
			};
			rect31 = new()
			{
				X = 1010,
				Y = 815,
				Width = 25,
				Height = 35,
				Fill = "lightgrey",
			};
			rect32 = new()
			{
				X = 911,
				Y = 816,
				Width = 25,
				Height = 35,
				Fill = "lightgrey",
			};
			Number0 = new()
			{
				InnerText = "0",
				X = 962,
				Y = 606,
				FontSize = 50,
				FontFamily = "Aria",
				Fill = "black",
				OnClick = (args) =>
				{
					int x = Convert.ToInt16(Number0.InnerText);
					Number01.InnerText = x.ToString();
					x = x + 1;
					if (x == 10)
					{
						x = 0;
					}
					Number0.InnerText = x.ToString();
					x = x + 1;
					if (x == 10)
					{
						x = 0;
					}
					Number02.InnerText = x.ToString();

					Update();
				}
			};
			Number01 = new()
			{
				InnerText = "9",
				X = 1015,
				Y = 600,
				FontSize = 35,
				FontFamily = "Aria",
				Fill = "black",
			};
			Number02 = new()
			{
				InnerText = "1",
				X = 910,
				Y = 600,
				FontSize = 35,
				FontFamily = "Aria",
				Fill = "black",
			};
			Number1 = new()
			{
				InnerText = "0",
				X = 962,
				Y = 689,
				FontSize = 50,
				FontFamily = "Aria",
				Fill = "black",
				OnClick = (args) =>
				{
					int q = Convert.ToInt16(Number1.InnerText);
					Number11.InnerText = q.ToString();
					q = q + 1;
					if (q == 10)
					{
						q = 0;
					}
					Number1.InnerText = q.ToString();
					q = q + 1;
					if (q == 10)
					{
						q = 0;
					}
					Number12.InnerText = q.ToString();

					Update();
				}
			};
			Number11 = new()
			{
				InnerText = "9",
				X = 1015,
				Y = 680,
				FontSize = 35,
				FontFamily = "Aria",
				Fill = "black",
			};
			Number12 = new()
			{
				InnerText = "1",
				X = 913,
				Y = 682,
				FontSize = 35,
				FontFamily = "Aria",
				Fill = "black",
			};
			Number2 = new()
			{
				InnerText = "0",
				X = 962,
				Y = 772,
				FontSize = 50,
				FontFamily = "Aria",
				Fill = "black",
				OnClick = (args) =>
				{
					int z = Convert.ToInt16(Number2.InnerText);
					Number21.InnerText = z.ToString();
					z = z + 1;
					if (z == 10)
					{
						z = 0;
					}
					Number2.InnerText = z.ToString();
					z = z + 1;
					if (z == 10)
					{
						z = 0;
					}
					Number22.InnerText = z.ToString();

					Update();
				}
			};
			Number21 = new()
			{
				InnerText = "9",
				X = 1015,
				Y = 762,
				FontSize = 35,
				FontFamily = "Aria",
				Fill = "black",
			};
			Number22 = new()
			{
				InnerText = "1",
				X = 914,
				Y = 766,
				FontSize = 35,
				FontFamily = "Aria",
				Fill = "black",
			};
			Number3 = new()
			{
				InnerText = "0",
				X = 962,
				Y = 853,
				FontSize = 50,
				FontFamily = "Aria",
				Fill = "black",
				OnClick = (args) =>
				{
					int y = Convert.ToInt16(Number3.InnerText);
					Number31.InnerText = y.ToString();
					y = y + 1;
					if (y == 10)
					{
						y = 0;
					}
					Number3.InnerText = y.ToString();
					y = y + 1;
					if (y == 10)
					{
						y = 0;
					}
					Number32.InnerText = y.ToString();

					Update();
				}
			};
			Number31 = new()
			{
				InnerText = "9",
				X = 1015,
				Y = 844,
				FontSize = 35,
				FontFamily = "Aria",
				Fill = "black",
			};
			Number32 = new()
			{
				InnerText = "1",
				X = 914,
				Y = 843,
				FontSize = 35,
				FontFamily = "Aria",
				Fill = "black",
			};
			Confirm = new()
			{
				InnerText = "Confirm",
				X = 1400,
				Y = 1010,
				FontSize = 60,
				FontFamily = "Aria",
				Fill = "white",
				OnClick = (args) =>
				{
					if (Convert.ToInt16(Number0.InnerText) == 1)
					{
						if (Convert.ToInt16(Number1.InnerText) == 9)
						{
							if (Convert.ToInt16(Number2.InnerText) == 6)
							{
								if (Convert.ToInt16(Number3.InnerText) == 8)
								{
									BackgroundImage = "minigame_assets/LockPick_assets/images/GeoeffneterLocker.png";
									Update();
									Finish(null,"BioVid");
                    
								}
								else
								{
									int x = Convert.ToInt16(TriesNumber.InnerText) - 1;
									if (x == 0)
									{
										rectLock.Kill();
										PunchNumber.Visible = true;
										RageNumber.Visible = true;
										SoundService.PlayMusic("audio/music/lock_smasher.wav");
									}
									if (x == -1)
									{
										x = 0;

									}
									TriesNumber.InnerText = x.ToString();
									Update();
								}
							}
							else
							{
								int x = Convert.ToInt16(TriesNumber.InnerText) - 1;
								if (x == 0)
								{
									rectLock.Kill();
									PunchNumber.Visible = true;
									RageNumber.Visible = true;
									SoundService.PlayMusic("audio/music/lock_smasher.wav");
								}
								if (x == -1)
								{
									x = 0;
								}
								TriesNumber.InnerText = x.ToString();
								Update();
							}
						}
						else
						{
							int x = Convert.ToInt16(TriesNumber.InnerText) - 1;
							if (x == 0)
							{
								rectLock.Kill();
								PunchNumber.Visible = true;
								RageNumber.Visible = true;
								SoundService.PlayMusic("audio/music/lock_smasher.wav");
							}
							if (x == -1)
							{
								x = 0;
							}
							TriesNumber.InnerText = x.ToString();
							Update();
						}
					}
					else
					{
						int x = Convert.ToInt16(TriesNumber.InnerText) - 1;
						if (x == 0)
						{
							rectLock.Kill();
							PunchNumber.Visible = true;
							RageNumber.Visible = true;
							SoundService.PlayMusic("audio/music/lock_smasher.wav");
						}
						if (x == -1)
						{
							x = 0;
						}
						TriesNumber.InnerText = x.ToString();
						Update();
					}
				}
			};
			TriesText = new()
			{
				InnerText = "Tries:",
				X = 40,
				Y = 90,
				FontSize = 60,
				FontFamily = "Aria",
				Fill = "white",
			};
			TriesNumber = new()
			{
				InnerText = "10",
				X = 175,
				Y = 90,
				FontSize = 60,
				FontFamily = "Aria",
				Fill = "white",
			};

			ragemode = new()
			{
				X = 631,
				Y = 425,
				Width = 420,
				Height = 550,
				Fill = "transparent",
				OnClick = (args) =>
				{
					if (Convert.ToInt16(TriesNumber.InnerText) == 0)
					{
						SoundService.PlaySound("minigame_assets/LockPick_assets/audio/punch.wav");
						int x = Convert.ToInt16(RageNumber.InnerText) + 1;
						RageNumber.InnerText = x.ToString();
						if (x == 80)
						{
							BackgroundImage = "minigame_assets/LockPick_assets/images/GeoeffneterLocker.png";
							SoundService.StopMusic();
							Update();
							Finish(null, "BioVid");
						}
						Update();
					}
				}
			};
			RageNumber = new()
			{
				InnerText = "0",
				X = 805,
				Y = 110,
				FontSize = 60,
				FontFamily = "Aria",
				Fill = "white",
			};
			PunchNumber.Visible = false;
			RageNumber.Visible = false;
		}
	}
}
