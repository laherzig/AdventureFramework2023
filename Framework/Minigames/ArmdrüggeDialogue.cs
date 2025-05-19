namespace Framework.Minigames.MinigameDefClasses;

public class ArmdruggeDialogueStart : DialogueBase
{
	readonly List<List<string>> StartMessages = [
		["Du", "Entschuldigung, wisst ihr etw-"],
		["Rüpel", "Was denkst du, wer du bist!?!"],
		["Du", "Was?"],
		["Rüpel", "Weisst du überhaupt, mit wem du hier sprichst?"],
		["Rüpel", "Wenn du etwas von uns willst, musst du dich zuerst beweisen"],
		["Du", "Was muss ich machen?"],
		["Rüpel", "Du musst den Chef im Armdrücken besiegen."],
		["Rüpel", "Aber mach dich auf was gefasst, der Chef ist dreifacher Weltmeister im Armdrücken."],
		["Rüpel", "Mach dich bereit, der Chef hat nicht ewig Zeit."],
		["Du", "Ich werde mein Bestes geben!"],
		["Chef", "..."],
	];

	readonly List<List<string>> RetryMessages = [
		["Rüpel", "Schon zurück? Das ging ja schnell!"],
		["Du", "Ich würde es gerne erneut versuchen."],
		["Chef", "Du hast Mumm."],
		["Rüpel", "Zeig ihm, wo der Hammer hängt, Chef!."],
	];

	public override string DefaultRoute { get; set; } = "Hall2";
	public override string? ForwardRoute { get; set; } = "ArmdruggeMinigame";
	public override string BackgroundImage { get; set; } = "images/Routing/Haupthall/Answer.png";

	public override void BeforeExit()
	{
		GameState.SetState("Armdrugge.Tried", true);
	}

	public override void AfterInit()
	{
		ActiveMessages = GameState.GetState("Armdrugge.Tried") ? RetryMessages : StartMessages;
		BackButton.Visible = true;
		RunDialogue();
	}
}

public class ArmdruggeDialogueWin : DialogueBase
{
	readonly List<List<string>> WinMessages = [
		["Chef", "Was?? Wie kann das sein?"],
		["Rüpel", "Er hat es tatsächlich geschafft!"],
		["Du", "Puh, das war schwieriger als gedacht."],
		["Chef", "Du hast mich fair geschlagen"],
		["Chef", "Du hast dir unseren Respekt verdient."],
		["Du", "Ich wollte euch fraen, ob ihr meinen Laptop gesehen habt."],
		["Du", "Er ist während der Zwischenpause verschwunden!."],
		["Chef", "Hinweis"],
	];

	public override string DefaultRoute { get; set; } = "Hall2";
	public override string BackgroundImage { get; set; } = "images/Routing/Haupthall/Answer.png";

	public ArmdruggeDialogueWin()
	{
		ActiveMessages = WinMessages;
		BackButton.Visible = true;
		RunDialogue();
	}

	public override void AfterInit()
	{
		GameState.SetState("Hall2.polygon", false);
	}
}

public class ArmdruggeDialogueLoose : DialogueBase
{
	readonly List<List<string>> LooseMessages = [
		["Du", "Verdammt! Der Chef ist ja wirklich viel zu stark."],
		["Chef", "Du verschwendest meine Zeit."],
		["Rüpel", "Du hast den Chef gehört."],
		["Rüpel", "Komm zurück, wenn du bereit bist."],
	];

	public override string DefaultRoute { get; set; } = "Hall2";
	public override string BackgroundImage { get; set; } = "images/Routing/Haupthall/Answer.png";

	public ArmdruggeDialogueLoose()
	{
		ActiveMessages = LooseMessages;
		BackButton.Visible = true;
		RunDialogue();
	}
}


public class Test1 : MinigameDefBase
{
	public override string BackgroundImage { get; set; } = "images/Routing/Haupthall/Answer.png";

	public Test1()
	{
		AddElement(new Rectangle()
		{
			X = 0,
			Y = 0,
			Width = 100,
			Height = 100,
			Fill = "red",
			OnClick = (args) =>
			{
				Finish(null, "Test2");
			}
		});
	}
}
public class Test2 : MinigameDefBase
{
	public override string BackgroundImage { get; set; } = "images/Routing/Haupthall/Answer.png";

	public Test2()
	{
		AddElement(new Rectangle()
		{
			X = 0,
			Y = 0,
			Width = 100,
			Height = 100,
			Fill = "yellow",
			OnClick = (args) =>
			{
				Finish(null, "Test1");
			}
		});
	}
}

