/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;

namespace extDebug
{
    public class DMAction : DMItem
    {
		#region Public Vars


		#endregion

		#region Private Vars

		private Action<DMAction, EventArgs> _action;

		#endregion

		#region Public Methods
		
		public DMAction(DMBranch parent, string path, string description = null, Action<DMAction, EventArgs> action = null, int order = 0) : base(parent, path, description, order)
		{
			_action = action;
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
						DM.Back();

						break;
					}
					case KeyType.Right:
					{
						if (_action == null)
							break;

						_action.Invoke(this, eventArgs);
						
						FlashName(DM.Colors.ActionFlash, true);
						
						break;
					}
					case KeyType.Back:
						DM.Back();
						break;
				}
			}
		}

		#endregion
	}
}
