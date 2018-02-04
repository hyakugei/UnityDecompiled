using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Modules;
using UnityEngine;

internal abstract class DesktopStandaloneBuildWindowExtension : DefaultBuildWindowExtension
{
	private GUIContent m_StandaloneTarget = EditorGUIUtility.TrTextContent("Target Platform", "Destination platform for standalone build", null);

	private GUIContent m_Architecture = EditorGUIUtility.TrTextContent("Architecture", "Build m_Architecture for standalone", null);

	private BuildTarget[] m_StandaloneSubtargets;

	private GUIContent[] m_StandaloneSubtargetStrings;

	private bool m_HasIl2CppPlayers;

	private bool m_IsRunningOnHostPlatform;

	public DesktopStandaloneBuildWindowExtension(bool hasIl2CppPlayers)
	{
		this.SetupStandaloneSubtargets();
		this.m_IsRunningOnHostPlatform = (Application.platform == this.GetHostPlatform());
		this.m_HasIl2CppPlayers = hasIl2CppPlayers;
	}

	private void SetupStandaloneSubtargets()
	{
		List<BuildTarget> list = new List<BuildTarget>();
		List<GUIContent> list2 = new List<GUIContent>();
		if (ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(BuildTarget.StandaloneWindows)))
		{
			list.Add(BuildTarget.StandaloneWindows);
			list2.Add(EditorGUIUtility.TrTextContent("Windows", null, null));
		}
		if (ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(BuildTarget.StandaloneOSX)))
		{
			list.Add(BuildTarget.StandaloneOSX);
			list2.Add(EditorGUIUtility.TrTextContent("Mac OS X", null, null));
		}
		if (ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(BuildTarget.StandaloneLinux)))
		{
			list.Add(BuildTarget.StandaloneLinux);
			list2.Add(EditorGUIUtility.TrTextContent("Linux", null, null));
		}
		this.m_StandaloneSubtargets = list.ToArray();
		this.m_StandaloneSubtargetStrings = list2.ToArray();
	}

	internal static BuildTarget GetBestStandaloneTarget(BuildTarget selectedTarget)
	{
		BuildTarget result;
		if (ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(selectedTarget)))
		{
			result = selectedTarget;
		}
		else if (Application.platform == RuntimePlatform.WindowsEditor && ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(BuildTarget.StandaloneWindows)))
		{
			result = BuildTarget.StandaloneWindows;
		}
		else if (Application.platform == RuntimePlatform.OSXEditor && ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(BuildTarget.StandaloneOSX)))
		{
			result = BuildTarget.StandaloneOSX;
		}
		else if (ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(BuildTarget.StandaloneOSX)))
		{
			result = BuildTarget.StandaloneOSX;
		}
		else if (ModuleManager.IsPlatformSupportLoaded(ModuleManager.GetTargetStringFromBuildTarget(BuildTarget.StandaloneLinux)))
		{
			result = BuildTarget.StandaloneLinux;
		}
		else
		{
			result = BuildTarget.StandaloneWindows;
		}
		return result;
	}

	private static Dictionary<GUIContent, BuildTarget> GetArchitecturesForPlatform(BuildTarget target)
	{
		Dictionary<GUIContent, BuildTarget> result;
		switch (target)
		{
		case BuildTarget.StandaloneLinux:
			goto IL_66;
		case (BuildTarget)18:
			IL_16:
			if (target == BuildTarget.StandaloneLinux64 || target == BuildTarget.StandaloneLinuxUniversal)
			{
				goto IL_66;
			}
			if (target != BuildTarget.StandaloneWindows)
			{
				result = null;
				return result;
			}
			goto IL_32;
		case BuildTarget.StandaloneWindows64:
			goto IL_32;
		}
		goto IL_16;
		IL_32:
		result = new Dictionary<GUIContent, BuildTarget>
		{
			{
				EditorGUIUtility.TrTextContent("x86", null, null),
				BuildTarget.StandaloneWindows
			},
			{
				EditorGUIUtility.TrTextContent("x86_64", null, null),
				BuildTarget.StandaloneWindows64
			}
		};
		return result;
		IL_66:
		result = new Dictionary<GUIContent, BuildTarget>
		{
			{
				EditorGUIUtility.TrTextContent("x86", null, null),
				BuildTarget.StandaloneLinux
			},
			{
				EditorGUIUtility.TrTextContent("x86_64", null, null),
				BuildTarget.StandaloneLinux64
			},
			{
				EditorGUIUtility.TrTextContent("x86 + x86_64 (Universal)", null, null),
				BuildTarget.StandaloneLinuxUniversal
			}
		};
		return result;
	}

	private static BuildTarget DefaultTargetForPlatform(BuildTarget target)
	{
		BuildTarget result;
		switch (target)
		{
		case BuildTarget.StandaloneOSX:
		case BuildTarget.StandaloneOSXIntel:
			goto IL_5B;
		case (BuildTarget)3:
			IL_19:
			switch (target)
			{
			case BuildTarget.StandaloneLinux64:
			case BuildTarget.StandaloneLinuxUniversal:
				goto IL_53;
			case BuildTarget.WP8Player:
				IL_32:
				switch (target)
				{
				case BuildTarget.StandaloneLinux:
					goto IL_53;
				case BuildTarget.StandaloneWindows64:
					goto IL_4C;
				}
				result = target;
				return result;
			case BuildTarget.StandaloneOSXIntel64:
				goto IL_5B;
			}
			goto IL_32;
			IL_53:
			result = BuildTarget.StandaloneLinux;
			return result;
		case BuildTarget.StandaloneWindows:
			goto IL_4C;
		}
		goto IL_19;
		IL_4C:
		result = BuildTarget.StandaloneWindows;
		return result;
		IL_5B:
		result = BuildTarget.StandaloneOSX;
		return result;
	}

	public override void ShowPlatformBuildOptions()
	{
		BuildTarget bestStandaloneTarget = DesktopStandaloneBuildWindowExtension.GetBestStandaloneTarget(EditorUserBuildSettings.selectedStandaloneTarget);
		BuildTarget buildTarget = EditorUserBuildSettings.selectedStandaloneTarget;
		int num = Math.Max(0, Array.IndexOf<BuildTarget>(this.m_StandaloneSubtargets, DesktopStandaloneBuildWindowExtension.DefaultTargetForPlatform(bestStandaloneTarget)));
		int num2 = EditorGUILayout.Popup(this.m_StandaloneTarget, num, this.m_StandaloneSubtargetStrings, new GUILayoutOption[0]);
		if (num2 == num)
		{
			Dictionary<GUIContent, BuildTarget> architecturesForPlatform = DesktopStandaloneBuildWindowExtension.GetArchitecturesForPlatform(bestStandaloneTarget);
			if (architecturesForPlatform != null)
			{
				GUIContent[] array = new List<GUIContent>(architecturesForPlatform.Keys).ToArray();
				int num3 = 0;
				if (num2 == num)
				{
					foreach (KeyValuePair<GUIContent, BuildTarget> current in architecturesForPlatform)
					{
						if (current.Value == bestStandaloneTarget)
						{
							num3 = Math.Max(0, Array.IndexOf<GUIContent>(array, current.Key));
							break;
						}
					}
				}
				num3 = EditorGUILayout.Popup(this.m_Architecture, num3, array, new GUILayoutOption[0]);
				buildTarget = architecturesForPlatform[array[num3]];
			}
		}
		else
		{
			buildTarget = this.m_StandaloneSubtargets[num2];
		}
		if (buildTarget != EditorUserBuildSettings.selectedStandaloneTarget)
		{
			EditorUserBuildSettings.selectedStandaloneTarget = buildTarget;
			GUIUtility.ExitGUI();
		}
		this.ShowIl2CppErrorIfNeeded();
	}

	private void ShowIl2CppErrorIfNeeded()
	{
		if (PlayerSettings.GetScriptingBackend(BuildTargetGroup.Standalone) == ScriptingImplementation.IL2CPP)
		{
			string cannotBuildIl2CppPlayerInCurrentSetupError = this.GetCannotBuildIl2CppPlayerInCurrentSetupError();
			if (!string.IsNullOrEmpty(cannotBuildIl2CppPlayerInCurrentSetupError))
			{
				EditorGUILayout.HelpBox(cannotBuildIl2CppPlayerInCurrentSetupError, MessageType.Error);
			}
		}
	}

	public override bool EnabledBuildButton()
	{
		return PlayerSettings.GetScriptingBackend(BuildTargetGroup.Standalone) == ScriptingImplementation.Mono2x || string.IsNullOrEmpty(this.GetCannotBuildIl2CppPlayerInCurrentSetupError());
	}

	protected virtual string GetCannotBuildIl2CppPlayerInCurrentSetupError()
	{
		string result;
		if (!this.m_IsRunningOnHostPlatform)
		{
			result = string.Format("{0} IL2CPP player can only be built on {0}.", this.GetHostPlatformName());
		}
		else if (!this.m_HasIl2CppPlayers)
		{
			result = "Currently selected scripting backend (IL2CPP) is not installed.";
		}
		else
		{
			result = null;
		}
		return result;
	}

	protected abstract RuntimePlatform GetHostPlatform();

	protected abstract string GetHostPlatformName();

	public override bool EnabledBuildAndRunButton()
	{
		return true;
	}
}
