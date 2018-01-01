using System;
using System.Collections.Generic;

namespace UnityEngine.XR.Tango
{
	public struct PointCloudData
	{
		public uint version;

		public double timestamp;

		public List<Vector4> points;
	}
}
