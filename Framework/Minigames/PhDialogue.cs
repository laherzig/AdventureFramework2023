namespace Framework.Minigames.MinigameDefClasses;

public class PhDialogue : DialogueBase
{
	public override string BackgroundImage { get; set; } = "minigame_assets/PhMinigame_assets/Forum_5.jpg";
	public override string DefaultRoute { get; set; } = "Forum4";

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
		if (!GameState.GetState("Ph-Forum.Visited"))
		{
			ActiveMessages = messages;
		}
		else
		{
			ActiveMessages = GameState.GetState("Ph-Game.Complete") ? VictoryMessage : FailureMessage;
		}
		RunDialogue();
	}

	public override void BeforeExit()
	{
		if (GameState.GetState("Ph-Game.Complete") && GameState.GetState("Ph-Forum.Visited"))
		{
			// make dialogue unclickable
			GameState.SetState("Forum4.talk", false);
		}
		GameState.SetState("Ph-Forum.Visited", true);
	}
}
