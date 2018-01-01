using System;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA.Input
{
	[MovedFrom("UnityEngine.VR.WSA.Input"), RequiredByNativeCode]
	public struct InteractionSource
	{
		internal uint m_Id;

		internal InteractionSourceKind m_SourceKind;

		internal InteractionSourceHandedness m_Handedness;

		internal InteractionSourceFlags m_Flags;

		internal ushort m_VendorId;

		internal ushort m_ProductId;

		internal ushort m_ProductVersion;

		public uint id
		{
			get
			{
				return this.m_Id;
			}
		}

		public InteractionSourceKind kind
		{
			get
			{
				return this.m_SourceKind;
			}
		}

		public InteractionSourceHandedness handedness
		{
			get
			{
				return this.m_Handedness;
			}
		}

		public bool supportsGrasp
		{
			get
			{
				return (this.m_Flags & InteractionSourceFlags.SupportsGrasp) != InteractionSourceFlags.None;
			}
		}

		public bool supportsMenu
		{
			get
			{
				return (this.m_Flags & InteractionSourceFlags.SupportsMenu) != InteractionSourceFlags.None;
			}
		}

		public bool supportsPointing
		{
			get
			{
				return (this.m_Flags & InteractionSourceFlags.SupportsPointing) != InteractionSourceFlags.None;
			}
		}

		public bool supportsThumbstick
		{
			get
			{
				return (this.m_Flags & InteractionSourceFlags.SupportsThumbstick) != InteractionSourceFlags.None;
			}
		}

		public bool supportsTouchpad
		{
			get
			{
				return (this.m_Flags & InteractionSourceFlags.SupportsTouchpad) != InteractionSourceFlags.None;
			}
		}

		public ushort vendorId
		{
			get
			{
				return this.m_VendorId;
			}
		}

		public ushort productId
		{
			get
			{
				return this.m_ProductId;
			}
		}

		public ushort productVersion
		{
			get
			{
				return this.m_ProductVersion;
			}
		}

		public override bool Equals(object obj)
		{
			InteractionSource? interactionSource = obj as InteractionSource?;
			return interactionSource.HasValue && interactionSource.Value.m_Id == this.m_Id;
		}

		public override int GetHashCode()
		{
			return (int)this.m_Id;
		}
	}
}
