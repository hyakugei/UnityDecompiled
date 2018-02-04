using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.Experimental.U2D
{
	public class SpriteShapeUtility
	{
		public static int[] Generate(Mesh mesh, SpriteShapeParameters shapeParams, ShapeControlPoint[] points, SpriteShapeMetaData[] metaData, AngleRangeInfo[] angleRange, Sprite[] sprites, Sprite[] corners)
		{
			return SpriteShapeUtility.Generate_Injected(mesh, ref shapeParams, points, metaData, angleRange, sprites, corners);
		}

		public static void GenerateSpriteShape(SpriteShapeRenderer renderer, SpriteShapeParameters shapeParams, ShapeControlPoint[] points, SpriteShapeMetaData[] metaData, AngleRangeInfo[] angleRange, Sprite[] sprites, Sprite[] corners)
		{
			SpriteShapeUtility.GenerateSpriteShape_Injected(renderer, ref shapeParams, points, metaData, angleRange, sprites, corners);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int[] Generate_Injected(Mesh mesh, ref SpriteShapeParameters shapeParams, ShapeControlPoint[] points, SpriteShapeMetaData[] metaData, AngleRangeInfo[] angleRange, Sprite[] sprites, Sprite[] corners);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GenerateSpriteShape_Injected(SpriteShapeRenderer renderer, ref SpriteShapeParameters shapeParams, ShapeControlPoint[] points, SpriteShapeMetaData[] metaData, AngleRangeInfo[] angleRange, Sprite[] sprites, Sprite[] corners);
	}
}
