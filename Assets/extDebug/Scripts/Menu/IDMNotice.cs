/* Copyright (c) 2024 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

namespace extDebug.Menu
{
	public interface IDMNotice
	{
		#region Methods

		void Notify(DMItem item, Color? nameColor = null, Color? valueColor = null);

		#endregion
	}
}