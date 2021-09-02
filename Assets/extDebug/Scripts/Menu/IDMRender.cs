/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System.Collections.Generic;

namespace extDebug.Menu
{
	public interface IDMRender
	{
		#region Methods

		void Repaint(DMBranch branch, IReadOnlyList<DMItem> items);

		#endregion
	}
}
