/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;

namespace extDebug.Menu
{
    public abstract class DMValue<T> : DMItem
    {
        #region Private Vars

        private readonly Func<T> _getter;

        private readonly Action<T> _setter;

        private T _defaultValue;

        #endregion

        #region Public Methods

        protected DMValue(DMBranch parent, string path, Func<T> getter, Action<T> setter = null, int order = 0) : base(parent, path, string.Empty, order)
        {
	        _getter = getter;
	        _setter = setter;

	        _defaultValue = getter.Invoke();
        }

        #endregion

        #region Protected Methods

        protected override void OnEvent(EventArgs eventArgs)
        {
			if (eventArgs.Tag == EventTag.Repaint)
			{
				var value = _getter.Invoke();

				_valueColor = _defaultValue.Equals(value) ? DM.Colors.Value : DM.Colors.ValueFlash;
				_value = ValueToString(value);
			}
			else if (eventArgs.Tag == EventTag.Input)
			{
				if (eventArgs.Key == EventKey.Left && _setter != null)
				{
					var value = ValueDecrement(_getter.Invoke());

					_setter.Invoke(value);
					_value = ValueToString(value);

					FlashValue(DM.Colors.ActionFlash, true);
				}
				else if (eventArgs.Key == EventKey.Right && _setter != null)
				{
					var value = ValueIncrement(_getter.Invoke());

					_setter.Invoke(value);
					_value = ValueToString(value);
				
					FlashValue(DM.Colors.ActionFlash, true);
				}
				else if (eventArgs.Key == EventKey.Reset && _setter != null)
				{
					_setter.Invoke(_defaultValue);
					_value = ValueToString(_defaultValue);

					FlashValue(DM.Colors.ActionFlash, true);
				}
				else if (eventArgs.Key == EventKey.Back)
				{
					DM.Back();
				}	
			}
        }

        protected abstract string ValueToString(T value);

        protected abstract T ValueIncrement(T value);

        protected abstract T ValueDecrement(T value);

        #endregion
    }
}
