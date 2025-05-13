using Microsoft.AspNetCore.Components;

namespace Framework.Minigames;

public class DialogueBase : MinigameDefBase
{
	// public 
	public const int StandX = 410;
	public const int StandY = 830;
	public const int StandWidth = 800;
	public const int StandHeight = 200;
	public const float FontSize = 30f;
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
		AddElement(MakeSpeechBubble(message[0], message[1]));
		Update();
	}

	public CustomObject MakeSpeechBubble(string speaker, string text)
	{
		CustomObject bubble = new()
		{
			CustomTagName = "svg",
		};
		bubble.Attributes["x"] = StandX;
		bubble.Attributes["y"] = StandY;
		bubble.Attributes["width"] = StandWidth;
		bubble.Attributes["height"] = StandHeight;
		bubble.Content = [
			(MarkupString)$"""
			<rect width="100%" height="100%" fill="rgba(0,0,0,0.7)" rx="20" />
			<text 
				font-size="{FontSize}" 
				font-family="{FontFamily}" 
				font-weight="bold" 
				fill="white" 
				x="50%" 
				y="25%" 
				text-anchor="middle" >
					{speaker}
				</text>
			<foreignObject width="90%" height="60%" x="5%" y="30%">
				<p style="color: rgba(255,255,255,0.8); font-size: {FontSize}px; font-family: {FontFamily};">
					{text}
				<p>
			<foreignObject>
			""",
			new Rectangle()
			{
				Width = StandWidth,
				Height = StandHeight,
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

	[Obsolete("Use MakeSpeechBubble instead")]
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