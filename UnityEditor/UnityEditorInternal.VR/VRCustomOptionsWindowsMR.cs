using System;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal.VR
{
	internal class VRCustomOptionsWindowsMR : VRCustomOptions
	{
		private static GUIContent[] s_DepthOptions = new GUIContent[]
		{
			EditorGUIUtility.TrTextContent("16-bit depth", null, null),
			EditorGUIUtility.TrTextContent("24-bit depth", null, null)
		};

		private static GUIContent s_DepthFormatLabel = EditorGUIUtility.TrTextContent("Depth Format", null, null);

		private static GUIContent s_DepthBufferSharingLabel = EditorGUIUtility.TrTextContent("Enable Depth Buffer Sharing", null, null);

		private SerializedProperty m_DepthFormat;

		private SerializedProperty m_DepthBufferSharingEnabled;

		public override void Initialize(SerializedObject settings, string propertyName)
		{
			base.Initialize(settings, "hololens");
			this.m_DepthFormat = base.FindPropertyAssert("depthFormat");
			this.m_DepthBufferSharingEnabled = base.FindPropertyAssert("depthBufferSharingEnabled");
		}

		public override Rect Draw(Rect rect)
		{
			rect.y += EditorGUIUtility.standardVerticalSpacing;
			rect.height = EditorGUIUtility.singleLineHeight;
			GUIContent label = EditorGUI.BeginProperty(rect, VRCustomOptionsWindowsMR.s_DepthFormatLabel, this.m_DepthFormat);
			EditorGUI.BeginChangeCheck();
			int intValue = EditorGUI.Popup(rect, label, this.m_DepthFormat.intValue, VRCustomOptionsWindowsMR.s_DepthOptions);
			if (EditorGUI.EndChangeCheck())
			{
				this.m_DepthFormat.intValue = intValue;
			}
			EditorGUI.EndProperty();
			rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
			label = EditorGUI.BeginProperty(rect, VRCustomOptionsWindowsMR.s_DepthBufferSharingLabel, this.m_DepthBufferSharingEnabled);
			EditorGUI.BeginChangeCheck();
			bool boolValue = EditorGUI.Toggle(rect, VRCustomOptionsWindowsMR.s_DepthBufferSharingLabel, this.m_DepthBufferSharingEnabled.boolValue);
			if (EditorGUI.EndChangeCheck())
			{
				this.m_DepthBufferSharingEnabled.boolValue = boolValue;
			}
			EditorGUI.EndProperty();
			rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
			return rect;
		}

		public override float GetHeight()
		{
			return EditorGUIUtility.singleLineHeight * 2f;
		}
	}
}
