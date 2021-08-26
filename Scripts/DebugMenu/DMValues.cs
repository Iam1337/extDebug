/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

namespace extDebug
{
	public enum EventTag
	{
		None,       // Nothing
		OpenBranch,   // Menu open
		CloseBranch,  // Menu closed
		ToggleMenu, // Menu toggled
		Repaint,    // Repaint item
		Up,    // Key up
		Down,  // Key down
		Left,  // Key left
		Right, // Key right
		Back,  // Key back
		Reset  // Key reset value
	}

	public delegate void ActionDelegate(DMAction actionItem, EventTag @event);
}