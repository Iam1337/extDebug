/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System;

namespace extDebug
{
	internal static class VectorUtils
	{
		#region Public Vars

		public static readonly string[] Fields = new[] { "X", "Y", "Z", "W" };

		#endregion

		#region Public Methods

		public static int GetFieldsCount(Type type)
		{
			if (type == typeof(Vector3) ||
			    type == typeof(Vector3Int))
				return 3;
			if (type == typeof(Vector2) ||
			    type == typeof(Vector2Int))
				return 2;
			if (type == typeof(Vector4))
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