using System;
using System.Collections.Generic;

namespace UnityEngine.XR.Tango
{
	public struct ImageData
	{
		public uint width;

		public uint height;

		public uint stride;

		public double timestamp;

		public long frameNumber;

		public int format;

		public List<byte> data;

		public long exposureDurationNs;
	}
}
