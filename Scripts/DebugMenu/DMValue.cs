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

        protected override void OnEvent(EventArgs eventArgs)
        {
			if (eventArgs.Event == EventType.Repaint)
			{
				var value = _getter.Invoke();

				_valueColor = _defaultValue.Equals(value) ? DM.Colors.Value : DM.Colors.ValueFlash;
				_value = ValueToString(value);
			}
			else if (eventArgs.Event == EventType.KeyDown)
			{
				switch (eventArgs.Key)
				{
					case KeyType.Left:
					{
						_setter?.Invoke(ValueDecrement(_getter.Invoke()));
						break;
					}
					case KeyType.Right:
					{
						_setter?.Invoke(ValueIncrement(_getter.Invoke()));
						break;
					}
					case KeyType.Reset:
					{
						_setter?.Invoke(_defaultValue);
						break;
					}
					case KeyType.Back:
					{
						DM.Back();
						break;
					}
				}
			}
		}

        protected abstract string ValueToString(T value);

        protected abstract T ValueIncrement(T value);

        protected abstract T ValueDecrement(T value);

        #endregion
    }
}
