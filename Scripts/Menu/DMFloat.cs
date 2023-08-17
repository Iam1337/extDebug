/* Copyright (c) 2023 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System;

namespace extDebug.Menu
{
	public class DMFloat : DMValue<float>
	{
		#region Public Vars

		public string Format;

		public int Step = 1;

		public int ShiftStep = 10;

		#endregion

		#region Private Vars

		private int _precision;

		private int _floatPointScale;

		#endregion

		#region Public Methods

		public DMFloat(DMBranch parent, string path, Func<float> getter, Action<float> setter = null, float[] variants = null, int order = 0) : base(parent, path, getter, setter, variants, order)
		{
			SetPrecision(2);
		}

		public DMFloat SetPrecision(int value)
		{
			_precision = Mathf.Clamp(value, 0, FloatUtils.Formats.Length - 1);
			_floatPointScale = (int)Mathf.Pow(10, _precision);

			return this;
		}

		#endregion

		#region Private Methods

		protected override string ValueToString(float value) => value.ToString(string.IsNullOrEmpty(Format) ? FloatUtils.Formats[_precision] : Format);

		protected override float ValueIncrement(float value, bool isShift) => (Mathf.Floor(value * _floatPointScale + 0.1f) + (isShift ? ShiftStep : Step)) / _floatPointScale;

		protected override float ValueDecrement(float value, bool isShift) => (Mathf.Floor(value * _floatPointScale + 0.1f) - (isShift ? ShiftStep : Step)) / _floatPointScale;

		#endregion
	}
}