/* Copyright (c) 2024 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

namespace extDebug.Menu
{
    public interface IDMLogsContainer
    {
        public bool IsDirty();
        public bool GetLog(int index, out string tag, out Color tagColor, out string message, out Color messageColor);
        public int GetLogsCount();
    }
}