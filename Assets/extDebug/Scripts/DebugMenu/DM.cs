/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System;
using System.Text;
using System.Collections.Generic;

namespace extDebug
{
	public static class DM
	{
		#region External

		public struct ColorScheme
		{
			public Color Name;
			public Color NameFlash;

			public Color Value;
			public Color ValueFlash;

			public Color ToggleDisabled;
			public Color ToggleEnabled;

			public Color Action;
			public Color ActionFlash;
			public Color ActionDisabled;
		}

		#endregion

		#region Static Public Vars

		// Colors
		public static readonly ColorScheme Colors = new ColorScheme
		{
			Name = new Color32(238, 238, 238, 255),
			NameFlash = new Color32(255, 255, 0, 255),

			Value = new Color(201, 227, 219, 255),
			ValueFlash = new Color32(255, 255, 0, 255),

			ToggleDisabled = new Color32(255, 255, 0, 255),
			ToggleEnabled = new Color32(255, 255, 0, 255),

			Action = new Color32(238, 238, 238, 255),
			ActionFlash = new Color32(90, 177, 144, 255),
			ActionDisabled = new Color32(112, 112, 112, 255)
		};

		// Main
		public static readonly DMBranch Root = new DMBranch(null, "Debug Menu");

		public static IDMInput Input = new DMDefaultInput();

		public static IDMRender Render = new DMDefaultRender();

		// Manage
		public static bool IsVisible;

		#endregion

		#region Private Vars

		private static DMBranch _currentBranch;

		private static DMBranch _previousBranch => _branchesStack.Count > 0 ? _branchesStack.Peek() : null;

		private static readonly Stack<DMBranch> _branchesStack = new Stack<DMBranch>();

		#endregion

		#region Public Methods

		static DM()
		{
			Hooks.UpdateCallback += Update;
		}

		public static void Open() => Open(Root);

		public static void Open(DMBranch branch)
		{
			if (_currentBranch != null)
			{
				_branchesStack.Push(_currentBranch);
			}

            _currentBranch = branch;
            _currentBranch.SendEvent(EventTag.OpenBranch);

            IsVisible = true;
		}

		public static void Back()
		{
			if (_previousBranch != null)
			{
				_currentBranch.SendEvent(EventTag.CloseBranch);

				_currentBranch = _previousBranch;
				_currentBranch.SendEvent(EventTag.OpenBranch);

				_branchesStack.Pop();
			}
			else
			{
				IsVisible = false;
			}
		}

		public static void SendEvent(EventTag eventTag)
		{
			if (eventTag == EventTag.ToggleMenu)
			{
				IsVisible = !IsVisible;
			}

			if (_currentBranch != null) 
				_currentBranch.SendEvent(eventTag);
		}


		public static void Notify(DMItem item, Color? nameColor = null, Color? valueColor = null)
		{ }

		public static DMBranch Add(string path, string description = "", int order = 0) => Add(Root, path, description, order);

		public static DMBranch Add(DMBranch parent, string path, string description = "", int order = 0) => parent == null ? new DMBranch(null, path, description, order) : Root.Get(path) ?? new DMBranch(parent, path, description, order);

		// Action
		public static DMAction Add(string path, Action<DMAction> action, string description = "", int order = 0) => Add(Root, path, action, description, order);

		public static DMAction Add(DMBranch parent, string path, Action<DMAction> action, string description = "", int order = 0) => new DMAction(parent, path, description, action, order);

		// Integer
		public static DMInteger Add(string path, Func<int> getter, Action<int> setter, int order = 0) => Add(Root, path, getter, setter, order);

		public static DMInteger Add(DMBranch parent, string path, Func<int> getter, Action<int> setter = null, int order = 0) => new DMInteger(parent, path, getter, setter, order);

		// Float
		public static DMFloat Add(string path, Func<float> getter, Action<float> setter, int order = 0) => Add(Root, path, getter, setter, order);

		public static DMFloat Add(DMBranch parent, string path, Func<float> getter, Action<float> setter = null, int order = 0) => new DMFloat(parent, path, getter, setter, order);

		// Bool
		public static DMBool Add(string path, Func<bool> getter, Action<bool> setter, int order = 0) => Add(Root, path, getter, setter, order);

		public static DMBool Add(DMBranch parent, string path, Func<bool> getter, Action<bool> setter = null, int order = 0) => new DMBool(parent, path, getter, setter, order);

		// Enum
		public static DMEnum<T> Add<T>(string path, Func<T> getter, Action<T> setter, int order = 0) where T : struct, Enum => Add(Root, path, getter, setter, order);

		public static DMEnum<T> Add<T>(DMBranch parent, string path, Func<T> getter, Action<T> setter = null, int order = 0) where T : struct, Enum => new DMEnum<T>(parent, path, getter, setter, order);

		#endregion

		#region Private Methods

		private const float kRepeatDelay = 0.75f;

		private const float kRepeatInterval = 0.1f;

		private static EventTag _previousEvent;

		private static float _repeatTime;

		private static EventTag GetEvent()
		{
			if (Input != null)
			{
				var eventArg = Input.GetEvent();
				if (eventArg != _previousEvent)
				{
					_previousEvent = eventArg;
					_repeatTime = Time.unscaledTime + kRepeatDelay;

					return eventArg;
				}

				if (eventArg != EventTag.None && _repeatTime < Time.unscaledTime)
				{
					_repeatTime = Time.unscaledTime + kRepeatInterval;

					return eventArg;
				}
			}

			return EventTag.None;
		}

		private static void Update()
		{
			// Input
			if (Input != null)
			{
				var eventTag = GetEvent();
				if (eventTag != EventTag.None)
				{
					SendEvent(eventTag);
				}
			}

			// Render
			if (Render != null && _currentBranch != null && _currentBranch.CanRepaint())
				Render.Repaint(_currentBranch);
		}

		#endregion
	}
}