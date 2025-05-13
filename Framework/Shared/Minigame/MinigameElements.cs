using System.Dynamic;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Framework.Minigames;


public interface IGameObject
{
	public string Id { get; set; }

	// for render order
	public int? ZIndex { get; set; }

	// visibilty not over style, but directly when rendering
	public bool Visible { get; set; }
	public RenderFragment GetRenderFragment();

	public event EventHandler? OnKill;
	public void Kill();
}

// implementing this interface causes `Style` to break cause there is somehow
// an ambiguity between the property defined in the class and the on in the interface
// public interface ISVGElement : IGameObject
// {
// 	string TagName { get; }
// 	string? Style { get; }
// 	public int? ZIndex { get; set; }
// }


// you can use this as a base class, but you have to make the
// GetRenderFragment method yourself
public abstract class GameObject : IGameObject
{
	// Make sure that every element always has an Id
	public string Id { get; set; } = Guid.NewGuid().ToString("N");

	public virtual int? ZIndex { get; set; }

	public virtual bool Visible { get; set; } = true;

	public event EventHandler? OnKill;

	public void Kill()
	{
		OnKill?.Invoke(this, EventArgs.Empty);
	}

	public abstract RenderFragment GetRenderFragment();
}


// This is the main class you should use for your custom game objects
// It already includes the rendering logic
// Basically just a wrapper for an SVGElement
// Abstracted away here so that you don't have to subclass the SVGElement classes
// directly, but can use this as a base class if you want to add stuff
public abstract class SVGElementGameObject : GameObject
{
	// it is mandatory to set the element, otherwise you could just subclass GameObject
	public SVGElement Element { get; set; } = null!;

	public override RenderFragment GetRenderFragment()
	{
		return Element.GetRenderFragment();
	}

	public string? Style { get => Element.Style; }

	public override int? ZIndex
	{
		get => Element.ZIndex;
		set => Element.ZIndex = value;
	}

	public string TagName { get => Element.TagName; }
}


public abstract class SVGElement : GameObject
{
	[Html("style")] public string? Style { get => GetStyleString(); }

	// Normal implementation (maybe slightly slower, but I understand it better)
	public abstract string TagName { get; }
	// maybe virtual for more custom elements?
	// public virtual string TagName { get; } = "div";


	public virtual string? CustomStyle { get; set; }

	// some properties that are the same across all elements
	//  event handlers, as they are the same over all elements
	[Callback("onclick")] public Action<EventArgs>? OnClick { get; set; }
	[Callback("ondblclick")] public Action<EventArgs>? OnDoubleClick { get; set; }
	[Callback("onmouseenter")] public Action<EventArgs>? OnMouseEnter { get; set; }
	[Callback("onmouseleave")] public Action<EventArgs>? OnMouseLeave { get; set; }

	// set the cursor (probably most often to "pointer")
	[Style("cursor")] public string? Cursor { get; set; }
	[Style("opacity")] public double? Opacity { get; set; }


	public List<object> Content { get; set; } = [];

	public Dictionary<string, object> Attributes { get; set; } = [];


	public override RenderFragment GetRenderFragment()
	{
		return builder =>
		{
			builder.OpenElement(0, TagName);
			builder.AddAttribute(1, "id", Id);
			builder.AddMultipleAttributes(2, GetElementAttributeDictionary());
			builder.AddMultipleAttributes(3, Attributes);
			builder.AddMultipleAttributes(4, GetCallbackDictionary());
			builder.OpenRegion(5);
			int i = 0;
			foreach (var item in Content)
			{
				if (item is GameObject g) { builder.AddContent(i, g.GetRenderFragment()); }
				else if (item is string s) { builder.AddContent(i, s); }
				else if (item is MarkupString m) { builder.AddContent(i, m); }
				else if (item is RenderFragment r) { builder.AddContent(i, r); }
				i++;
			}
			builder.CloseRegion();
			builder.CloseElement();
		};
	}

	// // [Obsolete("Strings cannot contain blazor stuff, use RenderFragments instead")]
	// // public abstract string GetMarkupString();

