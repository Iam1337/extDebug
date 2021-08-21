/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;
using System.Collections.Generic;

namespace extDebug
{
	public class DMEnumRequest<TEnum, TObject> : DMItemRequest<TEnum, TObject> where TEnum : struct, Enum
	{
		#region Public Methods

		public DMEnumRequest(Func<IList<TObject>> request, Func<TEnum> getter, Action<TEnum> setter = null, Func<TObject, string> name = null) : base(request, getter, setter, name)
		{ }

		#endregion

		#region Protected Methods

		protected override DMItem BuildItem(DMBranch parent, TObject @object, string name, Func<TEnum> getter, Action<TEnum> setter, int order) => DM.Add(parent, name, getter, setter, order);

		#endregion
	}
}