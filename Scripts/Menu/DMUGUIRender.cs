/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;
using UnityEngine.UI;

using System;
using System.Text;
using System.Collections.Generic;

using TextMeshPro = TMPro.TextMeshProUGUI;

namespace extDebug.Menu
{
	public class DMUGUIRender : IDMRender, IDMRender_Update
	{
		#region Private Vars
		
		private readonly StringBuilder _builder = new StringBuilder();

		private string _text;

		private GameObject _menuObject;
		
		private Text _menuUGUIText;
		
		private TextMeshPro _menuMeshProText;

		#endregion

		#region Constructor

		public DMUGUIRender(GameObject menuObject, Text menuText) : this(menuObject) => _menuUGUIText = menuText;

		public DMUGUIRender(GameObject menuObject, TextMeshPro menuText) : this(menuObject) => _menuMeshProText = menuText;

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
			
			// set value in text components
			if (_menuMeshProText != null)
				_menuMeshProText.text = _builder.ToString();
			else
				_menuUGUIText.text = _builder.ToString();
		}

		void IDMRender_Update.Update()
		{
			_menuObject.SetActive(DM.IsVisible);
		}
		
		#endregion

		#region Private Methods
		
		private DMUGUIRender(GameObject menuObject) => _menuObject = menuObject;
		
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