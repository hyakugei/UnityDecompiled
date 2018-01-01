using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.iOS
{
	public static class OnDemandResources
	{
		public static extern bool enabled
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern OnDemandResourcesRequest PreloadAsyncImpl(string[] tags);

		public static OnDemandResourcesRequest PreloadAsync(string[] tags)
		{
			return OnDemandResources.PreloadAsyncImpl(tags);
		}
	}
}
