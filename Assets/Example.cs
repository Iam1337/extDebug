using System;
using extDebug.Menu;
using UnityEngine;

public class Example : MonoBehaviour
{
	private int _int = 0;

	private float _float = 0;

	private bool _bool;

	private enum ExampleEnums
	{
		One,
		Two,
		Three
	}

	private ExampleEnums _enum;

	[Flags]
	private enum ExampleFlags
	{
		One = 1 << 0,
		Two = 1 << 1,
		Three = 1 << 2,
	}

	private ExampleFlags _flags;

	void Start()
	{
		string GetName(Component component) => $"{component.name} ({component.GetType().Name})";
		void Action(DMAction action) => Debug.Log(action.Data); 

		DM.Add("Hello/Action", action => { Debug.Log("Hello/Action"); });
		DM.Add("Hello/World/Action", action => { Debug.Log("Hello/World/Action"); });
		DM.Add("Hello/Integer", () => _int, v => _int = v);
		DM.Add("Hello/Float", () => _float, v => _float = v).SetPrecision(2);
		DM.Add("Hello/Bool", () => _bool, v => _bool = v);
		DM.Add("Hello/Enum", () => _enum, v => _enum = v);
		DM.Add("Hello/Flags", () => _flags, v => _flags = v);
		DM.Add("Hello/Notice", action => { DN.Notify("Notice Example"); });

		// DMBranchRequest
		var branch = DM.Add("Hello/Request Branch").Add(FindObjectsOfType<Component>, GetName);
		branch.Add("Debug.Log", action => { Debug.Log(action.Data); });
		branch.Add("Object.Destroy", action => { Destroy((Component)action.Data); });

		// DMActionRequest
		DM.Add("Hello/Request Action").Add(FindObjectsOfType<Component>, Action);

		// DMFloatRequest
		//DM.Add("Hello/Request Float").Add(FindObjectOfType<Transform>(), ())

		DM.Open();
	}
}
