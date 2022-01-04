/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System;
using System.Collections.Generic;

namespace extDebug.Menu
{
	public class DMContainer : IDMBranch
	{
		#region Public Vars

		public readonly DMBranch Root;

		public IDMInput Input;

		public IDMRender Render;

		public bool IsVisible { get; private set; }

		#endregion

		#region Private Vars

		private const float kRepeatDelay = 0.75f;

		private const float kRepeatInterval = 0.1f;

		private DMBranch _currentBranch;

		private DMBranch _previousBranch => _branchesStack.Count > 0 ? _branchesStack.Peek() : null;

		private readonly Stack<DMBranch> _branchesStack = new Stack<DMBranch>();

		private EventKey _previousKey;

		private float _repeatTime;

		#endregion

		#region Public Methods

		public DMContainer(string name) : this(name, new DMDefaultInput(), new DMDefaultRender())
		{ }

		public DMContainer(string name, IDMInput input, IDMRender render)
		{
			// Create Root object.
			Root = new DMBranch(null, name);
			Root.Container = this;

			// Setup modules.
			Input = input;
			Render = render;
		}

		public void Open() => Open(Root);

		public void Open(DMBranch branch)
		{
			if (branch == null)
				throw new ArgumentNullException(nameof(branch));

			if (_currentBranch != null)
			{
				_branchesStack.Push(_currentBranch);
			}

			_currentBranch = branch;
			_currentBranch.Container = this;
			_currentBranch.SendEvent(EventArgs.OpenBranch);
			_currentBranch.RequestRepaint();

			IsVisible = true;
		}

		public void Close()
		{
			while (_previousBranch != null)
			{
				_currentBranch.SendEvent(EventArgs.CloseBranch);
				_currentBranch = _previousBranch;

				_branchesStack.Pop();
			}

			_currentBranch.RequestRepaint();

			IsVisible = false;
		}

		public void Repaint()
		{
			_currentBranch.RequestRepaint();
		}

		public void Back()
		{
			if (_previousBranch != null)
			{
				_currentBranch.SendEvent(EventArgs.CloseBranch);
				_currentBranch = _previousBranch;
				//_currentBranch.SendEvent(EventArgs.OpenBranch); // INFO: Is required?
				_currentBranch.RequestRepaint();

				_branchesStack.Pop();
			}
			else
			{
				IsVisible = false;
			}
		}

		// Shared
		public void Update()
		{
			// Input
			if (Input != null)
			{
				var eventKey = GetKey(Time.unscaledTime, out var isShift);
				if (eventKey != EventKey.None)
				{
					SendKey(eventKey, isShift);
				}
			}

			// Render
			if (Render != null)
			{
				// Cool! Looks like shit
				// Invoke Update callback
				(Render as IDMRender_Update)?.Update(IsVisible);

				if (IsVisible && _currentBranch != null && _currentBranch.CanRepaint())
				{
					var items = _currentBranch.GetItems();

					foreach (var item in items)
					{
						item.SendEvent(EventArgs.Repaint);
					}
					
					Render.Repaint(_currentBranch, items);

					_currentBranch.CompleteRepaint();
				}
			}
		}

		public void OnGUI()
		{
			// Invoke OnGUI callback
			(Render as IDMRender_OnGUI)?.OnGUI(IsVisible);
		}

		public void Notify(DMItem item, Color? nameColor = null, Color? valueColor = null) =>
			DM.Notify(item, nameColor, valueColor);

		// Branch
		public DMBranch Add(string path, string description = "", int order = 0) =>
			Root.Add(path, description, order);

		// String
		public DMString Add(string path, Func<string> getter, int order = 0) => 
			Root.Add(path, getter, order);

		// Action
		public DMAction Add(string path, Action<DMAction> action, string description = "", int order = 0) =>
			Root.Add(path, action, description, order);

		// Bool
		public DMBool Add(string path, Func<bool> getter, Action<bool> setter = null, int order = 0) =>
			Root.Add(path, getter, setter, order);

		// Enums
		public DMEnum<T> Add<T>(string path, Func<T> getter, Action<T> setter = null, int order = 0) where T : struct, Enum =>
			Root.Add(path, getter, setter, order);

