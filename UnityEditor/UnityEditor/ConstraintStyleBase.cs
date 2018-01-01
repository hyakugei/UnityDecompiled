using System;
using UnityEngine;

namespace UnityEditor
{
	internal abstract class ConstraintStyleBase
	{
		private GUIContent m_Activate = EditorGUIUtility.TextContent("Activate|Activate the constraint at the current offset from the sources.");

		private GUIContent m_Zero = EditorGUIUtility.TextContent("Zero|Activate the constraint at zero offset from the sources.");

		private GUIContent m_Sources = EditorGUIUtility.TextContent("Sources");

		private GUIContent m_Weight = EditorGUIUtility.TextContent("Weight");

		private GUIContent m_IsActive = EditorGUIUtility.TextContent("Is Active|When set, the constraint is being evaluated.");

		private GUIContent m_IsLocked = EditorGUIUtility.TextContent("Lock|When set, evaluate with the current offset. When not set, update the offset based on the current transform.");

		private GUIContent[] m_Axes = new GUIContent[]
		{
			EditorGUIUtility.TextContent("X"),
			EditorGUIUtility.TextContent("Y"),
			EditorGUIUtility.TextContent("Z")
		};

		private GUIContent m_ConstraintSettings = EditorGUIUtility.TextContent("Constraint Settings");

		public virtual GUIContent Activate
		{
			get
			{
				return this.m_Activate;
			}
		}

		public virtual GUIContent Zero
		{
			get
			{
				return this.m_Zero;
			}
		}

		public abstract GUIContent AtRest
		{
			get;
		}

		public abstract GUIContent Offset
		{
			get;
		}

		public virtual GUIContent Sources
		{
			get
			{
				return this.m_Sources;
			}
		}

		public virtual GUIContent Weight
		{
			get
			{
				return this.m_Weight;
			}
		}

		public virtual GUIContent IsActive
		{
			get
			{
				return this.m_IsActive;
			}
		}

		public virtual GUIContent IsLocked
		{
			get
			{
				return this.m_IsLocked;
			}
		}

		public virtual GUIContent[] Axes
		{
			get
			{
				return this.m_Axes;
			}
		}

		public virtual GUIContent ConstraintSettings
		{
			get
			{
				return this.m_ConstraintSettings;
			}
		}
	}
}
