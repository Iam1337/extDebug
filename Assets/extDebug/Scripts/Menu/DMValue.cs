/* Copyright (c) 2023 dr. ext (Vladimir Sigalkin) */

using System;
using System.Collections.Generic;

namespace extDebug.Menu
{
	public abstract class DMValue<T> : DMItem
	{
		#region Protected Methods

		protected readonly Func<T> _getter;

		protected readonly Action<T> _setter;

		#endregion

		#region Private Vars

		private readonly string _path;

		private T _defaultValue;

		private T[] _variants;

		private IDMStorage _storage;

		#endregion

		#region Public Methods

		protected DMValue(DMBranch parent, string path, Func<T> getter, Action<T> setter = null, T[] variants = null, int order = 0) : base(parent, path, string.Empty, string.Empty, order)
		{
			_getter = getter;
			_setter = setter;
			_path = path;

			_defaultValue = getter.Invoke();
			_variants = variants;
		}

		public void SetStorage(IDMStorage storage)
		{
			_storage = storage ?? throw new NullReferenceException(nameof(storage));

			if (_setter == null)
				return;

			var value = _storage.Load(_path, typeof(T));
			if (value != null)
				_setter.Invoke((T)value);
		}

		#endregion

		#region Protected Methods

		protected override void OnEvent(EventArgs eventArgs)
		{
			if (eventArgs.Tag == EventTag.Repaint)
			{
				var value = _getter.Invoke();

				_valueField.Color = _defaultValue.Equals(value) ? DM.Colors.Value : DM.Colors.ValueFlash;
				_valueField.Value = ValueToString(value);
			}
			else if (eventArgs.Tag == EventTag.Input && IsEnabled())
			{
				if (eventArgs.Key == EventKey.Left && _setter != null)
				{
					T value;
					
					if (_variants != null && _variants.Length > 0)
					{
						value = PrevVariant(_getter.Invoke());
					}
					else
					{
                        value = ValueDecrement(_getter.Invoke(), eventArgs.IsShift);
					}

					ChangeValue(value, false);
				}
				else if (eventArgs.Key == EventKey.Right && _setter != null)
				{
					T value;
					
					if (_variants != null && _variants.Length > 0)
					{
						value = NextVariant(_getter.Invoke());
					}
					else
					{
						value = ValueIncrement(_getter.Invoke(), eventArgs.IsShift);
					}

					ChangeValue(value, false);
				}
				else if (eventArgs.Key == EventKey.Reset && _setter != null)
				{
					ChangeValue(_defaultValue, true);
				}
			}
		}

		protected abstract string ValueToString(T value);

		protected abstract T ValueIncrement(T value, bool isShift);

		protected abstract T ValueDecrement(T value, bool isShift);

		protected virtual T NextVariant(T value)
		{
			var index = Math.Max(Array.IndexOf(_variants, value), 0) + 1;
			return index >= _variants.Length ? _variants[0] : _variants[index];
		}

		protected virtual T PrevVariant(T value)
		{
			var index = Math.Max(Array.IndexOf(_variants, value), 0) - 1;
			return index < 0 ? _variants[_variants.Length] : _variants[index];
		}

		#endregion

		#region Private Methods

		private void ChangeValue(T value, bool isReset)
		{
			// Change value.
			_setter.Invoke(value);
			_valueField.Value = ValueToString(value);

			// Save value.
			if (_storage == null || _storage.Save(_path, value))
			{
				FlashValue(DM.Colors.ActionSuccess, true);
			}
			else
			{
				FlashValue(DM.Colors.ActionFailed, false);
			}
		}

		#endregion
	}
}