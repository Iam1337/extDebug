/* Copyright (c) 2024 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System;

namespace extDebug
{
	public static class Hooks
	{
		#region Static Public Vars

		public static Action Update;

		public static Action LateUpdate;

		public static Action FixedUpdate;

		public static Action OnGUI;

		#endregion

		#region Static Public Methods

		static Hooks() =>
			UnityEngine.Object.DontDestroyOnLoad(new GameObject("extDebug Hooks", typeof(HooksBehaviour)));

		#endregion
	}
}