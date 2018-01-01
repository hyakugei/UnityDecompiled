using System;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.PlatformSupport
{
	internal abstract class PlatformIconField
	{
		protected const int kSlotSize = 64;

		protected const float kHeaderHeight = 18f;

		protected const int kMaxElementHeight = 116;

		protected const int kMaxPreviewSize = 86;

		protected const int kIconSpacing = 8;

		protected BuildTargetGroup m_TargetGroup;

		protected string m_HeaderString;

		protected string m_SizeLabel;

		internal ReorderableList.Defaults s_Defaults = new ReorderableList.Defaults();

		public PlatformIcon platformIcon
		{
			get;
			protected set;
		}

		internal PlatformIconField(PlatformIcon platformIcon, BuildTargetGroup targetGroup)
		{
			this.platformIcon = platformIcon;
			this.m_TargetGroup = targetGroup;
			this.m_HeaderString = this.platformIcon.description;
			this.m_SizeLabel = string.Format("{0}x{1}px", platformIcon.width, platformIcon.height);
		}

		public static Rect GetContentRect(Rect rect, float paddingVertical = 0f, float paddingHorizontal = 0f)
		{
			Rect result = rect;
			result.yMin += paddingVertical;
			result.yMax -= paddingVertical;
			result.xMin += paddingHorizontal;
			result.xMax -= paddingHorizontal;
			return result;
		}

		internal abstract void DrawAt();
	}
}
