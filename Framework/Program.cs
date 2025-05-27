using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Framework;
using JsonUtilities;
using Framework.State;
using Blazored.Toast;
using Framework.Slides;
using Framework.Keyboard;
using Framework.Mouse;
using Framework.Sound;
using Framework.Video;
using Framework.Items;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<JsonUtility>();

builder.Services.AddScoped<ItemService>();

// Notifications
builder.Services.AddBlazoredToast();

// First register both of those, then execute the initialization for both
// builder.Services.AddScoped<SlideService>();
builder.Services.AddScoped<SlideService>();

builder.Services.AddScoped<GameState>();

builder.Services.AddScoped<SlidesVerifier>();


// // TODO: Find better solution to execute functions at startup
// // Apparently this only contains Services registered prior to its initialization, and it doesn't update
// var ServiceProvider = builder.Services.BuildServiceProvider();

// // Initialize the slides
// //! This does not seem to work as intended, don't know why though
// // await ServiceProvider.GetRequiredService<SlideService>().Init();

// // execute the async method to load the gamestate and items
// await ServiceProvider.GetRequiredService<GameState>().LoadGameStateAndItemsAsync();
// // builder.Sexrvices.AddScoped(sp => {new GameState(sp.GetRequiredService<JsonUtility>(), sp.GetRequiredService<Framework.Items.Items>())});

builder.Services.AddScoped<KeyboardService>();
builder.Services.AddScoped<MouseService>();

// Make C# use non-retarded decimal format
/*
Like honestly Microsoft, why in the world did you think that it would be a good idea to localize
number formatting? You basically just created the possibility of the exact same code not working on
a machine that is in another place. I can only puzzle about the reasons why you chose to implement 
such a "feature", when there is absolutely no fucking reason to do such a completely stupid thing.
So Microsoft, thank you for just helping me chose the framework for my next project, I now know
that it's not going to be the one that uses C#.
*/
System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
customCulture.NumberFormat.NumberDecimalSeparator = ".";

System.Globalization.CultureInfo.DefaultThreadCurrentCulture = customCulture;

builder.Services.AddScoped<SoundService>();

builder.Services.AddScoped<VideoService>();

await builder.Build().RunAsync();
