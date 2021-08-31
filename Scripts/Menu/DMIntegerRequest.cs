/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;
using System.Collections.Generic;

namespace extDebug.Menu
{
	public class DMIntegerRequest<T> : DMItemRequest<int, T>
	{
		#region Public Methods

		public DMIntegerRequest(Func<IList<T>> request, Func<int> getter, Action<int> setter = null, Func<T, string> name = null) : base(request, getter, setter, name)
		{ }

		#endregion

		#region Protected Methods

		protected override DMItem BuildItem(DMBranch parent, T @object, string name, Func<int> getter, Action<int> setter, int order) => DM.Add(parent, name, getter, setter, order);

		#endregion
	}
}