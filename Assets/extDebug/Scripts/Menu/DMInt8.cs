/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;

namespace extDebug.Menu
{
	public class DMInt8 : DMValue<sbyte>
	{
		#region Public Vars

		public sbyte Step = 1;

		public string Format = "0";

		#endregion

		#region Public Methods

		public DMInt8(DMBranch parent, string path, Func<sbyte> getter, Action<sbyte> setter = null, int order = 0) : base(parent, path, getter, setter, order)
		{ }

		#endregion

		#region Protected Methods

		protected override string ValueToString(sbyte value) => value.ToString(Format);

		protected override sbyte ValueIncrement(sbyte value) => (sbyte)(value + Step);

		protected override sbyte ValueDecrement(sbyte value) => (sbyte)(value - Step);

		#endregion
	}
}