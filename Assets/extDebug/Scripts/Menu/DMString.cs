/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System;

namespace extDebug.Menu
{
	public class DMString : DMValue<string>
	{
		#region Public Methods

		public DMString(DMBranch parent, string path, Func<string> getter, Action<string> setter = null, int order = 0) : base(parent, path, getter, setter, order)
		{ }

		#endregion

		#region Private Methods

		protected override string ValueToString(string value) => value;

		protected override string ValueIncrement(string value) => throw new NotImplementedException(nameof(ValueIncrement));

		protected override string ValueDecrement(string value) => throw new NotImplementedException(nameof(ValueDecrement));

		#endregion
	}
}