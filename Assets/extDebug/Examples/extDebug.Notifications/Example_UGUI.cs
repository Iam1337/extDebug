/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using extDebug.Menu;
using extDebug.Notifications;

using UnityEngine.Serialization;

namespace extDebug.Examples.Notifications
{
	public class Example_UGUI : MonoBehaviour
	{
		#region Public Vars

		public DNUGUIAnimation NotifyAnimation;

		public RectTransform NotifyAnchor;

		public DNUGUIItem NotifyPrefab;

		#endregion
		
		#region Private Vars

		private object _longContext = new object();

		private object _infinityContext = new object();

		#endregion
		
		#region Unity Methods

		private void Start()
		{
			DN.Render = new DNUGUIRender(NotifyAnchor, NotifyPrefab, NotifyAnimation);

			DM.Add("Simple Notice", action => DN.Notify("Simple notification"), order: 0);
			
			DM.Add("Show Long Notice", action => DN.Notify(_longContext, "Long Notice Example", 15f), order: 10);
			DM.Add("Kill Long Notice", action => DN.Kill(_longContext), order: 11);
			
			DM.Add("Show Infinity Notice", action => DN.Notify(_infinityContext, "Infinity Notice Example", -1), order: 20);
			DM.Add("Kill Infinity Notice", action => DN.Kill(_infinityContext), order: 21);

			DM.Open();
		}

		#endregion
	}
}