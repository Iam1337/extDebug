/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;

using UnityEngine;

namespace extDebug.Menu
{
    public abstract class DMVector<T> : DMValue<T> where T : struct, IFormattable
    {
        #region Public Vars

        public string Format;

        public int Step = 1;

        #endregion

        #region Private Vars

        private readonly DMBranch _fieldsBranch;

        private readonly DMFloat[] _fields;

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

        #endregion

        #region Protected Methods

        protected DMVector(DMBranch parent, string path, Func<T> getter, Action<T> setter = null, int order = 0) : base(parent, path, getter, setter, order)
        {
            if (setter != null)
            {
                var count = VectorUtils.GetFieldsCount(typeof(T));

                _fields = new DMFloat[count];
                _fieldsBranch = new DMBranch(null, GetPathName(path));
                _fieldsBranch.Container = Container;

                for (var i = 0; i < count; i++)
                {
                    var fieldIndex = i;
                    _fields[i] = _fieldsBranch.Add(VectorUtils.Fields[i], () => VectorFieldGetter(getter.Invoke(), fieldIndex), v =>
                    {
                        var vector = getter.Invoke();
                        VectorFieldSetter(ref vector, fieldIndex, v);
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

        protected sealed override string ValueToString(T value) => value.ToString(string.IsNullOrEmpty(Format) ? FloatUtils.Formats[_precision] : Format, null);

        protected sealed override T ValueIncrement(T value, bool isShift)
        {
            var count = VectorUtils.GetFieldsCount(typeof(T));
            for (var i = 0; i < count; i++)
            {
                VectorFieldSetter(ref value, i, (Mathf.Floor(VectorFieldGetter(value, i) * _floatPointScale + 0.1f) + Step) / _floatPointScale);
            }
            return value;
        }

        protected sealed override T ValueDecrement(T value, bool isShift)
        {
            var count = VectorUtils.GetFieldsCount(typeof(T));
            for (var i = 0; i < count; i++)
            {
                VectorFieldSetter(ref value, i, (Mathf.Floor(VectorFieldGetter(value, i) * _floatPointScale + 0.1f) - Step) / _floatPointScale);
            }
            return value;
        }

        protected abstract float VectorFieldGetter(T vector, int fieldIndex);

        protected abstract void VectorFieldSetter(ref T vector, int fieldIndex, float value);

        #endregion

        #region Private Methods

        private void BackAction(DMAction action)
        {
            Container.Back();
        }

        #endregion
    }
}
