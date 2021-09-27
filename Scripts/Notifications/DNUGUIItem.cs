/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;

using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

using TextMeshPro = TMPro.TextMeshProUGUI;

namespace extDebug.Notifications
{
	public class DNUGUIItem : MonoBehaviour
	{
		#region Public Vars

		public string Text
		{
			get => _label_TextMeshPro != null ? _label_TextMeshPro.text : _label_Text.text;
			set
			{
				if (_label_TextMeshPro != null)
					_label_TextMeshPro.text = value;
				else
					_label_Text.text = value;
			}
		}

		public Vector2 Position
		{
			get => _rectTransform.anchoredPosition;
			set => _rectTransform.anchoredPosition = value;
		}

		public Vector2 Size => _rectTransform.sizeDelta;

		public float Width =>  _label_TextMeshPro != null ? _label_TextMeshPro.GetPreferredValues().x : _label_Text.preferredWidth;

		public float Alpha
		{
			get => _canvasGroup.alpha;
			set => _canvasGroup.alpha = value;
		}

		[NonSerialized]
		public Vector2 Velocity;
		
		#endregion

		#region Private Vars

		[FormerlySerializedAs("Transform")] 
		[SerializeField]
		private RectTransform _rectTransform;

		[SerializeField]
		private CanvasGroup _canvasGroup;
		
		[FormerlySerializedAs("Label_Text")] 
		[SerializeField]
		private Text _label_Text;

		[FormerlySerializedAs("Label_TextMeshPro")] 
		[SerializeField]
		private TextMeshPro _label_TextMeshPro;

		#endregion
	}
}