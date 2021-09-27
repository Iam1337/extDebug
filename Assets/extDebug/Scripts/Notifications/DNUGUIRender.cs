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
			var noticeData = Object.Instantiate(_noticePrefab, _noticeCanvas.transform);
			noticeData.Text = notice.Text;
			noticeData.Position = new Vector2(noticeData.Width, -ItemsOffset.y - currentHeight - (noticeData.Size.y + 4));
			notice.Data = noticeData;
		}

		void IDNRender.RemoveNotice(DNNotice notice)
		{
			if (notice.Data is DNUGUIItem noticeData)
				Object.Destroy(noticeData.gameObject);
		}

		void IDNRender.Repaint(DNNotice notice, float timeLeft, ref float currentHeight)
		{
			if (!(notice.Data is DNUGUIItem noticeData))
				return;
			
			var size = noticeData.Size;
			var position = noticeData.Position;

			var width = size.x;
			var height = size.y + ItemSpace;

			var targetX = - ItemsOffset.x;
			var targetY = height - currentHeight - ItemsOffset.y;

			// Calculate targets
			if (timeLeft < 0.2f) targetX += width * 2;
			else if (timeLeft < 0.7f) targetX -= 50;

			// Calculate new positions
			var speed = Time.unscaledDeltaTime * 15;
			position.x += noticeData.Velocity.x * speed;
			position.y += noticeData.Velocity.y * speed;

			// Calculate speed
			var distance = targetX - position.x;
			noticeData.Velocity.x += distance * speed * 1;
			if (Mathf.Abs(distance) < 2 && Mathf.Abs(noticeData.Velocity.x) < 0.1) noticeData.Velocity.x = 0;

			distance = targetY - position.y;
			noticeData.Velocity.y += distance * speed * 1;
			if (Mathf.Abs(distance) < 2 && Mathf.Abs(noticeData.Velocity.y) < 0.1) noticeData.Velocity.y = 0;

			var friction = 0.95f - Time.unscaledDeltaTime * 8;
			noticeData.Velocity.x *= friction;
			noticeData.Velocity.y *= friction;
			
			noticeData.Position = position;
			noticeData.Text = notice.Text;

			currentHeight -= height;
		}

		#endregion
	}
}