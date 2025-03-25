using Microsoft.AspNetCore.Components;
using Framework.Slides.JsonClasses;
using Framework.State;

namespace Framework.Slides;

public class SlideBase : ComponentBase
{

	[Parameter]
	public string SlideId { get; set; } = null!;

	// [Parameter]
	// public JsonSlide SlideData { get; set; } = null!; // Get the slide data
	protected JsonSlide SlideData { get; set; } = null!;

	[Parameter]
	public EventCallback<List<List<string>>> OnButtonClick { get; set; }

	[Inject]
	public GameState GameState { get; set; } = null!;

	[Inject]
	public SlideService SlideService { get; set; } = null!;

	// Fill color for the polygons
	//? what color will the polygons have (if any)
	protected string fillColor = "rgba(255, 255, 147, 0)";

	protected override async Task OnParametersSetAsync()
	{
		SlideData = SlideService.GetSlide(SlideId);
		if (SlideData.OnEnter != null)
		{
			await ButtonClick(SlideData.OnEnter);
		}
		// Console.WriteLine(SlideId);
	}

	protected async Task ButtonClick(List<List<string>> actions)
	{
		await OnButtonClick.InvokeAsync(actions);
	}

	protected async Task HandleButtonClick(JsonButton button)
	{
		if (button.Actions is List<List<string>> actions)
		{
			await ButtonClick(actions);
		}
	}

	// protected async Task EvaluateActions(List<List<string>> actions)
	// {
	// 	foreach (List<string> action in actions)
	// 	{
	// 		switch (action[0])
	// 		{
	// 			case "Route":
	// 				await SlideChange(action[1]);
	// 				break;
	// 			case "Require":
	// 				if (GameState.CheckForItem(action[1]))
	// 				{
	// 					GameState.RemoveItem(action[1]);
	// 				}
	// 				else
	// 				{
	// 					return;
	// 				}
	// 				break;
	// 			default:
	// 				break;
	// 		}
	// 	}
	// }
}