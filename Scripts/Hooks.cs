/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;
using UnityEngine;

namespace extDebug
{
	public class Hooks : MonoBehaviour
	{
		#region Static Public Vars

		public static Action UpdateCallback;

		public static Action LateUpdateCallback;

		public static Action ImGuiCallback;

		#endregion

		#region Static Public Methods

		static Hooks() => DontDestroyOnLoad(new GameObject("extDebug Hooks", typeof(Hooks)));

        #endregion

        #region Unity Methods

        protected void Update() => UpdateCallback?.Invoke();

        protected void LateUpdate() => LateUpdateCallback?.Invoke();

		protected void OnGUI() => ImGuiCallback?.Invoke();

		#endregion
	}
}