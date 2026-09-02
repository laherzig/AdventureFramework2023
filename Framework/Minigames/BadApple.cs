using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace Framework.Minigames.MinigameDefClasses;

public class BadApple : MinigameDefBase {

    public const int w = 32;
    public const int h = 24;
    public const int s = 20;

    public const int offX = 400;
    public const int offY = 400;

    public const int l = 768;
    public override string BackgroundImage { get; set; } = "minigame_assets/Biology_assets/Cooles_Bild.jpg";
    public Rectangle[,] rects = new Rectangle[h,w];

    public Rectangle[][] layers = new Rectangle[l][];
    public Rectangle[] bgs = new Rectangle[l];
    public bool clicked = false;

    public BadApple() {
        AddElement(new Rectangle() {
            X = 0,
            Y = 0,
            Width = 10,
            Height = 10,
            Fill = "red",
            OnClick = Spawn
        });
        

    }

    public void Spawn(EventArgs args) {

        for (int i = l-1; i >= 0; i--) {
            layers[i] = [
                new Rectangle() {
                    X = offX + (i * s) % (h * s),
                    Y = offY + (i * s) % (h * s),
                    Height = s,
                    Width = s,
                    Fill = "white",
                    //ZIndex = 2*i + 1
                }
            ];
            bgs[i] = new Rectangle() {
                X = offX,
                Y = offY,
                Width = w * s,
                Height = h * s,
                Fill = "black",
                //ZIndex = 2 * i
            };
            Elements.Add(bgs[i]);
            Elements.AddMultiple(layers[i]);
        }


        /*
        List<Rectangle> r = new();

        Console.WriteLine("func");

        int x = 400;
        int y = h;
        int size = s;

        for (int j = 0; j < h; j++) {
            for (int i = 0; i < w; i++) {
                var rec = new Rectangle() {
                    X = x + size * i,
                    Y = y + size * j,
                    Width = size,
                    Height = size,
                    Fill = "black"
                };
                rects[j,i] = rec;
                r.Add(rec);
            }
            Console.WriteLine(j);
        }

        Elements.AddMultiple(r);
        Update();
        */
        Console.WriteLine("fin");
        clicked = true;
    }


    public override async Task GameLoop(CancellationToken ct)
    {
        bool first = true;
        int iter = 0;
        int layer = 0;
        while (true) {

            if (clicked) {
                if (first) {
                    first = false;
                    await Task.Delay(10000, ct);
                    Console.WriteLine("waiting fininshed");
                } else {
                    Parallel.ForEach(layers[layer], x => x.Visible = false);
                    // foreach (var R in layers[layer]){ R.Kill(); }
                    bgs[layer].Visible = false;
                    layer++;
                }
            }

            iter++;
            Update();
            // await Task.Delay(100, ct);

            ct.ThrowIfCancellationRequested();
        }
    }
}