/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;

namespace extDebug
{
    public class DebugMenuAction : DebugMenuItem
    {
		#region Public Vars


		#endregion

		#region Private Vars

		private Action<DebugMenuAction, EventArgs> _action;

		#endregion

		#region Public Methods
		
		public DebugMenuAction(DebugMenuBranch parent, string path, string description = null, Action<DebugMenuAction, EventArgs> action = null, int order = 0) : base(parent, path, order)
		{
			_action = action;
			_value = description;
		}

		#endregion

		#region Protected Methods

		protected override void OnEvent(EventArgs eventArgs)
		{
			if (eventArgs.Event == EventType.Repaint)
			{
				// TODO: Repaint
			}
			else if (eventArgs.Event == EventType.KeyDown)
			{
				switch (eventArgs.Key)
				{
					case KeyType.Up:
					{
						break;
					}
					case KeyType.Down:
					{
						break;
					}
					case KeyType.Left:
					{
						DebugMenu.Back();

						break;
					}
					case KeyType.Right:
					{
						if (_action == null)
							break;

						_action.Invoke(this, eventArgs);
						
						FlashName(DebugMenu.Colors.ActionFlash, true);
						
						break;
					}
					case KeyType.Back:
						DebugMenu.Back();
						break;
				}
			}
		}

		#endregion
	}
}
