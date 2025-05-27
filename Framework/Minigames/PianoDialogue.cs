namespace Framework.Minigames.MinigameDefClasses;

public class PianoDialogue : DialogueBase
{
	public override string BackgroundImage { get; set; } = "images/Routing/GangR/RGang_2.3.1.jpg";
	public override string DefaultRoute { get; set; } = "GangR2.3";

	readonly List<List<string>> messages = [
		["Du", "Hallo! Weisst du wo mein Laptop ist?"],
		["Hans", "Nein, aber ich habe etwas viel Interessanteres..."],
		["Du", "Ich werde die Polizei rufen."],
		["Hans", "NEIN, du Idiot!!! "],
		["Hans", "Es gibt ein Gerücht über ein Portal in eine andere Dimension in unserer Schule."],
		["Hans", "Es ist hinter einer Tür mit einem Löwensymbol versteckt."],
		["Hans", "Ich vermute, es ist keine normale Tür."],
		["Du", "Warum erzählst du mir das?"],
		["Hans", "."],
		["Hans", ".."],
		["Hans", "..."],
		["Hans", "Weil du wie ein Verrückter aussiehst..."],
		["Du", "Daaamn..."],
	];
	readonly List<List<string>> VictoryMessage = [
		["Du", "Ich habe es gefunden!"],
		["Hans", "Was?"],
		["Du", "Ein Zimmer"],
		["Du", "Das Portal, von dem du gesprochen hast."],
		["Hans", "Bist du irre? Geh weg!"],
	];
	readonly List<List<string>> FailureMessage = [
		["Du", "Was war das noch mal?"],
		["Hans", "Tshh, leise!!!!!"], 
		["Hans", "Irgendwo in der Schule gibt es eine Tür mit dem Löwensymbol."],
		["Hans", "Man munkelt, dass es ein Portal ist."],
		["Du", "Verstanden, danke nochmals"],
	];

	public override void AfterInit()
	{
		BackButton.Visible = true;
		if (!GameState.GetState("Music-1.Visited"))
		{
			ActiveMessages = messages;
		}
		else
		{
			ActiveMessages = GameState.GetState("Music-Red.Complete") ? VictoryMessage : FailureMessage;
		}
		RunDialogue();
	}

	public override void BeforeExit()
	{
		if (GameState.GetState("Music-Red.Complete") && GameState.GetState("Music-1.Visited"))
		{
			// make dialogue unclickable
			GameState.SetState("GangR2.3.talk", false);
		}
		GameState.SetState("Music-1.Visited", true);
	}
}
