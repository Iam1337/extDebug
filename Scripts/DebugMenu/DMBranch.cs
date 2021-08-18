/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System;
using System.Text;
using System.Collections.Generic;

namespace extDebug
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

		public DMBranchRequest<T> Branch<T>(Func<IList<T>> request, Func<T, string> name = null, Func<T, string> description = null)
		{
			var item = new DMBranchRequest<T>(request, name, description);
			_requests.Add(item);
			return item;
		}

		public DMActionRequest<T> Action<T>(Func<IList<T>> request, Action<DMAction, EventArgs> action = null, Func<T, string> name = null, Func<T, string> description = null)
		{
			var item = new DMActionRequest<T>(request, action, name, description);
			_requests.Add(item);
			return item;
		}

		public DMIntegerRequest<T> Integer<T>(Func<IList<T>> request, Func<int> getter, Action<int> setter = null, Func<T, string> name = null)
		{
			var item = new DMIntegerRequest<T>(request, getter, setter, name);
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

		#region Internal Methods

		internal void Build(StringBuilder builder)
		{
			const string kSuffix = " ";
			const string kPrefix = " ";
			const string kPrefix_Selected = ">";
			const string kSpace = "  ";
			const char kHorizontalChar = '─';

			// send event.
			foreach (var item in _items)
			{
				item.SendEvent(new EventArgs
				{
					Event = EventType.Repaint,
					Key = KeyType.None
				});
			}

			CalculateLengths(kSpace.Length, out var fullLength, out var maxNameLength, out var maxValueLength);

			var order = -1;
			var lineLength = fullLength + kSuffix.Length + kPrefix.Length;
			var lineEmpty = new string(kHorizontalChar, lineLength);

			// header
			builder.AppendFormat($"{kPrefix}<color=#{ColorUtility.ToHtmlStringRGB(NameColor)}>{{0,{-fullLength}}}</color>{kSuffix}{Environment.NewLine}", Name);
			builder.AppendLine(lineEmpty);

			// items
			for (var i = 0; i < _items.Count; i++)
			{
				var item = _items[i];
				var prefix = i == _currentItem ? kPrefix_Selected : kPrefix;
				
				// items separator
				if (order >= 0 && Math.Abs(order - item.Order) > 1)
					builder.AppendLine(lineEmpty);

				order = item.Order;

				var name = item.Name;
				var value = item.Value;

				if (item is DMBranch)
					name += "...";

				if (string.IsNullOrEmpty(value))
				{
					// only name
					builder.AppendFormat($"{prefix}<color=#{ColorUtility.ToHtmlStringRGB(item.NameColor)}>{{0,-{fullLength}}}</color>{kSuffix}{Environment.NewLine}", name);
				}
				else
				{
					// with value
					builder.AppendFormat($"{prefix}<color=#{ColorUtility.ToHtmlStringRGB(item.NameColor)}>{{0,-{maxNameLength}}}</color>{kSpace}<color=#{ColorUtility.ToHtmlStringRGB(item.ValueColor)}>{{1,-{maxValueLength}}}</color>{kSuffix}{Environment.NewLine}", name, value);
				}
			}
		}

		#endregion

		#region Protected Methods

		protected override void OnEvent(EventArgs eventArgs)
		{
			var currentItem = Current;

			if (eventArgs.Event == EventType.KeyDown)
			{
				switch (eventArgs.Key)
				{
					case KeyType.Up:
					{
						_currentItem--;

						if (_currentItem < 0)
							_currentItem = _items.Count - 1;

						break;
					}
					case KeyType.Down:
					{
						_currentItem++;

						if (_currentItem >= _items.Count)
							_currentItem = 0;

						break;
					}
					case KeyType.Left:
					{
						if (currentItem is DMBranch)
						{
							if (DM.IsVisible)
								DM.Back();
						}
						else
						{
							if (currentItem != null)
								currentItem.SendEvent(eventArgs);
						}

						break;
					}
					case KeyType.Right:
					{
						if (currentItem is DMBranch currentBranch)
						{
							if (DM.IsVisible)
								DM.Open(currentBranch);
						}
						else
						{
							if (currentItem != null)
								currentItem.SendEvent(eventArgs);
						}

						break;
					}
					case KeyType.Reset:
					{
						if (currentItem is DMBranch currentBranch)
						{
							
						}
						else
						{
							if (currentItem != null)
								currentItem.SendEvent(eventArgs);
						}

						break;
					}
					case KeyType.Back:
					{
						DM.Back();
						break;
					}
				}
			}
			else if (eventArgs.Event == EventType.OpenMenu)
			{
				// Requests
				if (_requestsItems.Count == 0)
				{
					foreach (var request in _requests)
					{
						_requestsItems.AddRange(request.BuildItems(this));
					}
				}

				if (OnOpen != null)
					OnOpen.Invoke(this);
			}
			else if (eventArgs.Event == EventType.CloseMenu)
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

				if (OnClose != null)
					OnClose.Invoke(this);
			}
		}

		#endregion

		#region Private Methods

		private void CalculateLengths(int space, out int fullLength, out int maxNameLength, out int maxValueLength)
		{
			maxNameLength = 0;
			maxValueLength = 0;
			fullLength = _name.Length;

			for (var i = 0; i < _items.Count; i++)
			{
				var item = _items[i];
				var nameLength = item.Name.Length;
				var valueLength = item.Name.Length;

				if (item is DMBranch)
					nameLength += 3; // TODO: 3 for "..."

				maxNameLength = Math.Max(maxNameLength, nameLength);
				maxValueLength = Math.Max(maxValueLength, valueLength);
			}

			fullLength = Math.Max(fullLength, maxNameLength + maxValueLength + space);
			maxNameLength = Math.Max(maxNameLength, fullLength - maxValueLength - space);
		}

		#endregion
	}
}
