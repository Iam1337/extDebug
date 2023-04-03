/* Copyright (c) 2023 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System;
using System.Collections.Generic;

namespace extDebug.Notifications
{
	public static class DN
	{
		#region Static Public Methods

		public static IDNRender Render = new DNDefaultRender();

		#endregion

		#region Static Private Methods

		private const float kDurationDefault = 5f;

		private static readonly List<DNNotice> _notices = new List<DNNotice>();

		private static readonly Dictionary<object, DNNotice> _noticesContext = new Dictionary<object, DNNotice>();

		private static readonly List<DNNotice> _noticesToRemove = new List<DNNotice>();

		private static float _currentHeight;

		private static float _timeLeft;

		#endregion

		#region Static Public Methods

		static DN()
		{
			Hooks.Update += Update;
			Hooks.OnGUI += OnGUI;
		}

		public static void Notify(string text, float duration = kDurationDefault) => Notify(null, text, duration);

		public static void Notify(object context, string text, float duration = kDurationDefault, Action<object> leftCallback = null)
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
			notice.StartTime = Time.unscaledTime;
			notice.Duration = duration;
			notice.Context = context;
			notice.LeftCallback = leftCallback;

			_notices.Add(notice);

			if (context != null)
			{
				_noticesContext.Add(context, notice);
			}

			Render.SetupNotice(notice, _currentHeight);
		}

		public static void Kill(object context, bool ignoreCallback = false)
		{
			if (!_noticesContext.TryGetValue(context, out var notice))
				return;

			notice.StartTime = Time.unscaledTime;
			notice.Duration = 0.75f;

			if (ignoreCallback)
				notice.LeftCallback = null;
		}

		#endregion

		#region Static Private Methods

		private static void Update()
		{
			_timeLeft = 0;
			_currentHeight = 0;

			// Update position
			foreach (var notice in _notices)
			{
				if (notice.Duration < 0) _timeLeft = 1;
				else _timeLeft = notice.StartTime - (Time.unscaledTime - notice.Duration);

				Render.Repaint(notice, _timeLeft, ref _currentHeight);

				if (_timeLeft <= 0)
					_noticesToRemove.Add(notice);
			}

			// Remove
			foreach (var notice in _noticesToRemove)
			{
				_notices.Remove(notice);

				if (notice.Context != null)
				{
					if (notice.LeftCallback != null)
						notice.LeftCallback.Invoke(notice.Context);

					_noticesContext.Remove(notice.Context);

					notice.LeftCallback = null;
				}

				Render.RemoveNotice(notice);

				notice.Data = null;
			}

			_noticesToRemove.Clear();

			// Invoke Update callback
			(Render as IDNRender_Update)?.Update();
		}

		private static void OnGUI()
		{
			(Render as IDNRender_OnGUI)?.OnGUI();
		}

		#endregion
	}
}