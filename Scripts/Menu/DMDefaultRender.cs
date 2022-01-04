/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System;
using System.Text;
using System.Collections.Generic;

namespace extDebug.Menu
{
	internal class DMDefaultRender : IDMRender, IDMRender_OnGUI
	{
		#region Static Private Vars

		private static readonly GUISkin _skin = Resources.Load<GUISkin>("extDebug/Skins/Default Skin");

		#endregion

		#region Private Vars

		private readonly StringBuilder _builder = new StringBuilder();

		private string _text;

		#endregion

		#region IDMRender Methods

		void IDMRender.Repaint(DMBranch branch, IReadOnlyList<DMItem> items)
		{
			const string kPrefix = " ";
			const string kPrefix_Selected = ">";
			const string kSpace = "  ";
			const char kHorizontalChar = '─';

			CalculateLengths(branch, items, kSpace.Length, 
				out var fullLength, 
				out var maxNameLength, 
				out var maxValueLength, 
				out var maxDescriptionLength);

			var order = -1;
			var lineLength = fullLength + kPrefix.Length;
			var lineEmpty = new string(kHorizontalChar, lineLength);

			// header
			_builder.Clear();
			_builder.AppendFormat($"{kPrefix}<color=#{ColorUtility.ToHtmlStringRGB(branch.NameColor)}>{{0,{-fullLength}}}</color>{Environment.NewLine}", branch.Name);
			_builder.AppendLine(lineEmpty);

			// items
			for (var i = 0; i < items.Count; i++)
			{
				var item = items[i];
				var selected = item == branch.Current;
				var prefix = selected ? kPrefix_Selected : kPrefix;
				var alpha = selected ? 1 : 0.75f;

				// items separator
				if (order >= 0 && Math.Abs(order - item.Order) > 1)
					_builder.AppendLine(lineEmpty);

				order = item.Order;

				var name = item.Name;
				var value = item.Value;
				var description = item.Description;

				if (item is DMBranch)
					name += "...";

				_builder.AppendFormat($"{prefix}<color=#{ColorUtility.ToHtmlStringRGB(item.NameColor * alpha)}>{{0,{-maxNameLength}}}</color>", name);
				_builder.AppendFormat($"{kSpace}<color=#{ColorUtility.ToHtmlStringRGB(item.ValueColor * alpha)}>{{0,{maxValueLength}}}</color>", value);
				_builder.AppendFormat($"{kSpace}<color=#{ColorUtility.ToHtmlStringRGB(item.DescriptionColor * alpha)}>{{0,{-maxDescriptionLength}}}</color>", description);
				_builder.Append(Environment.NewLine);
			}

			_builder.Remove(_builder.Length - Environment.NewLine.Length, Environment.NewLine.Length);
			_text = _builder.ToString();
		}

		void IDMRender_OnGUI.OnGUI(bool isVisible)
		{
			if (!isVisible)
				return;
			
			GUI.skin = _skin;

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

		private void CalculateLengths(DMBranch branch, IReadOnlyList<DMItem> items, int spaceLength, out int fullLength, out int maxNameLength, out int maxValueLength, out int maxDescriptionLength)
		{
			maxNameLength = 0;
			maxValueLength = 0;
			maxDescriptionLength = 0;
			fullLength = branch.Name.Length;

			for (var i = 0; i < items.Count; i++)
			{
				var item = items[i];
				var nameLength = item.Name.Length;
				var valueLength = item.Value.Length;
				var descriptionLength = item.Description.Length;

				if (item is DMBranch)
					nameLength += 3;

				maxNameLength = Math.Max(maxNameLength, nameLength);
				maxValueLength = Math.Max(maxValueLength, valueLength);
				maxDescriptionLength = Math.Max(maxDescriptionLength, descriptionLength);
			}

			fullLength = Math.Max(fullLength, maxNameLength + spaceLength + maxValueLength + spaceLength + maxDescriptionLength);
		}

		#endregion
	}
}