using System;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal.VR
{
	internal class VRCustomOptionsGoogleVR : VRCustomOptions
	{
		private static GUIContent[] s_DepthOptions = new GUIContent[]
		{
			EditorGUIUtility.TrTextContent("16-bit depth", null, null),
			EditorGUIUtility.TrTextContent("24-bit depth", null, null),
			EditorGUIUtility.TrTextContent("24-bit depth | 8-bit stencil", null, null)
		};

		private static GUIContent s_DepthFormatLabel = EditorGUIUtility.TrTextContent("Depth Format", null, null);

		private SerializedProperty m_DepthFormat;

		public override void Initialize(SerializedObject settings, string propertyName)
		{
			base.Initialize(settings, propertyName);
			this.m_DepthFormat = base.FindPropertyAssert("depthFormat");
		}

		public override Rect Draw(Rect rect)
		{
			rect.y += EditorGUIUtility.standardVerticalSpacing;
			rect.height = EditorGUIUtility.singleLineHeight;
			GUIContent label = EditorGUI.BeginProperty(rect, VRCustomOptionsGoogleVR.s_DepthFormatLabel, this.m_DepthFormat);
			EditorGUI.BeginChangeCheck();
			int intValue = EditorGUI.Popup(rect, label, this.m_DepthFormat.intValue, VRCustomOptionsGoogleVR.s_DepthOptions);
			if (EditorGUI.EndChangeCheck())
			{
				this.m_DepthFormat.intValue = intValue;
			}
			EditorGUI.EndProperty();
			rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
			return rect;
		}

		public override float GetHeight()
		{
			return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2f;
		}
	}
}
