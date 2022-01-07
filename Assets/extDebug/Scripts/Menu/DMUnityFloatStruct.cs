/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System;

namespace extDebug.Menu
{
	public abstract class DMUnityFloatStruct<TStruct> : DMValue<TStruct> where TStruct : struct, IFormattable
	{
		#region Public Vars

		public string Format;

		public int Step = 1;

		#endregion

		#region Private Vars

		private readonly DMBranch _fieldsBranch;

		private readonly DMFloat[] _fields;

		private DMBranch _presetsBranch;

		private int _precision;

		private int _floatPointScale;

		#endregion

		#region Public Vars

		public void SetPrecision(int value)
		{
			_precision = Mathf.Clamp(value, 0, FloatUtils.Formats.Length - 1);
			_floatPointScale = (int)Mathf.Pow(10, _precision);

			for (var i = 0; i < _fields.Length; i++)
			{
				_fields[i].SetPrecision(value);
			}
		}

		public void AddPreset(string name, TStruct value)
		{
			if (_setter == null)
				return; // TODO: Add exception.

			if (_presetsBranch == null)
			{
				_presetsBranch = _fieldsBranch.Add("Presets", order: byte.MaxValue - 2);
				_presetsBranch.Add("Back", BackAction, string.Empty, int.MaxValue);
			}

			_presetsBranch.Add(name, a =>
			{
				_setter.Invoke(value);
				Container.Back();
			}, ValueToString(value));
		}

		#endregion

		#region Protected Methods

		protected DMUnityFloatStruct(DMBranch parent, string path, Func<TStruct> getter, Action<TStruct> setter = null, int order = 0) : base(parent, path, getter, setter, order)
		{
			if (setter != null)
			{
				var names = StructUtils.GetFieldsNames(typeof(TStruct));
				var count = StructUtils.GetFieldsCount(typeof(TStruct));

				_fields = new DMFloat[count];
				_fieldsBranch = new DMBranch(null, GetPathName(path));
				_fieldsBranch.Container = Container;

				for (var i = 0; i < count; i++)
				{
					var fieldIndex = i;
					_fields[i] = _fieldsBranch.Add(names[i],
						() => StructFieldGetter(getter.Invoke(), fieldIndex), v =>
						{
							var vector = getter.Invoke();
							StructFieldSetter(ref vector, fieldIndex, v);
							setter.Invoke(vector);
						}, i);
				}

				_fieldsBranch.Add("Back", BackAction, string.Empty, int.MaxValue);
			}

			SetPrecision(2);
		}

		protected override void OnEvent(EventArgs eventArgs)
		{
			if (eventArgs.Tag == EventTag.Input && !eventArgs.IsShift)
			{
				if (_fieldsBranch != null)
				{
					if (eventArgs.Key == EventKey.Left)
					{
						if (Container.IsVisible)
							Container.Back();

						return;
					}

					if (eventArgs.Key == EventKey.Right)
					{
						if (Container.IsVisible)
							Container.Open(_fieldsBranch);

						return;
					}
				}
			}

			base.OnEvent(eventArgs);
		}

		protected sealed override string ValueToString(TStruct value) => value.ToString(string.IsNullOrEmpty(Format) ? FloatUtils.Formats[_precision] : Format, null);

		protected sealed override TStruct ValueIncrement(TStruct value, bool isShift)
		{
			var count = StructUtils.GetFieldsCount(typeof(TStruct));
			for (var i = 0; i < count; i++)
			{
				StructFieldSetter(ref value, i, (Mathf.Floor(StructFieldGetter(value, i) * _floatPointScale + 0.1f) + Step) / _floatPointScale);
			}

			return value;
		}

		protected sealed override TStruct ValueDecrement(TStruct value, bool isShift)
		{
			var count = StructUtils.GetFieldsCount(typeof(TStruct));
			for (var i = 0; i < count; i++)
			{
				StructFieldSetter(ref value, i, (Mathf.Floor(StructFieldGetter(value, i) * _floatPointScale + 0.1f) - Step) / _floatPointScale);
			}

			return value;
		}

		protected abstract float StructFieldGetter(TStruct vector, int fieldIndex);

		protected abstract void StructFieldSetter(ref TStruct vector, int fieldIndex, float value);

		#endregion

		#region Private Methods

		private void BackAction(DMAction action) => Container.Back();

		#endregion
	}
}