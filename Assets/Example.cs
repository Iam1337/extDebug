using extDebug;
using UnityEngine;
using EventType = extDebug.EventType;

public class Example : MonoBehaviour
{
	private int valueInt = 0;

	void Start()
	{
		string GetName(Component component) => $"{component.name} ({component.GetType().Name})";
		void Action(DMAction action, EventArgs args) => Debug.Log(action.Data); 

		DM.Add("Hello/Action 1", (action, args) => { Debug.Log("Action 1"); });
		DM.Add("Hello/Action 2", (action, args) => { Debug.Log("Action 2"); });
		DM.Add("Hello/World/Action 3", (action, args) => { Debug.Log("Action 3"); });
		DM.Add("Hello/World/Action 4", (action, args) => { Debug.Log("Action 4"); });
		DM.Add("Hello/Integer", () => valueInt, v => valueInt = v);

		// DMBranchRequest
		var branch = DM.Add("Hello/Request Branch").Branch(FindObjectsOfType<Component>, GetName);
		branch.Add("Debug.Log", (action, args) => { Debug.Log(action.Data); });
		branch.Add("Object.Destroy", (action, args) => { Object.Destroy((Component)action.Data); });

		// DMActionRequest
		DM.Add("Hello/Request Action").Action(FindObjectsOfType<Component>, Action);

		

		DM.IsVisible = true;
		DM.Open();
	}

	void Update()
	{
		var keyDown = GetKeyDown();
		if (keyDown != KeyType.None)
		{
			DM.SendEvent(new EventArgs
			{
				Event = EventType.KeyDown,
				Key = keyDown
			});
		}

		var keyUp = GetKeyUp();
		if (keyUp != KeyType.None)
		{
			DM.SendEvent(new EventArgs
			{
				Event = EventType.KeyUp,
				Key = keyUp
			});
		}
	}

	void OnGUI()
	{
		GUILayout.Label(DM.Build());
	}

	KeyType GetKeyDown()
	{
		if (Input.GetKeyDown(KeyCode.W))
			return KeyType.Up;
		if (Input.GetKeyDown(KeyCode.S))
			return KeyType.Down;
		if (Input.GetKeyDown(KeyCode.A))
			return KeyType.Left;
		if (Input.GetKeyDown(KeyCode.D))
			return KeyType.Right;
		if (Input.GetKeyDown(KeyCode.R))
			return KeyType.Reset;

		return KeyType.None;
	}

	KeyType GetKeyUp()
	{
		if (Input.GetKeyUp(KeyCode.W))
			return KeyType.Up;
		if (Input.GetKeyUp(KeyCode.S))
			return KeyType.Down;
		if (Input.GetKeyUp(KeyCode.A))
			return KeyType.Left;
		if (Input.GetKeyUp(KeyCode.D))
			return KeyType.Right;
		if (Input.GetKeyUp(KeyCode.R))
			return KeyType.Reset;

		return KeyType.None;
	}
}
