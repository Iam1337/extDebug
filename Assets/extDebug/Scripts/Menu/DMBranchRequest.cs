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
		public DMBranch Add(string path, string description = "", int order = 0) => Insert(DM.Add(null, path, description, order));

		// Action
		public DMAction Add(string path, Action<DMAction> action, string description = "", int order = 0) => Insert(DM.Add(null, path, action, description, order));

		// Bool
		public DMBool Add(string path, Func<bool> getter, Action<bool> setter = null, int order = 0) => Insert(DM.Add(path, getter, setter, order));

		// Enum
		public DMEnum<TEnum> Add<TEnum>(string path, Func<TEnum> getter, Action<TEnum> setter = null, int order = 0) where TEnum : struct, Enum => Insert(DM.Add(path, getter, setter, order));

		// UInt8
		public DMUInt8 Add(string path, Func<byte> getter, Action<byte> setter = null, int order = 0) => Insert(DM.Add(path, getter, setter, order));

        // UInt16
        public DMUInt16 Add(string path, Func<UInt16> getter, Action<UInt16> setter = null, int order = 0) => Insert(DM.Add(path, getter, setter, order));

        // UInt32
        public DMUInt32 Add(string path, Func<UInt32> getter, Action<UInt32> setter = null, int order = 0) => Insert(DM.Add(path, getter, setter, order));

        // UInt64
        public DMUInt64 Add(string path, Func<UInt64> getter, Action<UInt64> setter = null, int order = 0) => Insert(DM.Add(path, getter, setter, order));

        // Int8
        public DMInt8 Add(string path, Func<sbyte> getter, Action<sbyte> setter = null, int order = 0) => Insert(DM.Add(path, getter, setter, order));

        // Int16
		public DMInt16 Add(string path, Func<Int16> getter, Action<Int16> setter = null, int order = 0) => Insert(DM.Add(path, getter, setter, order));

		// Int32
		public DMInt32 Add(string path, Func<Int32> getter, Action<Int32> setter = null, int order = 0) => Insert(DM.Add(path, getter, setter, order));

		// Int64
        public DMInt64 Add(string path, Func<Int64> getter, Action<Int64> setter = null, int order = 0) => Insert(DM.Add(path, getter, setter, order));

        // Float
        public DMFloat Add(string path, Func<float> getter, Action<float> setter = null, int order = 0) => Insert(DM.Add(path, getter, setter, order));

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

			var branch = DM.Add(parent, name, GetDescription(@object), order);
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
