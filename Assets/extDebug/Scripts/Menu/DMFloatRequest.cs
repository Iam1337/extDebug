/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;
using System.Collections.Generic;

namespace extDebug.Menu
{
	public class DMFloatRequest<T> : DMItemRequest<float, T>
	{
		#region Public Methods

		public DMFloatRequest(Func<IList<T>> request, Func<float> getter, Action<float> setter = null, Func<T, string> name = null) : base(request, getter, setter, name)
		{ }

		#endregion

		#region Protected Methods

		protected override DMItem BuildItem(DMBranch parent, T @object, string name, Func<float> getter, Action<float> setter, int order) => DM.Add(parent, name, getter, setter, order);

		#endregion
	}
}