/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System;
using System.Collections.Generic;

namespace extDebug.Menu
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

			public Color Description;
		}

		#endregion

		#region Static Public Vars

		// Colors
		public static readonly ColorScheme Colors = new ColorScheme
		{
			Name = new Color32(238, 238, 238, 255),
			NameFlash = new Color32(255, 255, 0, 255),

			Value = new Color32(201, 227, 219, 255),
			ValueFlash = new Color32(255, 255, 0, 255),

			ToggleDisabled = new Color32(201, 227, 219, 255),
			ToggleEnabled = new Color32(255, 255, 0, 255),

			Action = new Color32(238, 238, 238, 255),
			ActionFlash = new Color32(90, 177, 144, 255),
			ActionDisabled = new Color32(112, 112, 112, 255),

			Description = new Color32(112, 112, 112, 255)
		};

		// Main
		public static readonly DMBranch Root = new DMBranch(null, "Debug Menu");

		public static IDMInput Input = new DMDefaultInput();

		public static IDMRender Render = new DMDefaultRender();

		public static IDMNotice Notice = new DMDefaultNotice();

		// Manage
		public static bool IsVisible { get; private set; }

		#endregion

		#region Private Vars

		private const float kRepeatDelay = 0.75f;

		private const float kRepeatInterval = 0.1f;

		private static DMBranch _currentBranch;

		private static DMBranch _previousBranch => _branchesStack.Count > 0 ? _branchesStack.Peek() : null;

		private static readonly Stack<DMBranch> _branchesStack = new Stack<DMBranch>();

		private static EventKey _previousKey;

		private static float _repeatTime;

		#endregion

		#region Public Methods

		static DM()
		{
			Hooks.Update += Update;
		}

		public static void Open() => Open(Root);

		public static void Open(DMBranch branch)
		{
			if (branch == null)
				throw new ArgumentNullException(nameof(branch));

			if (_currentBranch != null)
			{
				_branchesStack.Push(_currentBranch);
			}

            _currentBranch = branch;
            _currentBranch.SendEvent(EventArgs.OpenBranch);
			_currentBranch.RequestRepaint();

            IsVisible = true;
		}

		public static void Back()
		{
			if (_previousBranch != null)
			{
				_currentBranch.SendEvent(EventArgs.CloseBranch);

				_currentBranch = _previousBranch;
				_currentBranch.SendEvent(EventArgs.OpenBranch);
				_currentBranch.RequestRepaint();

				_branchesStack.Pop();
			}
			else
			{
				IsVisible = false;
			}
		}

		public static void Notify(DMItem item, Color? nameColor = null, Color? valueColor = null) => Notice?.Notify(item, nameColor, valueColor);

		// Branch
		public static DMBranch Add(string path, string description = "", int order = 0) => Add(Root, path, description, order);

		public static DMBranch Add(DMBranch parent, string path, string description = "", int order = 0) => parent == null ? new DMBranch(null, path, description, order) : Root.Get(path) ?? new DMBranch(parent, path, description, order);

		// Action
		public static DMAction Add(string path, Action<DMAction> action, string description = "", int order = 0) => Add(Root, path, action, description, order);

		public static DMAction Add(DMBranch parent, string path, Action<DMAction> action, string description = "", int order = 0) => new DMAction(parent, path, description, action, order);

		// Bool
		public static DMBool Add(string path, Func<bool> getter, Action<bool> setter = null, int order = 0) => Add(Root, path, getter, setter, order);

		public static DMBool Add(DMBranch parent, string path, Func<bool> getter, Action<bool> setter = null, int order = 0) => new DMBool(parent, path, getter, setter, order);

		// Enum
		public static DMEnum<T> Add<T>(string path, Func<T> getter, Action<T> setter = null, int order = 0) where T : struct, Enum => Add(Root, path, getter, setter, order);

		public static DMEnum<T> Add<T>(DMBranch parent, string path, Func<T> getter, Action<T> setter = null, int order = 0) where T : struct, Enum => new DMEnum<T>(parent, path, getter, setter, order);

		// UInt8
		public static DMUInt8 Add(string path, Func<byte> getter, Action<byte> setter = null, int order = 0) => Add(Root, path, getter, setter, order);

		public static DMUInt8 Add(DMBranch parent, string path, Func<byte> getter, Action<byte> setter = null, int order = 0) => new DMUInt8(parent, path, getter, setter, order);

		// UInt16
		public static DMUInt16 Add(string path, Func<UInt16> getter, Action<UInt16> setter = null, int order = 0) => Add(Root, path, getter, setter, order);

		public static DMUInt16 Add(DMBranch parent, string path, Func<UInt16> getter, Action<UInt16> setter = null, int order = 0) => new DMUInt16(parent, path, getter, setter, order);

		// UInt32
		public static DMUInt32 Add(string path, Func<UInt32> getter, Action<UInt32> setter = null, int order = 0) => Add(Root, path, getter, setter, order);

		public static DMUInt32 Add(DMBranch parent, string path, Func<UInt32> getter, Action<UInt32> setter = null, int order = 0) => new DMUInt32(parent, path, getter, setter, order);
		
		// UInt64
		public static DMUInt64 Add(string path, Func<UInt64> getter, Action<UInt64> setter = null, int order = 0) => Add(Root, path, getter, setter, order);

		public static DMUInt64 Add(DMBranch parent, string path, Func<UInt64> getter, Action<UInt64> setter = null, int order = 0) => new DMUInt64(parent, path, getter, setter, order);

		// Int8
		public static DMInt8 Add(string path, Func<sbyte> getter, Action<sbyte> setter = null, int order = 0) => Add(Root, path, getter, setter, order);

		public static DMInt8 Add(DMBranch parent, string path, Func<sbyte> getter, Action<sbyte> setter = null, int order = 0) => new DMInt8(parent, path, getter, setter, order);

		// Int16
		public static DMInt16 Add(string path, Func<Int16> getter, Action<Int16> setter = null, int order = 0) => Add(Root, path, getter, setter, order);

		public static DMInt16 Add(DMBranch parent, string path, Func<Int16> getter, Action<Int16> setter = null, int order = 0) => new DMInt16(parent, path, getter, setter, order);

		// Int32
		public static DMInt32 Add(string path, Func<Int32> getter, Action<Int32> setter = null, int order = 0) => Add(Root, path, getter, setter, order);

		public static DMInt32 Add(DMBranch parent, string path, Func<Int32> getter, Action<Int32> setter = null, int order = 0) => new DMInt32(parent, path, getter, setter, order);

		// Int64
		public static DMInt64 Add(string path, Func<Int64> getter, Action<Int64> setter = null, int order = 0) => Add(Root, path, getter, setter, order);

		public static DMInt64 Add(DMBranch parent, string path, Func<Int64> getter, Action<Int64> setter = null, int order = 0) => new DMInt64(parent, path, getter, setter, order);

		// Float
		public static DMFloat Add(string path, Func<float> getter, Action<float> setter = null, int order = 0) => Add(Root, path, getter, setter, order);

		public static DMFloat Add(DMBranch parent, string path, Func<float> getter, Action<float> setter = null, int order = 0) => new DMFloat(parent, path, getter, setter, order);

		#endregion

		#region Private Methods

		private static EventKey GetKey(out bool shift)
		{
			if (Input != null)
			{
				var eventKey = Input.GetKey(out shift);
				if (eventKey != _previousKey)
				{
					_previousKey = eventKey;
					_repeatTime = Time.unscaledTime + kRepeatDelay;

					return eventKey;
				}

				if (eventKey != EventKey.None && _repeatTime < Time.unscaledTime)
				{
					_repeatTime = Time.unscaledTime + kRepeatInterval;

					return eventKey;
				}
			}

			shift = false;
			
			return EventKey.None;
		}

		private static void SendKey(EventKey eventKey, bool isShift)
		{
			if (eventKey == EventKey.ToggleMenu)
			{
				IsVisible = !IsVisible;
			}

			_currentBranch?.SendEvent(new EventArgs
			{
				Tag = EventTag.Input,
				Key = eventKey,
				IsShift = isShift
			});
		}
		
		private static void Update()
		{
			// Input
			if (Input != null)
			{
				var eventKey = GetKey(out var isShift);
				if (eventKey != EventKey.None)
				{
					SendKey(eventKey, isShift);
				}
			}

			// Render
			if (IsVisible && Render != null && _currentBranch != null && _currentBranch.CanRepaint())
			{
				var items = _currentBranch.GetItems();

				foreach (var item in items)
				{
					item.SendEvent(EventArgs.Repaint);
				}

				Render.Repaint(_currentBranch, items);

				_currentBranch.CompleteRepaint();
			}
		}

		#endregion
	}
}