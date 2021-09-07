/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;

namespace extDebug.Menu
{
	public class DMUInt32 : DMValue<UInt32>
	{
		#region Public Vars

		public UInt32 Step = 1;
		
		public UInt32 ShiftStep = 10;

		public string Format = "0";

		#endregion

		#region Public Methods

		public DMUInt32(DMBranch parent, string path, Func<UInt32> getter, Action<UInt32> setter = null, int order = 0) : base(parent, path, getter, setter, order)
		{ }

		#endregion

		#region Protected Methods

		protected override string ValueToString(UInt32 value) => value.ToString(Format);

		protected override UInt32 ValueIncrement(UInt32 value, bool isShift) => value + (isShift ? ShiftStep : Step);

		protected override UInt32 ValueDecrement(UInt32 value, bool isShift) => value - (isShift ? ShiftStep : Step);

		#endregion
	}
}