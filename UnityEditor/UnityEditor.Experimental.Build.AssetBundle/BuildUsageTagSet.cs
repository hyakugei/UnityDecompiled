using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace UnityEditor.Experimental.Build.AssetBundle
{
	[UsedByNativeCode]
	[Serializable]
	public class BuildUsageTagSet : ISerializable, IDisposable
	{
		private IntPtr m_Ptr;

		public BuildUsageTagSet()
		{
			this.m_Ptr = BuildUsageTagSet.Internal_Create();
		}

		protected BuildUsageTagSet(System.Runtime.Serialization.SerializationInfo info, StreamingContext context)
		{
			this.m_Ptr = BuildUsageTagSet.Internal_Create();
			string data = (string)info.GetValue("tags", typeof(string));
			this.DeserializeNativeFromString(data);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Internal_Create();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_Destroy(IntPtr ptr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetHash();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern string SerializeNativeToString();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void DeserializeNativeFromString(string data);

		internal string GetBuildUsageJson(ObjectIdentifier objectId)
		{
			return this.GetBuildUsageJson_Injected(ref objectId);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern ObjectIdentifier[] GetObjectIdentifiers();

		~BuildUsageTagSet()
		{
			this.Dispose();
		}

		public void Dispose()
		{
			if (this.m_Ptr != IntPtr.Zero)
			{
				BuildUsageTagSet.Internal_Destroy(this.m_Ptr);
				this.m_Ptr = IntPtr.Zero;
			}
		}

		public override bool Equals(object obj)
		{
			BuildUsageTagSet buildUsageTagSet = obj as BuildUsageTagSet;
			return buildUsageTagSet != null && buildUsageTagSet.GetHash() == this.GetHash();
		}

		public override int GetHashCode()
		{
			return this.GetHash();
		}

		public void GetObjectData(System.Runtime.Serialization.SerializationInfo info, StreamingContext context)
		{
			string value = this.SerializeNativeToString();
			info.AddValue("tags", value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern string GetBuildUsageJson_Injected(ref ObjectIdentifier objectId);
	}
}
