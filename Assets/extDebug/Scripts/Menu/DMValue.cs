/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;

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

		private IDMStorage _storage;

		#endregion

		#region Public Methods

		protected DMValue(DMBranch parent, string path, Func<T> getter, Action<T> setter = null, int order = 0) : base(parent, path, string.Empty, string.Empty, order)
		{
			_getter = getter;
			_setter = setter;
			_path = path;

			_defaultValue = getter.Invoke();
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
					var value = ValueDecrement(_getter.Invoke(), eventArgs.IsShift);

					ChangeValue(value, false);
				}
				else if (eventArgs.Key == EventKey.Right && _setter != null)
				{
					var value = ValueIncrement(_getter.Invoke(), eventArgs.IsShift);

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