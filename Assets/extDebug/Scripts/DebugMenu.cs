/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

namespace extDebug
{
	public static class DebugMenu
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
		public static DebugMenuBranch Root => null;

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

		#endregion

		#region Public Methods

		public static void Open(DebugMenuBranch branch)
		{

		}

		public static void Back()
		{ }


		public static void Notify(DebugMenuItem item, Color? nameColor = null, Color? valueColor = null)
		{ }


		#endregion
	}
}