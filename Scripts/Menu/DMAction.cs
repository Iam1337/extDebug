/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;

namespace extDebug.Menu
{
	public struct ActionEvent
	{
		#region Static Public Methods

		public static ActionEvent Create(DMAction action, EventArgs eventArgs) => new()
		{
			_action = action,
			_eventArgs = eventArgs
		};

		#endregion
		
		#region Public Vars

		public DMAction Action => _action;

		public object Data => _action.Data;

		public EventKey Key => _eventArgs.Key;

		public bool IsShift => _eventArgs.IsShift;

		#endregion

		#region Private Vars

		private DMAction _action;

		private EventArgs _eventArgs;

		#endregion
	}

	public class DMAction : DMItem
	{
		#region Private Vars

		private readonly Action<ActionEvent> _action;

		#endregion

		#region Public Methods

		public DMAction(DMBranch parent, string path, string description = null, Action<ActionEvent> action = null, int order = 0) : base(parent, path, string.Empty, description, order)
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
				_action.Invoke(ActionEvent.Create(this, eventArgs));

				FlashName(DM.Colors.ActionSuccess, true);
			}
		}

		#endregion
	}
}