using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Framework.Minigames.MinigameDefClasses;

public class BiologyMinigame : MinigameDefBase
{
    int currentScore = 0;
    int lives = 3;
    int questionNumber = 0;
    bool answer = true;
    public override string BackgroundImage { get; set; } = "minigame_assets/Biology_assets/Cooles_Bild.jpg";
    [Element]
    public Rectangle Rect { get; set; }
    [Element]
    public Rectangle Rect2 { get; set; }
    [Element]
    public Text TheText { get; set; }
    [Element]
    public Text TheText2 { get; set; }
    [Element]
    public Text LifeText { get; set; }
    [Element]
    public Text QuestionText { get; set; }
    [Element]
    public Text ScoreText { get; set; }
    [Element]
    public Text RealLifeText { get; set; }
    [Element]
    public Text RealScoreText { get; set; }
    [Element]
    public Text TitleOrVictoryOrFailure { get; set; }
    [Element]
    public Text AttentionText { get; set; }
    [Element]
    public Text Question { get; set; }
    public GameObjectContainer<Rectangle> Rects { get; set; } = new();
    public GameObjectContainer<Rectangle> Rects2 { get; set; } = new();
    public GameObjectContainer<Text> TheTexts { get; set; } = new();
    public GameObjectContainer<Text> TheTexts2 { get; set; } = new();
    public GameObjectContainer<Text> LifeTexts { get; set; } = new();
    public GameObjectContainer<Text> ScoreTexts { get; set; } = new();
    public GameObjectContainer<Text> RealScoreTexts { get; set; } = new();
    public BiologyMinigame()
    {
        Question = new()
        {
            InnerText = "Question:",
            X = 400,
            Y = 340,
            FontSize = 50,
            FontFamily = "sans-serif",
            Fill = "green"
        };

        TitleOrVictoryOrFailure = new()
        {
            InnerText = "Get 5 points to win!",
            X = 500,
            Y = 100,
            FontSize = 75,
            FontFamily = "sans-serif",
            Fill = "yellow"
        };
        ScoreText = new()
        {
            InnerText = "Score:",
            X = 5,
            Y = 250,
            FontSize = 65,
            FontFamily = "sans-serif",
            Fill = "red"
        };
        ScoreTexts.Add(ScoreText);
        QuestionText = new()
        {
            InnerText = "Alle Säugetiere legen Eier",
            X = 400,
            Y = 400,
            FontSize = 34,
            FontFamily = "sans-serif",
            Fill = "black"
        };

        RealLifeText = new()
        {
            InnerText = "3",
            X = 220,
            Y = 150,
            FontSize = 65,
            FontFamily = "sans-serif",
            Fill = "red"
        };

        LifeText = new()
        {
            InnerText = "Lives: ",
            X = 5,
            Y = 150,
            FontSize = 65,
            FontFamily = "sans-serif",
            Fill = "red",

        };
        LifeTexts.Add(LifeText);

        RealScoreText = new()
        {
            InnerText = "0",
            X = 220,
            Y = 250,
            FontSize = 65,
            FontFamily = "sans-serif",
            Fill = "red",
        };
        RealScoreTexts.Add(RealScoreText);

        TheText = new()
        {
            InnerText = "Richtig",
            X = 300,
            Y = 1000,
            FontSize = 100,
            FontFamily = "sans-serif",
            Fill = "white",
        };
        TheTexts.Add(TheText);

        TheText2 = new()
        {
            InnerText = "Falsch",
            X = 1100,
            Y = 1000,
            FontSize = 100,
            FontFamily = "sans-serif",
            Fill = "white",
        };
        TheTexts2.Add(TheText2);

        Rect = new()
        {
            X = 200,
            Y = 800,
            Width = 500,
            Height = 300,
            Fill = "green",
            OnClick = (args) =>
{
    int x = Convert.ToInt16(RealLifeText.InnerText);
    int y = Convert.ToInt16(RealScoreText.InnerText);
    string z = QuestionText.InnerText;
    string a = TitleOrVictoryOrFailure.InnerText;
    questionNumber++;
    if (questionNumber % 2 != 0 && x > 1 && questionNumber == 1)
    {
        x = x - 1;
        z = "Pflanzen produzieren Sauerstoff durch Photosynthese";
        SoundService.PlaySound("minigame_assets/Biology_assets/buzzer-or-wrong-answer-20582.mp3");
    }
    else if (questionNumber % 2 != 0 && x > 1 && questionNumber == 2)
    {
        x = x - 1;
        z = "Ein Virus kann sich ohne einen Wirt vermehren";
        SoundService.PlaySound("minigame_assets/Biology_assets/buzzer-or-wrong-answer-20582.mp3");
    }
    else if (questionNumber % 2 != 0 && x > 1 && questionNumber == 3)
    {
        x = x - 1;
        z = "Menschen haben 206 Knochen";
        SoundService.PlaySound("minigame_assets/Biology_assets/buzzer-or-wrong-answer-20582.mp3");
    }
    else if (questionNumber % 2 != 0 && x > 1 && questionNumber == 4)
    {
        x = x - 1;
        z = "Pilze sind Pflanzen";
        SoundService.PlaySound("minigame_assets/Biology_assets/buzzer-or-wrong-answer-20582.mp3");
    }
    else if (questionNumber % 2 != 0 && x > 1 && questionNumber == 5)
    {
        x = x - 1;
        z = "Der Herzschlag beträgt 70/min im Ruhezustand";
        SoundService.PlaySound("minigame_assets/Biology_assets/buzzer-or-wrong-answer-20582.mp3");
    }
    else if (questionNumber % 2 != 0 && x > 1 && questionNumber == 6)
    {
        x = x - 1;
        z = "Fische atmen Luft durch die Lungen";
        SoundService.PlaySound("minigame_assets/Biology_assets/buzzer-or-wrong-answer-20582.mp3");
    }
    else if (questionNumber % 2 == 0 && y < 4 && questionNumber == 1)
    {
        y = y + 1;
        currentScore++;
        z = "Pflanzen produtzieren Sauerstoff durch Photosynthese";
        SoundService.PlaySound("/minigame_assets/Biology_assets/ding.wav");
    }
    else if (questionNumber % 2 == 0 && y < 4 && questionNumber == 2)
    {
        y = y + 1;
        currentScore++;
        z = "Ein Virus kann sich ohne Wirt vermehren";
        SoundService.PlaySound("/minigame_assets/Biology_assets/ding.wav");
    }
    else if (questionNumber % 2 == 0 && y < 4 && questionNumber == 3)
    {
        y = y + 1;
        currentScore++;
        z = "Menschen haben 206 Knochen im Körper";
        SoundService.PlaySound("/minigame_assets/Biology_assets/ding.wav");
    }
    else if (questionNumber % 2 == 0 && y < 4 && questionNumber == 4)
    {
        y = y + 1;
        currentScore++;
        z = "Pilze sind Pflanzen";
        SoundService.PlaySound("/minigame_assets/Biology_assets/ding.wav");
    }
    else if (questionNumber % 2 == 0 && y < 4 && questionNumber == 5)
    {
        y = y + 1;
        currentScore++;
        z = "Der Herzschlag beträgt 70/min im Ruhezustand";
        SoundService.PlaySound("/minigame_assets/Biology_assets/ding.wav");
    }
    else if (questionNumber % 2 == 0 && y < 4 && questionNumber == 6)
    {
        y = y + 1;
        currentScore++;
        z = "Fische atmen Luft durch die Lungen";
        SoundService.PlaySound("/minigame_assets/Biology_assets/ding.wav");
    }
    else if (x == 1)
    {
        x = 0;
        z = "";
        a = "Game Over!";
        SoundService.PlayMusic("minigame_assets/Biology_assets/SpongeBob sad music.m4a");
    }
    else if (y == 4)
    {
        y = 5;
        z = "";
        a = "You win!";
        SoundService.PlayMusic("minigame_assets/Biology_assets/Yippee - Meme Sound Effect.m4a");
    }
    RealLifeText.InnerText = x.ToString();
    RealScoreText.InnerText = y.ToString();
    QuestionText.InnerText = z.ToString();
    TitleOrVictoryOrFailure.InnerText = a.ToString();
    Update();
}
        };
        Rects.Add(Rect);

        Rect2 = new()
        {
            X = 1000,
            Y = 800,
            Width = 500,
            Height = 300,
            Fill = "red",
            OnClick = (args) =>
            {
                int x = Convert.ToInt16(RealLifeText.InnerText);
                int y = Convert.ToInt16(RealScoreText.InnerText);
                string z = QuestionText.InnerText;
                string a = TitleOrVictoryOrFailure.InnerText;
                questionNumber++;
                if (questionNumber % 2 == 0 && x > 1 && questionNumber == 1)
                {
                    x = x - 1;
                    z = "Pflanzen produzieren Sauerstoff durch Photosynthese";
                    SoundService.PlaySound("minigame_assets/Biology_assets/buzzer-or-wrong-answer-20582.mp3");
                }
                else if (questionNumber % 2 == 0 && x > 1 && questionNumber == 2)
                {
                    x = x - 1;
                    z = "Ein Virus kann sich ohne Wirt vermehren";
                    SoundService.PlaySound("minigame_assets/Biology_assets/buzzer-or-wrong-answer-20582.mp3");
                }
                else if (questionNumber % 2 == 0 && x > 1 && questionNumber == 3)
                {
                    x = x - 1;
                    z = "Menscheb haben 206 Knochen im Körper";
                    SoundService.PlaySound("minigame_assets/Biology_assets/buzzer-or-wrong-answer-20582.mp3");
                }
                else if (questionNumber % 2 == 0 && x > 1 && questionNumber == 4)
                {
                    x = x - 1;
                    z = "Pilze sind Pflanzen";
                    SoundService.PlaySound("minigame_assets/Biology_assets/buzzer-or-wrong-answer-20582.mp3");
                }
                else if (questionNumber % 2 == 0 && x > 1 && questionNumber == 5)
                {
                    x = x - 1;
                    z = "Der Herzschlag beträgt 70/min im Ruhezustand";
                    SoundService.PlaySound("minigame_assets/Biology_assets/buzzer-or-wrong-answer-20582.mp3");
                }
                else if (questionNumber % 2 == 0 && x > 1 && questionNumber == 6)
                {
                    x = x - 1;
                    z = "Fische atmen Luft durch die Lungen";
                    SoundService.PlaySound("minigame_assets/Biology_assets/buzzer-or-wrong-answer-20582.mp3");
                }
                else if (questionNumber % 2 != 0 && y < 4 && questionNumber == 1)
                {
                    y = y + 1;
                    currentScore++;
                    z = "Pflanzen produzieren Sauerstoff durch Photosynthese";
                    SoundService.PlaySound("/minigame_assets/Biology_assets/ding.wav");
                }
                else if (questionNumber % 2 != 0 && y < 4 && questionNumber == 2)
                {
                    y = y + 1;
                    currentScore++;
                    z = "Ein Virus kann sich ohne Wirt vermehren";
                    SoundService.PlaySound("/minigame_assets/Biology_assets/ding.wav");
                }
                else if (questionNumber % 2 != 0 && y < 4 && questionNumber == 3)
                {
                    y = y + 1;
                    currentScore++;
                    z = "Menschen haben 206 Knochen im Körper";
                    SoundService.PlaySound("/minigame_assets/Biology_assets/ding.wav");
                }
                else if (questionNumber % 2 != 0 && y < 4 && questionNumber == 4)
                {
                    y = y + 1;
                    currentScore++;
                    z = "Pilze sind Pflanzen";
                    SoundService.PlaySound("/minigame_assets/Biology_assets/ding.wav");
                }
                else if (questionNumber % 2 != 0 && y < 4 && questionNumber == 5)
                {
                    y = y + 1;
                    currentScore++;
                    z = "Der Herzschlag beträgt 70/min im Ruhezustand";
                    SoundService.PlaySound("/minigame_assets/Biology_assets/ding.wav");
                }
                else if (questionNumber % 2 != 0 && y < 4 && questionNumber == 6)
                {
                    y = y + 1;
                    currentScore++;
                    z = "Fische atmen Luft durch Lungen";
                    SoundService.PlaySound("/minigame_assets/Biology_assets/ding.wav");
                }
                else if (x == 1)
                {
                    x = 0;
                    z = "";
                    a = "Game Over!";
                    SoundService.PlayMusic("minigame_assets/Biology_assets/SpongeBob sad music.m4a");
                }
                else if (y == 4)
                {
                    y = 5;
                    z = "";
                    a = "You win!";
                    SoundService.PlayMusic("minigame_assets/Biology_assets/Yippee - Meme Sound Effect.m4a");
                }
                RealLifeText.InnerText = x.ToString();
                RealScoreText.InnerText = y.ToString();
                QuestionText.InnerText = z.ToString();
                TitleOrVictoryOrFailure.InnerText = a.ToString();
                Update();
            }
        };
        Rects.Add(Rect2);
    }
}