/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;

namespace extDebug.Menu
{
	public class DMAction : DMItem
	{
		#region Private Vars

		private readonly Action<DMAction> _action;

		#endregion

		#region Public Methods

		public DMAction(DMBranch parent, string path, string description = null, Action<DMAction> action = null, int order = 0) : base(parent, path, string.Empty, description, order)
		{
			_action = action;
		}

		#endregion

		#region Protected Methods

		protected override void OnEvent(EventArgs eventArgs)
		{
			if (eventArgs.Tag != EventTag.Input)
				return;

			if (eventArgs.Key == EventKey.Left)
			{
				Container.Back();
			}
			else if (eventArgs.Key == EventKey.Right && IsEnabled() && _action != null)
			{
				_action.Invoke(this);

				FlashName(DM.Colors.ActionSuccess, true);
			}
		}

		#endregion
	}
}