using System;
using UnityEngine;

namespace extDebug.Gizmos
{
    [RequireComponent(typeof(Camera))]
    internal class DGHooks : MonoBehaviour
    {
        #region Public Vars

        public Action BeginRendering;

        #endregion

        #region Unity Methods

        private void OnPreCull() => BeginRendering?.Invoke();

        #endregion
    }
}