/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System;

namespace extDebug.Menu
{
	public class DMVector3Int : DMUnityIntStruct<Vector3Int>
	{
		#region Public Methods

		public DMVector3Int(DMBranch parent, string path, Func<Vector3Int> getter, Action<Vector3Int> setter = null, int order = 0) : base(parent, path, getter, setter, order)
		{ }

		#endregion

		#region Protected Methods

		protected override int StructFieldGetter(Vector3Int vector, int fieldIndex) => vector[fieldIndex];

		protected override void StructFieldSetter(ref Vector3Int vector, int fieldIndex, int value) => vector[fieldIndex] = value;

		#endregion
	}
}