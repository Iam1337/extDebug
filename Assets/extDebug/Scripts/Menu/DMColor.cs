/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System;

namespace extDebug.Menu
{
	public class DMColor : DMUnityFloatStruct<Color>
	{
		#region Public Methods

		public DMColor(DMBranch parent, string path, Func<Color> getter, Action<Color> setter = null, int order = 0) : base(parent, path, getter, setter, order)
		{ }

		#endregion

		#region Protected Methods

		protected override float StructFieldGetter(Color vector, int fieldIndex) => vector[fieldIndex];

		protected override void StructFieldSetter(ref Color vector, int fieldIndex, float value) => vector[fieldIndex] = value;

		#endregion
	}
}