/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System.Collections.Generic;

using UnityEngine;

namespace extDebug.Notifications
{
	public class DNDefaultRender : IDNRender, IDNRender_OnGUI
	{
		#region Extensions

		private struct NoticeData
		{
			public Vector2 Size;

			public Vector2 Velocity;

			public Vector2 Position;
		}

		#endregion
		
		#region Public Vars

		public Vector2 ItemsOffset = new Vector2(10, 10);

		public float ItemSpace = 4;

		#endregion
		
		#region Private Vars
		
		private GUISkin _currentSkin;

		private readonly List<DNNotice> _notices = new List<DNNotice>();
		
		#endregion

		#region IDNRender Methods

		void IDNRender.SetupNotice(DNNotice notice, float currentHeight)
		{
			// Calculate start data
			var noticeData = new NoticeData();
			noticeData.Size = CalcSize(notice.Text);
			noticeData.Velocity = new Vector2(-5, 0);
			noticeData.Position = new Vector2(Screen.width, Screen.height - ItemsOffset.y - currentHeight - (noticeData.Size.y + ItemSpace));

			notice.Data = noticeData;
			
			_notices.Add(notice);
		}

		void IDNRender.RemoveNotice(DNNotice notice)
		{
			_notices.Remove(notice);
		}

		void IDNRender.Repaint(DNNotice notice, float timeLeft, ref float currentHeight)
		{
			if (!(notice.Data is NoticeData noticeData))
				return;
				
			var width = noticeData.Size.x;
			var height = noticeData.Size.y + ItemSpace;

			var targetX = Screen.width - width - ItemsOffset.x;
			var targetY = Screen.height - height - currentHeight - ItemsOffset.y;
			
			// Calculate targets
			if (timeLeft < 0.2f) targetX += width * 2;
			else if (timeLeft < 0.7f) targetX -= 50;

			// Calculate new positions
			var speed = Time.unscaledDeltaTime * 15;
			noticeData.Position.x += noticeData.Velocity.x * speed;
			noticeData.Position.y += noticeData.Velocity.y * speed;

			// Calculate speed
			var distance = targetX - noticeData.Position.x;
			noticeData.Velocity.x += distance * speed * 1;
			if (Mathf.Abs(distance) < 2 && Mathf.Abs(noticeData.Velocity.x) < 0.1) noticeData.Velocity.x = 0;

			distance = targetY - noticeData.Position.y;
			noticeData.Velocity.y += distance * speed * 1;
			if (Mathf.Abs(distance) < 2 && Mathf.Abs(noticeData.Velocity.y) < 0.1) noticeData.Velocity.y = 0;

			var friction = 0.95f - Time.unscaledDeltaTime * 8;
			noticeData.Velocity.x *= friction;
			noticeData.Velocity.y *= friction;
			
			noticeData.Size = CalcSize(notice.Text);

			currentHeight += height;
			notice.Data = noticeData;
		}

		void IDNRender_OnGUI.OnGUI()
		{
			_currentSkin = GUI.skin;

			foreach (var notice in _notices)
			{
				if (!(notice.Data is NoticeData noticeData))
					continue;
				
				var rect = new Rect(noticeData.Position, noticeData.Size);

				GUI.Box(rect, GUIContent.none);
				rect.x += 5f;
				rect.width -= 10f;
				rect.y += 5f;
				rect.height -= 10f;
				GUI.Label(rect, new GUIContent(notice.Text));
			}
		}

		#endregion

		#region Private Methods
		
		private Vector2 CalcSize(string text)
		{
			return _currentSkin == null ? Vector2.zero : _currentSkin.label.CalcSize(new GUIContent(text)) + new Vector2(10, 10);
		}

		#endregion
	}
}