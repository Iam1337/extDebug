/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;

namespace extDebug.Menu
{
	public class DMInt16 : DMValue<Int16>
	{
		#region Public Vars

		public Int16 Step = 1;

		public string Format = "0";

		#endregion

		#region Public Methods

		public DMInt16(DMBranch parent, string path, Func<Int16> getter, Action<Int16> setter = null, int order = 0) : base(parent, path, getter, setter, order)
		{ }

		#endregion

		#region Protected Methods

		protected override string ValueToString(Int16 value) => value.ToString(Format);

		protected override Int16 ValueIncrement(Int16 value) => (Int16)(value + Step);

		protected override Int16 ValueDecrement(Int16 value) => (Int16)(value - Step);

		#endregion
	}
}