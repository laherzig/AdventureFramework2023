using System.Data;

namespace Framework.Minigames.MinigameDefClasses;

public class MinigameJacket : MinigameDefBase
{
	public override string BackgroundImage { get; set; } = "images/William.jpg";

	private Dialogue dialogue;

	private Image QuitButton { get; set; }

	GameObjectContainer<SVGElement> Bubble;

	int gamestate = 0;


	string WilliamFill = "transparent";
	string WilliamStroke = "transparent";

	public MinigameJacket()
	{
		{
			List<string> Values = [];
			List<List<string>> messages =
			[
				["William", "Brate, wo isch mini Jacke?"],
				["npc", "bring me something"],
				["player", "Hello"],
				["npc", "did you bring it?"],
			];

			dialogue = new Dialogue(messages);

			AddElement(
				new Rectangle()
				{
					Id = "Willy",
					X = 950,
					Y = 265,
					Width = 160,
					Height = 860,
					Fill = "transparent",
					Stroke = "transparent",
					StrokeWidth = 6,
					OnClick = (args) =>
					{
						Bubble = dialogue.DrawSpeechBubble(
							"Wiliam",
							"Finde meine Jacke und du wirst belohnt tmm",
							true
						);
						AddElementsInContainer(Bubble);
						QuitButton = dialogue.DrawQuitButton();
						Update();
					}
				}
			);

			AddElement(
				new Rectangle()
				{
					Id = "ToJacket",
					X = 50,
					Y = 65,
					Width = 120,
					Height = 100,
					Fill = "green",
					Stroke = "green",
					StrokeWidth = 6,
					OnClick = (args) =>
					{
						BackgroundImage = "/images/Jacke.jpg";
						WilliamFill = "pink";
						WilliamStroke = "pink";
						Update();
						gamestate = 1;
					}
				}
			);

			AddElement(
				new Rectangle()
				{
					Id = "WilliamsJacket",
					X = 570,
					Y = 365,
					Width = 140,
					Height = 600,
					Fill = "transparent",
					Stroke = "transparent",
					StrokeWidth = 6,
					OnClick = (args) =>
					{
						if (gamestate == 1)
						{
							GameState.AddItem("Jacket2");
						}
					}
				}
			);

			Update();

			AddElement(
				new Rectangle()
				{
					Id = "ToWilliam",
					X = 1350,
					Y = 65,
					Width = 120,
					Height = 100,
					Fill = "pink",
					Stroke = "pink",
					StrokeWidth = 6,
					OnClick = (args) =>
					{
						BackgroundImage = "/images/William.jpg";
						gamestate = 0;
						Update();
					}
				}
			);
		}
	}
}
