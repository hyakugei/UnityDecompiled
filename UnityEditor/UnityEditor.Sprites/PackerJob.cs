using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor.Sprites
{
	public sealed class PackerJob
	{
		internal PackerJob()
		{
		}

		private static void Internal_AddAtlas(string atlasName, AtlasSettings settings)
		{
			PackerJob.Internal_AddAtlas_Injected(atlasName, ref settings);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_AssignToAtlas(string atlasName, Sprite sprite, SpritePackingMode packingMode, SpritePackingRotation packingRotation);

		public void AddAtlas(string atlasName, AtlasSettings settings)
		{
			PackerJob.Internal_AddAtlas(atlasName, settings);
		}

		public void AssignToAtlas(string atlasName, Sprite sprite, SpritePackingMode packingMode, SpritePackingRotation packingRotation)
		{
			PackerJob.Internal_AssignToAtlas(atlasName, sprite, packingMode, packingRotation);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_AddAtlas_Injected(string atlasName, ref AtlasSettings settings);
	}
}
