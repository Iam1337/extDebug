/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System;

namespace extDebug.Menu
{
	public class DMVector4 : DMUnityFloatStruct<Vector4>
	{
		#region Public Methods

		public DMVector4(DMBranch parent, string path, Func<Vector4> getter, Action<Vector4> setter = null, int order = 0) : base(parent, path, getter, setter, order)
		{ }

		#endregion

		#region Protected Methods

		protected override float StructFieldGetter(Vector4 vector, int fieldIndex) => vector[fieldIndex];

		protected override void StructFieldSetter(ref Vector4 vector, int fieldIndex, float value) => vector[fieldIndex] = value;

		#endregion
	}
}