/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System;
using System.Collections.Generic;

namespace extDebug.Menu
{
	public class DMBranch : DMItem, IDMContainer
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

		public float AutoRepaint
		{
			get => _autoRepaintPeriod;
			set
			{
				_autoRepaintPeriod = value;
				_autoRepaintAt = value > 0 ? 0 : float.MaxValue;
			}
		}

		#endregion

		#region Private Vars

		private readonly List<DMItem> _items = new List<DMItem>();

        private int _currentItem;

		private bool _canRepaint;

		private float _canRepaintUntil = float.MinValue;

		private float _autoRepaintPeriod;

		private float _autoRepaintAt = float.MaxValue;

		#endregion

		#region Public Methods

		public DMBranch(DMBranch parent, string path, string description = "", int order = 0) : base(parent, path, description, order)
		{
			_valueColor = DM.Colors.Description;
		}

        // Manage
		public void Insert(DMItem item)
		{
			_items.Add(item);

			Resort();
		}

		public void Remove(DMItem item)
		{
			_items.Remove(item);
		}

        public void Clear()
        {
            _currentItem = 0;
			_items.Clear();
        }

		// Container
        public DMBranch Add(string path, string description = "", int order = 0) => Get(path) ?? new DMBranch(this, path, description, order);
        
        public DMString Add(string path, Func<string> getter, int order = 0) => new DMString(this, path, getter, order);

        public DMAction Add(string path, Action<DMAction> action, string description = "", int order = 0) => new DMAction(this, path, description, action, order);

        public DMBool Add(string path, Func<bool> getter, Action<bool> setter = null, int order = 0) => new DMBool(this, path, getter, setter, order);

		public DMEnum<T> Add<T>(string path, Func<T> getter, Action<T> setter = null, int order = 0) where T : struct, Enum => new DMEnum<T>(this, path, getter, setter, order);

		public DMUInt8 Add(string path, Func<byte> getter, Action<byte> setter = null, int order = 0) => new DMUInt8(this, path, getter, setter, order);

		public DMUInt16 Add(string path, Func<ushort> getter, Action<ushort> setter = null, int order = 0) => new DMUInt16(this, path, getter, setter, order);

		public DMUInt32 Add(string path, Func<uint> getter, Action<uint> setter = null, int order = 0) => new DMUInt32(this, path, getter, setter, order);

		public DMUInt64 Add(string path, Func<ulong> getter, Action<ulong> setter = null, int order = 0) => new DMUInt64(this, path, getter, setter, order);

		public DMInt8 Add(string path, Func<sbyte> getter, Action<sbyte> setter = null, int order = 0) => new DMInt8(this, path, getter, setter, order);

		public DMInt16 Add(string path, Func<short> getter, Action<short> setter = null, int order = 0) => new DMInt16(this, path, getter, setter, order);

		public DMInt32 Add(string path, Func<int> getter, Action<int> setter = null, int order = 0) => new DMInt32(this, path, getter, setter, order);

		public DMInt64 Add(string path, Func<long> getter, Action<long> setter = null, int order = 0) => new DMInt64(this, path, getter, setter, order);

		public DMFloat Add(string path, Func<float> getter, Action<float> setter = null, int order = 0) => new DMFloat(this, path, getter, setter, order);
        public DMVector2 Add(string path, Func<Vector2> getter, Action<Vector2> setter = null, int order = 0) => new DMVector2(this, path, getter, setter, order);

        public DMVector3 Add(string path, Func<Vector3> getter, Action<Vector3> setter = null, int order = 0) => new DMVector3(this, path, getter, setter, order);

        public DMVector4 Add(string path, Func<Vector4> getter, Action<Vector4> setter = null, int order = 0) => new DMVector4(this, path, getter, setter, order);

        public DMBranch Add<T>(string path, Func<IEnumerable<T>> getter, Action<DMBranch, T> buildCallback = null, Func<T, string> nameCallback = null, string description = "", int order = 0)
        {
            if (getter == null)
                throw new NullReferenceException(nameof(getter));

            var dynamicBranch = Add(path, description, order);
            dynamicBranch.OnOpen += dBranch =>
            {
                dBranch.Clear();

                var index = 0;
                var objects = getter.Invoke();

                foreach (var obj in objects)
                {
                    var name = nameCallback != null ? nameCallback.Invoke(obj) : obj.ToString();
                    var objectTemp = obj;
                    var objectBranch = dBranch.Add(name, string.Empty, index++);

                    objectBranch.Data = objectTemp;
                    objectBranch.OnOpen += oBranch =>
                    {
                        oBranch.Clear();
                        buildCallback?.Invoke(oBranch, objectTemp);
                    };
                }
            };

            return dynamicBranch;
		}

        // Repaint
		public void RequestRepaint() => _canRepaint = true;

		public void RequestRepaint(float duration) => _canRepaintUntil = Time.unscaledTime + duration;

		#endregion

		#region Internal Methods

		internal IReadOnlyList<DMItem> GetItems() => _items.AsReadOnly();

		internal void Resort()
		{
			int Comparison(DMItem x, DMItem y) => x.Order.CompareTo(y.Order);

			_items.Sort(Comparison);
			_canRepaint = true;
		}
		
		internal DMBranch Get(string path, bool create = false) => Get(string.IsNullOrEmpty(path) ? null : path.Split('/'), create);

		internal DMBranch Get(string[] path, bool create = false)
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
                    return null;
				}

				branch = newBranch;
			}
			
			return branch;
		}

		internal bool CanRepaint() => _canRepaint || _canRepaintUntil > Time.unscaledTime || Time.unscaledTime > _autoRepaintAt;

		internal void CompleteRepaint()
		{
			if (_autoRepaintPeriod > 0)
				_autoRepaintAt = Time.unscaledTime + _autoRepaintPeriod;

			_canRepaint = false;
		}

		#endregion

		#region Protected Methods

		protected override void OnEvent(EventArgs eventArgs)
		{
			if (eventArgs.Tag == EventTag.OpenBranch)
			{
                OnOpen?.Invoke(this);
			}
			else if (eventArgs.Tag == EventTag.CloseBranch)
			{
                OnClose?.Invoke(this);
			}
			else if (eventArgs.Tag == EventTag.Input)
			{
				if (eventArgs.Key == EventKey.Up)
				{
					_currentItem--;

					if (_currentItem < 0)
						_currentItem = _items.Count - 1;

					RequestRepaint();
				}
				else if (eventArgs.Key == EventKey.Down)
				{
					_currentItem++;

					if (_currentItem >= _items.Count)
						_currentItem = 0;

					RequestRepaint();
				}
				else if (eventArgs.Key == EventKey.Left)
				{
					var currentItem = Current;
					if (currentItem is DMBranch)
					{
						if (Container.IsVisible)
							Container.Back();
					}
					else
					{
						currentItem?.SendEvent(eventArgs);
					}
				}
				else if (eventArgs.Key == EventKey.Right)
				{
					var currentItem = Current;
					if (currentItem is DMBranch currentBranch)
					{
						if (Container.IsVisible)
							Container.Open(currentBranch);
					}
					else
					{
						currentItem?.SendEvent(eventArgs);
					}
				}
				else if (eventArgs.Key == EventKey.Reset)
				{
					var currentItem = Current;
					if (currentItem is DMBranch currentBranch)
					{
						// None
					}
					else
					{
						currentItem?.SendEvent(eventArgs);
					}
				}
				else if (eventArgs.Key == EventKey.Back)
				{
					Container.Back();
				}
			}
		}

		#endregion
    }
}