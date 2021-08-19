/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;

namespace extDebug
{
	public class DMInteger : DMValue<int>
	{
		#region Public Vars

		public int Step = 1;

		public string Format = "0";

		#endregion

		#region Public Methods

		public DMInteger(DMBranch parent, string path, Func<int> getter, Action<int> setter = null, int order = 0) : base(parent, path, getter, setter, order)
		{ }

		#endregion

		#region Protected Methods

		protected override string ValueToString(int value) => value.ToString(Format);

		protected override int ValueIncrement(int value) => value + Step;

		protected override int ValueDecrement(int value) => value - Step;

		#endregion
	}
}