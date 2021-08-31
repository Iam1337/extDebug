/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using extDebug.Notifications;

namespace extDebug.Menu
{
    internal class DMDefaultNotice : IDMNotice
    {
	    #region Public Methods

	    public void Notify(DMItem item, Color? nameColor = null, Color? valueColor = null)
	    {
			if (nameColor != null && valueColor != null)
			{
				DN.Notify(item, $"<color=#{ColorUtility.ToHtmlStringRGB(nameColor.Value)}>{item.Name}</color> <color=#{ColorUtility.ToHtmlStringRGB(valueColor.Value)}>{item.Value}</color>", 5);
			}
			else if (nameColor != null)
			{
				DN.Notify(item, $"<color=#{ColorUtility.ToHtmlStringRGB(nameColor.Value)}>{item.Name}</color> {item.Value}", 5);
			}
			else if (valueColor != null)
			{
				DN.Notify(item, $"{item.Name} <color=#{ColorUtility.ToHtmlStringRGB(valueColor.Value)}>{item.Value}</color>", 5);
			}
			else
			{
				DN.Notify(item, $"{item.Name} {item.Value}", 5);
			}
		}

	    #endregion
	}
}
