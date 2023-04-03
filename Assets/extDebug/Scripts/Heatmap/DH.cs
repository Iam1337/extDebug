/* Copyright (c) 2023 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

namespace extDebug.Heatmap
{
    public static class DH
    {
        #region Static Private Vars

        private static DHSession _currentSession;

        #endregion

        #region Static Public Methods

        static DH()
        {
            Hooks.Update += Update;
        }

        public static void StartSession(string sessionName)
        {
            // TODO: Create session.
        }

        public static void StopSession()
        {
            // TODO: Stop session.

            _currentSession = null;
        }

        public static void Point(string tag, Vector3 position, Quaternion rotation) => _currentSession?.Point(tag, position, rotation);

        public static void StartTrack(string tag, Transform transform) => _currentSession?.StartTrack(tag, transform);

        public static void StopTrack(string tag) => _currentSession?.StopTrack(tag);

        #endregion

        #region Static Private Methods

        private static void Update()
        {
	         _currentSession?.Update();
        }

        #endregion
    }
}