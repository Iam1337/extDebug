/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

namespace extDebug.Menu
{
	internal class DMDefaultInput : IDMInput
	{
		public EventKey GetKey(out bool shift)
		{
			shift = false;
			
			if (Input.GetKey(KeyCode.Q))
				return EventKey.ToggleMenu;

			if (DM.IsVisible)
			{
				shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
				
				if (Input.GetKey(KeyCode.W))
					return EventKey.Up;
				if (Input.GetKey(KeyCode.S))
					return EventKey.Down;
				if (Input.GetKey(KeyCode.A))
					return EventKey.Left;
				if (Input.GetKey(KeyCode.D))
					return EventKey.Right;
				if (Input.GetKey(KeyCode.R))
					return EventKey.Reset;
			}
			else if (Input.GetKey(KeyCode.LeftShift))
			{
				if (Input.GetKey(KeyCode.A))
					return EventKey.Left;
				if (Input.GetKey(KeyCode.D))
					return EventKey.Right;
				if (Input.GetKey(KeyCode.R))
					return EventKey.Reset;
			}

			return EventKey.None;
		}
	}
}
