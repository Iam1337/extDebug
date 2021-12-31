/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

namespace extDebug.Notifications
{
	public interface IDNRender
	{
		#region Methods

		void SetupNotice(DNNotice notice, float currentHeight);

		void RemoveNotice(DNNotice notice);

		void Repaint(DNNotice notice, float timeLeft, ref float currentHeight);

		#endregion
	}

	public interface IDNRender_OnGUI
	{
		#region Methods

		void OnGUI();

		#endregion
	}

	public interface IDNRender_Update
	{
		#region Methods

		void Update();

		#endregion
	}
}