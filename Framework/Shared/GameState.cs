using Microsoft.AspNetCore.Components;



using JsonUtilities;
using Framework.Items;
// using static InventoryEvent;
using ObjectEncoding;
//Notifications
using Blazored.Toast.Services;

using Framework.Toast;
using Blazored.Toast;

namespace Framework.State;

public interface IGameState
{
	// if value exists, set it, else create new entry
	void SetState(string name, bool value);
	bool GetState(string name);
	bool CheckForState(string name);
	bool TryGetState(string name, out bool value);
	void ToggleState(string name);
	bool TryToggleState(string name);

	void AddItem(string id);
	void RemoveItem(string id);
	bool CheckForItem(string id);

	//* Scrapped idea for minigame saving in GameState

	string CurrentSlide { get; set; }

	void SetFromSaveString(string saveString);
	string GetSaveString();

	int RedBullCount { get; }
	void ChangeRedBull(int amount);
}

// interface for the minigames so that they don't accidentally overwrite everything
// they can still brick everything tho, but they have to try harder
public interface IMinigameGameState
{
	void SetState(string name, bool value);
	bool GetState(string name);
	bool CheckForState(string name);
	bool TryGetState(string name, out bool value);
	void ToggleState(string name);
	bool TryToggleState(string name);
	void AddItem(string id);
	void RemoveItem(string id);
	bool CheckForItem(string id);

	int RedBullCount { get; }
	void ChangeRedBull(int amount);
}

public class GameState(JsonUtility jsonUtility, ItemService items, IToastService toastService) : IGameState, IMinigameGameState
{
	// dependencies
	private readonly IToastService ToastService = toastService;
	private readonly JsonUtility JsonUtility = jsonUtility;
	private readonly ItemService Items = items;

	// volume. Not saved but in game state for easy sync between different components
	public double Volume { get; set; } = 0;

	// initialize with empty data
	private GameStateData Data = new();

	// they just point to the data, so you can swap it out by just changing the data, not the properties
	private List<string> ItemsInInventory { get { return Data.Items; } set { Data.Items = value; } }
	private Dictionary<string, bool> State { get { return Data.GameState; } set { Data.GameState = value; } }
	// should be set by the slide service
	public string CurrentSlide { get { return Data.CurrentSlide; } set { Data.CurrentSlide = value; } }

	// Red Bull methods
	private int RedBulls { get { return Data.RedBulls; } set { Data.RedBulls = value; } }
	public int RedBullCount => RedBulls;
	public void ChangeRedBull(int amount)
	{
		RedBulls += amount;
		if (RedBulls < 0)
		{
			RedBulls = 0;
		}
		// to update UI
		OnGameStateChange?.Invoke(this, EventArgs.Empty);
	}

	public event EventHandler? OnGameStateChange;

	public async Task LoadGameStateAndItemsAsync(string path = "gamestate.json")
	{
		State = await JsonUtility.LoadFromJsonAsync<Dictionary<string, bool>>(path);
		await Items.LoadItemsAsync();
	}

	public void SetState(string name, bool value)
	{
		if (!State.TryAdd(name, value))
		{
			State[name] = value;
		}
	}

	public void ToggleState(string name)
	{
		State[name] = !State[name];
	}

	public bool GetState(string name)
	{
		try
		{
			return State[name];
		}
		catch (KeyNotFoundException)
		{
			// Everything is visible by default
			return true;
		}
	}
	public bool CheckForState(string name) => State.ContainsKey(name);
	// kinda pointless, but I had the urge to overengineer
	public bool TryGetState(string name, out bool value) => State.TryGetValue(name, out value);
	public bool TryToggleState(string name)
	{
		if (State.TryGetValue(name, out bool value))
		{
			State[name] = !value;
			return true;
		}
		return false;
	}

	public void RemoveItem(string id)
	{
		bool removed = ItemsInInventory.Remove(id);

		if (!removed)
		{
			return;
			//? maybe not throw an exception?
			// throw new ArgumentException($"Element {id} is not in Inventory");
		}
		// Console.WriteLine($"Successfully removed {id} from inventory");
		// ToastService.ShowSuccess($"Removed {Items.items[id].Name} from inventory");
		ToastParameters parameters = new();
		parameters.Add(nameof(ToastMessage.Message), $"Removed {Items.items[id].Name} from inventory");
		ToastService.ShowToast<ToastMessage>(parameters);
		OnGameStateChange?.Invoke(this, EventArgs.Empty);
	}

	public void AddItem(string id)
	{

		if (Items.DoesItemExist(id) == false)
		{
			throw new Exception("Item doesn't exist in items.json Dictionary");
		}
		// make sure there are no duplicates
		if (ItemsInInventory.Contains(id))
		{
			return;
			//? maybe not throw an exception?
			// throw new ArgumentException($"Element {id} is already in Inventory");
		}
		ItemsInInventory.Add(id);

		// ToastService.ShowSuccess($"Added {Items.items[id].Name} to inventory");

		ToastParameters parameters = new();
		parameters.Add(nameof(ToastMessage.Message), $"Added {Items.items[id].Name} to inventory");
		ToastService.ShowToast<ToastMessage>(parameters);

		// //Event handler for updateing inventory images
		// InventoryEvent.OnGameStateChange(this, new ItemAddedEventArgs { ItemId = id });
		OnGameStateChange?.Invoke(this, EventArgs.Empty);
	}

	public bool CheckForItem(string id)
	{
		return ItemsInInventory.Contains(id);
	}

	public List<string> GetItemStrings() => ItemsInInventory;

	public Dictionary<string, Item> GetItemObjects()
	{
		Dictionary<string, Item> ItemObjects = new();

		foreach (string id in ItemsInInventory)
		{
			if (Items.items.TryGetValue(id, out Item? value))
			{
				ItemObjects.Add(id, value);
			}
		}
		return ItemObjects;
	}

	public string GetSaveString()
	{
		return ObjectEncoder.EncodeObject(Data);
	}

	public void SetFromSaveString(string hex)
	{
		GameStateData data = ObjectEncoder.DecodeObject<GameStateData>(hex) ??
		throw new Exception("GameStateData is null");
		Data = data;
	}
}

public class GameStateData
{
	public List<string> Items { get; set; } = [];
	public Dictionary<string, bool> GameState { get; set; } = [];
	public string CurrentSlide { get; set; } = "";
	public int RedBulls { get; set; } = 0;

	//public Dictionary<string, Dictionary<string, > DialogueProgress {get; set;} = [];
}

public class DialogueProgress
{

	public string QuestName { get; set; }
	public List<List<string>> Messages { get; set; }
	public int Progress { get; set; }
	public DialogueProgress(string questName, List<List<string>> messages, int progress)
	{
		QuestName = questName;
		Messages = messages;
		Progress = progress;
	}

}

