/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System;

namespace extDebug.Menu
{
	public class DMVector2 : DMVector<Vector2>
	{
		#region Public Methods

		public DMVector2(DMBranch parent, string path, Func<Vector2> getter, Action<Vector2> setter = null, int order = 0) : base(parent, path, getter, setter, order)
		{ }

		#endregion

		#region Protected Methods

		protected override float VectorFieldGetter(Vector2 vector, int fieldIndex) => vector[fieldIndex];

		protected override void VectorFieldSetter(ref Vector2 vector, int fieldIndex, float value) => vector[fieldIndex] = value;

		#endregion
	}
}