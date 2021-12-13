/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;
using System.Collections.Generic;

namespace extDebug.Menu
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

		// Branch
		public DMBranch Add(string path, string description = "", int order = 0) => Insert(new DMBranch(null, path, description, order));

		// Action
		public DMAction Add(string path, Action<DMAction> action, string description = "", int order = 0) => Insert(new DMAction(null, path, description, action, order));

		// Bool
		public DMBool Add(string path, Func<bool> getter, Action<bool> setter = null, int order = 0) => Insert(new DMBool(null, path, getter, setter, order));

		// Enum
		public DMEnum<TEnum> Add<TEnum>(string path, Func<TEnum> getter, Action<TEnum> setter = null, int order = 0) where TEnum : struct, Enum => Insert(new DMEnum<TEnum>(null, path, getter, setter, order));

		// UInt8
		public DMUInt8 Add(string path, Func<byte> getter, Action<byte> setter = null, int order = 0) => Insert(new DMUInt8(null, path, getter, setter, order));

        // UInt16
        public DMUInt16 Add(string path, Func<UInt16> getter, Action<UInt16> setter = null, int order = 0) => Insert(new DMUInt16(null, path, getter, setter, order));

        // UInt32
        public DMUInt32 Add(string path, Func<UInt32> getter, Action<UInt32> setter = null, int order = 0) => Insert(new DMUInt32(null, path, getter, setter, order));

        // UInt64
        public DMUInt64 Add(string path, Func<UInt64> getter, Action<UInt64> setter = null, int order = 0) => Insert(new DMUInt64(null, path, getter, setter, order));

        // Int8
        public DMInt8 Add(string path, Func<sbyte> getter, Action<sbyte> setter = null, int order = 0) => Insert(new DMInt8(null, path, getter, setter, order));

        // Int16
		public DMInt16 Add(string path, Func<Int16> getter, Action<Int16> setter = null, int order = 0) => Insert(new DMInt16(null, path, getter, setter, order));

		// Int32
		public DMInt32 Add(string path, Func<Int32> getter, Action<Int32> setter = null, int order = 0) => Insert(new DMInt32(null, path, getter, setter, order));

		// Int64
        public DMInt64 Add(string path, Func<Int64> getter, Action<Int64> setter = null, int order = 0) => Insert(new DMInt64(null, path, getter, setter, order));

        // Float
        public DMFloat Add(string path, Func<float> getter, Action<float> setter = null, int order = 0) => Insert(new DMFloat(null, path, getter, setter, order));

        #endregion

		#region Protected Methods

		protected override DMItem BuildItem(DMBranch parent, T @object, string name, int order)
		{
			void OnOpenCallback(DMBranch branch)
			{
				foreach (var item in _items)
				{
					item.Data = branch.Data;
					item.Container = branch.Container;
				}
			}

			void OnCloseCallback(DMBranch branch)
			{
				foreach (var item in _items)
				{
					item.Data = null;
					item.Container = null;
				}
			}

			var branch = parent.Container.Add(parent, name, GetDescription(@object), order);
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
