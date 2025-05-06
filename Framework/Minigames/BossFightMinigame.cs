using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using Framework.Sound;
using Framework.Video;
using Framework.Game;
using Framework.InventoryUI;
using Framework.Slides;

namespace Framework.Minigames.MinigameDefClasses;

public class BossFightMinigame : MinigameDefBase
{
	public override string BackgroundImage { get; set; } = "minigame_assets/BossFight/FightPoitionToPunch_2.png";


	public Text? Infotext { get; set; }
	public Text? Infotext2 { get; set; }
	public Text? Infotext3 { get; set; }
	public Text? Infotext4 { get; set; }
	public Text? Infotext5 { get; set; }
	public Text? Infotext6 { get; set; }
	public Rectangle? AttackButton1 { get; set; }
	public Rectangle? AttackButton2 { get; set; }
	public Rectangle? StatusButton { get; set; }
	public Rectangle? HealButton { get; set; }

	[Element]
	public Image? RedBull { get; set; }
	[Element]
	public Image? MenuBar { get; set; }
	[Element]
	public Rectangle? HealthBar { get; set; }
	[Element]
	public Rectangle? Villan_HealthBar { get; set; }
	[Element]
	public Rectangle? Blend { get; set; }
	[Element]
	public Rectangle? Blend2 { get; set; }
	[Element]
	public Rectangle? Blend3 { get; set; }
	[Element]
	public Text? PlayerHPText { get; set; }
	[Element]
	public Text? VillanHPText { get; set; }
	public GameObjectContainer<Rectangle> buttons { get; set; } = new();
	public GameObjectContainer<Rectangle> moving_rects { get; set; } = new();
	public GameObjectContainer<Rectangle> decoration { get; set; } = new();

	public GameObjectContainer<Circle> buttons1 { get; set; } = new();
	public GameObjectContainer<Circle> moving_rects1 { get; set; } = new();
	public GameObjectContainer<Circle> decoration1 { get; set; } = new();

	public GameObjectContainer<Text> buttons2 { get; set; } = new();
	public GameObjectContainer<Text> moving_rects2 { get; set; } = new();
	public GameObjectContainer<Text> decoration2 { get; set; } = new();

	public int VillanHealth = 300;
	public int VillanHealth2 = 300;
	public int PlayerHealth = 100;
	public int PlayerHealth2 = 100;
	public bool TaskComplete = false;
	public int AttackBuff = 1;
	public int HitPropability = 0;
	public int CritPropability = 13;
	public int Critmultiplier = 1;

