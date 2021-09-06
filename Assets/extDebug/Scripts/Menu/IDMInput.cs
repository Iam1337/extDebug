/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

namespace extDebug.Menu
{
	public enum EventTag
	{
		None, // -
		OpenBranch, // Menu open
		CloseBranch, // Menu closed
		Repaint, // Repaint item
		Input // Process input
	}

	public enum EventKey
	{
		None, // -
		ToggleMenu, // Key menu toggled
		Up,    // Key up
		Down,  // Key down
		Left,  // Key left
		Right, // Key right
		Back,  // Key back
		Reset  // Key reset value
	}

	public class EventArgs
	{
		#region Static Public Methods

		public static EventArgs OpenBranch => new EventArgs { Tag = EventTag.OpenBranch };
		
		public static EventArgs CloseBranch => new EventArgs { Tag = EventTag.CloseBranch };
		
		public static EventArgs Repaint => new EventArgs { Tag = EventTag.Repaint };
		
		public static EventArgs Input => new EventArgs { Tag = EventTag.Input };

		#endregion
		
		#region Public Vars

		public EventTag Tag;
		
		public EventKey Key;
		
		public bool IsShift;
		
		#endregion
	}

	public interface IDMInput
	{
		#region Methods

		EventKey GetKey(out bool shift);

		#endregion
	}
}