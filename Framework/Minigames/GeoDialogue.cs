namespace Framework.Minigames.MinigameDefClasses;

public class GeoDialogue : DialogueBase
{
	public override string BackgroundImage { get; set; } = "minigame_assets/Minigame_Geographie/Forum_7.jpg";
	public override string DefaultRoute { get; set; } = "Forum5";

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
		if (!GameState.GetState("Geo-Forum.Visited"))
		{
			ActiveMessages = messages;
		}
		else
		{
			ActiveMessages = GameState.GetState("Geo-Game.Complete") ? VictoryMessage : FailureMessage;
		}
		RunDialogue();
	}

	public override void BeforeExit()
	{
		if (GameState.GetState("Geo-Game.Complete") && GameState.GetState("Geo-Forum.Visited"))
		{
			// make dialogue unclickable
			GameState.SetState("Forum5.talk", false);
		}
		GameState.SetState("Geo-Forum.Visited", true);
	}
}