	//Falls Code hängen bleibt bitte ALLES schliessen und über powershell neu öffnen. Framework überlastet?
	public override async Task GameLoop(CancellationToken ct)
	{
		while (true)
		{
			ct.ThrowIfCancellationRequested();
			await Task.Delay(100, ct);
			await Task.Delay(2000);
			await VillanAttack();
			Update();
			await Task.Delay(2000);
			if (PlayerHealth <= 0)
			{
				Finish(null, "Bad"); //!!!!!!bitte SlideID von Abspann einfügen
			}
			else if (VillanHealth <= 0)
			{
				Finish(null, "Good"); ////!!!!!!!!bitte SlideID von Abspann einfügen
			}
		}

	}
	public async void Villan_Health_Bar()
	{
		while (VillanHealth2 > VillanHealth)    //VilHealth2 eiert Original mit 5ms delay hinterher, für Animation
		{
			VillanHealth2 -= 1;
			Villan_HealthBar.Width = VillanHealth2 * 2;
			Update();
			await Task.Delay(5);
		}
		while (VillanHealth2 < VillanHealth)
		{
			VillanHealth2 += 1;
			Villan_HealthBar.Width = VillanHealth2 * 2;
			Update();
			await Task.Delay(5);
		}

		Update();
		if (VillanHealth < 0)
		{
			VillanHealth = 0;
			Villan_HealthBar.Width = 0;
		}

	}
	async public void Health_Bar()
	{
		while (PlayerHealth2 > PlayerHealth)
		{
			PlayerHealth2 -= 1;
			HealthBar.Width = PlayerHealth2 * 7 / 2;
			Update();
			await Task.Delay(5);
		}
		Update();

		while (PlayerHealth2 < PlayerHealth)
		{
			PlayerHealth2 += 1;
			HealthBar.Width = PlayerHealth2 * 7 / 2;
			Update();
			await Task.Delay(5);
		}
		Update();
		switch (PlayerHealth)
		{
			case < 0:
				PlayerHealth = 0;
				HealthBar.Width = 0;
				break;
			case <= 25:
				HealthBar.Fill = "Red";  // Covers 0 to 25
				break;
			case <= 50:
				HealthBar.Fill = "Yellow";  // Covers 26 to 50
				break;
			default:
				HealthBar.Fill = "Green";
				//healthbar grün
				break;
		}


	}
	public async Task PlayAudio(string path)//Audio aus DOC
	{
		await SoundService.PlaySound(path);
	}
	async public void Healing()
	{
		//* Edit by laurin made here
		// <original code>
		// if (GameState.CheckForItem("RedBull"))//Item mit ID "RedBull" muss in den GameState
		// </original code>
		// <edited by laurin>
		if (GameState.RedBullCount > 0)
		// </edited by laurin>
		{
			BackgroundImage = "minigame_assets/BossFight/RedbullFromBoss_2.png";
			Update();
			await PlayAudio("minigame_assets/BossFight/drink.wav");
			await Task.Delay(1000);
			BackgroundImage = "minigame_assets/BossFight/DrinkRedbull.png";
			Update();
			await Task.Delay(1500);
			BackgroundImage = "minigame_assets/BossFight/FightPoitionToPunch_2.png";
			PlayerHealth = PlayerHealth + 30;
			if (PlayerHealth > 100)
			{
				PlayerHealth = 100;
			}
			Health_Bar();
			//* Edit by laurin made here
			// <original code>
			// GameState.RemoveItem("RedBull"); //Wir hoffen nur ein RedBull wird hier entfernt
			// </original code>
			// <edited by laurin>
			//GameState.ChangeRedBull(-1);
			// </edited by laurin>
			Update();
			await Task.Delay(1000);
			TaskComplete = true;
		}
		else
		{
			TaskComplete = true;
			RedBull.X = 200;
			await Task.Delay(750);
			RedBull.X = 10000;
		}
	}
	async public Task VillanAttack()
	{
		if (TaskComplete == true)
		{
			var rand = new Random();

			int picture = rand.Next(0, 1);
			if (picture == 0)
			{
				BackgroundImage = "minigame_assets/BossFight/KickFromBoss.png";
			}
			else if (picture == 1)
			{
				BackgroundImage = "minigame_assets/BossFight/PunchFromBoss_1.png";
			}
			await PlayAudio("minigame_assets/BossFight/punch.wav");
			Update();
			await Task.Delay(250);
			PlayerHealth -= rand.Next(10, 16);
			Health_Bar();
			Update();
			await Task.Delay(1500);
			BackgroundImage = "minigame_assets/BossFight/FightPoitionToPunch_2.png";
			Infotext4.Fill = "black";
			Infotext.Fill = "black";
			Infotext2.Fill = "black";
			Infotext3.Fill = "black";
			Update();
			TaskComplete = false;
		}




	}

	async public void Attack_1()
	{

		var rand = new Random();
		HitPropability = rand.Next(1, 11);
		CritPropability = rand.Next(1, 26);
		if (HitPropability < 9)
		{
			if (CritPropability == 13)
			{
				Critmultiplier += 1;
			}
			await PlayAudio("minigame_assets/BossFight/punch.wav");
			BackgroundImage = "minigame_assets/BossFight/Right_Punch.png";
			Update();
			VillanHealth = VillanHealth - 9 * AttackBuff * Critmultiplier;
			Villan_Health_Bar();
			Critmultiplier = 1;
			Update();
			await Task.Delay(2000);
			BackgroundImage = "minigame_assets/BossFight/FightPoitionToPunch_2.png";
		}
		else
		{
			Infotext6.Opacity = 1;
			Infotext6.ZIndex = 1000000;
			await Task.Delay(500);
			Infotext6.Opacity = 0;
			Infotext6.ZIndex = -1;
		}
		AttackBuff = 1;
		TaskComplete = true;
	}

