/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;

namespace extDebug
{
	public class DMInteger : DMItem
	{
		#region Public Vars

		public int Step = 1;

		public string Format = "0";

		#endregion

		#region Private Vars

		private readonly Func<int> _getter;

		private readonly Action<int> _setter;

		private int _defaultValue;

		#endregion

		#region Public Methods

		public DMInteger(DMBranch parent, string path, Func<int> getter, Action<int> setter = null, int order = 0) : base(parent, path, "", order)
		{
			_getter = getter;
			_setter = setter;

			_defaultValue = getter.Invoke();
			_value = _defaultValue.ToString(Format);
		}

		#endregion

		#region Protected Methods

		protected override void OnEvent(EventArgs eventArgs)
		{
			if (eventArgs.Event == EventType.Repaint)
			{
				var value = _getter.Invoke();

				_value = value.ToString(Format);
				_valueColor = value == _defaultValue ? DM.Colors.Value : DM.Colors.ValueFlash;
			}
			else if (eventArgs.Event == EventType.KeyDown)
			{
				switch (eventArgs.Key)
				{
					case KeyType.Up:
					{
						break;
					}
					case KeyType.Down:
					{
						break;
					}
					case KeyType.Left:
					{
						if (_setter == null)
							break;

						_setter.Invoke(_getter.Invoke() - Step);

						break;
					}
					case KeyType.Right:
					{
						if (_setter == null)
							break;

						_setter.Invoke(_getter.Invoke() + Step);

						break;
					}
					case KeyType.Reset:
						if (_setter == null)
							break;

						_setter.Invoke(_defaultValue);

						break;
					case KeyType.Back:
						DM.Back();
						break;
				}
			}
		}

		#endregion
	}
}