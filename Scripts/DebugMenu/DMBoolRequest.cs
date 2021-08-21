/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;
using System.Collections.Generic;

namespace extDebug
{
	public class DMBoolRequest<T> : DMItemRequest<bool, T>
	{
		#region Public Methods

		public DMBoolRequest(Func<IList<T>> request, Func<bool> getter, Action<bool> setter = null, Func<T, string> name = null) : base(request, getter, setter, name)
		{ }

		#endregion

		#region Protected Methods

		protected override DMItem BuildItem(DMBranch parent, T @object, string name, Func<bool> getter, Action<bool> setter, int order) => DM.Add(parent, name, getter, setter, order);

		#endregion
	}
}