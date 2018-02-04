using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Bindings;

namespace UnityEditor
{
	[NativeType(Header = "Modules/AssetDatabase/Editor/Public/AssetImportInProgressProxy.h")]
	internal class AssetImportInProgressProxy : UnityEngine.Object
	{
		public GUID asset
		{
			get
			{
				GUID result;
				this.get_asset_Injected(out result);
				return result;
			}
			set
			{
				this.set_asset_Injected(ref value);
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsProxyAsset(int instanceID);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_asset_Injected(out GUID ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_asset_Injected(ref GUID value);
	}
}