	// maybe problems with reactivity, don't know yet
	[Obsolete("Not used, no reason to ever use, there is another that does the same thing")]
	private string? GetStyleStringOld()
	{
		string style = "";
		foreach (var property in GetType().GetProperties())
		{
			if (Attribute.IsDefined(property, typeof(StyleAttribute)))
			{
				var value = property.GetValue(this);
				if (value != null)
				{
					style += $"{Translate(property.Name)}: {property.GetValue(this)};";
				}
			}
		}
		return style == "" ? null : style;
	}

	public string? GetStyleString()
	{
		string style = "";
		PropertyInfo[] properties = GetType()
		.GetProperties(
		BindingFlags.Instance |
		BindingFlags.Public |
		BindingFlags.NonPublic
		)
		.Where(p => Attribute.IsDefined(p, typeof(StyleAttribute)))
		.ToArray();
		foreach (var property in properties)
		{
			var value = property.GetValue(this);
			if (value != null)
			{
				//*Note: Added null suppression because safety is ensured, but keep in mind regardless
				// with translate {...ibute<StyleAttribute>()!.name ?? Translate(property.Name)}
				style +=
				$"{property.GetCustomAttribute<StyleAttribute>()!.name}: {value};";
			}
		}
		// add custom style
		style += CustomStyle;
		return style == "" ? null : style;
	}

	[Obsolete("MarkupStrings cannot contain blazor stuff, use RenderFragments instead")]
	public string? GetElementAttributeString()
	{
		string attributes = "";
		foreach (var property in this.GetType().GetProperties())
		{
			if (Attribute.IsDefined(property, typeof(HtmlAttribute)))
			{
				var value = property.GetValue(this);
				if (value != null)
				{
					attributes += $"{Translate(property.Name)}=\"{property.GetValue(this)}\" ";
				}
			}
		}
		return attributes == "" ? null : attributes;
	}

	[Obsolete("MarkupStrings cannot contain blazor stuff, use RenderFragments instead")]
	public string? GetCallbackString()
	{
		string callbacks = "";
		foreach (var method in this.GetType().GetMethods())
		{
			if (Attribute.IsDefined(method, typeof(CallbackAttribute)))
			{
				callbacks += $"{Translate(method.Name)}=\"{method.Name}\" ";
			}
		}
		return callbacks == "" ? null : callbacks;
	}

	public Dictionary<string, object>? GetElementAttributeDictionary()
	{
		Dictionary<string, object> attributes = new();
		PropertyInfo[] properties = GetType()
		.GetProperties(
			BindingFlags.Instance |
			BindingFlags.Public |
			BindingFlags.NonPublic
		)
		.Where(p => Attribute.IsDefined(p, typeof(HtmlAttribute)))
		.ToArray();
		foreach (var property in properties)
		{
			var value = property.GetValue(this);

			if (value != null)
			{
				attributes.Add(
					//*Note: Added null suppression because safety is ensured, but keep in mind regardless
					property.GetCustomAttribute<HtmlAttribute>()!.name,
					value
				);
			}
		}
		return attributes.Count == 0 ? null : attributes;
	}

	public Dictionary<string, object>? GetCallbackDictionary()
	{
		Dictionary<string, object> callbacks = [];
		PropertyInfo[] methods = GetType()
		.GetProperties(
			BindingFlags.Instance |
			BindingFlags.Public |
			BindingFlags.NonPublic
		)
		.Where(p => Attribute.IsDefined(p, typeof(CallbackAttribute)))
		.ToArray();
		foreach (var method in methods)
		{
			// callbacks.Add(
			// 	Translate(method.Name),
			// 	EventCallback.Factory.Create(this, (Action)Delegate.CreateDelegate(typeof(EventHandler), this, method))
			// );
			if (method.GetValue(this) is Action<EventArgs> action)
			{
				callbacks.Add(
					//*Note: I added null suppresion here, because it is impossible that the attribute is null
					// But still, I could be overlooking somehting, so be wary of this
					// If this throws an error, we're in trouble, as it means that something greater
					// beyond my mortal understanding has broken
					method.GetCustomAttribute<CallbackAttribute>()!.name,
					EventCallback.Factory.Create(this, action)
				);
			}
		}
		return callbacks.Count == 0 ? null : callbacks;
	}

