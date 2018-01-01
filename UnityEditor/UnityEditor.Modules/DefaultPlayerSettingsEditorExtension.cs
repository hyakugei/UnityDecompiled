using System;
using System.Linq;
using UnityEngine;

namespace UnityEditor.Modules
{
	internal abstract class DefaultPlayerSettingsEditorExtension : ISettingEditorExtension
	{
		protected PlayerSettingsEditor m_playerSettingsEditor;

		protected SerializedProperty m_MTRendering;

		private static readonly GUIContent m_MTRenderingTooltip = EditorGUIUtility.TrTextContent("Multithreaded Rendering*", null, null);

		protected PlayerSettingsEditor playerSettingsEditor
		{
			get
			{
				return this.m_playerSettingsEditor;
			}
		}

		public virtual void OnEnable(PlayerSettingsEditor settingsEditor)
		{
			this.m_playerSettingsEditor = settingsEditor;
			this.m_MTRendering = this.playerSettingsEditor.FindPropertyAssert("m_MTRendering");
		}

		public virtual bool HasPublishSection()
		{
			return true;
		}

		public virtual void PublishSectionGUI(float h, float midWidth, float maxWidth)
		{
		}

		public virtual bool HasIdentificationGUI()
		{
			return false;
		}

		public virtual void IdentificationSectionGUI()
		{
		}

		public virtual void ConfigurationSectionGUI()
		{
		}

		public virtual bool SupportsOrientation()
		{
			return false;
		}

		public virtual bool CanShowUnitySplashScreen()
		{
			return false;
		}

		public virtual void SplashSectionGUI()
		{
		}

		public virtual bool UsesStandardIcons()
		{
			return true;
		}

		public virtual void IconSectionGUI()
		{
		}

		public virtual bool HasResolutionSection()
		{
			return false;
		}

		public virtual bool SupportsStaticBatching()
		{
			return true;
		}

		public virtual bool SupportsDynamicBatching()
		{
			return true;
		}

		public virtual void ResolutionSectionGUI(float h, float midWidth, float maxWidth)
		{
		}

		public virtual bool HasBundleIdentifier()
		{
			return true;
		}

		public virtual bool SupportsHighDynamicRangeDisplays()
		{
			return false;
		}

		public virtual bool SupportsGfxJobModes()
		{
			return false;
		}

		public string FixTargetOSVersion(string version)
		{
			int[] array = (from i in Enumerable.Range(0, version.Length)
			where version[i] == '.'
			select i).ToArray<int>();
			string result;
			if (array.Length <= 0)
			{
				result = (version + ".0").Trim();
			}
			else if (array.Length > 1)
			{
				result = version.Substring(0, array[1]).Trim();
			}
			else if (array.Length == 1 && array[0] == version.Length - 1)
			{
				result = (version + "0").Trim();
			}
			else
			{
				result = version.Trim();
			}
			return result;
		}

		public virtual bool SupportsMultithreadedRendering()
		{
			return false;
		}

		protected virtual GUIContent MultithreadedRenderingGUITooltip()
		{
			return DefaultPlayerSettingsEditorExtension.m_MTRenderingTooltip;
		}

		public virtual void MultithreadedRenderingGUI(BuildTargetGroup targetGroup)
		{
			if (this.playerSettingsEditor.IsMobileTarget(targetGroup))
			{
				bool mobileMTRendering = PlayerSettings.GetMobileMTRendering(targetGroup);
				bool flag = EditorGUILayout.Toggle(this.MultithreadedRenderingGUITooltip(), mobileMTRendering, new GUILayoutOption[0]);
				if (mobileMTRendering != flag)
				{
					PlayerSettings.SetMobileMTRendering(targetGroup, flag);
				}
			}
			else
			{
				EditorGUILayout.PropertyField(this.m_MTRendering, DefaultPlayerSettingsEditorExtension.m_MTRenderingTooltip, new GUILayoutOption[0]);
			}
		}

		public virtual bool SupportsCustomLightmapEncoding()
		{
			return false;
		}
	}
}