	async public void Attack_2()
	{

		var rand = new Random();
		HitPropability = rand.Next(1, 11);
		if (HitPropability < 4)
		{
			await PlayAudio("minigame_assets/BossFight/punch.wav");
			BackgroundImage = "minigame_assets/BossFight/LeftKickAndRightPunch.png";
			Update();
			VillanHealth = VillanHealth - 19 * AttackBuff;
			Villan_Health_Bar();
			await Task.Delay(2000);
			BackgroundImage = "minigame_assets/BossFight/FightPoitionToPunch_2.png";
			Update();
		}
		else
		{
			Infotext6.Opacity = 1;
			Infotext6.ZIndex = 6;
			await Task.Delay(100);
			Infotext6.Opacity = 0;
			Infotext6.ZIndex = -1;
		}
		AttackBuff = 1;
		TaskComplete = true;
	}

	async public void Status_Attack()
	{
		var rand = new Random();
		HitPropability = rand.Next(1, 11);
		if (HitPropability < 10)
		{
			await PlayAudio("minigame_assets/BossFight/push.wav");
			BackgroundImage = "minigame_assets/BossFight/PushBoss.png";
			Update();
			await Task.Delay(2000);
			AttackBuff += 1 + 1 / 5;
			BackgroundImage = "minigame_assets/BossFight/FightPoitionToPunch_2.png";
			Update();
			Villan_Health_Bar();
			Update();
		}
		else
		{
			Infotext6.Opacity = 1;
			Infotext6.ZIndex = 6;
			await Task.Delay(100);
			Infotext6.Opacity = 0;
			Infotext6.ZIndex = -1;
		}
		TaskComplete = true;

	}
	public BossFightMinigame()
	{

		AttackButton1 = new()
		{
			Id = "2ADSFG",
			X = 450,
			Y = 880,
			ZIndex = 2,
			Width = 300,
			Height = 75,
			Fill = "lightgrey",
			FillOpacity = 0,
			OnMouseEnter = async (args) =>
			{
				AttackButton1.FillOpacity = 0.3; //Anmerkung: Opacity funktioniert nicht wie es sollte, es ist entweder unsichtbar oder deckend
				Infotext5.InnerText = "Dmg: 9" +
								"  ACC: 80%";
				Infotext5.Opacity = 1;
				Update();
			},
			OnMouseLeave = async (args) =>
			{
				AttackButton1.FillOpacity = 0;
				Infotext5.Opacity = 0;
				Update();
			},
			OnClick = async (args) =>
			{
				if (TaskComplete == false)
				{
					TaskComplete = true;
					Infotext4.Fill = "grey";
					Infotext.Fill = "grey";
					Infotext2.Fill = "grey";
					Infotext3.Fill = "grey";
					AttackButton1.Y += 5;
					Update();
					await Task.Delay(50);
					AttackButton1.Y -= 5;
					Update();
					Attack_1();
					Update();
				}
				else if (TaskComplete == true)
				{
					Infotext4.Fill = "grey";
					Infotext.Fill = "grey";
					Infotext2.Fill = "grey";
					Infotext3.Fill = "grey";
					Update();
				}

			}
		};
		moving_rects.Add(AttackButton1);
		AddElement(AttackButton1);



		AttackButton2 = new()
		{
			Id = "rectbutton55",
			X = 850,
			Y = 880,
			ZIndex = 2,
			Width = 300,
			Height = 75,
			Fill = "lightgrey",
			FillOpacity = 0,
			OnMouseEnter = async (args) =>
			{
				AttackButton2.FillOpacity = 0.3;
				Infotext5.InnerText = "Dmg: 20" +
								"  ACC: 30%";
				Infotext5.Opacity = 1;
				Update();
			},
			OnMouseLeave = async (args) =>
			{
				AttackButton2.FillOpacity = 0;
				Infotext5.Opacity = 0;
				Update();
			},
			OnClick = async (args) =>
			{
				if (TaskComplete == false)
				{
					TaskComplete = true;
					Infotext4.Fill = "grey";
					Infotext.Fill = "grey";
					Infotext2.Fill = "grey";
					Infotext3.Fill = "grey";
					AttackButton2.Y += 5;
					Update();
					await Task.Delay(50);
					AttackButton2.Y -= 5;
					Update();
					Attack_2();
					Update();
				}
				else if (TaskComplete == true)
				{
					Infotext4.Fill = "grey";
					Infotext.Fill = "grey";
					Infotext2.Fill = "grey";
					Infotext3.Fill = "grey";
					Update();
				}
			}
		};


		moving_rects.Add(AttackButton2);
		AddElement(AttackButton2);



		StatusButton = new()
		{
			Id = "statusrect2345151",
			X = 450,
			Y = 980,
			Width = 300,
			Height = 75,
			ZIndex = 2,
			Fill = "lightgrey",
			FillOpacity = 0,
			OnMouseEnter = async (args) =>
			{
				StatusButton.FillOpacity = 0.3;
				Infotext5.InnerText = "MUL: 1.2" +
								"  ACC: 90%";
				Infotext5.Opacity = 1;
				Update();
			},
			OnMouseLeave = async (args) =>
			{
				StatusButton.FillOpacity = 0;
				Infotext5.Opacity = 0;
				Update();
			},
			OnClick = async (args) =>
			{
				if (TaskComplete == false)
				{
					TaskComplete = true;
					Infotext4.Fill = "grey";
					Infotext.Fill = "grey";
					Infotext2.Fill = "grey";
					Infotext3.Fill = "grey";
					StatusButton.Y += 5;
					Update();
					await Task.Delay(50);
					StatusButton.Y -= 5;
					Update();
					Status_Attack();
					Update();
				}
				else if (TaskComplete == true)
				{
					Infotext4.Fill = "grey";
					Infotext.Fill = "grey";
					Infotext2.Fill = "grey";
					Infotext3.Fill = "grey";
					Update();
				}
			}
		};

		moving_rects.Add(StatusButton);
		AddElement(StatusButton);

		Infotext = new()
		{
			Id = "23z75425647563341525235234323423445654793",
			ContentMode = false,
			InnerText = "Push",
			X = 535,
			Y = 1035,
			ZIndex = 1,
			Fill = "black",
			Rotate = 0,
			FontSize = 50,
			StretchLetters = false,
			FontFamily = "Goudy Bookletter 1911",

		};
		AddElement(Infotext);
		Update();

		Infotext2 = new()
		{
			Id = "23z7544566723235235235345121534552793",
			ContentMode = false,
			InnerText = "RedBull",
			X = 920,
			Y = 1035,
			ZIndex = 1,
			Fill = "black",
			Rotate = 0,

			FontSize = 50,
			StretchLetters = false,
			FontFamily = "Goudy Bookletter 1911",
		};
		AddElement(Infotext2);
		Update();

		Infotext3 = new()
		{
			Id = "22352533z7542512412423423145431256z5472793",
			ContentMode = false,
			InnerText = "Punch",
			X = 475,
			Y = 835,
			DX = 50,
			DY = 100,
			ZIndex = 1,
			Fill = "black",
			Rotate = 0,

			FontSize = 50,
			StretchLetters = false,
			FontFamily = "Goudy Bookletter 1911",
		};
		AddElement(Infotext3);
		Update();

		Infotext4 = new()
		{
			Id = "143152352125t2362451413z75434557656442793",
			ContentMode = false,
			InnerText = "Kick",
			X = 900,
			Y = 835,
			DX = 50,
			DY = 100,
			ZIndex = 1,
			Fill = "black",
			Rotate = 0,

			FontSize = 50,
			StretchLetters = false,
			FontFamily = "Goudy Bookletter 1911",
		};
		AddElement(Infotext4);
		Update();


		HealButton = new()
		{
			Id = "healrect777151",
			X = 850,
			Y = 980,
			ZIndex = 2,
			Width = 300,
			Height = 75,
			Fill = "lightgrey",
			FillOpacity = 0,
			OnMouseEnter = async (args) =>
			{
				HealButton.FillOpacity = 0.3;
				Infotext5.InnerText = "VIG: +30" +
								"  REQ: RB";
				Infotext5.Opacity = 1;
				Update();
			},
			OnMouseLeave = async (args) =>
			{
				HealButton.FillOpacity = 0;
				Infotext5.Opacity = 0;
				Update();
			},
			OnClick = async (args) =>
			{
				if (TaskComplete == false)
				{
					TaskComplete = true;
					Infotext4.Fill = "grey";
					Infotext.Fill = "grey";
					Infotext2.Fill = "grey";
					Infotext3.Fill = "grey";

					HealButton.Y += 5;
					Update();
					await Task.Delay(50);
					HealButton.Y -= 5;
					Update();
					Healing();
					Update();

				}
				else if (TaskComplete == true)
				{
					Infotext4.Fill = "grey";
					Infotext.Fill = "grey";
					Infotext2.Fill = "grey";
					Infotext3.Fill = "grey";
					Update();
				}
			}
		};

		moving_rects.Add(HealButton);
		AddElement(HealButton);
		HealthBar = new()
		{
			Id = "heatlthbar4252515",
			X = 50,
			Y = 750,
			Width = PlayerHealth * 7 / 2,
			Height = 35,
			Fill = "green",
			ZIndex = 1
		};

		Villan_HealthBar = new()
		{
			Id = "Villanheatlthbar4252564642515",
			X = 950,
			Y = 100,
			Width = VillanHealth * 2,
			Height = 50,
			Fill = "red"
		};

		MenuBar = new()
		{
			Id = "menu4526146276",
			X = 400,
			Y = 770,
			ZIndex = -1,
			Width = 1200,
			Height = 400,
			ImagePath = "minigame_assets/BossFight/Menu.png"
		};
		RedBull = new()
		{
			Id = "redbull46276",
			X = 10000,
			Y = 370,
			ZIndex = -1,
			Width = 1200,
			Height = 400,
			ImagePath = "minigame_assets/BossFight/no_redbull.png"

		};
		Blend = new()
		{
			Id = "beldninidn",
			X = 1230,
			Y = 880,
			Width = 335,
			Height = 175,
			ZIndex = 2,
			Fill = "white"
		};
		Blend2 = new()
		{
			Id = "b23525n",
			X = 40,
			Y = 695,
			Width = 370,
			Height = 100,
			ZIndex = -1,
			Fill = "white"
		};
		Blend3 = new()
		{
			Id = "b223gerg5n",
			X = 935,
			Y = 50,
			Width = 630,
			Height = 120,
			ZIndex = -1,
			Fill = "white"
		};
		PlayerHPText = new()
		{
			Id = "dqefwei3319",
			ContentMode = false,
			InnerText = "Player",
			X = 100,
			Y = 735,
			ZIndex = 1,
			Fill = "black",
			Rotate = 0,

			FontSize = 50,
			StretchLetters = false,
			FontFamily = "Goudy Bookletter 1911",
		};
		VillanHPText = new()
		{
			Id = "dqefw2341512r2",
			ContentMode = false,
			InnerText = "Ultimate End Boss",
			X = 950,
			Y = 90,
			ZIndex = 1,
			Fill = "black",
			Rotate = 0,

			FontSize = 50,
			StretchLetters = false,
			FontFamily = "Goudy Bookletter 1911",
		};
		Infotext5 = new()
		{
			Id = "23z4546354534t27354542793",
			ContentMode = false,
			Opacity = 0,
			InnerText = "Dmg: 5" +
								"  ACC: 80%",
			X = 1250,
			Y = 1000,
			ZIndex = 5,
			Fill = "black",
			FontSize = 40,
			StretchLetters = false,
			FontFamily = "Goudy Bookletter 1911",
		};
		Update();
		AddElement(Infotext5);

		Infotext6 = new()
		{
			Id = "23z4546354534t2735452356254523142793",
			ContentMode = false,
			Opacity = 0,
			InnerText = "Miss",
			X = 600,
			Y = 400,
			ZIndex = 0,
			Fill = "Red",
			FontSize = 200,
			StretchLetters = false,
			FontFamily = "Goudy Bookletter 1911",
		};
		Update();
		AddElement(Infotext6);
	}
}
