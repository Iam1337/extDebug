/* Copyright (c) 2024 dr. ext (Vladimir Sigalkin) */

using System.Collections.Generic;

namespace extDebug.Menu
{
	public interface IDMRender
	{
		#region Methods

		// Debug Menu Hooks
		void Repaint(IDMBranch itemsContainer, IReadOnlyList<DMItem> items);

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