/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;
using UnityEngine;

namespace extDebug
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
					_flagBranch = new DMBranch(null, GetPathName(path), getter.Invoke().ToString());

					var i = 0;
					var values = (T[])Enum.GetValues(type);
					for (; i < values.Length; i++)
					{
						var value = values[i];

						DM.Add(_flagBranch, value.ToString(), () =>
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

					DM.Add(_flagBranch, "Back", _ => DM.Back(), string.Empty, i + 10);
				}
			}
		}

		#endregion

		#region Protected Methods

		protected override void OnEvent(EventTag eventTag)
		{
			if (_flagBranch != null && eventTag == EventTag.Left)
			{
				DM.Back();
			}
			else if (_flagBranch != null && eventTag == EventTag.Right)
			{
				DM.Open(_flagBranch);
			}
			else
			{
				base.OnEvent(eventTag);
			}
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

		protected override T ValueIncrement(T value) => NextEnum(value);

		protected override T ValueDecrement(T value) => PrevEnum(value);

		#endregion
	}
}
