namespace Framework.Minigames.MinigameDefClasses;

public class BioDialogue : DialogueBase
{
	public override string BackgroundImage { get; set; } = "minigame_assets/Biology_assets/Forum_9.jpg";
	public override string DefaultRoute { get; set; } = "Forum2";

	readonly List<List<string>> messages = [
		["Du", "Hallo! Könnt ihr mir sagen wo mein Laptop ist?"],
		["Mauzi", "Vielleicht... wenn du uns helfen kannst :3"],
		["Du", "Was gibts?"],
		["Bautzi", "Wir sind richtig schlecht in Bio, kannst du uns helfen?"],
		["Du", "ok..."],
	];
	readonly List<List<string>> VictoryMessage = [
		["Du", "Viel zu einfach"],
		["Mauzi", "OMG danke!"],
		["Bautzi", "Endlich kann ich meinen Eltern eine Note zeigen"],
		["Du", "Nun raus mit der Sprache, helft mir jetzt!"],
		["Mautzi", ""],
	];
	readonly List<List<string>> FailureMessage = [
		["Du", "Noch nicht fertig, sorry"],
		["Mauzi", "Dann flieg ich halt raus"],
		["Bautzi", "L"],
	];

	public override void AfterInit()
	{
		BackButton.Visible = true;
		if (!GameState.GetState("Bio-Forum.Visited"))
		{
			ActiveMessages = messages;
		}
		else
		{
			ActiveMessages = GameState.GetState("Bio-Game.Complete") ? VictoryMessage : FailureMessage;
		}
		RunDialogue();
	}

	public override void BeforeExit()
	{
		if (GameState.GetState("Bio-Game.Complete") && GameState.GetState("Bio-Forum.Visited"))
		{
			// make dialogue unclickable
			GameState.SetState("Forum2.talk", false);
		}
		GameState.SetState("Bio-Forum.Visited", true);
	}
}