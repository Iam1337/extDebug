/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

namespace extDebug
{
	public sealed class HooksBehaviour : MonoBehaviour
    {
	    #region Unity Methods

	    protected void Update() => Hooks.Update?.Invoke();

	    protected void LateUpdate() => Hooks.LateUpdate?.Invoke();

	    protected void FixedUpdate() => Hooks.FixedUpdate?.Invoke();

	    protected void OnGUI() => Hooks.OnGUI?.Invoke();

	    #endregion
    }
}
