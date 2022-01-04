/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System;
using System.Collections.Generic;

namespace extDebug.Menu
{
	public interface IDMBranch
	{
		#region Methods

		// Branch
		public DMBranch Add(string path, string description = "", int order = 0);

		// String
		public DMString Add(string path, Func<string> getter, int order = 0);

		// Action
		public DMAction Add(string path, Action<DMAction> action, string description = "", int order = 0);

		// Bool
		public DMBool Add(string path, Func<bool> getter, Action<bool> setter = null, int order = 0);

		// Enum
		public DMEnum<T> Add<T>(string path, Func<T> getter, Action<T> setter = null, int order = 0) where T : struct, Enum;

		// UInt8
		public DMUInt8 Add(string path, Func<byte> getter, Action<byte> setter = null, int order = 0);

		// UInt16
		public DMUInt16 Add(string path, Func<UInt16> getter, Action<UInt16> setter = null, int order = 0);

		// UInt32
		public DMUInt32 Add(string path, Func<UInt32> getter, Action<UInt32> setter = null, int order = 0);

		// UInt64
		public DMUInt64 Add(string path, Func<UInt64> getter, Action<UInt64> setter = null, int order = 0);

		// Int8
		public DMInt8 Add(string path, Func<sbyte> getter, Action<sbyte> setter = null, int order = 0);

		// Int16
		public DMInt16 Add(string path, Func<Int16> getter, Action<Int16> setter = null, int order = 0);

		// Int32
		public DMInt32 Add(string path, Func<Int32> getter, Action<Int32> setter = null, int order = 0);

		// Int64
		public DMInt64 Add(string path, Func<Int64> getter, Action<Int64> setter = null, int order = 0);

		// Float
		public DMFloat Add(string path, Func<float> getter, Action<float> setter = null, int order = 0);

		// Vector 2
		public DMVector2 Add(string path, Func<Vector2> getter, Action<Vector2> setter = null, int order = 0);

		// Vector 3
		public DMVector3 Add(string path, Func<Vector3> getter, Action<Vector3> setter = null, int order = 0);

		// Vector 4
		public DMVector4 Add(string path, Func<Vector4> getter, Action<Vector4> setter = null, int order = 0);

		// Quaternion
		public DMQuaternion Add(string path, Func<Quaternion> getter, Action<Quaternion> setter = null, int order = 0);

		// Color
		public DMColor Add(string path, Func<Color> getter, Action<Color> setter = null, int order = 0);

		// Vector 2 Int
		public DMVector2Int Add(string path, Func<Vector2Int> getter, Action<Vector2Int> setter = null, int order = 0);

		// Vector 3 Int
		public DMVector3Int Add(string path, Func<Vector3Int> getter, Action<Vector3Int> setter = null, int order = 0);

		// Dynamic
		public DMBranch Add<T>(string path, Func<IEnumerable<T>> getter, Action<DMBranch, T> buildCallback = null, Func<T, string> nameCallback = null, string description = "", int order = 0);

		#endregion
	}
}