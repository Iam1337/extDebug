/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System;

namespace extDebug
{
	public class DMFloat : DMValue<float>
	{
		#region Public Vars

		private static readonly string[] _formats = new []{ "0", "0.0", "0.00", "0.000", "0.0000", "0.00000", "0.000000", "0.0000000", "0.00000000" };

		#endregion

		#region Public Vars

		public string Format;

		public int Step = 1;

		#endregion

		#region Private Vars

		private int _precision;

		private int _floatPointScale;

		#endregion

		#region Public Methods

		public DMFloat(DMBranch parent, string path, Func<float> getter, Action<float> setter = null, int order = 0) : base(parent, path, getter, setter, order)
		{
			SetPrecision(1);
		}

		public void SetPrecision(int value)
		{
			_precision = Mathf.Clamp(value, 0, _formats.Length - 1);
			_floatPointScale = (int)Mathf.Pow(10, _precision);
		}

		#endregion

		#region Private Methods

		protected override string ValueToString(float value) => value.ToString(string.IsNullOrEmpty(Format) ? _formats[_precision] : Format );

		protected override float ValueIncrement(float value) => (Mathf.Floor(value * _floatPointScale + 0.1f) + Step) / _floatPointScale;

		protected override float ValueDecrement(float value) => (Mathf.Floor(value * _floatPointScale + 0.1f) - Step) / _floatPointScale;

		#endregion
	}
}