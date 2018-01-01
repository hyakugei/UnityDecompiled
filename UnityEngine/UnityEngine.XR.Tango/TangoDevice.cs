using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UnityEngine.XR.Tango
{
	public static class TangoDevice
	{
		private static ARBackgroundRenderer m_BackgroundRenderer = null;

		private static string m_AreaDescriptionUUID = "";

		[CompilerGenerated]
		private static Action <>f__mg$cache0;

		[CompilerGenerated]
		private static Action <>f__mg$cache1;

		public static extern CoordinateFrame baseCoordinateFrame
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern uint depthCameraRate
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool synchronizeFramerateWithColorCamera
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern bool isServiceConnected
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static string areaDescriptionUUID
		{
			get
			{
				return TangoDevice.m_AreaDescriptionUUID;
			}
			set
			{
				TangoDevice.m_AreaDescriptionUUID = value;
			}
		}

		public static ARBackgroundRenderer backgroundRenderer
		{
			get
			{
				return TangoDevice.m_BackgroundRenderer;
			}
			set
			{
				if (value != null)
				{
					if (TangoDevice.m_BackgroundRenderer != null)
					{
						ARBackgroundRenderer arg_39_0 = TangoDevice.m_BackgroundRenderer;
						if (TangoDevice.<>f__mg$cache0 == null)
						{
							TangoDevice.<>f__mg$cache0 = new Action(TangoDevice.OnBackgroundRendererChanged);
						}
						arg_39_0.backgroundRendererChanged -= TangoDevice.<>f__mg$cache0;
					}
					TangoDevice.m_BackgroundRenderer = value;
					ARBackgroundRenderer arg_67_0 = TangoDevice.m_BackgroundRenderer;
					if (TangoDevice.<>f__mg$cache1 == null)
					{
						TangoDevice.<>f__mg$cache1 = new Action(TangoDevice.OnBackgroundRendererChanged);
					}
					arg_67_0.backgroundRendererChanged += TangoDevice.<>f__mg$cache1;
					TangoDevice.OnBackgroundRendererChanged();
				}
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool Connect(string[] boolKeys, bool[] boolValues, string[] intKeys, int[] intValues, string[] longKeys, long[] longValues, string[] doubleKeys, double[] doubleValues, string[] stringKeys, string[] stringValues);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Disconnect();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool TryGetHorizontalFov(out float fovOut);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool TryGetVerticalFov(out float fovOut);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetRenderMode(ARRenderMode mode);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetBackgroundMaterial(Material material);

		public static bool TryGetLatestPointCloud(ref PointCloudData pointCloudData)
		{
			if (pointCloudData.points == null)
			{
				pointCloudData.points = new List<Vector4>();
			}
			pointCloudData.points.Clear();
			return TangoDevice.TryGetLatestPointCloudInternal(pointCloudData.points, out pointCloudData.version, out pointCloudData.timestamp);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool TryGetLatestPointCloudInternal(List<Vector4> pointCloudData, out uint version, out double timestamp);

		public static bool TryGetLatestImageData(ref ImageData imageData)
		{
			if (imageData.data == null)
			{
				imageData.data = new List<byte>();
			}
			imageData.data.Clear();
			return TangoDevice.TryGetLatestImageDataInternal(imageData.data, out imageData.width, out imageData.height, out imageData.stride, out imageData.timestamp, out imageData.frameNumber, out imageData.format, out imageData.exposureDurationNs);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool TryGetLatestImageDataInternal(List<byte> imageData, out uint width, out uint height, out uint stride, out double timestamp, out long frameNumber, out int format, out long exposureDurationNs);

		private static void OnBackgroundRendererChanged()
		{
			TangoDevice.SetBackgroundMaterial(TangoDevice.m_BackgroundRenderer.backgroundMaterial);
			TangoDevice.SetRenderMode(TangoDevice.m_BackgroundRenderer.mode);
		}

		public static bool Connect(TangoConfig config)
		{
			string[] boolKeys;
			bool[] boolValues;
			TangoDevice.CopyDictionaryToArrays<bool>(config.m_boolParams, out boolKeys, out boolValues);
			string[] intKeys;
			int[] intValues;
			TangoDevice.CopyDictionaryToArrays<int>(config.m_intParams, out intKeys, out intValues);
			string[] longKeys;
			long[] longValues;
			TangoDevice.CopyDictionaryToArrays<long>(config.m_longParams, out longKeys, out longValues);
			string[] doubleKeys;
			double[] doubleValues;
			TangoDevice.CopyDictionaryToArrays<double>(config.m_doubleParams, out doubleKeys, out doubleValues);
			string[] stringKeys;
			string[] stringValues;
			TangoDevice.CopyDictionaryToArrays<string>(config.m_stringParams, out stringKeys, out stringValues);
			return TangoDevice.Connect(boolKeys, boolValues, intKeys, intValues, longKeys, longValues, doubleKeys, doubleValues, stringKeys, stringValues);
		}

		private static void CopyDictionaryToArrays<T>(Dictionary<string, T> dictionary, out string[] keys, out T[] values)
		{
			if (dictionary.Count == 0)
			{
				keys = null;
				values = null;
			}
			else
			{
				keys = new string[dictionary.Count];
				values = new T[dictionary.Count];
				int num = 0;
				foreach (KeyValuePair<string, T> current in dictionary)
				{
					keys[num] = current.Key;
					values[num++] = current.Value;
				}
			}
		}
	}
}
