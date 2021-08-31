/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;
using System.Collections.Generic;

namespace extDebug.Menu
{
	public class DMInt32Request<T> : DMItemRequest<Int32, T>
	{
		#region Public Methods

		public DMInt32Request(Func<IList<T>> request, Func<Int32> getter, Action<Int32> setter = null, Func<T, string> name = null) : base(request, getter, setter, name)
		{ }

		#endregion

		#region Protected Methods

		protected override DMItem BuildItem(DMBranch parent, T @object, string name, Func<Int32> getter, Action<Int32> setter, int order) => DM.Add(parent, name, getter, setter, order);

		#endregion
	}
}