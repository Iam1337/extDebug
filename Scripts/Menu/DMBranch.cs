/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System;
using System.Collections.Generic;

namespace extDebug.Menu
{
    public class DMBranch : DMItem
    {
		#region Public Methods

		public DMItem Current
		{
			get
			{
				if (_items.Count == 0)
					return null;

				if (_items.Count <= _currentItem)
					_currentItem = 0;

				return _items[_currentItem];
			}
		}

		public List<DMItem> Items => _items;

		public Action<DMBranch> OnOpen;

		public Action<DMBranch> OnClose;

		#endregion

		#region Private Vars

		private readonly List<DMItem> _items = new List<DMItem>();

		private readonly List<DMRequest> _requests = new List<DMRequest>();

		private readonly List<DMItem> _requestsItems = new List<DMItem>();

		private int _currentItem;

		private bool _canRepaint;

		private float _canRepaintUntil = float.MinValue;

		#endregion

		#region Public Methods

		public DMBranch(DMBranch parent, string path, string description = "", int order = 0) : base(parent, path, description, order)
		{ }

		// Requests
		public DMBranchRequest<T> Add<T>(Func<IList<T>> request, Func<T, string> name = null, Func<T, string> description = null)
		{
			var item = new DMBranchRequest<T>(request, name, description);
			_requests.Add(item);
			return item;
		}

		public DMActionRequest<T> Add<T>(Func<IList<T>> request, Action<DMAction> action = null, Func<T, string> name = null, Func<T, string> description = null)
		{
			var item = new DMActionRequest<T>(request, action, name, description);
			_requests.Add(item);
			return item;
		}

		public DMIntegerRequest<T> Add<T>(Func<IList<T>> request, Func<int> getter, Action<int> setter = null, Func<T, string> name = null)
		{
			var item = new DMIntegerRequest<T>(request, getter, setter, name);
			_requests.Add(item);
			return item;
		}

		public DMFloatRequest<T> Add<T>(Func<IList<T>> request, Func<float> getter, Action<float> setter = null, Func<T, string> name = null)
		{
			var item = new DMFloatRequest<T>(request, getter, setter, name);
			_requests.Add(item);
			return item;
		}

		public DMBoolRequest<T> Add<T>(Func<IList<T>> request, Func<bool> getter, Action<bool> setter = null, Func<T, string> name = null)
		{
			var item = new DMBoolRequest<T>(request, getter, setter, name);
			_requests.Add(item);
			return item;
		}

		public DMEnumRequest<TEnum, TObject> Add<TEnum, TObject>(Func<IList<TObject>> request, Func<TEnum> getter, Action<TEnum> setter = null, Func<TObject, string> name = null) where TEnum : struct, Enum
		{
			var item = new DMEnumRequest<TEnum, TObject>(request, getter, setter, name);
			_requests.Add(item);
			return item;
		}

		// Manage
		public void Insert(DMItem item)
		{
			_items.Add(item);

			Resort();
		}

		public DMBranch Get(string path, bool create = false) => Get(string.IsNullOrEmpty(path) ? null : path.Split('/'), create);

		public DMBranch Get(string[] path, bool create = false)
		{
			if (path == null)
				return this;

			var branch = this;

			for (var i = 0; i < path.Length; i++)
			{
				var name = path[i];

				var item = branch._items.Find(item => item.Name == name);
				if (item == null)
				{
					if (create)
                    {
						item = new DMBranch(branch, name);
					}
					else
					{
						return null;
					}
				}

				var newBranch = item as DMBranch;
				if (newBranch == null)
				{
					// TODO: Exception.
					return null;
				}

				branch = newBranch;
			}


			return branch;
		}

		public void Remove(DMItem item)
		{
			_items.Remove(item);
		}

		public void Resort()
		{
			int Comparison(DMItem x, DMItem y) => x.Order.CompareTo(y.Order);

			_items.Sort(Comparison);
			_canRepaint = true;
		}

		// Repaint
		public bool CanRepaint() => _canRepaint || _canRepaintUntil > Time.unscaledDeltaTime;

		public void RequestRepaint() => _canRepaint = true;

		public void RequestRepaint(float duration) => _canRepaintUntil = Time.unscaledTime + duration;

		#endregion

		#region Protected Methods

		protected override void OnEvent(EventTag eventTag)
		{
			if (eventTag == EventTag.OpenBranch)
			{
				// Requests
				if (_requestsItems.Count == 0)
				{
					foreach (var request in _requests)
					{
						_requestsItems.AddRange(request.BuildItems(this));
					}
				}

				OnOpen?.Invoke(this);
			}
			else if (eventTag == EventTag.CloseBranch)
			{
				// Requests
				if (_requestsItems.Count != 0)
				{
					foreach (var item in _requestsItems)
					{
						Remove(item);
					}

					_requestsItems.Clear();
				}

				OnClose?.Invoke(this);
			}
			else if (eventTag == EventTag.Up)
			{
				_currentItem--;

				if (_currentItem < 0)
					_currentItem = _items.Count - 1;
			}
			else if (eventTag == EventTag.Down)
			{
				_currentItem++;

				if (_currentItem >= _items.Count)
					_currentItem = 0;
			}
			else if (eventTag == EventTag.Left)
			{
				var currentItem = Current;
				if (currentItem is DMBranch)
				{
					if (DM.IsVisible)
						DM.Back();
				}
				else
				{
					currentItem?.SendEvent(eventTag);
				}
			}
			else if (eventTag == EventTag.Right)
			{
				var currentItem = Current;
				if (currentItem is DMBranch currentBranch)
				{
					if (DM.IsVisible)
						DM.Open(currentBranch);
				}
				else
				{
					currentItem?.SendEvent(eventTag);
				}
			}
			else if (eventTag == EventTag.Reset)
			{
				var currentItem = Current;
				if (currentItem is DMBranch currentBranch)
				{
					// None
				}
				else
				{
					currentItem?.SendEvent(eventTag);
				}
			}
			else if (eventTag == EventTag.Back)
			{
				DM.Back();
			}
		}

		#endregion
    }
}
