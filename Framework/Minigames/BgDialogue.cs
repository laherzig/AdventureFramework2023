namespace Framework.Minigames.MinigameDefClasses;

public class BgDialogue : DialogueBase
{
	public override string BackgroundImage { get; set; } = "images/Routing/Forum/Forum_3.jpg";
	public override string DefaultRoute { get; set; } = "Forum3";

	readonly List<List<string>> Messages1 = [
		["Du", "Ey"],
		["Du", "Habt ihr vielleicht gesehen wer meinen Labtop genommen hat?"],
		["Junge", "Hmmmm..."],
		["Junge", "Wenn du etwas für uns machst können wir dir einen Hinweis geben."],
		["Mädchen", "Ja genau. Im BG-Zimmer sind Leute eingebrochen."],
		["Mädchen", "Sie haben die Sachen aus dem Schrank im Zimmer ausgeräumt oder komisch verstellt."],
		["Junge", "Jetzt müssen wir alles wieder so einräumen, wie es vorher war. Du kannst uns aber jetzt bestimmt helfen."],
		["Mädchen", "Wir geben dir auch nachher den Hinweis für deinen Labtop"],
		["Junge", "Noch etwas. Es gibt ein Bild am Schrank. Das Bild zeigt dir wie der Schrank vorher aussah."],
		["Mädchen", "Ich rate dir aber zuerst die Unterschiede zu finden, dann kannst du ja alles einräumen"],
		["Du", "Gut, wenns sein muss."]
	];

	readonly List<List<string>> Messages2 = [
		["Junge", "Im BG-Zimmer sind Leute eingebrochen und haben alles verstellt oder asugeräumt"],
		["Junge", "Wenn du alle Sachen wieder einräumst geben wir den Hinweis"],
		["Mädchen", "Finde aber zuerst die Unterschiede, dann kannst du ja alles einräumen."],
	];

	public override void AfterInit()
	{
		BackButton.Visible = true;
		ActiveMessages = !GameState.GetState("BG-Forum.Visited") ? Messages1 : Messages2;
		RunDialogue();
	}

	public override void BeforeExit()
	{
		GameState.SetState("BG-Forum.Visited", true);
	}
}
