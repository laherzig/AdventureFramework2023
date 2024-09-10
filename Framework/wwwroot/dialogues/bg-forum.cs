
namespace Framework.Minigames.MinigameDefClasses;

public class BGDialogue : MinigameDefBase
{

    public override string BackgroundImage {get; set;} = "images/BG_Tisch.JPG"; // Background Image


	//DIALOGUE STUFF
	private Image QuitButton { get; set;}
	private Image ForwardButton { get; set;}

	private Dialogue dialogue;

	//MINIGAME STUFF
	[Element]
	public required Rectangle NpcHitBox { get; set;} 

	List<List<string>> messages = [
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

	List<List<string>> message2 = [
		["Junge", "Im BG-Zimmer sind Leute eingebrochen und haben alles verstellt oder asugeräumt"],
		["Junge", "Wenn du alle Sachen wieder einräumst geben wir den Hinweis"],
		["Mädchen", "Finde aber zuerst die Unterschiede, dann kannst du ja alles einräumen."],
	];

	public BGDialogue(){

		dialogue = new Dialogue(messages);

		NpcHitBox = new(){
			X = 1500,
			Y = 1000,
			Width = 100,
			Height = 100,
			Fill = "green",
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


		foreach (List<string> speech in messages){

			int x_message = 100;
            int y_message = 0;

            if (speech[0] == "Du"){
                x_message = 700;
                y_message = 900;
                Console.WriteLine("Du is speaking");
            }
            else if (speech[0] == "Mädchen"){
                x_message = 800;
                y_message = 300;
            }
            else if (speech[1] == "Du"){
                x_message = 500;
                y_message = 500;
            }
            else if (speech[0] == "Junge"){
                x_message = 200;
                y_message = 300;
            }
            else
            {
                x_message = 500;
                y_message = 1000;
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