using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine
{
	public sealed class Shader : Object
	{
		internal extern string customEditor
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		[Obsolete("Use Graphics.activeTier instead (UnityUpgradable) -> UnityEngine.Graphics.activeTier", false)]
		public static ShaderHardwareTier globalShaderHardwareTier
		{
			get
			{
				return (ShaderHardwareTier)Graphics.activeTier;
			}
			set
			{
				Graphics.activeTier = (GraphicsTier)value;
			}
		}

		public extern int maximumLOD
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public static extern int globalMaximumLOD
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern bool isSupported
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern string globalRenderPipeline
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern int renderQueue
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		internal extern DisableBatchingType disableBatching
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		private Shader()
		{
		}

		[GeneratedByOldBindingsGenerator, ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int PropertyToID(string name);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string IDToProperty(int id);

		[Obsolete("SetGlobalTexGenMode is not supported anymore. Use programmable shaders to achieve the same effect.", true)]
		public static void SetGlobalTexGenMode(string propertyName, TexGenMode mode)
		{
		}

		[Obsolete("SetGlobalTextureMatrixName is not supported anymore. Use programmable shaders to achieve the same effect.", true)]
		public static void SetGlobalTextureMatrixName(string propertyName, string matrixName)
		{
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Shader Find(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Shader FindBuiltin(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void EnableKeyword(string keyword);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DisableKeyword(string keyword);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool IsKeywordEnabled(string keyword);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void WarmupAllShaders();

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int TagToID(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string IDToTag(int name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetGlobalFloatImpl(int name, float value);

		private static void SetGlobalVectorImpl(int name, Vector4 value)
		{
			Shader.SetGlobalVectorImpl_Injected(name, ref value);
		}

		private static void SetGlobalMatrixImpl(int name, Matrix4x4 value)
		{
			Shader.SetGlobalMatrixImpl_Injected(name, ref value);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetGlobalTextureImpl(int name, Texture value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetGlobalBufferImpl(int name, ComputeBuffer value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetGlobalFloatImpl(int name);

		private static Vector4 GetGlobalVectorImpl(int name)
		{
			Vector4 result;
			Shader.GetGlobalVectorImpl_Injected(name, out result);
			return result;
		}

		private static Matrix4x4 GetGlobalMatrixImpl(int name)
		{
			Matrix4x4 result;
			Shader.GetGlobalMatrixImpl_Injected(name, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Texture GetGlobalTextureImpl(int name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetGlobalFloatArrayImpl(int name, float[] values, int count);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetGlobalVectorArrayImpl(int name, Vector4[] values, int count);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetGlobalMatrixArrayImpl(int name, Matrix4x4[] values, int count);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float[] GetGlobalFloatArrayImpl(int name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Vector4[] GetGlobalVectorArrayImpl(int name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Matrix4x4[] GetGlobalMatrixArrayImpl(int name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetGlobalFloatArrayCountImpl(int name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetGlobalVectorArrayCountImpl(int name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetGlobalMatrixArrayCountImpl(int name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ExtractGlobalFloatArrayImpl(int name, [Out] float[] val);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ExtractGlobalVectorArrayImpl(int name, [Out] Vector4[] val);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ExtractGlobalMatrixArrayImpl(int name, [Out] Matrix4x4[] val);

		private static void SetGlobalFloatArray(int name, float[] values, int count)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			if (values.Length == 0)
			{
				throw new ArgumentException("Zero-sized array is not allowed.");
			}
			if (values.Length < count)
			{
				throw new ArgumentException("array has less elements than passed count.");
			}
			Shader.SetGlobalFloatArrayImpl(name, values, count);
		}

		private static void SetGlobalVectorArray(int name, Vector4[] values, int count)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			if (values.Length == 0)
			{
				throw new ArgumentException("Zero-sized array is not allowed.");
			}
			if (values.Length < count)
			{
				throw new ArgumentException("array has less elements than passed count.");
			}
			Shader.SetGlobalVectorArrayImpl(name, values, count);
		}

		private static void SetGlobalMatrixArray(int name, Matrix4x4[] values, int count)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			if (values.Length == 0)
			{
				throw new ArgumentException("Zero-sized array is not allowed.");
			}
			if (values.Length < count)
			{
				throw new ArgumentException("array has less elements than passed count.");
			}
			Shader.SetGlobalMatrixArrayImpl(name, values, count);
		}

		private static void ExtractGlobalFloatArray(int name, List<float> values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			values.Clear();
			int globalFloatArrayCountImpl = Shader.GetGlobalFloatArrayCountImpl(name);
			if (globalFloatArrayCountImpl > 0)
			{
				NoAllocHelpers.EnsureListElemCount<float>(values, globalFloatArrayCountImpl);
				Shader.ExtractGlobalFloatArrayImpl(name, (float[])NoAllocHelpers.ExtractArrayFromList(values));
			}
		}

		private static void ExtractGlobalVectorArray(int name, List<Vector4> values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			values.Clear();
			int globalVectorArrayCountImpl = Shader.GetGlobalVectorArrayCountImpl(name);
			if (globalVectorArrayCountImpl > 0)
			{
				NoAllocHelpers.EnsureListElemCount<Vector4>(values, globalVectorArrayCountImpl);
				Shader.ExtractGlobalVectorArrayImpl(name, (Vector4[])NoAllocHelpers.ExtractArrayFromList(values));
			}
		}

		private static void ExtractGlobalMatrixArray(int name, List<Matrix4x4> values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			values.Clear();
			int globalMatrixArrayCountImpl = Shader.GetGlobalMatrixArrayCountImpl(name);
			if (globalMatrixArrayCountImpl > 0)
			{
				NoAllocHelpers.EnsureListElemCount<Matrix4x4>(values, globalMatrixArrayCountImpl);
				Shader.ExtractGlobalMatrixArrayImpl(name, (Matrix4x4[])NoAllocHelpers.ExtractArrayFromList(values));
			}
		}

		public static void SetGlobalFloat(string name, float value)
		{
			Shader.SetGlobalFloatImpl(Shader.PropertyToID(name), value);
		}

		public static void SetGlobalFloat(int name, float value)
		{
			Shader.SetGlobalFloatImpl(name, value);
		}

		public static void SetGlobalInt(string name, int value)
		{
			Shader.SetGlobalFloatImpl(Shader.PropertyToID(name), (float)value);
		}

		public static void SetGlobalInt(int name, int value)
		{
			Shader.SetGlobalFloatImpl(name, (float)value);
		}

		public static void SetGlobalVector(string name, Vector4 value)
		{
			Shader.SetGlobalVectorImpl(Shader.PropertyToID(name), value);
		}

		public static void SetGlobalVector(int name, Vector4 value)
		{
			Shader.SetGlobalVectorImpl(name, value);
		}

		public static void SetGlobalColor(string name, Color value)
		{
			Shader.SetGlobalVectorImpl(Shader.PropertyToID(name), value);
		}

		public static void SetGlobalColor(int name, Color value)
		{
			Shader.SetGlobalVectorImpl(name, value);
		}

		public static void SetGlobalMatrix(string name, Matrix4x4 value)
		{
			Shader.SetGlobalMatrixImpl(Shader.PropertyToID(name), value);
		}

		public static void SetGlobalMatrix(int name, Matrix4x4 value)
		{
			Shader.SetGlobalMatrixImpl(name, value);
		}

		public static void SetGlobalTexture(string name, Texture value)
		{
			Shader.SetGlobalTextureImpl(Shader.PropertyToID(name), value);
		}

		public static void SetGlobalTexture(int name, Texture value)
		{
			Shader.SetGlobalTextureImpl(name, value);
		}

		public static void SetGlobalBuffer(string name, ComputeBuffer value)
		{
			Shader.SetGlobalBufferImpl(Shader.PropertyToID(name), value);
		}

		public static void SetGlobalBuffer(int name, ComputeBuffer value)
		{
			Shader.SetGlobalBufferImpl(name, value);
		}

		public static void SetGlobalFloatArray(string name, List<float> values)
		{
			Shader.SetGlobalFloatArray(Shader.PropertyToID(name), NoAllocHelpers.ExtractArrayFromListT<float>(values), values.Count);
		}

		public static void SetGlobalFloatArray(int name, List<float> values)
		{
			Shader.SetGlobalFloatArray(name, NoAllocHelpers.ExtractArrayFromListT<float>(values), values.Count);
		}

		public static void SetGlobalFloatArray(string name, float[] values)
		{
			Shader.SetGlobalFloatArray(Shader.PropertyToID(name), values, values.Length);
		}

		public static void SetGlobalFloatArray(int name, float[] values)
		{
			Shader.SetGlobalFloatArray(name, values, values.Length);
		}

		public static void SetGlobalVectorArray(string name, List<Vector4> values)
		{
			Shader.SetGlobalVectorArray(Shader.PropertyToID(name), NoAllocHelpers.ExtractArrayFromListT<Vector4>(values), values.Count);
		}

		public static void SetGlobalVectorArray(int name, List<Vector4> values)
		{
			Shader.SetGlobalVectorArray(name, NoAllocHelpers.ExtractArrayFromListT<Vector4>(values), values.Count);
		}

		public static void SetGlobalVectorArray(string name, Vector4[] values)
		{
			Shader.SetGlobalVectorArray(Shader.PropertyToID(name), values, values.Length);
		}

		public static void SetGlobalVectorArray(int name, Vector4[] values)
		{
			Shader.SetGlobalVectorArray(name, values, values.Length);
		}

		public static void SetGlobalMatrixArray(string name, List<Matrix4x4> values)
		{
			Shader.SetGlobalMatrixArray(Shader.PropertyToID(name), NoAllocHelpers.ExtractArrayFromListT<Matrix4x4>(values), values.Count);
		}

		public static void SetGlobalMatrixArray(int name, List<Matrix4x4> values)
		{
			Shader.SetGlobalMatrixArray(name, NoAllocHelpers.ExtractArrayFromListT<Matrix4x4>(values), values.Count);
		}

		public static void SetGlobalMatrixArray(string name, Matrix4x4[] values)
		{
			Shader.SetGlobalMatrixArray(Shader.PropertyToID(name), values, values.Length);
		}

		public static void SetGlobalMatrixArray(int name, Matrix4x4[] values)
		{
			Shader.SetGlobalMatrixArray(name, values, values.Length);
		}

		public static float GetGlobalFloat(string name)
		{
			return Shader.GetGlobalFloatImpl(Shader.PropertyToID(name));
		}

		public static float GetGlobalFloat(int name)
		{
			return Shader.GetGlobalFloatImpl(name);
		}

		public static int GetGlobalInt(string name)
		{
			return (int)Shader.GetGlobalFloatImpl(Shader.PropertyToID(name));
		}

		public static int GetGlobalInt(int name)
		{
			return (int)Shader.GetGlobalFloatImpl(name);
		}

		public static Vector4 GetGlobalVector(string name)
		{
			return Shader.GetGlobalVectorImpl(Shader.PropertyToID(name));
		}

		public static Vector4 GetGlobalVector(int name)
		{
			return Shader.GetGlobalVectorImpl(name);
		}

		public static Color GetGlobalColor(string name)
		{
			return Shader.GetGlobalVectorImpl(Shader.PropertyToID(name));
		}

		public static Color GetGlobalColor(int name)
		{
			return Shader.GetGlobalVectorImpl(name);
		}

		public static Matrix4x4 GetGlobalMatrix(string name)
		{
			return Shader.GetGlobalMatrixImpl(Shader.PropertyToID(name));
		}

		public static Matrix4x4 GetGlobalMatrix(int name)
		{
			return Shader.GetGlobalMatrixImpl(name);
		}

		public static Texture GetGlobalTexture(string name)
		{
			return Shader.GetGlobalTextureImpl(Shader.PropertyToID(name));
		}

		public static Texture GetGlobalTexture(int name)
		{
			return Shader.GetGlobalTextureImpl(name);
		}

		public static float[] GetGlobalFloatArray(string name)
		{
			return Shader.GetGlobalFloatArray(Shader.PropertyToID(name));
		}

		public static float[] GetGlobalFloatArray(int name)
		{
			return (Shader.GetGlobalFloatArrayCountImpl(name) == 0) ? null : Shader.GetGlobalFloatArrayImpl(name);
		}

		public static Vector4[] GetGlobalVectorArray(string name)
		{
			return Shader.GetGlobalVectorArray(Shader.PropertyToID(name));
		}

		public static Vector4[] GetGlobalVectorArray(int name)
		{
			return (Shader.GetGlobalVectorArrayCountImpl(name) == 0) ? null : Shader.GetGlobalVectorArrayImpl(name);
		}

		public static Matrix4x4[] GetGlobalMatrixArray(string name)
		{
			return Shader.GetGlobalMatrixArray(Shader.PropertyToID(name));
		}

		public static Matrix4x4[] GetGlobalMatrixArray(int name)
		{
			return (Shader.GetGlobalMatrixArrayCountImpl(name) == 0) ? null : Shader.GetGlobalMatrixArrayImpl(name);
		}

		public static void GetGlobalFloatArray(string name, List<float> values)
		{
			Shader.ExtractGlobalFloatArray(Shader.PropertyToID(name), values);
		}

		public static void GetGlobalFloatArray(int name, List<float> values)
		{
			Shader.ExtractGlobalFloatArray(name, values);
		}

		public static void GetGlobalVectorArray(string name, List<Vector4> values)
		{
			Shader.ExtractGlobalVectorArray(Shader.PropertyToID(name), values);
		}

		public static void GetGlobalVectorArray(int name, List<Vector4> values)
		{
			Shader.ExtractGlobalVectorArray(name, values);
		}

		public static void GetGlobalMatrixArray(string name, List<Matrix4x4> values)
		{
			Shader.ExtractGlobalMatrixArray(Shader.PropertyToID(name), values);
		}

		public static void GetGlobalMatrixArray(int name, List<Matrix4x4> values)
		{
			Shader.ExtractGlobalMatrixArray(name, values);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetGlobalVectorImpl_Injected(int name, ref Vector4 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetGlobalMatrixImpl_Injected(int name, ref Matrix4x4 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetGlobalVectorImpl_Injected(int name, out Vector4 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetGlobalMatrixImpl_Injected(int name, out Matrix4x4 ret);
	}
}