	// public Dictionary<string, object>? GetElementAttributeDictionary()
	// {
	// 	Dictionary<string, object> attributes = new();
	// 	foreach (var property in GetType().GetProperties())
	// 	{
	// 		if (Attribute.IsDefined(property, typeof(HtmlAttribute)))
	// 		{
	// 			var value = property.GetValue(this);
	// 			if (value != null)
	// 			{
	// 				attributes.Add(Translate(property.Name), value);
	// 			}
	// 		}
	// 	}
	// 	return attributes.Count == 0 ? null : attributes;
	// }

	[Obsolete("Not used and also a bodgy solution, use the AttrubteClasses's `name` property instead")]
	protected static string ConvertCamelToKebab(string camelCase)
	{
		string kebab = $"{char.ToLower(camelCase[0])}";
		foreach (char c in camelCase[1..])
		{
			if (char.IsUpper(c))
			{
				kebab += $"-{char.ToLower(c)}";
			}
			else
			{
				kebab += c;
			}
		}
		return kebab;
	}

	[Obsolete("Html and Style attrs now require `name`, this is just a possible error source")]
	public static string Translate(string key)
	{
		// for EventHandlers
		// everything that starts with an "On" and has a capital letter after that
		// will be translated to "on{lowercase}" with an @ in front, because blazor
		// e.g. OnClick -> @onclick
		// So just make sure that you name everything correctly or else everything will break
		// Update: I don't think the callbacks work with strings, but still working on it
		try
		{
			if (key[..2] == "On" && char.IsUpper(key[2]))
			{
				return $"on{key[2..].ToLower()}";
			}
		}
		catch (ArgumentOutOfRangeException)
		{
			// if the string is too short, use the default translation
			return ConvertCamelToKebab(key);
		}
		return ConvertCamelToKebab(key);
	}
}

public abstract class ShapeElement : SVGElement
{
	[Style("fill")] public string? Fill { get; set; }
	[Style("fill-opacity")] public double? FillOpacity { get; set; }
	// stroke stuff
	[Style("stroke")] public string? Stroke { get; set; }
	[Style("stroke-width")] public double? StrokeWidth { get; set; }
	[Style("stroke-opacity")] public double? StrokeOpacity { get; set; }
}


// Some classes like ImgButton, PolygonButton, RectButton are some Ideas I had
// They are set as properties in a  MinigameBase instance and are then
// "generated" in the markup of the Minigame
// Should have functions like check for click, disable/enable, Set/GetPos, show/hide et.

public class Polygon : ShapeElement
{
	public override string TagName { get; } = "polygon";

	public List<int[]> Points { get; set; } = [];

	[Html("points")]
	private string PointString => string.Join(",", Points.SelectMany(i => i));

	// [Style("fill")] public string? Fill { get; set; }
}

public class Polyline : ShapeElement
{
	public override string TagName { get; } = "polyline";

	public List<int[]> Points { get; set; } = [];

	[Html("points")]
	private string PointString => string.Join(",", Points.SelectMany(i => i));
}

public class Rectangle : ShapeElement
{
	// // Implementation of the TagName property as static
	// public new static string TagName { get; } = "rect";

	// "normal" implementation of TagName
	public override string TagName { get; } = "rect";

	[Html("x")] public int? X { get; set; }
	[Html("y")] public int? Y { get; set; }
	[Html("width")] public int Width { get; set; }
	[Html("height")] public int Height { get; set; }

	// [Style("fill")] public string? Fill { get; set; }

	// now inherited
	// [Callback("onclick")] public Action<EventArgs>? OnClick { get; set; }
}

public class Circle : ShapeElement
{
	public override string TagName { get; } = "circle";
	[Html("cx")] public int? CX { get; set; }
	[Html("cy")] public int? CY { get; set; }
	[Html("r")] public int? R { get; set; }
	[Html("pathLength")] public int? PathLength { get; set; }
}

