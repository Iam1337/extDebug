/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;

namespace extDebug.Menu
{
	public class DMUInt16 : DMValue<UInt16>
	{
		#region Public Vars

		public UInt16 Step = 1;
		
		public UInt16 ShiftStep = 10;

		public string Format = "0";

		#endregion

		#region Public Methods

		public DMUInt16(DMBranch parent, string path, Func<UInt16> getter, Action<UInt16> setter = null, int order = 0) : base(parent, path, getter, setter, order)
		{ }

		#endregion

		#region Protected Methods

		protected override string ValueToString(UInt16 value) => value.ToString(Format);

		protected override UInt16 ValueIncrement(UInt16 value, bool isShift) => (UInt16)(value + (isShift ? ShiftStep : Step));

		protected override UInt16 ValueDecrement(UInt16 value, bool isShift) => (UInt16)(value - (isShift ? ShiftStep : Step));

		#endregion
	}
}