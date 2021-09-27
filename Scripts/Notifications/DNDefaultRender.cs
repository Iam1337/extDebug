/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

namespace extDebug.Notifications
{
	public class DNDefaultRender : IDNRender, IDNRender_OnGUI
	{
		#region Public Vars

		public float ScreenWidth => Screen.width;

		public float ScreenHeight => Screen.height;

		public Vector2 AreaOffset => new Vector2(20, 20);

		public float ItemSpace => 4;

		#endregion

		#region Private Vars

		private GUISkin _currentSkin;

		#endregion

		#region IDNRender Methods

		void IDNRender.AddNotice(DNNotice notice)
		{ }

		void IDNRender.RemoveNotice(DNNotice notice)
		{ }

		Vector2 IDNRender.CalcSize(string text)
		{
			return _currentSkin == null
				? Vector2.zero
				: _currentSkin.label.CalcSize(new GUIContent(text)) + new Vector2(10, 10);
		}

		void IDNRender_OnGUI.OnGUI()
		{
			_currentSkin = GUI.skin;

			foreach (var notice in DN.Notices)
			{
				notice.Size = ((IDNRender)this).CalcSize(notice.Text);

				var rect = new Rect(notice.Position, notice.Size);
				//if (rect.Contains(Event.current.mousePosition))
				//{
				//	notice.StartTime = Time.unscaledTime;
				//	notice.Duration = 0.75f;
				//}

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

		#endregion
	}
}