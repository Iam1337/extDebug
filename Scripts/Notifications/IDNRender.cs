﻿/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

namespace extDebug.Notifications
{
	public interface IDNRender
	{
		#region Vars

		float ScreenWidth { get; }

		float ScreenHeight { get; }

		Vector2 AreaOffset { get; }

		float ItemSpace { get; }

		#endregion

		#region Methods

		Vector2 CalcSize(string text);

		#endregion
	}
}