public class Ellipse : ShapeElement
{
	public override string TagName { get; } = "ellipse";
	[Html("cx")] public int? CX { get; set; }
	[Html("cy")] public int? CY { get; set; }
	[Html("rx")] public int? RX { get; set; }
	[Html("ry")] public int? RY { get; set; }
	[Html("pathLength")] public int? PathLength { get; set; }
}

public class Line : ShapeElement
{
	public override string TagName { get; } = "line";
	[Html("x1")] public int? X1 { get; set; }
	[Html("x2")] public int? X2 { get; set; }
	[Html("y1")] public int? Y1 { get; set; }
	[Html("y2")] public int? Y2 { get; set; }
	[Html("pathLength")] public int? PathLength { get; set; }
	// I know that fill isn't technically valid for line, but css will just ignore it
}

public class Text : SVGElement
{
	public override string TagName { get; } = "text";

	public string? InnerText { get; set; }

	// if true, only the stuff in Content will be rendered
	public bool ContentMode { get; set; } = false;
	// you can put strings, MarkupStrings, RenderFragments and Tspan objects in here
	// strings will be cast to MarkupStrings during render tree construction
	// the GetRenderFragment() method of Tspan objects is called automatically during rendering
	//* Is inherited now
	// public List<object> Content { get; set; } = [];

	[Html("x")] public int? X { get; set; }
	[Html("y")] public int? Y { get; set; }
	[Html("dx")] public int? DX { get; set; }
	[Html("dy")] public int? DY { get; set; }
	[Html("rotate")] public int? Rotate { get; set; }
	[Html("textLength")] public int? TextLength { get; set; }

	[Style("fill")] public string? Fill { get; set; }

	[Html("lengthAdjust")]
	private string? LengthAdjust => StretchLetters is true ? "spacingAndGlyphs" : null;
	public bool StretchLetters { get; set; }

	[Style("font-size")]
	private string? FontSizeString => FontSize != null ? $"{FontSize}px" : null;
	public int? FontSize { get; set; }

	[Style("font-family")] public string? FontFamily { get; set; }

	[Style("user-select")] private string? UserSelect => Selectable != true ? "none" : null;
	public bool? Selectable { get; set; }

	public override RenderFragment GetRenderFragment()
	{
		// render text as a string
		if (!ContentMode)
		{
			return builder =>
			{
				builder.OpenElement(0, TagName);
				builder.AddAttribute(1, "id", Id);
				builder.AddMultipleAttributes(2, GetElementAttributeDictionary());
				builder.AddMultipleAttributes(3, Attributes);
				builder.AddAttribute(4, "style", Style);
				builder.AddMultipleAttributes(5, GetCallbackDictionary());
				builder.AddContent(6, InnerText);
				builder.CloseElement();
			};
		}
		else
		{
			return builder =>
			{
				builder.OpenElement(0, TagName);
				builder.AddAttribute(1, "id", Id);
				builder.AddMultipleAttributes(2, GetElementAttributeDictionary());
				builder.AddMultipleAttributes(3, Attributes);
				builder.AddAttribute(4, "style", Style);
				builder.AddMultipleAttributes(5, GetCallbackDictionary());
				// to not worry about numbers
				builder.OpenRegion(6);
				int i = 0;
				foreach (var c in Content)
				{
					// cover all the possible conversions, ignore other stuff
					if (c is string s) { builder.AddContent(i, s); }
					else if (c is MarkupString m) { builder.AddContent(i, m); }
					else if (c is RenderFragment r) { builder.AddContent(i, r); }
					else if (c is Text t) { builder.AddContent(i, t.GetRenderFragment()); }
					i++;
				}
				builder.CloseRegion();
				builder.CloseElement();
			};
		}
	}
}

public class Tspan : Text
{
	public override string TagName { get; } = "tspan";
}

// probably useful for text and stuff
public class RawMarkup : GameObject
{
	public string Markup { get; set; } = "";

	public override RenderFragment GetRenderFragment()
	{
		return builder =>
		{
			builder.AddContent(0, (MarkupString)Markup);
		};
	}
}

