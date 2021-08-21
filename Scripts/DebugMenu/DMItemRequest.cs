/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;
using System.Collections.Generic;

namespace extDebug
{
    public abstract class DMItemRequest<TValue, TObject> : DMRequest<TObject>
    {
		#region Private Vars

		private readonly Func<TValue> _getter;

		private readonly Action<TValue> _setter;

		#endregion

		#region Public Methods

		public DMItemRequest(Func<IList<TObject>> request, Func<TValue> getter, Action<TValue> setter = null, Func<TObject, string> name = null) : base(request, name)
		{
			_getter = getter;
			_setter = setter;
		}

		public override string ToString() => $"[{GetType().Name.ToUpper()}: {GetDataType().Name}]";

		#endregion

		#region Protected Methods

		protected override DMItem BuildItem(DMBranch parent, TObject @object, string name, int order) => BuildItem(parent, @object, name, _getter, _setter, order);

		protected abstract DMItem BuildItem(DMBranch parent, TObject @object, string name, Func<TValue> getter, Action<TValue> setter, int order);

		#endregion
	}
}
