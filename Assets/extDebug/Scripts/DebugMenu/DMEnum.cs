/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;

namespace extDebug
{
    public class DMEnum<T> : DMValue<T> where T : struct, Enum
    {
		#region Static Private Methods

		private static T NextEnum<T>(T value) where T : struct
		{
			var length = Enum.GetValues(typeof(T)).Length;
			var index = Convert.ToInt64(value) + 1;

			if (index >= length)
				index = 0;

			return (T) Enum.ToObject(typeof(T), index);
		}

		private static T PrevEnum<T>(T value) where T : struct
		{
			var length = Enum.GetValues(typeof(T)).Length;
			var index = Convert.ToInt64(value) - 1;

			if (index < 0)
				index = length - 1;

			return (T) Enum.ToObject(typeof(T), index);
		}

		#endregion

		#region Public Methods

		public DMEnum(DMBranch parent, string path, Func<T> getter, Action<T> setter = null, int order = 0) : base(parent, path, getter, setter, order)
		{ }

		#endregion

		#region Protected Methods

		protected override string ValueToString(T value)
		{
			return null;
		}

		protected override T ValueIncrement(T value) => NextEnum(value);

		protected override T ValueDecrement(T value) => PrevEnum(value);

		#endregion
	}
}
