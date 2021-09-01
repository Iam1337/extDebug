/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using extDebug.Notifications;

namespace extDebug.Menu
{
    internal class DMDefaultNotice : IDMNotice
    {
	    #region Private Vars

	    private const float kDefaultDuration = 5;

	    #endregion

		#region Public Methods

		public void Notify(DMItem item, Color? nameColor = null, Color? valueColor = null)
		{
			string notifyText;

			if (nameColor != null && valueColor != null)
			{
				notifyText = $"<color=#{ColorUtility.ToHtmlStringRGB(nameColor.Value)}>{item.Name}</color> <color=#{ColorUtility.ToHtmlStringRGB(valueColor.Value)}>{item.Value}</color>";
			}
			else if (nameColor != null)
			{
				notifyText = $"<color=#{ColorUtility.ToHtmlStringRGB(nameColor.Value)}>{item.Name}</color> {item.Value}";
			}
			else if (valueColor != null)
			{
				notifyText = $"{item.Name} <color=#{ColorUtility.ToHtmlStringRGB(valueColor.Value)}>{item.Value}</color>";
			}
			else
			{
				notifyText = $"{item.Name} {item.Value}";
			}

			DN.Notify(item, notifyText, kDefaultDuration);
		}

	    #endregion
	}
}
