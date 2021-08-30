/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;

namespace extDebug
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

        protected override void OnEvent(EventTag eventTag)
        {
			if (eventTag == EventTag.Repaint)
			{
				var value = _getter.Invoke();

				_valueColor = _defaultValue.Equals(value) ? DM.Colors.Value : DM.Colors.ValueFlash;
				_value = ValueToString(value);
			}
			else if (eventTag == EventTag.Left)
			{
				_setter?.Invoke(ValueDecrement(_getter.Invoke()));
			}
			else if (eventTag == EventTag.Right)
			{
				_setter?.Invoke(ValueIncrement(_getter.Invoke()));
			}
			else if (eventTag == EventTag.Reset)
			{
				_setter?.Invoke(_defaultValue);
			}
			else if (eventTag == EventTag.Back)
			{
				DM.Back();
			}
        }

        protected abstract string ValueToString(T value);

        protected abstract T ValueIncrement(T value);

        protected abstract T ValueDecrement(T value);

        #endregion
    }
}
