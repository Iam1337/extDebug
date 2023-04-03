/* Copyright (c) 2023 dr. ext (Vladimir Sigalkin) */

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