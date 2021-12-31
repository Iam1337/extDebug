/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;

namespace extDebug.Menu
{
    public class DMEnum<T> : DMValue<T> where T : struct, Enum
    {
        #region Static Private Methods

        private static T NextEnum(T value)
		{
			var length = Enum.GetValues(typeof(T)).Length;
			var index = Convert.ToInt64(value) + 1;

			if (index >= length)
				index = 0;

			return (T) Enum.ToObject(typeof(T), index);
		}

		private static T PrevEnum(T value)
		{
			var length = Enum.GetValues(typeof(T)).Length;
			var index = Convert.ToInt64(value) - 1;

			if (index < 0)
				index = length - 1;

			return (T) Enum.ToObject(typeof(T), index);
		}

		#endregion

		#region Private Vars

		private readonly DMBranch _flagBranch;

		#endregion

		#region Public Methods

		public DMEnum(DMBranch parent, string path, Func<T> getter, Action<T> setter = null, int order = 0) : base(parent, path, getter, setter, order)
		{
			if (setter != null)
			{
				var type = typeof(T);
				if (type.IsDefined(typeof(FlagsAttribute), false))
                {
                    _flagBranch = new DMBranch(null, GetPathName(path));
                    _flagBranch.Container = Container;
					
					var values = (T[])Enum.GetValues(type);
					for (var i = 0; i < values.Length; i++)
					{
						var value = values[i];

                        _flagBranch.Add(value.ToString(), () =>
						{
							var intGetter = (int)(object)getter.Invoke();
							var intValue = (int)(object)value;

							return (intGetter & intValue) != 0;
						}, v =>
						{
							var intGetter = (int)(object)getter.Invoke();
							var intValue = (int)(object)value;

							setter.Invoke((T)(object)(intGetter ^ intValue));
						}, i);
					}

                    _flagBranch.Add("Back", BackAction, string.Empty, int.MaxValue);
				}
			}
		}
		
		#endregion

		#region Protected Methods

		protected override void OnEvent(EventArgs eventArgs)
		{
            if (eventArgs.Tag == EventTag.Input)
            {
                if (_flagBranch != null)
                {
                    if (eventArgs.Key == EventKey.Left)
                    {
                        if (Container.IsVisible)
                            Container.Back();

						return;
                    }

                    if (eventArgs.Key == EventKey.Right)
                    {
                        if (Container.IsVisible)
                            Container.Open(_flagBranch);

                        return;
                    }
                }
            }

            base.OnEvent(eventArgs);
        }

		protected override string ValueToString(T value)
		{
			if (_flagBranch != null)
			{
				var intValue = (int)(object)value;
				if (intValue == 0)
				{
					return "None";
				}
			}

			return value.ToString();
		}

		protected override T ValueIncrement(T value, bool isShift) => NextEnum(value);

		protected override T ValueDecrement(T value, bool isShift) => PrevEnum(value);

		#endregion

		#region Private Methods
		
		private void BackAction(DMAction action)
		{
			Container.Back();
		}

		#endregion
	}
}
