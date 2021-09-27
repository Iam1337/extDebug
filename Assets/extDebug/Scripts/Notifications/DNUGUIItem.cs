/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;

using UnityEngine;
using UnityEngine.UI;

using TextMeshPro = TMPro.TextMeshProUGUI;

namespace extDebug.Notifications
{
	public class DNUGUIItem : MonoBehaviour
	{
		#region Public Vars

		public RectTransform Transform;

		public Text Label_Text;

		public TextMeshPro Label_TextMeshPro;

		[NonSerialized]
		public Vector2 Velocity;
		
		#endregion

		#region Public Methods

		public void SetText(string text)
		{
			if (Label_TextMeshPro != null)
				Label_TextMeshPro.text = text;
			else
				Label_Text.text = text;
		}

		public float GetWidth() => Label_TextMeshPro != null ? Label_TextMeshPro.GetPreferredValues().x : Label_Text.preferredWidth;

		#endregion
	}
}