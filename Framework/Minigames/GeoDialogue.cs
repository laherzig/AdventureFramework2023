namespace Framework.Minigames.MinigameDefClasses;

public class GeoDialogue : DialogueBase
{
	public override string BackgroundImage { get; set; } = "minigame_assets/Minigame_Geographie/Forum_7.jpg";
	public override string DefaultRoute { get; set; } = "Forum5";

	readonly List<List<string>> messages = [
		["Du", "Hey, hast du meinen Laptop gesehen? Es ist ein graues Chromebook."],
		["Anthony", "Oh, natürlich... dein einzigartiges graues Chromebook"],
		["Du", "Komm schon, hilf mir!"],
		["Anthony", "Ok, aber zuerst musst du mein Quiz im Geographieraum bestehen."],
		["Du", "Wirklich? Das macht doch keinen Sinn!"],
		["Anthony", "Dein Laptop, meine Regeln."],
	];
	readonly List<List<string>> VictoryMessage = [
		["Du", "Ich bin fertig mit dir dumme Aufgabe! Wo ist mein Laptop?"],
		["Anthony", "Keine Ahnung, Kumpel..."],
		["Du", "Ich werde di..."],
		["Anthony", "Ruhig, ruhig, das war ein Witz."],
		["Anthony", "Ich habe einen verdächtigen Typ mit einem Laptop gesehen, der am Geographieraum vorbeiging."],
	];
	readonly List<List<string>> FailureMessage = [
		["Anthony", "Du bist nicht so schlau, so schnell fertig zu sein."],
		["Du", "EiNaTmEn, AuSaTmEn..."],
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