public class Image : SVGElement
{
	public override string TagName { get; } = "image";

	[Html("x")] public int? X { get; set; }
	[Html("y")] public int? Y { get; set; }
	[Html("width")] public int? Width { get; set; }
	[Html("height")] public int? Height { get; set; }

	// [Style("display")] public string? Visibility { get; set; }

	[Html("href")] public string? ImagePath { get; set; }

	// now inherited
	// [Callback("onclick")] public Action<EventArgs>? OnClick { get; set; }

}

public class CustomObject : SVGElement
{
	public override string TagName { get; } = "div";
	//* this must be set or it will cause unexpected behaviour
	public string CustomTagName { get; set; } = null!;

	// public Dictionary<string, object> Attributes { get; set; } = [];
	public Dictionary<string, object> Styles { get; set; } = [];
	public Dictionary<string, Action<EventArgs>> Callbacks { get; set; } = [];

	//* is inherited now
	// public List<object> Content { get; set; } = [];

	public new string CustomStyle => GetStyle();

	private string GetStyle()
	{
		string str = "";
		foreach (var kvp in Styles)
		{
			str += $"{kvp.Key}: {kvp.Value}; ";
		}
		return str;
	}

	public Dictionary<string, object> GetCallbacks()
	{
		Dictionary<string, object> dict = [];
		foreach (var kvp in Callbacks)
		{
			dict.Add(
				kvp.Key,
				EventCallback.Factory.Create(this, kvp.Value)
			);
		}
		return dict;
	}

	public override RenderFragment GetRenderFragment()
	{
		return builder =>
		{
			builder.OpenElement(0, CustomTagName);
			builder.AddAttribute(1, "id", Id);
			builder.AddMultipleAttributes(2, Attributes);
			builder.AddAttribute(3, "style", CustomStyle);
			builder.AddMultipleAttributes(4, GetCallbacks());
			builder.OpenRegion(5);
			int i = 0;
			foreach (var c in Content)
			{
				if (c is GameObject g) { builder.AddContent(i, g.GetRenderFragment()); }
				else if (c is string s) { builder.AddContent(i, s); }
				else if (c is MarkupString m) { builder.AddContent(i, m); }
				else if (c is RenderFragment r) { builder.AddContent(i, r); }
				i++;
			}

			builder.CloseRegion();
			builder.CloseElement();

		};
	}
}

public class ForeignObject : SVGElement
{
	public override string TagName { get; } = "foreignObject";

	[Html("x")] public int? X { get; set; }
	[Html("y")] public int? Y { get; set; }
	[Html("width")] public int? Width { get; set; }
	[Html("height")] public int? Height { get; set; }

	public CustomObject CustomObject { get; set; } = null!;


	public override RenderFragment GetRenderFragment()
	{
		return builder =>
		{
			builder.OpenElement(0, TagName);
			builder.AddAttribute(1, "id", Id);
			builder.AddAttribute(2, "x", X);
			builder.AddAttribute(3, "y", Y);
			builder.AddAttribute(4, "width", Width);
			builder.AddAttribute(5, "height", Height);
			builder.AddMultipleAttributes(3, Attributes);
			builder.AddContent(7, CustomObject.GetRenderFragment());
			builder.CloseElement();
		};
	}
}

public class Dialogue
{
	//private readonly DialogueProgress progress;
	List<List<string>> Messages = [];

	List<string> Speakers = [];
	const int CENTERX = 750;
	const int CENTERY = 530;

	bool ContainsPlayer;

	public Dialogue(List<List<string>> messages)
	{
		Messages = messages;
		Speakers = ExtractUniqueSpeakers(messages);
		ContainsPlayer = Speakers.Contains("Player");

	}

	//*****************
	//HELPER FUNCTIONS
	//*****************

	private List<string> ExtractUniqueSpeakers(List<List<string>> nestedLists)
	{
		HashSet<string> uniqueElements = [];

		foreach (var nestedList in nestedLists)
		{
			if (nestedList.Count > 0)
			{
				uniqueElements.Add(nestedList[0]);
			}
		}

		return new List<string>(uniqueElements);
	}



