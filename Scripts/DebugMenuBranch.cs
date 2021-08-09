/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System.Collections.Generic;
using UnityEngine;

namespace extDebug
{
    public class DebugMenuBranch : DebugMenuItem
    {
		#region Public Methods

		public DebugMenuItem Current
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

		#endregion

		#region Private Vars

		private readonly List<DebugMenuItem> _items = new List<DebugMenuItem>();

		private int _currentItem;

		private bool _canRepaint;

		private float _canRepaintUntil = float.MinValue;

		#endregion

		#region Public Methods

		public DebugMenuBranch(DebugMenuBranch parent, string path, int order = 0) : base(parent, path, order)
		{ }

		// Manage
		public void Add(DebugMenuItem item)
		{
			_items.Add(item);

			Resort();
		}

		public DebugMenuBranch Get(string path, bool create = false) => Get(path.Split('/'), create);

		public DebugMenuBranch Get(string[] path, bool create = false)
		{
			var branch = this;

			for (var i = 0; i < path.Length; i++)
			{
				var name = path[i];

				var item = branch._items.Find(item => item.Name == name);
				if (item == null)
				{
					if (create)
                    {
						item = new DebugMenuBranch(branch, name);
					}
					else
					{
						return null;
					}
				}

				var newBranch = item as DebugMenuBranch;
				if (newBranch == null)
				{
					// TODO: Exception.
					return null;
				}

				branch = newBranch;
			}


			return branch;
		}

		public void Remove(DebugMenuItem item)
		{
			_items.Remove(item);
		}

		public void Resort()
		{
			int Comparison(DebugMenuItem x, DebugMenuItem y) => x.Order.CompareTo(y.Order);

			_items.Sort(Comparison);
			_canRepaint = true;
		}

		// Repaint
		public bool CanRepaint() => _canRepaint || _canRepaintUntil > Time.unscaledDeltaTime;

		public void RequestRepaint() => _canRepaint = true;

		public void RequestRepaint(float duration) => _canRepaintUntil = Time.unscaledTime + duration;

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
						if (DebugMenu.IsVisible && currentItem is DebugMenuBranch)
						{
							DebugMenu.Back();
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
						if (DebugMenu.IsVisible && currentItem is DebugMenuBranch currentBranch)
						{
							DebugMenu.Open(currentBranch);
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
						DebugMenu.Back();
						
						break;
					}
				}
			}
			else if (eventArgs.Event == EventType.OpenMenu)
			{
				// TODO: Event on open menu
			}
			else if (eventArgs.Event == EventType.CloseMenu)
			{
				// TODO: Event on close menu
			}
		}

		#endregion
	}
}
