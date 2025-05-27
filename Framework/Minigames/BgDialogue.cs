namespace Framework.Minigames.MinigameDefClasses;

public class BgDialogue : DialogueBase
{
	public override string BackgroundImage { get; set; } = "images/Routing/Forum/Forum_3.jpg";
	public override string DefaultRoute { get; set; } = "Forum3";

	readonly List<List<string>> messages = [
		["Du", "Ey"],
		["Du", "Habt ihr vielleicht gesehen wer meinen Laptop genommen hat?"],
		["Junge", "Hmmmm..."],
		["Junge", "Wenn du etwas für uns machst können wir dir einen Hinweis geben."],
		["Mädchen", "Ja genau. Im BG-Zimmer sind Leute eingebrochen."],
		["Mädchen", "Sie haben die Sachen aus dem Schrank im Zimmer ausgeräumt oder komisch verstellt."],
		["Junge", "Jetzt müssen wir alles wieder so einräumen, wie es vorher war. Du kannst uns aber jetzt bestimmt helfen."],
		["Mädchen", "Wir geben dir auch nachher den Hinweis für deinen Laptop."],
		["Junge", "Noch etwas. Es gibt ein Bild am Schrank. Das Bild zeigt dir wie der Schrank vorher aussah."],
		["Mädchen", "Ich rate dir aber zuerst die Unterschiede zu finden, dann kannst du ja alles einräumen."],
		["Du", "Gut, wenns sein muss."]
	];

	readonly List<List<string>> FailureMessage = [
		["Junge", "Im BG-Zimmer sind Leute eingebrochen und haben alles verstellt oder asugeräumt."],
		["Junge", "Wenn du alle Sachen wieder einräumst geben wir den Hinweis."],
		["Mädchen", "Finde aber zuerst die Unterschiede, dann kannst du ja alles einräumen."],
	];

	readonly List<List<string>> VictoryMessage = [
		["Du", "Ich habe alles aufgeräumt. Was wisst ihr?"],
		["Junge", "Gute Arbeit! Wir wissen nichts über deinen Laptop..."],
		["Mädchen", "Aber wir haben gehört, dass Hans, der Typ vor dem Forum, weiss, wo es einen magischen Red Bull gibt."],
		["Junge", "Es wird gemunkelt, dass es besondere Regenerationsfähigkeiten hat."],
		["Du", "Und?"],
		["Mädchen", "Vielleicht kann es dir in einem schwierigen Kampf helfen."],
		["Du", "(ಠ_ಠ)"],
	];
	public override void AfterInit()
	{
		BackButton.Visible = true;
		if (!GameState.GetState("BG-Forum.Visited"))
		{
			ActiveMessages = messages;
		}
		else
		{
			ActiveMessages = GameState.GetState("BG-Game.Complete") ? VictoryMessage : FailureMessage;
		}
		RunDialogue();
	}

	public override void BeforeExit()
	{
		if (GameState.GetState("BG-Game.Complete") && GameState.GetState("BG-Forum.Visited"))
		{
			// make dialogue unclickable
			GameState.SetState("Forum3.talk", false);
		}
		GameState.SetState("BG-Forum.Visited", true);
	}
}
