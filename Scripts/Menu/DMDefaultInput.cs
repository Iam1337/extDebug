/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

namespace extDebug.Menu
{
	internal class DMDefaultInput : IDMInput
	{
		public EventTag GetEvent()
		{
			if (Input.GetKey(KeyCode.Q))
				return EventTag.ToggleMenu;

			if (DM.IsVisible)
			{
				if (Input.GetKey(KeyCode.W))
					return EventTag.Up;
				if (Input.GetKey(KeyCode.S))
					return EventTag.Down;
				if (Input.GetKey(KeyCode.A))
					return EventTag.Left;
				if (Input.GetKey(KeyCode.D))
					return EventTag.Right;
				if (Input.GetKey(KeyCode.R))
					return EventTag.Reset;
			}
			else if (Input.GetKey(KeyCode.LeftShift))
			{
				if (Input.GetKey(KeyCode.A))
					return EventTag.Left;
				if (Input.GetKey(KeyCode.D))
					return EventTag.Right;
				if (Input.GetKey(KeyCode.R))
					return EventTag.Reset;
			}

			return EventTag.None;
		}
	}
}
