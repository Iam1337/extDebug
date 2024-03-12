/* Copyright (c) 2024 dr. ext (Vladimir Sigalkin) */

using System;

namespace extDebug.Menu
{
	public class DMUInt64 : DMValue<UInt64>
	{
		#region Public Vars

		public UInt64 Step = 1;

		public UInt64 ShiftStep = 10;

		public string Format = "0";

		#endregion

		#region Public Methods

		public DMUInt64(DMBranch parent, string path, Func<UInt64> getter, Action<UInt64> setter = null, UInt64[] variants = null, int order = 0) : base(parent, path, getter, setter, variants, order)
		{ }

		#endregion

		#region Protected Methods

		protected override string ValueToString(UInt64 value) => value.ToString(Format);

		protected override UInt64 ValueIncrement(UInt64 value, bool isShift) => value + (isShift ? ShiftStep : Step);

		protected override UInt64 ValueDecrement(UInt64 value, bool isShift) => value - (isShift ? ShiftStep : Step);

		#endregion
	}
}