	private int GetLongestMessage(List<List<string>> messages)
	{
		int longestLength = 0;

		foreach (var message in messages)
		{
			// Ensure that the nested list has at least two elements
			if (message.Count >= 2)
			{
				int currentLength = message[1].Length;
				if (currentLength > longestLength)
				{
					longestLength = currentLength;
				}
			}
		}

		return longestLength;
	}

	//*****************
	//DRAWING FUNCTIONS
	//*****************


	public Image DrawQuitButton()
	{

		return new Image()
		{
			ImagePath = "UI_Images/backImg.png",
			ZIndex = 6,
			X = 100,
			Y = 950,
			Width = 100,
			Height = 100,
		};

	}

	public Image DrawForwardButton()
	{

		return new Image()
		{
			ImagePath = "UI_Images/arrows/right.png",
			ZIndex = 6,
			X = 1400,
			Y = 950,
			Width = 100,
			Height = 100,
		};

	}

	private List<int> AutoPlacement(string currentSpeaker)
	{

		if (Speakers.Count == 1)
		{
			return [CENTERX, CENTERY];
		}

		if (ContainsPlayer)
		{

			Console.WriteLine("Messages Contains Player");

			if (currentSpeaker == "Player")
			{
				return [CENTERX, CENTERY + 300];
			}

			else
			{
				return [CENTERX, CENTERY - 200];
			}

		}
		else
		{

			if (currentSpeaker == Speakers[1])
			{
				return [CENTERX, CENTERY + 300];
			}

			else
			{
				return [CENTERX, CENTERY - 200];
			}
		}
	}

	public GameObjectContainer<SVGElement> DrawSpeechBubble(string speaker, string message, bool autoPlacement = true, int x = CENTERX, int y = CENTERY)
	{

		//Change Position depending on speaker
		if (autoPlacement)
		{
			List<int> positions = AutoPlacement(speaker);
			x = positions[0];
			y = positions[1];
		}

		// Initialize the container for the speech bubble elements
		GameObjectContainer<SVGElement> Bubble = new GameObjectContainer<SVGElement>();
		int fontSize = 20;
		int textHeight = fontSize * 2; // Approximate height based on font size
		int textWidth = message.Length * fontSize / 2;

		// Create the rectangle that acts as the text container
		Rectangle TextContainer = new Rectangle
		{
			X = x,
			Y = y,
			ZIndex = 4,
			Width = textWidth + 20,
			Height = textHeight + 40, // Increased height to accommodate speaker name and message
			Fill = "white"
		};

		// Calculate the widths of the speaker name and message text
		int speakerTextWidth = speaker.Length * fontSize / 2; // Approximate width based on character count
		int messageTextWidth = message.Length * fontSize / 2; // Approximate width based on character count

		// Calculate centered positions
		int containerCenterX = x + (TextContainer.Width / 2);
		int containerCenterY = y + (TextContainer.Height / 2);

		// Adjust the vertical positions to ensure spacing and that both texts are inside the rectangle
		int speakerTextX = containerCenterX - (speakerTextWidth / 2);
		int speakerTextY = y + 20; // Adjust Y position to be inside the rectangle

		int messageTextX = containerCenterX - (messageTextWidth / 2);
		int messageTextY = speakerTextY + fontSize + 10; // Position message below speaker name with some spacing

		// Create the text element for the speaker's message
		Text SpeakerText = new()
		{
			InnerText = message,
			X = messageTextX,
			Y = messageTextY,
			ZIndex = 6,
			FontSize = 20,
			Fill = "black"
		};

		// Create the text element for the speaker's name
		Text SpeakerName = new Text
		{
			InnerText = speaker,
			X = speakerTextX,
			Y = speakerTextY,
			ZIndex = 5,
			FontSize = 20,
			Fill = "black"
		};

		// Add elements to the bubble container
		Bubble.Add(TextContainer);
		Bubble.Add(SpeakerText);
		Bubble.Add(SpeakerName);


		// Return the assembled speech bubble
		return Bubble;
	}


}
