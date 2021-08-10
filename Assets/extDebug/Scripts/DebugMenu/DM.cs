/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;
using System.Text;
using UnityEngine;

namespace extDebug
{
	public static class DM
	{
		#region External

		public struct ColorScheme
		{
			public Color Label;
			public Color LabelFlash;

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
			Label = (ColorUtility.TryParseHtmlString("eeeeee", out var color0) ? color0 : Color.clear),
			LabelFlash = (ColorUtility.TryParseHtmlString("ffff00", out var color1) ? color1 : Color.clear),

			Value = (ColorUtility.TryParseHtmlString("c9e3db", out var color2) ? color2 : Color.clear),
			ValueFlash = (ColorUtility.TryParseHtmlString("ffff00", out var color3) ? color3 : Color.clear),

			ToggleDisabled = (ColorUtility.TryParseHtmlString("ffff00", out var color4) ? color4 : Color.clear),
			ToggleEnabled = (ColorUtility.TryParseHtmlString("ffff00", out var color5) ? color5 : Color.clear),

			Action = (ColorUtility.TryParseHtmlString("eeeeee", out var color7) ? color7 : Color.clear),
			ActionFlash = (ColorUtility.TryParseHtmlString("5ab190", out var color8) ? color8 : Color.clear),
			ActionDisabled = (ColorUtility.TryParseHtmlString("707070", out var color6) ? color6 : Color.clear),
		};

		#endregion

		#region Private Vars

		private static DMBranch _currentBranch;

		private static readonly StringBuilder _builder = new StringBuilder();

		#endregion

		#region Public Methods

		public static void Open(DMBranch branch) // TODO: Change name on Switch?
		{
			_currentBranch = branch;
			_currentBranch.SendEvent(new EventArgs
			{
				Event = EventType.OpenMenu,
				Key = KeyType.None
			});


		}

		public static void Close()
		{

		}

		public static void Back()
		{
			if (_currentBranch.Parent != null)
			{
				_currentBranch.SendEvent(new EventArgs
				{
					Event = EventType.CloseMenu,
					Key = KeyType.None
				});

				Open(_currentBranch.Parent);
			}
			else
			{
				Close();	
			}
		}


		public static void Notify(DMItem item, Color? nameColor = null, Color? valueColor = null)
		{ }

		public static DMBranch Add(string path, string description = "", int order = 0)
		{
			return Add(Root, path, description, order);
		}

		public static DMBranch Add(DMBranch parent, string path, string description = "", int order = 0)
		{
			return new DMBranch(parent, path, description, order);
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

		private static string Build(DMBranch branch)
		{
			_builder.Clear();
			branch.Build(_builder);
			return _builder.ToString();
		}

		#endregion
	}
}