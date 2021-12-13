/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System.Collections.Generic;

namespace extDebug.Menu
{
	public interface IDMRender
	{
		#region Methods

		// Debug Menu Hooks
		void Repaint(DMBranch branch, IReadOnlyList<DMItem> items);
		
		#endregion
	}

	public interface IDMRender_OnGUI
	{
		#region Methods

		void OnGUI(bool isVisible);

		#endregion
	}

	public interface IDMRender_Update
	{
		#region Methods

		void Update(bool isVisible);

		#endregion
	}
}
