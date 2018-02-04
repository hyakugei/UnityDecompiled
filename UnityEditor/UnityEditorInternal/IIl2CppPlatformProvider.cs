using System;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.Scripting.Compilers;

namespace UnityEditorInternal
{
	internal interface IIl2CppPlatformProvider
	{
		BuildTarget target
		{
			get;
		}

		bool emitNullChecks
		{
			get;
		}

		bool enableStackTraces
		{
			get;
		}

		bool enableArrayBoundsCheck
		{
			get;
		}

		bool enableDivideByZeroCheck
		{
			get;
		}

		string nativeLibraryFileName
		{
			get;
		}

		string il2CppFolder
		{
			get;
		}

		bool developmentMode
		{
			get;
		}

		string moduleStrippingInformationFolder
		{
			get;
		}

		bool supportsEngineStripping
		{
			get;
		}

		bool enableDebugger
		{
			get;
		}

		BuildReport buildReport
		{
			get;
		}

		string[] includePaths
		{
			get;
		}

		string[] libraryPaths
		{
			get;
		}

		INativeCompiler CreateNativeCompiler();

		Il2CppNativeCodeBuilder CreateIl2CppNativeCodeBuilder();

		CompilerOutputParserBase CreateIl2CppOutputParser();
	}
}
