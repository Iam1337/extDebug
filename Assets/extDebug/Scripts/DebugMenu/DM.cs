/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

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

		// Manage
		public static bool IsVisible;

		// Colors
		public static ColorScheme Colors = new ColorScheme()
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

		#endregion

		#region Private Vars

		private static DMBranch _currentBranch;

		private static DMBranch _previousBranch => _branchesStack.Count > 0 ? _branchesStack.Peek() : null;

		private static Stack<DMBranch> _branchesStack = new Stack<DMBranch>();

		private static readonly StringBuilder _builder = new StringBuilder();

		#endregion

		#region Public Methods

		public static void Open() => Open(Root);

		public static void Open(DMBranch branch) // TODO: Change name on Switch?
		{
			if (_currentBranch != null)
			{
				_branchesStack.Push(_currentBranch);
			}

            _currentBranch = branch;
            _currentBranch.SendEvent(new EventArgs
            {
                Event = EventType.OpenMenu,
                Key = KeyType.None
            });
        }

		public static void Back()
		{
			if (_previousBranch != null)
			{
				_currentBranch.SendEvent(new EventArgs
                {
                    Event = EventType.CloseMenu,
                    Key = KeyType.None
                });

				_currentBranch = _previousBranch;
				_currentBranch.SendEvent(new EventArgs
				{
					Event = EventType.OpenMenu,
					Key = KeyType.None
				});

				_branchesStack.Pop();
			}
			else
			{
				Close();
			}
		}

		public static void Close()
		{

		}

		public static void SendEvent(EventArgs eventArgs)
		{
			if (_currentBranch != null)
				_currentBranch.SendEvent(eventArgs);
		}

		public static void Notify(DMItem item, Color? nameColor = null, Color? valueColor = null)
		{ }

		public static DMBranch Add(string path, string description = "", int order = 0)
		{
			return Add(Root, path, description, order);
		}

		public static DMBranch Add(DMBranch parent, string path, string description = "", int order = 0)
		{
			return Root.Get(path) ?? new DMBranch(parent, path, description, order);
		}

		public static DMAction Add(string path, Action<DMAction, EventArgs> action, string description = "", int order = 0)
		{
			return Add(Root, path, action, description, order);
		}

		public static DMAction Add(DMBranch parent, string path, Action<DMAction, EventArgs> action, string description = "", int order = 0)
		{
			return new DMAction(parent, path, description, action, order);
		}

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