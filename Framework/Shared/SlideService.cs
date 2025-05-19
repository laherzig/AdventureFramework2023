using JsonUtilities;
using Framework.Slides.JsonClasses;
using Framework.State;
using Framework.Items;

namespace Framework.Slides;

public class SlideService(JsonUtility jsonUtility, GameState gameState, SlidesVerifier slidesVerifier)
{
	public readonly Dictionary<string, Dictionary<string, string>> PositionPresets = new()
	{

		{
			"left", new()
			{
				{"id", "pos-preset-left"},
				{"x", "0"},
				{"y", "340"},
				{"width", "150"},
				{"height", "400"},
			}
		},
		{
			"right", new()
			{
				{"id", "pos-preset-right"},
				{"x", "1470"},
				{"y", "340"},
				{"width", "150"},
				{"height", "400"},
			}
		},
		{
			"top", new()
			{
				{"id", "pos-preset-top"},
				{"x", "610"},
				{"y", "0"},
				{"width", "400"},
				{"height", "150"},
			}
		},
		{
			"bottom", new()
			{
				{"id", "pos-preset-bottom"},
				{"x", "610"},
				{"y", "930"},
				{"width", "400"},
				{"height", "150"},
			}
		}
	};

	private readonly JsonUtility jsonUtility = jsonUtility;
	private readonly GameState gameState = gameState;
	private readonly SlidesVerifier slidesVerifier = slidesVerifier;

	public Dictionary<string, JsonSlide> Slides { get; private set; } = null!;

	public Dictionary<JsonSlide, string> InverseSlides { get; private set; } = null!;

	// private readonly TaskCompletionSource<bool> _initCompletionSource = new();
	// public Task Initialization => _initCompletionSource.Task;

	private async Task<Dictionary<string, JsonSlide>> FetchSlidesAsync(string url)
	{
		// assign return value from GetFromJsonAsync to slides if it is not null, otherwise throw an exception
		// var slides = await Http.GetFromJsonAsync<Dictionary<string, JsonSlide>>(url) ?? 
		// throw new Exception("Slides is null");
		var slides = await jsonUtility.LoadFromJsonAsync<Dictionary<string, JsonSlide>>(url);
		return slides;
	}

	private async Task<Dictionary<string, JsonSlide>> FetchSlidesListFromFileAsync(string fileListFile)
	{
		// get the list of paths
		var fileList = await jsonUtility.LoadFromJsonAsync<List<string>>(fileListFile);
		var slidesList = new List<Dictionary<string, JsonSlide>>();
		// iterate over the paths and fetch the slides
		foreach (var file in fileList)
		{
			slidesList.Add(await FetchSlidesAsync(file));
		}
		// merge the dictionaries
		// will raise Exception if duplicate keys are found
		try
		{
			var slides = slidesList.SelectMany(dict => dict)
				.ToDictionary(pair => pair.Key, pair => pair.Value);
			return slides;
		}
		catch (ArgumentException e)
		{
			throw new SlidesJsonException("Duplicate keys in the slides files", e);
		}
	}

	public async Task Init(bool debugMode = false)
	{
		// Slides = await FetchSlidesAsync("Slides.json");

		Slides = await FetchSlidesListFromFileAsync("slidefiles.json");
		InverseSlides = Slides.ToDictionary(x => x.Value, x => x.Key);
		// _initCompletionSource.SetResult(true);
		CreateGameStateEntries();
		if (debugMode)
		{
			slidesVerifier.VerifySlides(Slides);
		}
		gameState.CurrentSlide = GetStartSlideId();
		//* Make relay slide for minigame to minigame routing
		//* This is a hacky solution, but it works
		Slides["__relay__"] = new JsonSlide()
		{
			Image = "",
			Buttons = [],
		};
	}

	public JsonSlide GetSlide(string slideId)
	{
		try
		{
			return Slides[slideId];
		}
		catch (KeyNotFoundException)
		{
			throw new KeyNotFoundException($"No Slide with Id `{slideId}` found");
		}

	}

	public string GetSlideId(JsonSlide slide)
	{
		try
		{
			// Works, but is slow
			// return Slides.FirstOrDefault(x => x.Value == slide).Key;
			// use inverted dictionary
			return InverseSlides[slide];
		}
		catch (KeyNotFoundException)
		{
			//TODO: Implement ToString in JsonSlide
			throw new KeyNotFoundException("No Id corresponding to given JsonSlide found");
		}
	}

	public string GetStartSlideId()
	{
		// Console.WriteLine(Slides);
		// return Slides.Keys.First();
		// return "error";
		// the slide with a tag "START" is the first slide
		return Slides.First(
			x =>
			{
				if (x.Value.Tags != null)
				{
					return x.Value.Tags.Contains("START");
				}
				else
				{
					return false;
				}
			}
		).Key;
	}

	public void CreateGameStateEntries()
	{
		// iterate over slides
		foreach (var slide in Slides)
		{
			// if slide is a minigame, skip it
			if (slide.Value.Type is string type)
			{
				if (type == "Minigame") { continue; }
			}
			foreach (var button in slide.Value.Buttons!)
			{
				if (button.Value.Visible is bool visible)
				{
					if (visible)
					{
						gameState.SetState($"{slide.Key}.{button.Key}", true);
					}
					else
					{
						gameState.SetState($"{slide.Key}.{button.Key}", false);
					}
				}
			}
		}
	}
}
