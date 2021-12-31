/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;

namespace extDebug.Menu
{
	public class DMInt32 : DMValue<Int32>
	{
		#region Public Vars

		public Int32 Step = 1;

		public Int32 ShiftStep = 10;

		public string Format = "0";

		#endregion

		#region Public Methods

		public DMInt32(DMBranch parent, string path, Func<Int32> getter, Action<Int32> setter = null, int order = 0) : base(parent, path, getter, setter, order)
		{
		}

		#endregion

		#region Protected Methods

		protected override string ValueToString(Int32 value) => value.ToString(Format);

		protected override Int32 ValueIncrement(Int32 value, bool isShift) => value + (isShift ? ShiftStep : Step);

		protected override Int32 ValueDecrement(Int32 value, bool isShift) => value - (isShift ? ShiftStep : Step);

		#endregion
	}
}