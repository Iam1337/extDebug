/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

namespace extDebug.Notifications
{
	// TODO: Rename
	public enum DNUGUIAnimation
	{
		Default,
		VR
	}
	
	// TODO: Move to extDebug.UGUI repo
	public class DNUGUIRender : IDNRender
	{
		#region Public Vars
		
		public Vector2 ItemsOffset = new Vector2(10, 10);

		public float ItemSpace = 5;

		#endregion

		#region Private Vars

		private readonly RectTransform _noticeAnchor;
		
		private readonly DNUGUIItem _noticePrefab;

		private readonly DNUGUIAnimation _animation;

		private Vector2 _previousSize;
		
		#endregion

		#region Public Methods

		public DNUGUIRender(RectTransform noticeAnchor, DNUGUIItem noticePrefab, DNUGUIAnimation animation = DNUGUIAnimation.Default)
		{
			_noticeAnchor = noticeAnchor;
			_noticePrefab = noticePrefab;
			_animation = animation;
		}

		#endregion
		
		#region IDNRender Methods

		void IDNRender.SetupNotice(DNNotice notice, float currentHeight)
		{
			var noticeData = Object.Instantiate(_noticePrefab, _noticeAnchor);
			noticeData.Text = notice.Text;
			
			if (_animation == DNUGUIAnimation.Default)
			{
				noticeData.Position = new Vector2(noticeData.Width, -ItemsOffset.y - currentHeight - (_previousSize.y + ItemSpace));
				noticeData.Alpha = 1f;
			}
			else if (_animation == DNUGUIAnimation.VR)
			{
				noticeData.Position = new Vector2(-noticeData.Width * 2,  currentHeight + _previousSize.y + ItemSpace);
				noticeData.Alpha = 0f;
			}
			
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

			_previousSize = size;
			
			var width = size.x;
			var height = size.y + ItemSpace;

			var targetX = 0f;
			var targetY = 0f;

			targetX = _animation == DNUGUIAnimation.Default ? -ItemsOffset.x : 0;
			targetY = _animation == DNUGUIAnimation.Default ? height - currentHeight - ItemsOffset.y : height + currentHeight;

			// Calculate targets
			if (timeLeft < 0.2f) targetX += width * 2;
			else if (timeLeft < 0.7f) targetX -= 50;

			if (_animation == DNUGUIAnimation.VR)
			{
				if (timeLeft > 0.2f)
				{
					noticeData.Alpha += Time.unscaledDeltaTime * 4.5f;
				}
				else
				{
					noticeData.Alpha -= Time.unscaledDeltaTime * 4.5f;
				}
				
			}

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