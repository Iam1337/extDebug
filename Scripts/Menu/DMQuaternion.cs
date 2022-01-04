/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System;

namespace extDebug.Menu
{
	public class DMQuaternion : DMUnityFloatStruct<Quaternion>
	{
		#region Public Methods

		public DMQuaternion(DMBranch parent, string path, Func<Quaternion> getter, Action<Quaternion> setter = null, int order = 0) : base(parent, path, getter, setter, order)
		{ }

		#endregion

		#region Protected Methods

		protected override float StructFieldGetter(Quaternion vector, int fieldIndex) => vector[fieldIndex];

		protected override void StructFieldSetter(ref Quaternion vector, int fieldIndex, float value) => vector[fieldIndex] = value;

		#endregion
	}
}