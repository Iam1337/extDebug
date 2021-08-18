/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;
using System.Collections.Generic;

namespace extDebug
{
	public class DMIntegerRequest<T> : DMRequest<T>
	{
		#region Private Vars

		private readonly Func<int> _getter;

		private readonly Action<int> _setter;

		#endregion

		#region Public Methods

		public DMIntegerRequest(Func<IList<T>> request, Func<int> getter, Action<int> setter = null, Func<T, string> name = null) : base(request, name)
		{
			_getter = getter;
			_setter = setter;
		}

		public override string ToString() => $"[INTEGER: {GetDataType().Name}]";

		#endregion

		#region Protected Methods

		protected override DMItem BuildItem(DMBranch parent, T @object, string name, int order) => DM.Add(parent, name, _getter, _setter, order);

		#endregion
	}
}