/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;

namespace extDebug
{
    public class DMAction : DMItem
    {
		#region Private Vars

		private readonly Action<DMAction> _action;

		#endregion

		#region Public Methods
		
		public DMAction(DMBranch parent, string path, string description = null, Action<DMAction> action = null, int order = 0) : base(parent, path, description, order)
		{
			_action = action;
		}

		#endregion

		#region Protected Methods

		protected override void OnEvent(EventTag eventTag)
		{
			if (eventTag == EventTag.Repaint)
			{
				// TODO: Repaint
			}
			else if (eventTag == EventTag.Left)
			{
				DM.Back();
			}
			else if (eventTag == EventTag.Right && _action != null)
			{
				_action.Invoke(this);

				FlashName(DM.Colors.ActionFlash, true);
			}
			else if (eventTag == EventTag.Back)
			{
				DM.Back();
			}
		}

		#endregion
	}
}
