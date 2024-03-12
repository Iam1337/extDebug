/* Copyright (c) 2024 dr. ext (Vladimir Sigalkin) */

using System;

namespace extDebug.Menu
{
	public class DMBool : DMValue<bool>
	{
		#region Public Methods

		public DMBool(DMBranch parent, string path, Func<bool> getter, Action<bool> setter = null, bool[] variants = null, int order = 0) : base(parent, path, getter, setter, variants, order)
		{ }

		#endregion

		#region Protected Methods

		protected override string ValueToString(bool value) => value ? "True" : "False";

		protected override bool ValueIncrement(bool value, bool isShift) => !value;

		protected override bool ValueDecrement(bool value, bool isShift) => !value;

		#endregion
	}
}