/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System;
using System.Runtime.CompilerServices;

namespace extDebug.Menu
{
    public abstract class DMItem
    {
		#region External

		private const float kFlashDuration = 0.3f;

		#endregion

		#region Static Protected Methods

		protected static string GetPathDirectory(string path)
		{
			var idx = path.LastIndexOf("/", StringComparison.Ordinal);
			return idx < 0 ? string.Empty : path.Substring(0, idx);
		}

		protected static string GetPathName(string path)
		{
			var idx = path.LastIndexOf("/", StringComparison.Ordinal);
			return idx < 0 ? path : path.Substring(idx + 1);
		}

		#endregion

		#region Static Private Methods

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static float _flashMap(float flashTime) => Mathf.Clamp01((Time.unscaledTime - flashTime) / kFlashDuration);

		#endregion

		#region Public Vars

		public int Order
		{
			get => _order;
			set
			{
				_order = value;
				_parent?.Resort();
			}
		}

		public string Name
		{
			get => _name;
			set
			{
				_name = value;
				_parent?.Resort();
			}
		}

		public Color NameColor
		{
			get => Color.Lerp(_flashNameColor, _nameColor, _flashMap(_flashNameTime));
			set
			{
				_nameColor = value;
				_parent?.RequestRepaint();
			}
		}

		public string Value
		{
			get => _value;
			set
			{
				_value = value;
				_parent?.RequestRepaint();
			}
		}

		public Color ValueColor
		{
			get => Color.Lerp(_flashValueColor, _valueColor, _flashMap(_flashValueTime));
			set
			{
				_valueColor = value;
				_parent?.RequestRepaint();
			}
		}

		public DMBranch Parent => _parent;

		public object Data
		{
			get => _data ?? _parent?._data;
			set => _data = value;
		}

		#endregion

		#region Protected Vars

		protected readonly DMBranch _parent;

		protected string _name;

		protected Color _nameColor = DM.Colors.Name;

		protected string _value;

		protected Color _valueColor = DM.Colors.Value;

		protected int _order;

		#endregion

		#region Private Vars

		private float _flashNameTime = 0;

		private Color _flashNameColor = DM.Colors.NameFlash;

		private float _flashValueTime = 0;

		private Color _flashValueColor = DM.Colors.ValueFlash;

		private object _data;

		#endregion

		#region Public Methods

		public void SendEvent(EventArgs eventArgs) => OnEvent(eventArgs); // Syntax sugar

		public void FlashName(Color color, bool notify)
		{
			_flashNameTime = Time.unscaledTime;
			_flashNameColor = color;

			_parent?.RequestRepaint();

			if (DM.IsVisible == false && notify)
				DM.Notify(this);
		}

		public void FlashValue(Color color, bool notify)
		{
			_flashValueTime = Time.unscaledTime;
			_flashValueColor = color;
			
			_parent?.RequestRepaint();

			if (DM.IsVisible == false && notify)
				DM.Notify(this, null, ValueColor);
		}

		public override string ToString() => $"{_name,-16}  {_value,-16}";

		#endregion

		#region Protected Methods

		protected DMItem(DMBranch parent, string path, string value = "", int order = 0)
		{
			var directory = GetPathDirectory(path);
			var name = GetPathName(path);

			_name = name;
			_value = value;
			_order = order;
			_parent = parent?.Get(directory, true);// ?? DM.Root?.Get(directory, true);
			_parent?.Insert(this);
		}

		protected abstract void OnEvent(EventArgs eventArgs);

		#endregion
    }
}
