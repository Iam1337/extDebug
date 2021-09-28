/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System;

using extDebug.Menu;

namespace extDebug.Examples.Menu
{
	public class Example : MonoBehaviour
	{
		#region Internal Types

		private enum ExampleEnums
		{
			One,
			Two,
			Three
		}
		
		[Flags]
		private enum ExampleFlags
		{
			One = 1 << 0,
			Two = 1 << 1,
			Three = 1 << 2,
		}

		#endregion
		
		#region Private Vars

		private byte _uint8;

		private UInt16 _uint16;

		private UInt32 _uint32;

		private UInt64 _uint64;

		private sbyte _int8;

		private Int16 _int16;

		private Int32 _int32;

		private Int64 _int64;

		private float _float;

		private bool _bool;

		private ExampleEnums _enum;

		private ExampleFlags _flags;

		private object _longContext = new object();

		private object _infinityContext = new object();

		#endregion

		#region Unity Methods

		private void Start()
		{
			string GetName(Component component) => $"{component.name} ({component.GetType().Name})";
			void ExampleAction(DMAction action) => Debug.Log(action.Data);

			// Simple Menus
			DM.Add("Simple Menus/Action", action => Debug.Log("Hello/Action"), order: 0);
			DM.Add("Simple Menus/UInt8", () => _uint8, v => _uint8 = v, order: 1);
			DM.Add("Simple Menus/UInt16", () => _uint16, v => _uint16 = v, order: 2);
			DM.Add("Simple Menus/UInt32", () => _uint32, v => _uint32 = v, order: 3);
			DM.Add("Simple Menus/UInt64", () => _uint64, v => _uint64 = v, order: 4);
			DM.Add("Simple Menus/Int8", () => _int8, v => _int8 = v, order: 5);
			DM.Add("Simple Menus/Int16", () => _int16, v => _int16 = v, order: 6);
			DM.Add("Simple Menus/Int32", () => _int32, v => _int32 = v, order: 7);
			DM.Add("Simple Menus/Int64", () => _int64, v => _int64 = v, order: 8);
			DM.Add("Simple Menus/Float", () => _float, v => _float = v, order: 9).SetPrecision(2);
			DM.Add("Simple Menus/Bool", () => _bool, v => _bool = v, order: 10);
			DM.Add("Simple Menus/Enum", () => _enum, v => _enum = v, order: 11);
			DM.Add("Simple Menus/Flags", () => _flags, v => _flags = v, order: 12);
			
			// Requests Menus
			// DMBranchRequest
			var branch = DM.Add("Requests Menus/Branch", order: 0).Add(FindObjectsOfType<Component>, GetName);
			branch.Add("Debug.Log", action => { Debug.Log(action.Data); });
			branch.Add("Object.Destroy", action => { Destroy((Component)action.Data); });

			// DMActionRequest
			DM.Add("Requests Menus/Actions", order: 1).Add(FindObjectsOfType<Component>, ExampleAction);

			DM.Open();
		}

		#endregion
	}
}