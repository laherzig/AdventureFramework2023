using Microsoft.AspNetCore.Components;

namespace Framework.Minigames;

public class DialogueBase : MinigameDefBase
{
	// public 
	public const int StandX = 560;
	public const int StandY = 780;
	public const int StandWidth = 500;
	public const int StandHeight = 300;
	public const float FontSize = 20f;
	public const string FontFamily = "Arial, sans-serif";


	public override string BackgroundImage { get; set; } = "";

	public List<List<string>> ActiveMessages { get; set; } = [];

	private int currentMessage = 0;



	public void RunDialogue()
	{
		Console.WriteLine($"Current message: {currentMessage}");
		Console.WriteLine($"Total messages: {ActiveMessages.Count}");
		if (currentMessage >= ActiveMessages.Count)
		{
			Finish(null, DefaultRoute);
			return;
		}
		List<string> message = ActiveMessages[currentMessage];
		currentMessage++;
		AddElement(DrawSpeechBubble(message[0], message[1]));
		Update();
	}

	public CustomObject DrawSpeechBubble(string speaker, string text, int x = StandX, int y = StandY, int width = StandWidth, int height = StandHeight)
	{
		CustomObject bubble = new()
		{
			CustomTagName = "svg",
		};
		bubble.Attributes["x"] = x;
		bubble.Attributes["y"] = y;
		bubble.Attributes["width"] = width;
		bubble.Attributes["height"] = height;
		bubble.Content = [
			new Rectangle()
			{
				Width = width,
				Height = height,
				Fill = "white",
			},
			new ForeignObject()
			{
				CustomObject = new CustomObject()
				{
					CustomTagName = "p",
					Content = [(MarkupString)$"<strong>{speaker}</strong>:<br>{text}"],
					Styles = new()
					{
						["font-size"] = $"{FontSize}px",
						["font-family"] = FontFamily,
					}
				},
				Attributes =
				{
					["width"] = "98%",
					["height"] = "98%",
				}
			},
			new Rectangle()
			{
				Width = width,
				Height = height,
				Fill = "transparent",
				OnClick = (e) =>
				{
					RunDialogue();
					Elements.KillId(bubble.Id);
				}
			}
		];
		return bubble;
	}

	// public DialogueBase()
	// {
	// 	BackButton.Visible = true;
	// 	RunDialogue();
	// }
}