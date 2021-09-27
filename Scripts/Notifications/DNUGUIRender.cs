/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

namespace extDebug.Notifications
{
	public class DNUGUIRender : IDNRender
	{
		#region Public Vars
		
		public Vector2 ItemsOffset = new Vector2(10, 10);

		public float ItemSpace = 5;

		#endregion

		#region Private Vars

		private readonly Canvas _noticeCanvas;
		
		private readonly DNUGUIItem _noticePrefab;

		#endregion

		#region Public Methods

		public DNUGUIRender(Canvas noticeCanvas, DNUGUIItem noticePrefab)
		{
			_noticeCanvas = noticeCanvas;
			_noticePrefab = noticePrefab;
		}

		#endregion
		
		#region IDNRender Methods

		void IDNRender.SetupNotice(DNNotice notice, float currentHeight)
		{
			var noticeInstance = Object.Instantiate(_noticePrefab, _noticeCanvas.transform);
			noticeInstance.SetText(notice.Text);

			var size = noticeInstance.GetWidth();
			noticeInstance.Transform.anchoredPosition = new Vector2(size, -ItemsOffset.y - currentHeight - (noticeInstance.Transform.sizeDelta.y + 4));
			notice.Data = noticeInstance;
		}

		void IDNRender.RemoveNotice(DNNotice notice)
		{
			if (notice.Data is DNUGUIItem noticeInstance)
				Object.Destroy(noticeInstance.gameObject);
		}

		void IDNRender.Repaint(DNNotice notice, float timeLeft, ref float currentHeight)
		{
			if (!(notice.Data is DNUGUIItem noticeInstance))
				return;
			
			var size = noticeInstance.Transform.sizeDelta;
			var position = noticeInstance.Transform.anchoredPosition;

			var width = size.x;
			var height = size.y + ItemSpace;

			var targetX = - ItemsOffset.x;
			var targetY = height - currentHeight - ItemsOffset.y;

			// Calculate targets
			if (timeLeft < 0.2f) targetX += width * 2;
			else if (timeLeft < 0.7f) targetX -= 50;

			// Calculate new positions
			var speed = Time.unscaledDeltaTime * 15;
			position.x += noticeInstance.Velocity.x * speed;
			position.y += noticeInstance.Velocity.y * speed;

			// Calculate speed
			var distance = targetX - position.x;
			noticeInstance.Velocity.x += distance * speed * 1;
			if (Mathf.Abs(distance) < 2 && Mathf.Abs(noticeInstance.Velocity.x) < 0.1) noticeInstance.Velocity.x = 0;

			distance = targetY - position.y;
			noticeInstance.Velocity.y += distance * speed * 1;
			if (Mathf.Abs(distance) < 2 && Mathf.Abs(noticeInstance.Velocity.y) < 0.1) noticeInstance.Velocity.y = 0;

			var friction = 0.95f - Time.unscaledDeltaTime * 8;
			noticeInstance.Velocity.x *= friction;
			noticeInstance.Velocity.y *= friction;
			
			noticeInstance.Transform.anchoredPosition = position;
			noticeInstance.SetText(notice.Text);

			currentHeight -= height;
		}

		#endregion
	}
}