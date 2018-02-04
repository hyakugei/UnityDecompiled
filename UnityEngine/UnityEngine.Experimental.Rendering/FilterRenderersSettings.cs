using System;

namespace UnityEngine.Experimental.Rendering
{
	public struct FilterRenderersSettings
	{
		private RenderQueueRange m_RenderQueueRange;

		private int m_LayerMask;

		private uint m_RenderingLayerMask;

		public RenderQueueRange renderQueueRange
		{
			get
			{
				return this.m_RenderQueueRange;
			}
			set
			{
				this.m_RenderQueueRange = value;
			}
		}

		public int layerMask
		{
			get
			{
				return this.m_LayerMask;
			}
			set
			{
				this.m_LayerMask = value;
			}
		}

		public uint renderingLayerMask
		{
			get
			{
				return this.m_RenderingLayerMask;
			}
			set
			{
				this.m_RenderingLayerMask = value;
			}
		}

		public FilterRenderersSettings(bool initializeValues = false)
		{
			this = default(FilterRenderersSettings);
			if (initializeValues)
			{
				this.m_RenderQueueRange = RenderQueueRange.all;
				this.m_LayerMask = -1;
				this.m_RenderingLayerMask = 4294967295u;
			}
		}
	}
}
