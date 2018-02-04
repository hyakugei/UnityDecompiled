using System;
using System.Linq;
using UnityEngine;

namespace UnityEditor.PlatformSupport
{
	internal class PlatformIconFieldMultiLayer : PlatformIconField
	{
		private ReorderableIconLayerList m_IconLayers;

		internal PlatformIconFieldMultiLayer(PlatformIcon platformIcon, BuildTargetGroup targetGroup) : base(platformIcon, targetGroup)
		{
			bool flag = base.platformIcon.minLayerCount != base.platformIcon.maxLayerCount;
			bool showControls = flag;
			this.m_IconLayers = new ReorderableIconLayerList(true, showControls);
			this.m_IconLayers.headerString = string.Format("{0} ({1})", base.platformIcon.description, this.m_SizeLabel);
			this.m_IconLayers.minItems = base.platformIcon.minLayerCount;
			this.m_IconLayers.maxItems = base.platformIcon.maxLayerCount;
			string[] customLayerLabels = platformIcon.kind.customLayerLabels;
			if (customLayerLabels != null && customLayerLabels.Length > 0)
			{
				this.m_IconLayers.SetElementLabels(platformIcon.kind.customLayerLabels);
			}
			int num = 86;
			int height = (int)((float)platformIcon.height / (float)platformIcon.height * (float)num);
			this.m_IconLayers.SetImageSize(num, height);
			this.m_IconLayers.textures = platformIcon.GetTextures().ToList<Texture2D>();
			this.EnsureMinimumNumberOfTextures();
		}

		private void EnsureMinimumNumberOfTextures()
		{
			while (this.m_IconLayers.textures.Count < this.m_IconLayers.minItems)
			{
				this.m_IconLayers.textures.Add(null);
			}
		}

		internal override void DrawAt()
		{
			this.m_IconLayers.textures = base.platformIcon.GetTextures().ToList<Texture2D>();
			this.m_IconLayers.previewTextures = base.platformIcon.GetPreviewTextures().ToList<Texture2D>();
			this.EnsureMinimumNumberOfTextures();
			this.m_IconLayers.DoLayoutList();
			base.platformIcon.SetTextures(this.m_IconLayers.textures.ToArray());
		}
	}
}