		// UInt8
		public DMUInt8 Add(string path, Func<byte> getter, Action<byte> setter = null, int order = 0) =>
			Root.Add(path, getter, setter, order);

		// UInt16
		public DMUInt16 Add(string path, Func<UInt16> getter, Action<UInt16> setter = null, int order = 0) =>
			Root.Add(path, getter, setter, order);

		// UInt32
		public DMUInt32 Add(string path, Func<UInt32> getter, Action<UInt32> setter = null, int order = 0) =>
			Root.Add(path, getter, setter, order);

		// UInt64
		public DMUInt64 Add(string path, Func<UInt64> getter, Action<UInt64> setter = null, int order = 0) =>
			Root.Add(path, getter, setter, order);

		// Int8
		public DMInt8 Add(string path, Func<sbyte> getter, Action<sbyte> setter = null, int order = 0) =>
			Root.Add(path, getter, setter, order);

		// Int16
		public DMInt16 Add(string path, Func<Int16> getter, Action<Int16> setter = null, int order = 0) =>
			Root.Add(path, getter, setter, order);

		// Int32
		public DMInt32 Add(string path, Func<Int32> getter, Action<Int32> setter = null, int order = 0) =>
			Root.Add(path, getter, setter, order);

		// Int64
		public DMInt64 Add(string path, Func<Int64> getter, Action<Int64> setter = null, int order = 0) =>
			Root.Add(path, getter, setter, order);

		// Float
		public DMFloat Add(string path, Func<float> getter, Action<float> setter = null, int order = 0) =>
			Root.Add(path, getter, setter, order);

		// Vector 2
		public DMVector2 Add(string path, Func<Vector2> getter, Action<Vector2> setter = null, int order = 0) =>
			Root.Add(path, getter, setter, order);

		// Vector 3
		public DMVector3 Add(string path, Func<Vector3> getter, Action<Vector3> setter = null, int order = 0) =>
			Root.Add(path, getter, setter, order);

		// Vector 4
		public DMVector4 Add(string path, Func<Vector4> getter, Action<Vector4> setter = null, int order = 0) =>
			Root.Add(path, getter, setter, order);
		
		// Quaternion
		public DMQuaternion Add(string path, Func<Quaternion> getter, Action<Quaternion> setter = null, int order = 0) =>
			Root.Add(path, getter, setter, order);

		// Color
		public DMColor Add(string path, Func<Color> getter, Action<Color> setter = null, int order = 0) =>
			Root.Add(path, getter, setter, order);

		// Vector 2 Int
		public DMVector2Int Add(string path, Func<Vector2Int> getter, Action<Vector2Int> setter = null, int order = 0) =>
			Root.Add(path, getter, setter, order);

		// Vector 3 Int
		public DMVector3Int Add(string path, Func<Vector3Int> getter, Action<Vector3Int> setter = null, int order = 0) =>
			Root.Add(path, getter, setter, order);

		// Dynamic
		public DMBranch Add<T>(string path, Func<IEnumerable<T>> getter, Action<DMBranch, T> buildCallback = null, Func<T, string> nameCallback = null, string description = "", int order = 0) =>
			Root.Add(path, getter, buildCallback, nameCallback, description, order);

		#endregion

		#region Private Methods

		private DMContainer()
		{ }

		private EventKey GetKey(float currentTime, out bool shift)
		{
			if (Input != null)
			{
				var eventKey = Input.GetKey(IsVisible, out shift);
				if (eventKey != _previousKey)
				{
					_previousKey = eventKey;
					_repeatTime = currentTime + kRepeatDelay;

					return eventKey;
				}

				if (eventKey != EventKey.None && _repeatTime < currentTime)
				{
					_repeatTime = currentTime + kRepeatInterval;

					return eventKey;
				}
			}

			shift = false;

			return EventKey.None;
		}

		private void SendKey(EventKey eventKey, bool isShift)
		{
			if (eventKey == EventKey.ToggleMenu)
			{
				IsVisible = !IsVisible;

				if (_currentBranch == null && IsVisible)
				{
					Open(Root);
				}
			}

			_currentBranch?.SendEvent(new EventArgs
			{
				Tag = EventTag.Input,
				Key = eventKey,
				IsShift = isShift
			});
		}

		#endregion
	}
}