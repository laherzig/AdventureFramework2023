namespace Framework.Minigames.MinigameDefClasses;

public class PhDialogue : DialogueBase
{
	public override string BackgroundImage { get; set; } = "minigame_assets/PhMinigame_assets/Forum_5.jpg";
	public override string DefaultRoute { get; set; } = "Forum4";

	readonly List<List<string>> messages = [
		["Du", "Hallo! Hast du vielleicht gesehen, was mit meinem Laptop passiert ist?"],
		["Giovanni", "Ciao! Nicht wirklich, aber ich habe einige Informationen zu teilen..."],
		["Giovanni", "Natürlich nicht umsonst."],
        ["Du", "Naaaatürlich... Ok!"],
		["Giovanni", "Wenn du mir bei meinem Projekt im Physiklabor hilfst, werde ich dir sagen, was ich weiss."],
        ["Du", "So gut wie erledigt!"]
	];
	readonly List<List<string>> VictoryMessage = [
		["Du", "Ein paar nervige Pieptöne und ich bin fertig!"],
		["Giovanni", "Grazie!"],
		["Giovanni", "Ich halte mein Versprechen. Hier ist, was ich gehört habe..."],
		["Du", "Endlich..."],
		["Giovanni", "Jemand hat den Schlüssel von der Kellertür auf der Skulptur neben der Tür vergessen."],
		["Du", "Wie soll mir das helfen?"],
		["Giovanni", "┐｜･ิω･ิ#｜┌"]
	];
	readonly List<List<string>> VictoryMessageAlt = [
		["Du", "Ein paar nervige Pieptöne und ich bin fertig!"],
		["Giovanni", "Grazie!"],
		["Giovanni", "Ich halte mein Versprechen. Hier ist, was ich gehört habe..."],
		["Du", "Endlich..."],
		["Giovanni", "Jemand hat den Schlüssel von der Kellertür auf der Skulptur neben der Tür vergessen."],
		["Du", "Ich hab ihn schon! Wie soll mir das helfen?"],
		["Giovanni", "┐｜･ิω･ิ#｜┌"]
	];
	readonly List<List<string>> FailureMessage = [
		["Du", "Noch nicht fertig"],
		["Giovanni", "Mamma mia!"]
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
            if (GameState.GetState("Ph-Game.Complete"))
            {
                ActiveMessages = GameState.GetState("KeyTaken") ? VictoryMessageAlt : VictoryMessage;
            }
            else
            {
                ActiveMessages = FailureMessage;
            } 
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
