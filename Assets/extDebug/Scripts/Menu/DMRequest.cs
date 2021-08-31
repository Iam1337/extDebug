/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;
using System.Collections.Generic;

namespace extDebug.Menu
{
	public abstract class DMRequest
	{
		#region Public Methods

		public abstract Type GetDataType();

		public override string ToString() => "[REQUEST]";

		#endregion

		#region Internal Methods

		internal abstract List<DMItem> BuildItems(DMBranch branch);

		#endregion
	}

	public abstract class DMRequest<T> : DMRequest
	{
		#region Private Methods

		private readonly Func<IList<T>> _request;

		private readonly Func<T, string> _name;

		#endregion

		#region Public Methods

		public override Type GetDataType() => typeof(T);

		public override string ToString() => $"[REQUEST: {GetDataType().Name}]";

		#endregion

		#region Internal Methods

		internal override List<DMItem> BuildItems(DMBranch parent)
		{
			var branches = new List<DMItem>();

			var objects = _request.Invoke();
			if (objects != null)
			{
				for (var i = 0; i < objects.Count; i++)
				{
					var @object = objects[i];
					var @objectItem = BuildItem(parent, @object, GetName(@object), i);
					@objectItem.Data = @object;

					branches.Add(@objectItem);
				}
			}

			return branches;
		}

		#endregion

		#region Protected Methods

		protected DMRequest(Func<IList<T>> request, Func<T, string> name)
		{
			_name = name;
			_request = request;
		}

		protected string GetName(T @object) => (_name != null ? _name.Invoke(@object) : @object.ToString()).Replace('/', '\\');

		protected abstract DMItem BuildItem(DMBranch parent, T @object, string name, int order);

		#endregion
	}
}
