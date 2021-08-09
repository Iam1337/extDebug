﻿/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System.IO;

namespace extDebug
{
    public abstract class DebugMenuItem
    {
		#region External

		private const float FLASH_DURATION = 0.3f;

		#endregion

		#region Static Private Vars

		private static float _flashMap(float flashTime) => Mathf.Clamp01((Time.unscaledTime - flashTime) / FLASH_DURATION);

		#endregion

		#region Public Vars

		public int Order
		{
			get => _order;
			set
			{
				_order = value;
				_parent.Resort();
			}
		}

		public string Name
		{
			get => _name;
			set
			{
				_name = value;
				_parent.Resort();
			}
		}

		public Color NameColor
		{
			get => Color.Lerp(_flashNameColor, _nameColor, _flashMap(_flashNameTime));
			set
			{
				_nameColor = value;
				_parent.RequestRepaint();
			}
		}

		public string Value
		{
			get => _value;
			set
			{
				_value = value;
				_parent.RequestRepaint();
			}
		}

		public Color ValueColor
		{
			get => Color.Lerp(_flashValueColor, _valueColor, _flashMap(_flashValueTime));
			set
			{
				_valueColor = value;
				_parent.RequestRepaint();
			}
		}

		public DebugMenuBranch Parent
		{
			get => _parent;
		}

		#endregion

		#region Protected Vars

		protected readonly DebugMenuBranch _parent;

		protected string _name;

		protected Color _nameColor;

		protected string _value;

		protected Color _valueColor;

		protected int _order;

		#endregion

		#region Private Vars

		private float _flashNameTime = float.MaxValue;

		private Color _flashNameColor = Color.clear;

		private float _flashValueTime = float.MaxValue;

		private Color _flashValueColor = Color.clear;

		#endregion

		#region Public Methods

		public void SendEvent(EventArgs eventArgs) => OnEvent(eventArgs);

		public void FlashName(Color color, bool notify)
		{
			_flashNameTime = Time.unscaledTime;
			_flashNameColor = color;

			if (_parent != null)
				_parent.RequestRepaint();

			if (DebugMenu.IsVisible == false && notify)
				DebugMenu.Notify(this);
		}

		public void FlashValue(Color color, bool notify)
		{
			_flashValueTime = Time.unscaledTime;
			_flashValueColor = color;

			if (_parent != null)
				_parent.RequestRepaint();

			if (DebugMenu.IsVisible == false && notify)
				DebugMenu.Notify(this);
		}

		#endregion

		#region Protected Methods

		protected DebugMenuItem(DebugMenuBranch parent, string path, int order = 0)
		{
			var directory = Path.GetDirectoryName(path);
			var name = Path.GetFileName(path);

			Order = order;

			_name = name;
			_parent = parent.Get(directory, true) ?? DebugMenu.Root.Get(directory, true);
			_parent.Add(this);
		}

		protected abstract void OnEvent(EventArgs eventArgs);

		#endregion

		#region Private Methods

		#endregion
	}
}
