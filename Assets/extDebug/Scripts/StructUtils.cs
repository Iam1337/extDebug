/* Copyright (c) 2024 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System;

namespace extDebug
{
	internal static class StructUtils
	{
		#region Public Vars

		public static readonly string[] VectorFields = new[] { "X", "Y", "Z", "W" };

		public static readonly string[] ColorsFields = new[] { "Red", "Green", "Blue", "Alpha" };

		#endregion

		#region Public Methods

		public static string[] GetFieldsNames(Type type)
		{
			if (type == typeof(Color))
				return ColorsFields;

			return VectorFields;
		}

		public static int GetFieldsCount(Type type)
		{
			if (type == typeof(Vector3) ||
			    type == typeof(Vector3Int))
				return 3;
			if (type == typeof(Vector2) ||
			    type == typeof(Vector2Int))
				return 2;
			if (type == typeof(Vector4) ||
			    type == typeof(Quaternion) ||
			    type == typeof(Color))
				return 4;

			return 0;
		}

		public static Type GetFieldsType(Type type)
		{
			if (type == typeof(Vector2Int) ||
			    type == typeof(Vector3Int))
				return typeof(int);

			return typeof(float);
		}

		#endregion
	}
}