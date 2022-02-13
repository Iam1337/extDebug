/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System;
using System.Runtime.CompilerServices;

namespace extDebug.Menu
{
	public abstract class DMItem
	{
		#region Internal

		protected class Field<T>
		{
			#region Private Static Methods

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static float _flashMap(float flashTime) =>
				Mathf.Clamp01((Time.unscaledTime - flashTime) / kFlashDuration);

			#endregion

			#region Public Vars

			public T Value;

			public Color Color
			{
				get => Color.Lerp(_flashColor, _color, _flashMap(_flashTime));
				set => _color = value;
			}

			#endregion

			#region Private Vars

			private Color _color;

			private Color _flashColor;

			private float _flashTime;

			#endregion

			#region Public Methods

			public Field(T value, Color color)
			{
				Value = value;
				_color = color;
			}

			public void Flash(Color color)
			{
				_flashTime = Time.unscaledTime;
				_flashColor = color;
			}

			public override string ToString() => Value.ToString();

			#endregion

			#region Private Methods

			private Field()
			{ }

			#endregion
		}

		#endregion

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
			get => _nameField.Value;
			set
			{
				_nameField.Value = value;
				_parent?.Resort();
			}
		}

		public Color NameColor
		{
			get => _nameField.Color;
			set
			{
				_nameField.Color = value;
				_parent?.RequestRepaint();
			}
		}

		public string Value
		{
			get => _valueField.Value;
			set
			{
				_valueField.Value = value;
				_parent?.RequestRepaint();
			}
		}

		public Color ValueColor
		{
			get => _valueField.Color;
			set
			{
				_valueField.Color = value;
				_parent?.RequestRepaint();
			}
		}

		public string Description
		{
			get => _descriptionField.Value;
			set
			{
				_descriptionField.Value = value;
				_parent?.RequestRepaint();
			}
		}

		public Color DescriptionColor
		{
			get => _descriptionField.Color;
			set
			{
				_descriptionField.Color = value;
				_parent?.RequestRepaint();
			}
		}

		public DMBranch Parent => _parent;

		public object Data
		{
			get => _data ?? _parent?.Data;
			set => _data = value;
		}

		public DMContainer Container
		{
			get => _container ?? _parent?.Container;
			internal set => _container = value;
		}

		public bool IsEnabled()
		{
			// If the parent component is disabled, then the child must be disabled. 
			if (_parent != null && !_parent.IsEnabled())
				return false;

			return _enabledCallback?.Invoke() ?? _enabled;
		}

		public void SetEnabled(bool enabled) => _enabled = enabled;

		public void SetEnabled(Func<bool> callback) => _enabledCallback = callback;

		#endregion

		#region Protected Vars

		protected bool _enabled;

		protected Func<bool> _enabledCallback;

		protected DMContainer _container;

		protected readonly DMBranch _parent;

		protected readonly Field<string> _nameField;

		protected readonly Field<string> _valueField;

		protected readonly Field<string> _descriptionField;

		protected int _order;

		#endregion

		#region Private Vars

		private object _data;

		#endregion

		#region Public Methods

		public void SendEvent(EventArgs eventArgs) => OnEvent(eventArgs); // Syntax sugar

		public void FlashName(Color color, bool notify)
		{
			_nameField.Flash(color);
			_parent?.RequestRepaint(kFlashDuration);

			if (Container.IsVisible == false && notify)
				Container.Notify(this);
		}

		public void FlashValue(Color color, bool notify)
		{
			_valueField.Flash(color);
			_parent?.RequestRepaint(kFlashDuration);

			if (Container.IsVisible == false && notify)
				Container.Notify(this, null, ValueColor);
		}

		public override string ToString()
		{
			var value = $"Item {_nameField.Value}";

			if (!string.IsNullOrEmpty(_valueField.Value))
				value += $", Value: {_valueField.Value}";

			if (!string.IsNullOrEmpty(_descriptionField.Value))
				value += $", Desc: {_descriptionField.Value}";

			return value;
		}

		#endregion

		#region Protected Methods

		protected DMItem(DMBranch parent, string path, string value, string description, int order)
		{
			var directory = GetPathDirectory(path);
			var name = GetPathName(path);

			_nameField = new Field<string>(name, DM.Colors.Name);
			_valueField = new Field<string>(value, DM.Colors.Value);
			_descriptionField = new Field<string>(description, DM.Colors.Description);

			_enabled = true;
			_order = order;

			_parent = parent?.Get(directory, true);
			_parent?.Insert(this);
		}

		protected abstract void OnEvent(EventArgs eventArgs);

		#endregion
	}
}