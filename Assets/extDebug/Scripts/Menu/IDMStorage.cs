/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;

namespace extDebug.Menu
{
    public interface IDMStorage
    {
        #region Methods

        bool Save(string key, object value);

        object Load(string key, Type valueType);

        #endregion
    }
}
