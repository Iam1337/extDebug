/* Copyright (c) 2023 dr. ext (Vladimir Sigalkin) */

using System;

namespace extDebug.Menu
{
	public class DMInt64 : DMValue<Int64>
	{
		#region Public Vars

		public Int64 Step = 1;

		public Int64 ShiftStep = 10;

		public string Format = "0";

		#endregion

		#region Public Methods

		public DMInt64(DMBranch parent, string path, Func<Int64> getter, Action<Int64> setter = null, int order = 0) : base(parent, path, getter, setter, order)
		{ }

		#endregion

		#region Protected Methods

		protected override string ValueToString(Int64 value) => value.ToString(Format);

		protected override Int64 ValueIncrement(Int64 value, bool isShift) => value + (isShift ? ShiftStep : Step);

		protected override Int64 ValueDecrement(Int64 value, bool isShift) => value - (isShift ? ShiftStep : Step);

		#endregion
	}
}