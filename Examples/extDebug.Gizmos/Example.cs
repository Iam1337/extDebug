using System;
using UnityEngine;

using extDebug.Gizmos;

namespace extDebug.Examples.Gizmos
{
    public class Example : MonoBehaviour
    {
        #region Unity Methods

        protected void Update()
        {
            DG.Color = Color.blue;
            DG.Matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
            DG.Line(Vector3.zero, Vector3.forward * 5f);
            DG.Cube(Vector3.zero, Vector3.one);
        }

        private void OnDrawGizmos()
        {
            UnityEngine.Gizmos.color = Color.blue;
            UnityEngine.Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale); 
            UnityEngine.Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }

        #endregion
    }
}