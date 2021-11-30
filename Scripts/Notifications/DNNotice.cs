/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;

namespace extDebug.Notifications
{
    public class DNNotice
    {
        #region Public Vars

	    public float StartTime;

	    public float Duration;

	    public string Text;

	    public object Context;

	    public object Data;

	    public Action<object> LeftCallback;

	    #endregion
    }
}
