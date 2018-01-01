using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.Experimental.U2D
{
	public class SpriteShapeUtility
	{
		public static void Generate(Mesh mesh, SpriteShapeParameters shapeParams, ShapeControlPoint[] points, SpriteShapeMetaData[] metaData, AngleRangeInfo[] angleRange, Sprite[] sprites, Sprite[] corners)
		{
			SpriteShapeUtility.Generate_Injected(mesh, ref shapeParams, points, metaData, angleRange, sprites, corners);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Generate_Injected(Mesh mesh, ref SpriteShapeParameters shapeParams, ShapeControlPoint[] points, SpriteShapeMetaData[] metaData, AngleRangeInfo[] angleRange, Sprite[] sprites, Sprite[] corners);
	}
}
