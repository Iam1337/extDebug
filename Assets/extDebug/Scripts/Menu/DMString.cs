/* Copyright (c) 2023 dr. ext (Vladimir Sigalkin) */

using System;

namespace extDebug.Menu
{
	public class DMString : DMValue<string>
	{
		#region Public Methods

		public DMString(DMBranch parent, string path, Func<string> getter, Action<string> setter = null, string[] variants = null, int order = 0) : base(parent, path, getter, setter, variants, order)
		{ }

		#endregion

		#region Private Methods

		protected override string ValueToString(string value) => value;

		protected override string ValueIncrement(string value, bool isShift) => value;

		protected override string ValueDecrement(string value, bool isShift) => value;

		#endregion
	}
}