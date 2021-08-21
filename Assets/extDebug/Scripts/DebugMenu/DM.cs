/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System;
using System.Collections.Generic;
using System.Text;

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

		// Main
		public static readonly DMBranch Root = new DMBranch(null, "Debug Menu");

		// Colors
		public static readonly ColorScheme Colors = new ColorScheme()
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

		// Manage
		public static bool IsVisible;

		#endregion

		#region Private Vars

		private static DMBranch _currentBranch;

		private static DMBranch _previousBranch => _branchesStack.Count > 0 ? _branchesStack.Peek() : null;

		private static Stack<DMBranch> _branchesStack = new Stack<DMBranch>();

		private static readonly StringBuilder _builder = new StringBuilder();

		#endregion

		#region Public Methods

		public static void Open() => Open(Root);

		public static void Open(DMBranch branch)
		{
			if (_currentBranch != null)
			{
				_branchesStack.Push(_currentBranch);
			}

            _currentBranch = branch;
            _currentBranch.SendEvent(new EventArgs
            {
                Event = EventType.OpenBranch,
                Key = KeyType.None
            });
        }

		public static void Back()
		{
			if (_previousBranch != null)
			{
				_currentBranch.SendEvent(new EventArgs
                {
                    Event = EventType.CloseBranch,
                    Key = KeyType.None
                });

				_currentBranch = _previousBranch;
				_currentBranch.SendEvent(new EventArgs
				{
					Event = EventType.OpenBranch,
					Key = KeyType.None
				});

				_branchesStack.Pop();
			}
			else
			{
				Hide();
			}
		}

		public static void Show()
		{ }

		public static void Hide()
		{ }


		public static void SendEvent(EventArgs eventArgs)
		{
			if (_currentBranch != null) 
				_currentBranch.SendEvent(eventArgs);
		}

		public static void Notify(DMItem item, Color? nameColor = null, Color? valueColor = null)
		{ }

		public static DMBranch Add(string path, string description = "", int order = 0) => Add(Root, path, description, order);

		public static DMBranch Add(DMBranch parent, string path, string description = "", int order = 0) => parent == null ? new DMBranch(null, path, description, order) : Root.Get(path) ?? new DMBranch(parent, path, description, order);

		// Action
		public static DMAction Add(string path, Action<DMAction, EventArgs> action, string description = "", int order = 0) => Add(Root, path, action, description, order);

		public static DMAction Add(DMBranch parent, string path, Action<DMAction, EventArgs> action, string description = "", int order = 0) => new DMAction(parent, path, description, action, order);

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

		public static string Build()
		{
			_builder.Clear();
			_currentBranch.Build(_builder);
			return _builder.ToString();
		}

		#endregion
	}
}