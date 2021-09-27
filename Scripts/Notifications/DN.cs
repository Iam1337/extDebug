/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System;
using System.Collections.Generic;

namespace extDebug.Notifications
{
	public static class DN
	{
		#region Static Public Methods

		public static IDNRender Render = new DNDefaultRender();

		public static List<DNNotice> Notices => _notices;

		#endregion

		#region Static Private Methods

		private const float kDurationDefault = 5f;

		private static readonly List<DNNotice> _notices = new List<DNNotice>();

		private static readonly Dictionary<object, DNNotice> _noticesContext = new Dictionary<object, DNNotice>();

		private static readonly List<DNNotice> _noticesToRemove = new List<DNNotice>();

		private static float _currentHeight;

		#endregion

		#region Static Public Methods

		static DN()
		{
			Hooks.Update += Update;
			Hooks.OnGUI += OnGUI;
		}

		public static void Notify(string text, float duration = kDurationDefault) => Notify(null, text, duration);

		public static void Notify(object context, string text, float duration = kDurationDefault)
		{
			if (context != null && _noticesContext.TryGetValue(context, out var notice))
			{
				notice.Text = text;
				notice.StartTime = Time.unscaledTime;
				notice.Duration = duration;

				return;
			}

			if (context == null && duration < 0)
				throw new Exception("[DN.Notify] Cannot declare infinite notification without context.");

			notice = new DNNotice();
			notice.Text = text;
			notice.Size = Render.CalcSize(text);
			notice.StartTime = Time.unscaledTime;
			notice.Duration = duration;
			notice.Velocity = new Vector2(-5, 0);
			notice.Position = new Vector2(Render.ScreenWidth, Render.ScreenHeight - Render.AreaOffset.y - _currentHeight - (notice.Size.y + Render.ItemSpace));
			notice.Context = context;

			_notices.Add(notice);

			if (context != null)
			{
				_noticesContext.Add(context, notice);
			}
			
			Render.AddNotice(notice);
		}

		public static void Kill(object context)
		{
			if (!_noticesContext.TryGetValue(context, out var notice))
				return;

			notice.StartTime = Time.unscaledTime;
			notice.Duration = 0.75f;
		}

		#endregion

		#region Static Private Methods

		private static void Update()
		{
			_currentHeight = 0;

			// Update position
			foreach (var notice in _notices)
			{
				Update(notice, ref _currentHeight, out var removeRequired);

				if (removeRequired)
					_noticesToRemove.Add(notice);
			}

			// Remove
			foreach (var notice in _noticesToRemove)
			{
				_notices.Remove(notice);

				if (notice.Context != null)
					_noticesContext.Remove(notice.Context);

				Render.RemoveNotice(notice);
			}

			_noticesToRemove.Clear();

			// Invoke Update callback
			(Render as IDNRender_Update)?.Update();
		}

		private static void Update(DNNotice notice, ref float currentHeight, out bool removeRequired)
		{
			var width = notice.Size.x;
			var height = notice.Size.y + Render.ItemSpace;

			var targetX = Render.ScreenWidth - width - Render.AreaOffset.x;
			var targetY = Render.ScreenHeight - height - currentHeight - Render.AreaOffset.y;
			
			// Calculate targets
			var timeLeft = notice.StartTime - (Time.unscaledTime - notice.Duration);
			if (notice.Duration < 0) timeLeft = 1;
			else if (timeLeft < 0.2f) targetX += width * 2;
			else if (timeLeft < 0.7f) targetX -= 50;

			// Calculate new positions
			var speed = Time.unscaledDeltaTime * 15;
			notice.Position.x += notice.Velocity.x * speed;
			notice.Position.y += notice.Velocity.y * speed;

			// Calculate speed
			var distance = targetX - notice.Position.x;
			notice.Velocity.x += distance * speed * 1;
			if (Mathf.Abs(distance) < 2 && Mathf.Abs(notice.Velocity.x) < 0.1) notice.Velocity.x = 0;

			distance = targetY - notice.Position.y;
			notice.Velocity.y += distance * speed * 1;
			if (Mathf.Abs(distance) < 2 && Mathf.Abs(notice.Velocity.y) < 0.1) notice.Velocity.y = 0;

			var friction = 0.95f - Time.unscaledDeltaTime * 8;
			notice.Velocity.x *= friction;
			notice.Velocity.y *= friction;

			currentHeight += height;
			removeRequired = timeLeft <= 0;
		}
		
		private static void OnGUI()
		{
			(Render as IDNRender_OnGUI)?.OnGUI();
		}

		#endregion
	}
}
