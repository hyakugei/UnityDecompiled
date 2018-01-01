using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[NativeType("Runtime/Graphics/SpriteFrame.h")]
	public sealed class Sprite : Object
	{
		public Bounds bounds
		{
			get
			{
				Bounds result;
				this.INTERNAL_get_bounds(out result);
				return result;
			}
		}

		public Rect rect
		{
			get
			{
				Rect result;
				this.INTERNAL_get_rect(out result);
				return result;
			}
		}

		public extern Texture2D texture
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern Texture2D associatedAlphaSplitTexture
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public Rect textureRect
		{
			get
			{
				Rect result;
				this.INTERNAL_get_textureRect(out result);
				return result;
			}
		}

		public Vector2 textureRectOffset
		{
			get
			{
				Vector2 result;
				Sprite.Internal_GetTextureRectOffset(this, out result);
				return result;
			}
		}

		public extern bool packed
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern SpritePackingMode packingMode
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern SpritePackingRotation packingRotation
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public Vector2 pivot
		{
			get
			{
				Vector2 result;
				Sprite.Internal_GetPivot(this, out result);
				return result;
			}
		}

		public Vector4 border
		{
			get
			{
				Vector4 result;
				this.INTERNAL_get_border(out result);
				return result;
			}
		}

		public extern Vector2[] vertices
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern ushort[] triangles
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern Vector2[] uv
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern float pixelsPerUnit
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, [DefaultValue("100.0f")] float pixelsPerUnit, [DefaultValue("0")] uint extrude, [DefaultValue("SpriteMeshType.Tight")] SpriteMeshType meshType, [DefaultValue("Vector4.zero")] Vector4 border)
		{
			return Sprite.INTERNAL_CALL_Create(texture, ref rect, ref pivot, pixelsPerUnit, extrude, meshType, ref border);
		}

		[ExcludeFromDocs]
		public static Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit, uint extrude, SpriteMeshType meshType)
		{
			Vector4 zero = Vector4.zero;
			return Sprite.INTERNAL_CALL_Create(texture, ref rect, ref pivot, pixelsPerUnit, extrude, meshType, ref zero);
		}

		[ExcludeFromDocs]
		public static Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit, uint extrude)
		{
			Vector4 zero = Vector4.zero;
			SpriteMeshType meshType = SpriteMeshType.Tight;
			return Sprite.INTERNAL_CALL_Create(texture, ref rect, ref pivot, pixelsPerUnit, extrude, meshType, ref zero);
		}

		[ExcludeFromDocs]
		public static Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit)
		{
			Vector4 zero = Vector4.zero;
			SpriteMeshType meshType = SpriteMeshType.Tight;
			uint extrude = 0u;
			return Sprite.INTERNAL_CALL_Create(texture, ref rect, ref pivot, pixelsPerUnit, extrude, meshType, ref zero);
		}

		[ExcludeFromDocs]
		public static Sprite Create(Texture2D texture, Rect rect, Vector2 pivot)
		{
			Vector4 zero = Vector4.zero;
			SpriteMeshType meshType = SpriteMeshType.Tight;
			uint extrude = 0u;
			float pixelsPerUnit = 100f;
			return Sprite.INTERNAL_CALL_Create(texture, ref rect, ref pivot, pixelsPerUnit, extrude, meshType, ref zero);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Sprite INTERNAL_CALL_Create(Texture2D texture, ref Rect rect, ref Vector2 pivot, float pixelsPerUnit, uint extrude, SpriteMeshType meshType, ref Vector4 border);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_bounds(out Bounds value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_rect(out Rect value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_textureRect(out Rect value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_GetTextureRectOffset(Sprite sprite, out Vector2 output);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_GetPivot(Sprite sprite, out Vector2 output);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_border(out Vector4 value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void OverrideGeometry(Vector2[] vertices, ushort[] triangles);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetPhysicsShapeCount();

		public int GetPhysicsShapePointCount(int shapeIdx)
		{
			int physicsShapeCount = this.GetPhysicsShapeCount();
			if (shapeIdx < 0 || shapeIdx >= physicsShapeCount)
			{
				throw new IndexOutOfRangeException(string.Format("Index({0}) is out of bounds(0 - {1})", shapeIdx, physicsShapeCount - 1));
			}
			return this.Internal_GetPhysicsShapePointCount(shapeIdx);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int Internal_GetPhysicsShapePointCount(int shapeIdx);

		public int GetPhysicsShape(int shapeIdx, List<Vector2> physicsShape)
		{
			int physicsShapeCount = this.GetPhysicsShapeCount();
			if (shapeIdx < 0 || shapeIdx >= physicsShapeCount)
			{
				throw new IndexOutOfRangeException(string.Format("Index({0}) is out of bounds(0 - {1})", shapeIdx, physicsShapeCount - 1));
			}
			Sprite.GetPhysicsShapeImpl(this, shapeIdx, physicsShape);
			return physicsShape.Count;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetPhysicsShapeImpl(Sprite sprite, int shapeIdx, List<Vector2> physicsShape);

		public void OverridePhysicsShape(IList<Vector2[]> physicsShapes)
		{
			for (int i = 0; i < physicsShapes.Count; i++)
			{
				Vector2[] array = physicsShapes[i];
				if (array == null)
				{
					throw new ArgumentNullException(string.Format("Physics Shape at {0} is null.", i));
				}
				if (array.Length < 3)
				{
					throw new ArgumentException(string.Format("Physics Shape at {0} has less than 3 vertices ({1}).", i, array.Length));
				}
			}
			Sprite.OverridePhysicsShapeCount(this, physicsShapes.Count);
			for (int j = 0; j < physicsShapes.Count; j++)
			{
				Sprite.OverridePhysicsShape(this, physicsShapes[j], j);
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void OverridePhysicsShapeCount(Sprite sprite, int physicsShapeCount);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void OverridePhysicsShape(Sprite sprite, Vector2[] physicsShape, int idx);
	}
}
