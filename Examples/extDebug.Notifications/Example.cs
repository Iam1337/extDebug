/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using extDebug.Menu;
using extDebug.Notifications;

namespace extDebug.Examples.Notifications
{
	public class Example : MonoBehaviour
	{
		#region Internal Types
		
		private object _longContext = new object();

		private object _infinityContext = new object();

		#endregion
		
		#region Unity Methods

		private void Start()
		{
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