using System;

namespace UnityEngine.Experimental.Rendering
{
	public abstract class RenderPipeline : IRenderPipeline, IDisposable
	{
		public bool disposed
		{
			get;
			private set;
		}

		public virtual void Render(ScriptableRenderContext renderContext, Camera[] cameras)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(string.Format("{0} has been disposed. Do not call Render on disposed RenderLoops.", this));
			}
		}

		public virtual void Dispose()
		{
			this.disposed = true;
		}
	}
}
