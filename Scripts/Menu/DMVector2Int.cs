/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System;

namespace extDebug.Menu
{
	public class DMVector2Int : DMUnityIntStruct<Vector2Int>
	{
		#region Public Methods

		public DMVector2Int(DMBranch parent, string path, Func<Vector2Int> getter, Action<Vector2Int> setter = null, int order = 0) : base(parent, path, getter, setter, order)
		{ }

		#endregion

		#region Protected Methods

		protected override int StructFieldGetter(Vector2Int vector, int fieldIndex) => vector[fieldIndex];

		protected override void StructFieldSetter(ref Vector2Int vector, int fieldIndex, int value) => vector[fieldIndex] = value;

		#endregion
	}
}