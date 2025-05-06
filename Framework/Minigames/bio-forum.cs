namespace Framework.Minigames.MinigameDefClasses;

public class BiologyDialogue : MinigameDefBase
{

    public override string BackgroundImage {get; set;} = "minigame_assests/Biology_assets/IMG_5785.JPG"; // Background Image


	//DIALOGUE STUFF
	private Image QuitButton { get; set;}
	private Image ForwardButton { get; set;}

	private Dialogue dialogue;

	//MINIGAME STUFF
	[Element]
	public required Rectangle NpcHitBox { get; set;}
	List<List<string>> messages = [
		["Du", "Hallo! Könnt ihr mir sagen wo mein Laptop ist?"],
		["Mauzi", "Vielleicht... wenn du uns helfen kannst :3"],
		["Du", "Was gibts?"],
		["Bautzi", "Wir sind richtig schlecht in Bio, kannst du uns helfen?"],
		["Du", "ok..."],
	];
		List<List<string>> VictoryMessage = [
		["Du", "Viel zu einfach"],
		["Mauzi", "OMG danke!"],
		["Bautzi", "Endlich kann ich meinen Eltern eine Note zeigen"],
		["Du", "Nun raus mit der Sprache, helft mir jetzt!"],
		["Mautzi", ""],
	];
		List<List<string>> FailureMessage = [
		["Du", "Noch nicht fertig, sorry"],
		["Mauzi", "Dann flieg ich halt raus"],
		["Bautzi", "L"],
	];
	public BiologyDialogue(){

		dialogue = new Dialogue(messages);

		NpcHitBox = new(){
			X = 115,
			Y = 400,
			Width = 500,
			Height = 800,
			Fill = "transparent",
			OnClick = async (args) => {Console.WriteLine("Click On NPC"); await StartDialogueAsync();}
		};

	}

	public async Task StartDialogueAsync(){
		bool quit = false;
		bool forward = false;

		//Create forward and quit button
		QuitButton = dialogue.DrawQuitButton();
		ForwardButton = dialogue.DrawForwardButton();

		AddElement(QuitButton);
		AddElement(ForwardButton);

		QuitButton.OnClick = (args) => {quit = true; Console.WriteLine("Quit"); };
		ForwardButton.OnClick = (args) => {forward = true; Console.WriteLine("Forward");};

		Update();
		if (GameState.GetState("MGBioDone") == true)
		{
			messages = VictoryMessage;
		}
		else if (GameState.GetState("MGBioStarted") == true && GameState.GetState("MGBioDone") == false)
		{
			messages = FailureMessage;
		}

		foreach (List<string> speech in messages){
			int x_message = 100;
            int y_message = 0;

            if (speech[0] == "Du"){
                x_message = 500;
                y_message = 200;
                Console.WriteLine("Du is speaking");
            }
            else if (speech[0] == "Mautzi"){
                x_message = 500;
                y_message = 201;
            }
            else if (speech[1] == "Du"){
                x_message = 500;
                y_message = 200;
            }
            else if (speech[0] == "Bautzi"){
                x_message = 500;
                y_message = 202;
            }
            else
            {
                x_message = 500;
                y_message = 200;
            }
			GameObjectContainer<SVGElement> Bubble = dialogue.DrawSpeechBubble(speech[0], speech[1], autoPlacement: false, x: x_message, y: y_message);

			AddElementsInContainer(Bubble);

			Update();

			await WaitForConditionAsync(() => forward || quit);

			foreach (string key in Bubble.Keys){
				Elements.Remove(key);
			}

			if (quit == true){

				Update();
				break;
			}

			forward = false;

			Update();
		}


	}

	private async Task WaitForConditionAsync(Func<bool> condition)
{
    while (!condition())
    {
        await Task.Delay(100); // Check the condition every 100 milliseconds
    }
}



}
