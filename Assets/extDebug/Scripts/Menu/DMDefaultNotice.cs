/* Copyright (c) 2023 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System.Text;
using System.Runtime.CompilerServices;

using extDebug.Notifications;

namespace extDebug.Menu
{
	internal class DMDefaultNotice : IDMNotice
	{
		#region Private Vars

		private const float kDefaultDuration = 5;

		private readonly StringBuilder _builder = new StringBuilder();

		#endregion

		#region Public Methods

		public void Notify(DMItem item, Color? nameColor = null, Color? valueColor = null)
		{
			_builder.Clear();

			// Name
			_builder.Append(OpenColorTag(nameColor));
			_builder.Append(item.Name);
			_builder.Append(CloseColorTag(nameColor));

			// Value
			if (!string.IsNullOrEmpty(item.Value))
			{
				_builder.Append(" ");
				_builder.Append(OpenColorTag(valueColor));
				_builder.Append(item.Value);
				_builder.Append(CloseColorTag(valueColor));
			}

			// Show notification
			DN.Notify(item, _builder.ToString(), kDefaultDuration);
		}

		#endregion

		#region Private Methods

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private string OpenColorTag(Color? color) => color != null ? $"<color=#{ColorUtility.ToHtmlStringRGB(color.Value)}>" : string.Empty;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private string CloseColorTag(Color? color) => color != null ? "</color>" : string.Empty;

		#endregion
	}
}