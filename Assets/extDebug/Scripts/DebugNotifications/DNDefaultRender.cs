/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

namespace extDebug
{
    public class DNDefaultRender : IDNRender
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

		#region Public Methods

		public DNDefaultRender()
		{
			Hooks.ImGuiCallback += ImGuiCallback;
		}

		~DNDefaultRender()
		{
			Hooks.ImGuiCallback -= ImGuiCallback;
		}

		public Vector2 CalcSize(string text)
		{
			return _currentSkin == null ? Vector2.zero : _currentSkin.label.CalcSize(new GUIContent(text)) + new Vector2(10, 10);
		}

		#endregion

		#region Private Methods

		private void ImGuiCallback()
		{
			_currentSkin = GUI.skin;

			foreach (var notice in DN.Notices)
			{
				notice.Size = CalcSize(notice.Text);

				var rect = new Rect(notice.Position, notice.Size);
				GUI.Box(rect, GUIContent.none);

				rect.x += 5f;
				rect.width -= 5f * 2f;
				rect.y += 5f;
				rect.height -= 5f * 2f;

				GUI.Label(rect, new GUIContent(notice.Text));
			}
		}

		#endregion
	}
}
