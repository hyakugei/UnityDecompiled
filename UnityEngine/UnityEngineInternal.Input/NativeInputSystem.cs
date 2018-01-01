using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Scripting;

namespace UnityEngineInternal.Input
{
	public class NativeInputSystem
	{
		public static NativeUpdateCallback onUpdate;

		public static NativeBeforeUpdateCallback onBeforeUpdate;

		private static NativeDeviceDiscoveredCallback s_OnDeviceDiscoveredCallback;

		public static NativeDeviceDiscoveredCallback onDeviceDiscovered
		{
			get
			{
				return NativeInputSystem.s_OnDeviceDiscoveredCallback;
			}
			set
			{
				NativeInputSystem.s_OnDeviceDiscoveredCallback = value;
				NativeInputSystem.hasDeviceDiscoveredCallback = (NativeInputSystem.s_OnDeviceDiscoveredCallback != null);
			}
		}

		public static extern double zeroEventTime
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern bool hasDeviceDiscoveredCallback
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		static NativeInputSystem()
		{
			NativeInputSystem.hasDeviceDiscoveredCallback = false;
		}

		[RequiredByNativeCode]
		internal static void NotifyBeforeUpdate(NativeInputUpdateType updateType)
		{
			NativeBeforeUpdateCallback nativeBeforeUpdateCallback = NativeInputSystem.onBeforeUpdate;
			if (nativeBeforeUpdateCallback != null)
			{
				nativeBeforeUpdateCallback(updateType);
			}
		}

		[RequiredByNativeCode]
		internal static void NotifyUpdate(NativeInputUpdateType updateType, int eventCount, IntPtr eventData)
		{
			NativeUpdateCallback nativeUpdateCallback = NativeInputSystem.onUpdate;
			if (nativeUpdateCallback != null)
			{
				nativeUpdateCallback(updateType, eventCount, eventData);
			}
		}

		[RequiredByNativeCode]
		internal static void NotifyDeviceDiscovered(NativeInputDeviceInfo deviceInfo)
		{
			NativeDeviceDiscoveredCallback nativeDeviceDiscoveredCallback = NativeInputSystem.s_OnDeviceDiscoveredCallback;
			if (nativeDeviceDiscoveredCallback != null)
			{
				nativeDeviceDiscoveredCallback(deviceInfo);
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int AllocateDeviceId();

		public static void QueueInputEvent<TInputEvent>(ref TInputEvent inputEvent) where TInputEvent : struct
		{
			NativeInputSystem.QueueInputEvent(UnsafeUtility.AddressOf<TInputEvent>(ref inputEvent));
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void QueueInputEvent(IntPtr inputEvent);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int ReadDeviceData(int deviceId, int type, IntPtr data, int sizeInBytes);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int WriteDeviceData(int deviceId, int type, IntPtr data, int sizeInBytes);

		public static void SetPollingFrequency(float hertz)
		{
			if (hertz < 1f)
			{
				throw new ArgumentException("Polling frequency cannot be less than 1Hz");
			}
			NativeInputSystem.SetPollingFrequencyInternal(hertz);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetPollingFrequencyInternal(float hertz);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Update(NativeInputUpdateType updateType);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int ReportNewInputDevice(string descriptor);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ReportInputDeviceDisconnect(int nativeDeviceId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ReportInputDeviceReconnect(int nativeDeviceId);
	}
}
