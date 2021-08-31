/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;

namespace extDebug.Menu
{
	public class DMUInt8 : DMValue<byte>
	{
		#region Public Vars

		public byte Step = 1;

		public string Format = "0";

		#endregion

		#region Public Methods

		public DMUInt8(DMBranch parent, string path, Func<byte> getter, Action<byte> setter = null, int order = 0) : base(parent, path, getter, setter, order)
		{ }

		#endregion

		#region Protected Methods

		protected override string ValueToString(byte value) => value.ToString(Format);

		protected override byte ValueIncrement(byte value) => (byte)(value + Step);

		protected override byte ValueDecrement(byte value) => (byte)(value - Step);

		#endregion
	}
}