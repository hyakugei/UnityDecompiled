using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEditor.Presets
{
	[NativeType(Header = "Modules/PresetsEditor/Preset.h"), ExcludeFromPreset, UsedByNativeCode]
	public sealed class Preset : UnityEngine.Object
	{
		public extern PropertyModification[] PropertyModifications
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public Preset(UnityEngine.Object source)
		{
			Preset.Internal_Create(this, source);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_Create([Writable] Preset notSelf, [NotNull] UnityEngine.Object source);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool ApplyTo([NotNull] UnityEngine.Object target);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool UpdateProperties([NotNull] UnityEngine.Object source);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string GetTargetFullTypeName();

		public string GetTargetTypeName()
		{
			string targetFullTypeName = this.GetTargetFullTypeName();
			int num = targetFullTypeName.LastIndexOf(".");
			if (num == -1)
			{
				num = targetFullTypeName.LastIndexOf(":");
			}
			string result;
			if (num != -1)
			{
				result = targetFullTypeName.Substring(num + 1);
			}
			else
			{
				result = targetFullTypeName;
			}
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsValid();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool CanBeAppliedTo(UnityEngine.Object target);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern UnityEngine.Object GetReferenceObject();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Preset GetDefaultForObject([NotNull] UnityEngine.Object target);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Preset GetDefaultForPreset([NotNull] Preset preset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool SetAsDefault([NotNull] Preset preset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void RemoveFromDefault([NotNull] Preset preset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsPresetExcludedFromDefaultPresets(Preset preset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsObjectExcludedFromDefaultPresets(UnityEngine.Object target);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsObjectExcludedFromPresets(UnityEngine.Object reference);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsExcludedFromPresetsByTypeID(int nativeTypeID);
	}
}
