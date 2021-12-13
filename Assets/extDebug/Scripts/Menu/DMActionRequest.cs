/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;
using System.Collections.Generic;

namespace extDebug.Menu
{
	public class DMActionRequest<T> : DMRequest<T>
	{
		#region Private Vars

		private readonly Func<T, string> _description;

		private readonly Action<DMAction> _action;

		#endregion

		#region Public Methods

		public DMActionRequest(Func<IList<T>> request, Action<DMAction> action = null, Func<T, string> name = null, Func<T, string> description = null) : base(request, name)
		{
			_action = action;
			_description = description;
		}

		public override string ToString() => $"[ACTION: {GetDataType().Name}]";

		#endregion

		#region Protected Methods

		protected override DMItem BuildItem(DMBranch parent, T @object, string name, int order) => parent.Container.Add(parent, name, _action, GetDescription(@object), order);

		#endregion

		#region Private Vars

		private string GetDescription(T @object) => _description != null ? _description.Invoke(@object) : string.Empty;

		#endregion
	}
}
