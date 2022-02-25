/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System;

namespace extDebug.Menu
{
	public abstract class DMUnityIntStruct<TStruct> : DMValue<TStruct> where TStruct : struct, IFormattable
	{
		#region Public Vars

		public int Step = 1;

		public string Format;

		#endregion

		#region Private Vars

		private readonly DMBranch _fieldsBranch;

		private readonly DMInt32[] _fields;

		private DMBranch _presetsBranch;

		#endregion

		#region Public Methods

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

		protected DMUnityIntStruct(DMBranch parent, string path, Func<TStruct> getter, Action<TStruct> setter = null,
			int order = 0) : base(parent, path, getter, setter, order)
		{
			if (setter != null)
			{
				var names = StructUtils.GetFieldsNames(typeof(TStruct));
				var count = StructUtils.GetFieldsCount(typeof(TStruct));

				_fields = new DMInt32[count];
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
						if (Container.IsVisible && IsEnabled())
							Container.Open(_fieldsBranch);

						return;
					}
				}
			}

			base.OnEvent(eventArgs);
		}

		protected sealed override string ValueToString(TStruct value) => value.ToString(Format, null);

		protected sealed override TStruct ValueIncrement(TStruct value, bool isShift)
		{
			var count = StructUtils.GetFieldsCount(typeof(TStruct));
			for (var i = 0; i < count; i++)
			{
				StructFieldSetter(ref value, i, StructFieldGetter(value, i) + Step);
			}

			return value;
		}

		protected sealed override TStruct ValueDecrement(TStruct value, bool isShift)
		{
			var count = StructUtils.GetFieldsCount(typeof(TStruct));
			for (var i = 0; i < count; i++)
			{
				StructFieldSetter(ref value, i, StructFieldGetter(value, i) - Step);
			}

			return value;
		}

		protected abstract int StructFieldGetter(TStruct vector, int fieldIndex);

		protected abstract void StructFieldSetter(ref TStruct vector, int fieldIndex, int value);

		#endregion

		#region Private Methods

		private void BackAction(ActionEvent actionEvent) => Container.Back();

		#endregion
	}

	public class DMVector2Int : DMUnityIntStruct<Vector2Int>
	{
		#region Public Methods

		public DMVector2Int(DMBranch parent, string path, Func<Vector2Int> getter, Action<Vector2Int> setter = null,
			int order = 0) : base(parent, path, getter, setter, order)
		{
		}

		#endregion

		#region Protected Methods

		protected override int StructFieldGetter(Vector2Int vector, int fieldIndex) => vector[fieldIndex];

		protected override void StructFieldSetter(ref Vector2Int vector, int fieldIndex, int value) =>
			vector[fieldIndex] = value;

		#endregion
	}

	public class DMVector3Int : DMUnityIntStruct<Vector3Int>
	{
		#region Public Methods

		public DMVector3Int(DMBranch parent, string path, Func<Vector3Int> getter, Action<Vector3Int> setter = null,
			int order = 0) : base(parent, path, getter, setter, order)
		{
		}

		#endregion

		#region Protected Methods

		protected override int StructFieldGetter(Vector3Int vector, int fieldIndex) => vector[fieldIndex];

		protected override void StructFieldSetter(ref Vector3Int vector, int fieldIndex, int value) =>
			vector[fieldIndex] = value;

		#endregion
	}
}