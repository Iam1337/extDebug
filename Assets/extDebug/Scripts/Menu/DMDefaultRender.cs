/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System;
using System.Text;
using System.Collections.Generic;

namespace extDebug.Menu
{
	internal class DMDefaultRender : IDMRender, IDMRender_OnGUI
	{
		#region Private Vars

		private readonly StringBuilder _builder = new StringBuilder();

		private string _text;

		#endregion

		#region IDMRender Methods
		
		void IDMRender.Repaint(DMBranch branch, IReadOnlyList<DMItem> items)
		{
			const string kSuffix = " ";
			const string kPrefix = " ";
			const string kPrefix_Selected = ">";
			const string kSpace = "  ";
			const char kHorizontalChar = '─';

			CalculateLengths(branch, items, kSpace.Length, out var fullLength, out var maxNameLength, out var maxValueLength);

			var order = -1;
			var lineLength = fullLength + kSuffix.Length + kPrefix.Length;
			var lineEmpty = new string(kHorizontalChar, lineLength);

			// header
			_builder.Clear();
			_builder.AppendFormat($"{kPrefix}<color=#{ColorUtility.ToHtmlStringRGB(branch.NameColor)}>{{0,{-fullLength}}}</color>{kSuffix}{Environment.NewLine}", branch.Name);
			_builder.AppendLine(lineEmpty);

			// items
			for (var i = 0; i < items.Count; i++)
			{
				var item = items[i];
				var prefix = item == branch.Current ? kPrefix_Selected : kPrefix;

				// items separator
				if (order >= 0 && Math.Abs(order - item.Order) > 1)
					_builder.AppendLine(lineEmpty);

				order = item.Order;

				var name = item.Name;
				var value = item.Value;

				if (item is DMBranch)
					name += "...";

				if (string.IsNullOrEmpty(value))
				{
					// only name
					_builder.AppendFormat($"{prefix}<color=#{ColorUtility.ToHtmlStringRGB(item.NameColor)}>{{0,-{fullLength}}}</color>{kSuffix}{Environment.NewLine}", name);
				}
				else
				{
					// with value
					_builder.AppendFormat($"{prefix}<color=#{ColorUtility.ToHtmlStringRGB(item.NameColor)}>{{0,-{maxNameLength}}}</color>{kSpace}<color=#{ColorUtility.ToHtmlStringRGB(item.ValueColor)}>{{1,-{maxValueLength}}}</color>{kSuffix}{Environment.NewLine}", name, value);
				}
			}

			_builder.Remove(_builder.Length - Environment.NewLine.Length, Environment.NewLine.Length);
			_text = _builder.ToString();
		}

		void IDMRender_OnGUI.OnGUI()
		{
			if (!DM.IsVisible)
				return;

			var textSize = GUI.skin.label.CalcSize(new GUIContent(_text)) + new Vector2(10, 10);
			var position = new Vector2(20, 20);
			var rect = new Rect(position, textSize);

			GUI.Box(rect, GUIContent.none);

			rect.x += 5f;
			rect.width -= 5f * 2f;
			rect.y += 5f;
			rect.height -= 5f * 2f;

			GUI.Label(rect, _text);
		}
		
        #endregion

        #region Private Methods

        private void CalculateLengths(DMBranch branch, IReadOnlyList<DMItem> items, int space, out int fullLength, out int maxNameLength, out int maxValueLength)
		{
			maxNameLength = 0;
			maxValueLength = 0;
			fullLength = branch.Name.Length;

			for (var i = 0; i < items.Count; i++)
			{
				var item = items[i];
				var nameLength = item.Name.Length;
				var valueLength = item.Value.Length;

				if (item is DMBranch)
					nameLength += 3;

				maxNameLength = Math.Max(maxNameLength, nameLength);
				maxValueLength = Math.Max(maxValueLength, valueLength);
			}

			fullLength = Math.Max(fullLength, maxNameLength + maxValueLength + space);
			maxNameLength = Math.Max(maxNameLength, fullLength - maxValueLength - space);
		}

		#endregion
	}
}