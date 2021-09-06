/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

namespace extDebug
{
	internal sealed class HooksBehaviour : MonoBehaviour
    {
	    #region Unity Methods

	    private void Update() => Hooks.Update?.Invoke();

	    private void LateUpdate() => Hooks.LateUpdate?.Invoke();

	    private void FixedUpdate() => Hooks.FixedUpdate?.Invoke();

	    private void OnGUI() => Hooks.OnGUI?.Invoke();

	    #endregion
    }
}
 