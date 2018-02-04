using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor.Experimental.AssetImporters
{
	[RequiredByNativeCode]
	public class AssetImportContext
	{
		internal IntPtr m_Self;

		public extern string assetPath
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			internal set;
		}

		public extern BuildTarget selectedBuildTarget
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		private AssetImportContext()
		{
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void LogMessage(string msg, string file, int line, UnityEngine.Object obj, bool isAnError);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetMainObject(UnityEngine.Object obj);

		public void AddObjectToAsset(string identifier, UnityEngine.Object obj)
		{
			this.AddObjectToAsset(identifier, obj, null);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void AddObjectToAsset(string identifier, UnityEngine.Object obj, Texture2D thumbnail);

		internal void DependOnHashOfSourceFile(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentNullException("path", "Cannot add a null path");
			}
			this.DependOnHashOfSourceFileInternal(path);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void DependOnHashOfSourceFileInternal(string path);

		public void LogImportError(string msg, UnityEngine.Object obj = null)
		{
			this.AddToLog(msg, true, obj);
		}

		public void LogImportWarning(string msg, UnityEngine.Object obj = null)
		{
			this.AddToLog(msg, false, obj);
		}

		private void AddToLog(string msg, bool isAnError, UnityEngine.Object obj)
		{
			StackTrace stackTrace = new StackTrace(2, true);
			System.Diagnostics.StackFrame frame = stackTrace.GetFrame(0);
			this.LogMessage(msg, frame.GetFileName(), frame.GetFileLineNumber(), obj, isAnError);
		}
	}
}
