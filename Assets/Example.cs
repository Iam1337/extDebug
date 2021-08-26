using extDebug;
using UnityEngine;

public class Example : MonoBehaviour
{
	private int _int = 0;

	private float _float = 0;

	private bool _bool;

	void Start()
	{
		string GetName(Component component) => $"{component.name} ({component.GetType().Name})";
		void Action(DMAction action) => Debug.Log(action.Data); 

		DM.Add("Hello/Action 1", action => { Debug.Log("Action 1"); });
		DM.Add("Hello/Action 2", action => { Debug.Log("Action 2"); });
		DM.Add("Hello/World/Action 3", action => { Debug.Log("Action 3"); });
		DM.Add("Hello/World/Action 4", action => { Debug.Log("Action 4"); });
		DM.Add("Hello/Integer", () => _int, v => _int = v);
		DM.Add("Hello/Float", () => _float, v => _float = v).SetPrecision(2);
		DM.Add("Hello/Bool", () => _bool, v => _bool = v);

		// DMBranchRequest
		var branch = DM.Add("Hello/Request Branch").Add(FindObjectsOfType<Component>, GetName);
		branch.Add("Debug.Log", action => { Debug.Log(action.Data); });
		branch.Add("Object.Destroy", action => { Destroy((Component)action.Data); });

		// DMActionRequest
		DM.Add("Hello/Request Action").Add(FindObjectsOfType<Component>, Action);

		// DMFloatRequest
		//DM.Add("Hello/Request Float").Add(FindObjectOfType<Transform>(), ())

		DM.IsVisible = true;
		DM.Open();
	}

	void Update()
	{
		DM.Update();
	}

	void OnGUI()
	{
		if (DM.IsVisible)
			GUILayout.Label(DM.Build());
	}
}
