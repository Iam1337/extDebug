/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;

using UnityEngine;

namespace extDebug.Menu
{
    public class DMVector3 : DMVector<Vector3>
    {
        #region Public Methods

        public DMVector3(DMBranch parent, string path, Func<Vector3> getter, Action<Vector3> setter = null, int order = 0) : base(parent, path, getter, setter, order)
        { }

        #endregion

        #region Protected Methods

        protected override float VectorFieldGetter(Vector3 vector, int fieldIndex) => vector[fieldIndex];

        protected override void VectorFieldSetter(ref Vector3 vector, int fieldIndex, float value) => vector[fieldIndex] = value;

        #endregion
    }
}
