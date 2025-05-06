namespace Framework.Minigames.MinigameDefClasses;

public class ArmdrueckenDialogue : MinigameDefBase
{

    public override string BackgroundImage {get; set;} = "images/ArmdrückenDialog/IMG_5775.JPG"; // Background Image


	//DIALOGUE STUFF
	private Image QuitButton { get; set;}
	private Image ForwardButton { get; set;}
	private Dialogue dialogue;

	//MINIGAME STUFF
	[Element]
	public required Rectangle NpcHitBox { get; set;}
	List<List<string>> PrintedMessage = [];
	List<List<string>> messages = [
		["Du", "Entschuldigung, wisst ihr etw-"],
		["Rüpel", "Was denkst du, wer du bist!?!"],
		["Du", "Was?"],
		["Rüpel", "Weisst du überhaupt, mit wem du hier sprichst?"],
		["Rüpel", "Wenn du etwas von uns willst, musst du dich zuerst beweisen"],
		["Du", "Was muss ich machen?"],
		["Rüpel", "Du musst den Chef im Armdrücken besiegen."],
		["Rüpel", "Aber mach dich auf was gefasst, der Chef ist dreifacher Weltmeister im Armdrücken."],
		["Rüpel", "Mach dich bereit, der Chef hat nicht ewig Zeit."],
		["Du", "Ich werde mein Bestes geben!"],
		["Chef", "..."],

	];

	List<List<string>> messages2 = [
		["Rüpel", "Schon zurück? Das ging ja schnell!"],
		["Du", "Ich würde es gerne erneut versuchen."],
		["Chef", "Du hast Mumm."],
		["Rüpel", "Zeig ihm, wo der Hammer hängt, Chef!."],

	];

	List<List<string>> LooseMessages = [
        ["Du", "Verdammt! Der Chef ist ja wirklich viel zu stark."],
        ["Chef", "Du verschwendest meine Zeit."],
        ["Rüpel", "Du hast den Chef gehört."],
        ["Rüpel", "Komm zurück, wenn du bereit bist."],

    ];

	List<List<string>> WinMessages = [
		["Chef", "Was?? Wie kann das sein?"],
		["Rüpel", "Er hat es tatsächlich geschafft!"],
		["Du", "Puh, das war schwieriger als gedacht."],
		["Chef", "Du hast mich fair geschlagen"],
		["Chef", "Du hast dir unseren Respekt verdient."],
		["Du", "Ich wollte euch fraen, ob ihr meinen Laptop gesehen habt."],
		["Du", "Er ist während der Zwischenpause verschwunden!."],
		["Chef", "Hinweis"],

	];
	
	public ArmdrueckenDialogue(){
		
		if (GameState.GetState("MG_ArmdrueckenStarted") == true && GameState.GetState("MG_ArmdrueckenDone") == false)
		{
			messages = LooseMessages;
		};
		if (GameState.GetState("MG_ArmdrueckenStarted") == true && GameState.GetState("MG_ArmdrueckenDone") == true)
		{
			messages = WinMessages;
		};
		if (GameState.GetState("MG_ArmdrueckenStarted") == false)
		{
			GameState.SetState("MG_ArmdrueckenStarted", true);
		};
		dialogue = new Dialogue(messages);

		NpcHitBox = new(){
			X = 325,
			Y = 400,
			Width = 1200,
			Height = 600,
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


		foreach (List<string> speech in messages){
			int x_message = 100;
            int y_message = 0;

            if (speech[0] == "Du"){
                x_message = 700;
                y_message = 900;
                Console.WriteLine("Du is speaking");
            }
            else if (speech[0] == "Rüpel"){
                x_message = 400;
                y_message = 350;
            }
            else if (speech[1] == "Du"){
                x_message = 500;
                y_message = 500;
            }
            else if (speech[0] == "Chef"){
                x_message = 1100;
                y_message = 300;
            }
            else
            {
                x_message = 500;
                y_message = 500;
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
