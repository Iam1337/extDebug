/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;
using System.Collections.Generic;
using System.Text;

namespace extDebug
{
	public class DMBranchRequest<T> : DMRequest<T>
	{
		#region Private Vars

		private readonly List<DMItem> _items = new List<DMItem>();

		private readonly Func<T, string> _description;

		#endregion

		#region Public Methods

		public DMBranchRequest(Func<IList<T>> request, Func<T, string> name = null, Func<T, string> description = null) : base(request, name)
		{
			_description = description;
		}

		public override string ToString() => $"[BRANCH: {GetDataType().Name}]";

		public DMBranch Add(string path, string description = "", int order = 0)
		{
			return Insert(DM.Add(null, path, description, order));
		}

		public DMAction Add(string path, Action<DMAction, EventArgs> action, string description = "", int order = 0)
		{
			return Insert(DM.Add(null, path, action, description, order));
		}

		#endregion

		#region Protected Methods

		protected override DMItem BuildItem(DMBranch parent, T @object, string name, int order)
		{
			void OnOpenCallback(DMBranch branch)
			{
				foreach (var item in _items)
					item.Data = branch.Data;
			}

			void OnCloseCallback(DMBranch branch)
			{
				foreach (var item in _items)
					item.Data = null;
			}

			var branch = DM.Add(parent, GetName(@object), GetDescription(@object), order);
			branch.OnOpen += OnOpenCallback;
			branch.OnClose += OnCloseCallback;

			foreach (var item in _items)
			{
				branch.Insert(item);
			}

			return branch;
		}

		#endregion

		#region Private Vars

		private TItem Insert<TItem>(TItem item) where TItem : DMItem
		{
			int Comparison(DMItem x, DMItem y) => x.Order.CompareTo(y.Order);

			_items.Add(item);
			_items.Sort(Comparison);

			return item;
		}

		private string GetDescription(T @object) => _description != null ? _description.Invoke(@object) : string.Empty;

		#endregion
	}

}
