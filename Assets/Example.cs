using UnityEngine;

using System;

using extDebug.Menu;
using extDebug.Notifications;

namespace extDebug.Example
{
	public class Example : MonoBehaviour
	{
		private byte _uint8;

		private UInt16 _uint16;

		private UInt32 _uint32;

		private UInt64 _uint64;

		private sbyte _int8;

		private Int16 _int16;

		private Int32 _int32;

		private Int64 _int64;

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
			DM.Add("Hello/UInt8", () => _uint8, v => _uint8 = v);
			DM.Add("Hello/UInt16", () => _uint16, v => _uint16 = v);
			DM.Add("Hello/UInt32", () => _uint32, v => _uint32 = v);
			DM.Add("Hello/UInt64", () => _uint64, v => _uint64 = v);
			DM.Add("Hello/Int8", () => _int8, v => _int8 = v);
			DM.Add("Hello/Int16", () => _int16, v => _int16 = v);
			DM.Add("Hello/Int32", () => _int32, v => _int32 = v);
			DM.Add("Hello/Int64", () => _int64, v => _int64 = v);
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
}