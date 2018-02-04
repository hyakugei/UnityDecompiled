using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEditor
{
	internal abstract class ModuleUI : SerializedModule
	{
		public enum VisibilityState
		{
			NotVisible,
			VisibleAndFolded,
			VisibleAndFoldedOut
		}

		public class PropertyGroupScope : GUI.Scope
		{
			private bool wasBoldFont;

			public PropertyGroupScope(params SerializedProperty[] properties)
			{
				this.wasBoldFont = EditorGUIUtility.GetBoldDefaultFont();
				bool boldDefaultFont = false;
				for (int i = 0; i < properties.Length; i++)
				{
					SerializedProperty serializedProperty = properties[i];
					if (serializedProperty.serializedObject.targetObjects.Length == 1 && serializedProperty.isInstantiatedPrefab)
					{
						if (serializedProperty.prefabOverride)
						{
							boldDefaultFont = true;
							break;
						}
					}
				}
				EditorGUIUtility.SetBoldDefaultFont(boldDefaultFont);
			}

			protected override void CloseScope()
			{
				EditorGUIUtility.SetBoldDefaultFont(this.wasBoldFont);
			}
		}

		public delegate bool CurveFieldMouseDownCallback(int button, Rect drawRect, Rect curveRanges);

		private class CurveStateCallbackData
		{
			public SerializedMinMaxCurve[] minMaxCurves;

			public MinMaxCurveState selectedState;

			public CurveStateCallbackData(MinMaxCurveState state, SerializedMinMaxCurve[] curves)
			{
				this.minMaxCurves = curves;
				this.selectedState = state;
			}
		}

		private class GradientCallbackData
		{
			public SerializedMinMaxGradient gradientProp;

			public MinMaxGradientState selectedState;

			public GradientCallbackData(MinMaxGradientState state, SerializedMinMaxGradient p)
			{
				this.gradientProp = p;
				this.selectedState = state;
			}
		}

		private class ColorCallbackData
		{
			public SerializedProperty boolProp;

			public bool selectedState;

			public ColorCallbackData(bool state, SerializedProperty bp)
			{
				this.boolProp = bp;
				this.selectedState = state;
			}
		}

		public ParticleSystemUI m_ParticleSystemUI;

		private string m_DisplayName;

		protected string m_ToolTip = "";

		private SerializedProperty m_Enabled;

		private ModuleUI.VisibilityState m_VisibilityState;

		public List<SerializedProperty> m_ModuleCurves = new List<SerializedProperty>();

		private List<SerializedProperty> m_CurvesRemovedWhenFolded = new List<SerializedProperty>();

		protected static readonly bool kUseSignedRange = true;

		protected static readonly Rect kUnsignedRange = new Rect(0f, 0f, 1f, 1f);

		protected static readonly Rect kSignedRange = new Rect(0f, -1f, 1f, 2f);

		protected const int kSingleLineHeight = 13;

		protected const float k_minMaxToggleWidth = 13f;

		protected const float k_toggleWidth = 9f;

		protected const float kDragSpace = 20f;

		protected const int kPlusAddRemoveButtonWidth = 12;

		protected const int kPlusAddRemoveButtonSpacing = 5;

		protected const int kSpacingSubLabel = 4;

		protected const int kSubLabelWidth = 10;

		protected const string kFormatString = "g7";

		protected const float kReorderableListElementHeight = 16f;

		public static float k_CompactFixedModuleWidth = 295f;

		public static float k_SpaceBetweenModules = 5f;

		public static readonly GUIStyle s_ControlRectStyle = new GUIStyle
		{
			margin = new RectOffset(0, 0, 2, 2)
		};

		[CompilerGenerated]
		private static GenericMenu.MenuFunction2 <>f__mg$cache0;

		[CompilerGenerated]
		private static GenericMenu.MenuFunction2 <>f__mg$cache1;

		[CompilerGenerated]
		private static GenericMenu.MenuFunction2 <>f__mg$cache2;

		public bool visibleUI
		{
			get
			{
				return this.m_VisibilityState != ModuleUI.VisibilityState.NotVisible;
			}
			set
			{
				this.SetVisibilityState((!value) ? ModuleUI.VisibilityState.NotVisible : ModuleUI.VisibilityState.VisibleAndFolded);
			}
		}

		public bool foldout
		{
			get
			{
				return this.m_VisibilityState == ModuleUI.VisibilityState.VisibleAndFoldedOut;
			}
			set
			{
				this.SetVisibilityState((!value) ? ModuleUI.VisibilityState.VisibleAndFolded : ModuleUI.VisibilityState.VisibleAndFoldedOut);
			}
		}

		public bool enabled
		{
			get
			{
				return this.m_Enabled.boolValue;
			}
			set
			{
				if (this.m_Enabled.boolValue != value)
				{
					this.m_Enabled.boolValue = value;
					if (value)
					{
						this.OnModuleEnable();
					}
					else
					{
						this.OnModuleDisable();
					}
				}
			}
		}

		public bool enabledHasMultipleDifferentValues
		{
			get
			{
				return this.m_Enabled.hasMultipleDifferentValues;
			}
		}

		public string displayName
		{
			get
			{
				return this.m_DisplayName;
			}
		}

		public string toolTip
		{
			get
			{
				return this.m_ToolTip;
			}
		}

		public bool isWindowView
		{
			get
			{
				return this.m_ParticleSystemUI.m_ParticleEffectUI.m_Owner is ParticleSystemWindow;
			}
		}

		public ModuleUI(ParticleSystemUI owner, SerializedObject o, string name, string displayName) : base(o, name)
		{
			this.Setup(owner, o, displayName, ModuleUI.VisibilityState.NotVisible);
		}

		public ModuleUI(ParticleSystemUI owner, SerializedObject o, string name, string displayName, ModuleUI.VisibilityState initialVisibilityState) : base(o, name)
		{
			this.Setup(owner, o, displayName, initialVisibilityState);
		}

		private void Setup(ParticleSystemUI owner, SerializedObject o, string displayName, ModuleUI.VisibilityState defaultVisibilityState)
		{
			this.m_ParticleSystemUI = owner;
			this.m_DisplayName = displayName;
			if (this is RendererModuleUI)
			{
				this.m_Enabled = base.GetProperty0("m_Enabled");
			}
			else
			{
				this.m_Enabled = base.GetProperty("enabled");
			}
			this.m_VisibilityState = ModuleUI.VisibilityState.NotVisible;
			UnityEngine.Object[] targetObjects = o.targetObjects;
			for (int i = 0; i < targetObjects.Length; i++)
			{
				UnityEngine.Object o2 = targetObjects[i];
				ModuleUI.VisibilityState @int = (ModuleUI.VisibilityState)SessionState.GetInt(base.GetUniqueModuleName(o2), (int)defaultVisibilityState);
				this.m_VisibilityState = (ModuleUI.VisibilityState)Mathf.Max((int)@int, (int)this.m_VisibilityState);
			}
			this.CheckVisibilityState();
			if (this.foldout)
			{
				this.Init();
			}
		}

		protected abstract void Init();

		public virtual void Validate()
		{
		}

		public virtual float GetXAxisScalar()
		{
			return 1f;
		}

		public abstract void OnInspectorGUI(InitialModuleUI initial);

		public virtual void OnSceneViewGUI()
		{
		}

		public virtual void UpdateCullingSupportedString(ref string text)
		{
		}

		protected virtual void OnModuleEnable()
		{
			Undo.undoRedoPerformed = (Undo.UndoRedoCallback)Delegate.Combine(Undo.undoRedoPerformed, new Undo.UndoRedoCallback(this.UndoRedoPerformed));
			this.Init();
		}

		public virtual void UndoRedoPerformed()
		{
			if (!this.enabled)
			{
				this.OnModuleDisable();
			}
		}

		protected virtual void OnModuleDisable()
		{
			Undo.undoRedoPerformed = (Undo.UndoRedoCallback)Delegate.Remove(Undo.undoRedoPerformed, new Undo.UndoRedoCallback(this.UndoRedoPerformed));
			ParticleSystemCurveEditor particleSystemCurveEditor = this.m_ParticleSystemUI.m_ParticleEffectUI.GetParticleSystemCurveEditor();
			foreach (SerializedProperty current in this.m_ModuleCurves)
			{
				if (particleSystemCurveEditor.IsAdded(current))
				{
					particleSystemCurveEditor.RemoveCurve(current);
				}
			}
		}

		internal void CheckVisibilityState()
		{
			if (!(this is RendererModuleUI) && !this.m_Enabled.boolValue && !ParticleEffectUI.GetAllModulesVisible())
			{
				this.SetVisibilityState(ModuleUI.VisibilityState.NotVisible);
			}
			if (this.m_Enabled.boolValue && !this.visibleUI)
			{
				this.SetVisibilityState(ModuleUI.VisibilityState.VisibleAndFolded);
			}
		}

		protected virtual void SetVisibilityState(ModuleUI.VisibilityState newState)
		{
			if (newState != this.m_VisibilityState)
			{
				if (newState == ModuleUI.VisibilityState.VisibleAndFolded)
				{
					ParticleSystemCurveEditor particleSystemCurveEditor = this.m_ParticleSystemUI.m_ParticleEffectUI.GetParticleSystemCurveEditor();
					foreach (SerializedProperty current in this.m_ModuleCurves)
					{
						if (particleSystemCurveEditor.IsAdded(current))
						{
							this.m_CurvesRemovedWhenFolded.Add(current);
							particleSystemCurveEditor.SetVisible(current, false);
						}
					}
					particleSystemCurveEditor.Refresh();
				}
				else if (newState == ModuleUI.VisibilityState.VisibleAndFoldedOut)
				{
					ParticleSystemCurveEditor particleSystemCurveEditor2 = this.m_ParticleSystemUI.m_ParticleEffectUI.GetParticleSystemCurveEditor();
					foreach (SerializedProperty current2 in this.m_CurvesRemovedWhenFolded)
					{
						particleSystemCurveEditor2.SetVisible(current2, true);
					}
					this.m_CurvesRemovedWhenFolded.Clear();
					particleSystemCurveEditor2.Refresh();
				}
				this.m_VisibilityState = newState;
				UnityEngine.Object[] targetObjects = base.serializedObject.targetObjects;
				for (int i = 0; i < targetObjects.Length; i++)
				{
					UnityEngine.Object o = targetObjects[i];
					SessionState.SetInt(base.GetUniqueModuleName(o), (int)this.m_VisibilityState);
				}
				if (newState == ModuleUI.VisibilityState.VisibleAndFoldedOut)
				{
					this.Init();
				}
			}
		}

		protected ParticleSystem GetParticleSystem()
		{
			return this.m_Enabled.serializedObject.targetObject as ParticleSystem;
		}

		public ParticleSystemCurveEditor GetParticleSystemCurveEditor()
		{
			return this.m_ParticleSystemUI.m_ParticleEffectUI.GetParticleSystemCurveEditor();
		}

		public virtual bool DrawHeader(Rect rect, GUIContent label)
		{
			label = EditorGUI.BeginProperty(rect, label, this.m_ModuleRootProperty);
			bool result = GUI.Toggle(rect, this.foldout, label, ParticleSystemStyles.Get().moduleHeaderStyle);
			EditorGUI.EndProperty();
			return result;
		}

		public void AddToModuleCurves(SerializedProperty curveProp)
		{
			this.m_ModuleCurves.Add(curveProp);
			if (!this.foldout)
			{
				this.m_CurvesRemovedWhenFolded.Add(curveProp);
			}
		}

		private static void Label(Rect rect, GUIContent guiContent)
		{
			GUI.Label(rect, guiContent, ParticleSystemStyles.Get().label);
		}

		protected static Rect GetControlRect(int height, params GUILayoutOption[] layoutOptions)
		{
			return GUILayoutUtility.GetRect(0f, (float)height, ModuleUI.s_ControlRectStyle, layoutOptions);
		}

		protected static Rect FieldPosition(Rect totalPosition, out Rect labelPosition)
		{
			labelPosition = new Rect(totalPosition.x + EditorGUI.indent, totalPosition.y, EditorGUIUtility.labelWidth - EditorGUI.indent, 13f);
			return new Rect(totalPosition.x + EditorGUIUtility.labelWidth, totalPosition.y, totalPosition.width - EditorGUIUtility.labelWidth, totalPosition.height);
		}

		internal static Rect PrefixLabel(Rect totalPosition, GUIContent label)
		{
			Rect result;
			if (!EditorGUI.LabelHasContent(label))
			{
				result = EditorGUI.IndentedRect(totalPosition);
			}
			else
			{
				Rect labelPosition;
				Rect rect = ModuleUI.FieldPosition(totalPosition, out labelPosition);
				EditorGUI.HandlePrefixLabel(totalPosition, labelPosition, label, 0, ParticleSystemStyles.Get().label);
				result = rect;
			}
			return result;
		}

		protected static Rect SubtractPopupWidth(Rect position)
		{
			position.width -= 14f;
			return position;
		}

		protected static Rect GetPopupRect(Rect position)
		{
			position.xMin = position.xMax - 13f;
			return position;
		}

		protected static bool PlusButton(Rect position)
		{
			return GUI.Button(new Rect(position.x - 2f, position.y - 2f, 12f, 13f), GUIContent.none, "OL Plus");
		}

		protected static bool MinusButton(Rect position)
		{
			return GUI.Button(new Rect(position.x - 2f, position.y - 2f, 12f, 13f), GUIContent.none, "OL Minus");
		}

		private static float FloatDraggable(Rect rect, SerializedProperty floatProp, float remap, float dragWidth)
		{
			return ModuleUI.FloatDraggable(rect, floatProp, remap, dragWidth, "g7");
		}

		public static float FloatDraggable(Rect rect, float floatValue, float remap, float dragWidth, string formatString)
		{
			int controlID = GUIUtility.GetControlID(1658656233, FocusType.Keyboard, rect);
			Rect dragHotZone = rect;
			dragHotZone.width = dragWidth;
			Rect position = rect;
			position.xMin += dragWidth;
			return EditorGUI.DoFloatField(EditorGUI.s_RecycledEditor, position, dragHotZone, controlID, floatValue * remap, formatString, ParticleSystemStyles.Get().numberField, true) / remap;
		}

		public static float FloatDraggable(Rect rect, SerializedProperty floatProp, float remap, float dragWidth, string formatString)
		{
			EditorGUI.BeginProperty(rect, GUIContent.none, floatProp);
			EditorGUI.BeginChangeCheck();
			float num = ModuleUI.FloatDraggable(rect, floatProp.floatValue, remap, dragWidth, formatString);
			if (EditorGUI.EndChangeCheck())
			{
				floatProp.floatValue = num;
			}
			EditorGUI.EndProperty();
			return num;
		}

		public static Vector3 GUIVector3Field(GUIContent guiContent, SerializedProperty vecProp, params GUILayoutOption[] layoutOptions)
		{
			Rect rect = ModuleUI.GetControlRect(13, layoutOptions);
			guiContent = EditorGUI.BeginProperty(rect, guiContent, vecProp);
			rect = ModuleUI.PrefixLabel(rect, guiContent);
			GUIContent[] array = new GUIContent[]
			{
				new GUIContent("X"),
				new GUIContent("Y"),
				new GUIContent("Z")
			};
			float num = (rect.width - 8f) / 3f;
			rect.width = num;
			SerializedProperty serializedProperty = vecProp.Copy();
			serializedProperty.Next(true);
			Vector3 vector3Value = vecProp.vector3Value;
			for (int i = 0; i < 3; i++)
			{
				EditorGUI.BeginProperty(rect, GUIContent.none, serializedProperty);
				ModuleUI.Label(rect, array[i]);
				EditorGUI.BeginChangeCheck();
				float floatValue = ModuleUI.FloatDraggable(rect, serializedProperty.floatValue, 1f, 25f, "g5");
				if (EditorGUI.EndChangeCheck())
				{
					serializedProperty.floatValue = floatValue;
				}
				EditorGUI.EndProperty();
				serializedProperty.Next(false);
				rect.x += num + 4f;
			}
			EditorGUI.EndProperty();
			return vector3Value;
		}

		public static float GUIFloat(string label, SerializedProperty floatProp, params GUILayoutOption[] layoutOptions)
		{
			return ModuleUI.GUIFloat(GUIContent.Temp(label), floatProp, layoutOptions);
		}

		public static float GUIFloat(GUIContent guiContent, SerializedProperty floatProp, params GUILayoutOption[] layoutOptions)
		{
			return ModuleUI.GUIFloat(guiContent, floatProp, "g7", layoutOptions);
		}

		public static float GUIFloat(GUIContent guiContent, SerializedProperty floatProp, string formatString, params GUILayoutOption[] layoutOptions)
		{
			Rect controlRect = ModuleUI.GetControlRect(13, layoutOptions);
			guiContent = EditorGUI.BeginProperty(controlRect, guiContent, floatProp);
			ModuleUI.PrefixLabel(controlRect, guiContent);
			float result = ModuleUI.FloatDraggable(controlRect, floatProp, 1f, EditorGUIUtility.labelWidth, formatString);
			EditorGUI.EndProperty();
			return result;
		}

		public static float GUIFloat(GUIContent guiContent, float floatValue, string formatString, params GUILayoutOption[] layoutOptions)
		{
			Rect controlRect = ModuleUI.GetControlRect(13, layoutOptions);
			ModuleUI.PrefixLabel(controlRect, guiContent);
			return ModuleUI.FloatDraggable(controlRect, floatValue, 1f, EditorGUIUtility.labelWidth, formatString);
		}

		public static void GUIButtonGroup(EditMode.SceneViewEditMode[] modes, GUIContent[] guiContents, Func<Bounds> getBoundsOfTargets, Editor caller)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(EditorGUIUtility.labelWidth);
			EditMode.DoInspectorToolbar(modes, guiContents, getBoundsOfTargets, caller);
			GUILayout.EndHorizontal();
		}

		public static void GUISortingLayerField(GUIContent guiContent, SerializedProperty sortProperty, params GUILayoutOption[] layoutOptions)
		{
			Rect controlRect = ModuleUI.GetControlRect(13, layoutOptions);
			GUIContent label = EditorGUI.BeginProperty(controlRect, guiContent, sortProperty);
			EditorGUI.SortingLayerField(controlRect, label, sortProperty, ParticleSystemStyles.Get().popup, ParticleSystemStyles.Get().label);
			EditorGUI.EndProperty();
		}

		private static bool Toggle(Rect rect, SerializedProperty boolProp)
		{
			EditorGUIInternal.mixedToggleStyle = ParticleSystemStyles.Get().toggleMixed;
			EditorGUI.BeginProperty(rect, GUIContent.none, boolProp);
			EditorGUI.BeginChangeCheck();
			bool flag = EditorGUI.Toggle(rect, boolProp.boolValue, ParticleSystemStyles.Get().toggle);
			if (EditorGUI.EndChangeCheck())
			{
				boolProp.boolValue = flag;
			}
			EditorGUI.EndProperty();
			EditorGUIInternal.mixedToggleStyle = EditorStyles.toggleMixed;
			return flag;
		}

		public static bool GUIToggle(string label, SerializedProperty boolProp, params GUILayoutOption[] layoutOptions)
		{
			return ModuleUI.GUIToggle(GUIContent.Temp(label), boolProp, layoutOptions);
		}

		public static bool GUIToggle(GUIContent guiContent, SerializedProperty boolProp, params GUILayoutOption[] layoutOptions)
		{
			Rect rect = ModuleUI.GetControlRect(13, layoutOptions);
			guiContent = EditorGUI.BeginProperty(rect, guiContent, boolProp);
			rect = ModuleUI.PrefixLabel(rect, guiContent);
			bool result = ModuleUI.Toggle(rect, boolProp);
			EditorGUI.EndProperty();
			return result;
		}

		public static void GUILayerMask(GUIContent guiContent, SerializedProperty boolProp, params GUILayoutOption[] layoutOptions)
		{
			Rect rect = ModuleUI.GetControlRect(13, layoutOptions);
			guiContent = EditorGUI.BeginProperty(rect, guiContent, boolProp);
			rect = ModuleUI.PrefixLabel(rect, guiContent);
			EditorGUI.LayerMaskField(rect, boolProp, null, ParticleSystemStyles.Get().popup);
			EditorGUI.EndProperty();
		}

		public static bool GUIToggle(GUIContent guiContent, bool boolValue, params GUILayoutOption[] layoutOptions)
		{
			Rect rect = ModuleUI.GetControlRect(13, layoutOptions);
			rect = ModuleUI.PrefixLabel(rect, guiContent);
			boolValue = EditorGUI.Toggle(rect, boolValue, ParticleSystemStyles.Get().toggle);
			return boolValue;
		}

		public static void GUIToggleWithFloatField(string name, SerializedProperty boolProp, SerializedProperty floatProp, bool invertToggle, params GUILayoutOption[] layoutOptions)
		{
			ModuleUI.GUIToggleWithFloatField(EditorGUIUtility.TempContent(name), boolProp, floatProp, invertToggle, layoutOptions);
		}

		public static void GUIToggleWithFloatField(GUIContent guiContent, SerializedProperty boolProp, SerializedProperty floatProp, bool invertToggle, params GUILayoutOption[] layoutOptions)
		{
			Rect rect = GUILayoutUtility.GetRect(0f, 13f, layoutOptions);
			guiContent = EditorGUI.BeginProperty(rect, guiContent, boolProp);
			rect = ModuleUI.PrefixLabel(rect, guiContent);
			Rect rect2 = rect;
			rect2.xMax = rect2.x + 9f;
			bool flag = ModuleUI.Toggle(rect2, boolProp);
			flag = ((!invertToggle) ? flag : (!flag));
			if (flag)
			{
				float dragWidth = 25f;
				Rect rect3 = new Rect(rect.x + EditorGUIUtility.labelWidth + 9f, rect.y, rect.width - 9f, rect.height);
				ModuleUI.FloatDraggable(rect3, floatProp, 1f, dragWidth);
			}
			EditorGUI.EndProperty();
		}

		public static void GUIToggleWithIntField(string name, SerializedProperty boolProp, SerializedProperty floatProp, bool invertToggle, params GUILayoutOption[] layoutOptions)
		{
			ModuleUI.GUIToggleWithIntField(EditorGUIUtility.TempContent(name), boolProp, floatProp, invertToggle, layoutOptions);
		}

		public static void GUIToggleWithIntField(GUIContent guiContent, SerializedProperty boolProp, SerializedProperty intProp, bool invertToggle, params GUILayoutOption[] layoutOptions)
		{
			Rect controlRect = ModuleUI.GetControlRect(13, layoutOptions);
			guiContent = EditorGUI.BeginProperty(controlRect, guiContent, boolProp);
			Rect rect = ModuleUI.PrefixLabel(controlRect, guiContent);
			Rect rect2 = rect;
			rect2.xMax = rect2.x + 9f;
			bool flag = ModuleUI.Toggle(rect2, boolProp);
			flag = ((!invertToggle) ? flag : (!flag));
			if (flag)
			{
				float dragWidth = 25f;
				Rect rect3 = new Rect(rect2.xMax, controlRect.y, controlRect.width - rect2.xMax + 9f, controlRect.height);
				EditorGUI.BeginChangeCheck();
				int intValue = ModuleUI.IntDraggable(rect3, null, intProp.intValue, dragWidth);
				if (EditorGUI.EndChangeCheck())
				{
					intProp.intValue = intValue;
				}
			}
			EditorGUI.EndProperty();
		}

		public static void GUIObject(GUIContent label, SerializedProperty objectProp, params GUILayoutOption[] layoutOptions)
		{
			ModuleUI.GUIObject(label, objectProp, null, layoutOptions);
		}

		public static void GUIObject(GUIContent label, SerializedProperty objectProp, Type objType, params GUILayoutOption[] layoutOptions)
		{
			Rect rect = ModuleUI.GetControlRect(13, layoutOptions);
			label = EditorGUI.BeginProperty(rect, label, objectProp);
			rect = ModuleUI.PrefixLabel(rect, label);
			EditorGUI.ObjectField(rect, objectProp, objType, GUIContent.none, ParticleSystemStyles.Get().objectField);
			EditorGUI.EndProperty();
		}

		public static void GUIObjectFieldAndToggle(GUIContent label, SerializedProperty objectProp, SerializedProperty boolProp, params GUILayoutOption[] layoutOptions)
		{
			Rect rect = ModuleUI.GetControlRect(13, layoutOptions);
			label = EditorGUI.BeginProperty(rect, label, objectProp);
			rect = ModuleUI.PrefixLabel(rect, label);
			rect.xMax -= 19f;
			EditorGUI.ObjectField(rect, objectProp, GUIContent.none);
			if (boolProp != null)
			{
				rect.x += rect.width + 10f;
				rect.width = 9f;
				ModuleUI.Toggle(rect, boolProp);
			}
			EditorGUI.EndProperty();
		}

		internal UnityEngine.Object ParticleSystemValidator(UnityEngine.Object[] references, Type objType, SerializedProperty property)
		{
			UnityEngine.Object result;
			for (int i = 0; i < references.Length; i++)
			{
				UnityEngine.Object @object = references[i];
				if (@object != null)
				{
					GameObject gameObject = @object as GameObject;
					if (gameObject != null)
					{
						ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
						if (component)
						{
							result = component;
							return result;
						}
					}
				}
			}
			result = null;
			return result;
		}

		public int GUIListOfFloatObjectToggleFields(GUIContent label, SerializedProperty[] objectProps, EditorGUI.ObjectFieldValidator validator, GUIContent buttonTooltip, bool allowCreation, params GUILayoutOption[] layoutOptions)
		{
			int result = -1;
			int num = objectProps.Length;
			Rect rect = GUILayoutUtility.GetRect(0f, (float)(15 * num), layoutOptions);
			rect.height = 13f;
			float num2 = 10f;
			float num3 = 35f;
			float num4 = 10f;
			float width = rect.width - num2 - num3 - num4 * 2f - 9f;
			using (new ModuleUI.PropertyGroupScope(objectProps))
			{
				ModuleUI.PrefixLabel(rect, label);
				for (int i = 0; i < num; i++)
				{
					SerializedProperty serializedProperty = objectProps[i];
					Rect rect2 = new Rect(rect.x + num2 + num3 + num4, rect.y, width, rect.height);
					int controlID = GUIUtility.GetControlID(1235498, FocusType.Keyboard, rect2);
					EditorGUI.BeginProperty(rect2, GUIContent.none, serializedProperty);
					EditorGUI.DoObjectField(rect2, rect2, controlID, null, null, serializedProperty, validator, true, ParticleSystemStyles.Get().objectField);
					EditorGUI.EndProperty();
					if (serializedProperty.objectReferenceValue == null)
					{
						rect2 = new Rect(rect.xMax - 9f, rect.y + 3f, 9f, 9f);
						if (!allowCreation || GUI.Button(rect2, buttonTooltip ?? GUIContent.none, ParticleSystemStyles.Get().plus))
						{
							result = i;
						}
					}
					rect.y += 15f;
				}
			}
			return result;
		}

		public static void GUIIntDraggableX2(GUIContent mainLabel, GUIContent label1, SerializedProperty intProp1, GUIContent label2, SerializedProperty intProp2, params GUILayoutOption[] layoutOptions)
		{
			Rect totalPosition = ModuleUI.GetControlRect(13, layoutOptions);
			using (new ModuleUI.PropertyGroupScope(new SerializedProperty[]
			{
				intProp1,
				intProp2
			}))
			{
				totalPosition = ModuleUI.PrefixLabel(totalPosition, mainLabel);
			}
			float num = (totalPosition.width - 4f) * 0.5f;
			Rect rect = new Rect(totalPosition.x, totalPosition.y, num, totalPosition.height);
			ModuleUI.IntDraggable(rect, label1, intProp1, 10f);
			rect.x += num + 4f;
			ModuleUI.IntDraggable(rect, label2, intProp2, 10f);
		}

		public static int IntDraggable(Rect rect, GUIContent label, SerializedProperty intProp, float dragWidth)
		{
			EditorGUI.BeginProperty(rect, GUIContent.none, intProp);
			EditorGUI.BeginChangeCheck();
			int intValue = ModuleUI.IntDraggable(rect, label, intProp.intValue, dragWidth);
			if (EditorGUI.EndChangeCheck())
			{
				intProp.intValue = intValue;
			}
			EditorGUI.EndProperty();
			return intProp.intValue;
		}

		public static int GUIInt(GUIContent guiContent, SerializedProperty intProp, params GUILayoutOption[] layoutOptions)
		{
			Rect rect = GUILayoutUtility.GetRect(0f, 13f, layoutOptions);
			EditorGUI.BeginProperty(rect, GUIContent.none, intProp);
			ModuleUI.PrefixLabel(rect, guiContent);
			EditorGUI.BeginChangeCheck();
			int intValue = ModuleUI.IntDraggable(rect, null, intProp.intValue, EditorGUIUtility.labelWidth);
			if (EditorGUI.EndChangeCheck())
			{
				intProp.intValue = intValue;
			}
			EditorGUI.EndProperty();
			return intProp.intValue;
		}

		public static int IntDraggable(Rect rect, GUIContent label, int value, float dragWidth)
		{
			float width = rect.width;
			Rect rect2 = rect;
			rect2.width = width;
			int controlID = GUIUtility.GetControlID(16586232, FocusType.Keyboard, rect2);
			Rect rect3 = rect2;
			rect3.width = dragWidth;
			if (label != null && !string.IsNullOrEmpty(label.text))
			{
				ModuleUI.Label(rect3, label);
			}
			Rect position = rect2;
			position.x += dragWidth;
			position.width = width - dragWidth;
			float dragSensitivity = Mathf.Max(1f, Mathf.Pow(Mathf.Abs((float)value), 0.5f) * 0.03f);
			return (int)EditorGUI.DoFloatField(EditorGUI.s_RecycledEditor, position, rect3, controlID, (float)value, EditorGUI.kIntFieldFormatString, ParticleSystemStyles.Get().numberField, true, dragSensitivity);
		}

		public static void GUIMinMaxRange(GUIContent label, SerializedProperty vec2Prop, params GUILayoutOption[] layoutOptions)
		{
			Rect rect = ModuleUI.GetControlRect(13, layoutOptions);
			label = EditorGUI.BeginProperty(rect, label, vec2Prop);
			rect = ModuleUI.SubtractPopupWidth(rect);
			rect = ModuleUI.PrefixLabel(rect, label);
			float num = (rect.width - 20f) * 0.5f;
			SerializedProperty serializedProperty = vec2Prop.Copy();
			serializedProperty.Next(true);
			EditorGUI.BeginChangeCheck();
			rect.width = num;
			rect.xMin -= 20f;
			ModuleUI.FloatDraggable(rect, serializedProperty, 1f, 20f, "g7");
			serializedProperty.Next(true);
			rect.x += num + 20f;
			ModuleUI.FloatDraggable(rect, serializedProperty, 1f, 20f, "g7");
			serializedProperty.Next(true);
			EditorGUI.EndProperty();
		}

		public static bool GUIBoolAsPopup(GUIContent label, SerializedProperty boolProp, string[] options, params GUILayoutOption[] layoutOptions)
		{
			Rect rect = ModuleUI.GetControlRect(13, layoutOptions);
			label = EditorGUI.BeginProperty(rect, label, boolProp);
			rect = ModuleUI.PrefixLabel(rect, label);
			EditorGUI.BeginChangeCheck();
			int num = EditorGUI.Popup(rect, null, (!boolProp.boolValue) ? 0 : 1, EditorGUIUtility.TempContent(options), ParticleSystemStyles.Get().popup);
			if (EditorGUI.EndChangeCheck())
			{
				boolProp.boolValue = (num > 0);
			}
			EditorGUI.EndProperty();
			return num > 0;
		}

		public static void GUIEnumMaskUVChannelFlags(GUIContent label, SerializedProperty enumProperty, params GUILayoutOption[] layoutOptions)
		{
			Rect rect = ModuleUI.GetControlRect(13, layoutOptions);
			label = EditorGUI.BeginProperty(rect, label, enumProperty);
			rect = ModuleUI.PrefixLabel(rect, label);
			EditorGUI.BeginChangeCheck();
			int intValue = (int)((UVChannelFlags)EditorGUI.EnumFlagsField(rect, (UVChannelFlags)enumProperty.intValue, ParticleSystemStyles.Get().popup));
			if (EditorGUI.EndChangeCheck())
			{
				enumProperty.intValue = intValue;
			}
			EditorGUI.EndProperty();
		}

		public static void GUIMask(GUIContent label, SerializedProperty intProp, string[] options, params GUILayoutOption[] layoutOptions)
		{
			Rect rect = ModuleUI.GetControlRect(13, layoutOptions);
			label = EditorGUI.BeginProperty(rect, label, intProp);
			rect = ModuleUI.PrefixLabel(rect, label);
			EditorGUI.BeginChangeCheck();
			int intValue = EditorGUI.MaskField(rect, label, intProp.intValue, options, ParticleSystemStyles.Get().popup);
			if (EditorGUI.EndChangeCheck())
			{
				intProp.intValue = intValue;
			}
			EditorGUI.EndProperty();
		}

		public static int GUIPopup(GUIContent label, SerializedProperty intProp, GUIContent[] options, params GUILayoutOption[] layoutOptions)
		{
			Rect rect = ModuleUI.GetControlRect(13, layoutOptions);
			label = EditorGUI.BeginProperty(rect, label, intProp);
			rect = ModuleUI.PrefixLabel(rect, label);
			EditorGUI.BeginChangeCheck();
			int intValue = EditorGUI.Popup(rect, null, intProp.intValue, options, ParticleSystemStyles.Get().popup);
			if (EditorGUI.EndChangeCheck())
			{
				intProp.intValue = intValue;
			}
			EditorGUI.EndProperty();
			return intProp.intValue;
		}

		public static int GUIPopup(GUIContent label, int intValue, GUIContent[] options, params GUILayoutOption[] layoutOptions)
		{
			Rect rect = ModuleUI.GetControlRect(13, layoutOptions);
			rect = ModuleUI.PrefixLabel(rect, label);
			return EditorGUI.Popup(rect, intValue, options, ParticleSystemStyles.Get().popup);
		}

		public static int GUIPopup(GUIContent label, int intValue, GUIContent[] options, SerializedProperty property, params GUILayoutOption[] layoutOptions)
		{
			Rect rect = ModuleUI.GetControlRect(13, layoutOptions);
			label = EditorGUI.BeginProperty(rect, label, property);
			rect = ModuleUI.PrefixLabel(rect, label);
			int result = EditorGUI.Popup(rect, intValue, options, ParticleSystemStyles.Get().popup);
			EditorGUI.EndProperty();
			return result;
		}

		public static Enum GUIEnumPopup(GUIContent label, Enum selected, SerializedProperty property, params GUILayoutOption[] layoutOptions)
		{
			Rect rect = ModuleUI.GetControlRect(13, layoutOptions);
			label = EditorGUI.BeginProperty(rect, label, property);
			rect = ModuleUI.PrefixLabel(rect, label);
			Enum result = EditorGUI.EnumPopup(rect, selected, ParticleSystemStyles.Get().popup);
			EditorGUI.EndProperty();
			return result;
		}

		private static Color GetColor(SerializedMinMaxCurve mmCurve)
		{
			return mmCurve.m_Module.m_ParticleSystemUI.m_ParticleEffectUI.GetParticleSystemCurveEditor().GetCurveColor(mmCurve.maxCurve);
		}

		private static void GUICurveField(Rect position, SerializedProperty maxCurve, SerializedProperty minCurve, Color color, Rect ranges, ModuleUI.CurveFieldMouseDownCallback mouseDownCallback)
		{
			int controlID = GUIUtility.GetControlID(1321321231, FocusType.Keyboard, position);
			Event current = Event.current;
			EventType typeForControl = current.GetTypeForControl(controlID);
			if (typeForControl != EventType.Repaint)
			{
				if (typeForControl != EventType.ValidateCommand)
				{
					if (typeForControl == EventType.MouseDown)
					{
						if (position.Contains(current.mousePosition))
						{
							if (mouseDownCallback != null && mouseDownCallback(current.button, position, ranges))
							{
								current.Use();
							}
						}
					}
				}
				else if (current.commandName == "UndoRedoPerformed")
				{
					AnimationCurvePreviewCache.ClearCache();
				}
			}
			else
			{
				Rect position2 = position;
				if (minCurve == null)
				{
					EditorGUIUtility.DrawCurveSwatch(position2, null, maxCurve, color, EditorGUI.kCurveBGColor, ranges);
				}
				else
				{
					EditorGUIUtility.DrawRegionSwatch(position2, maxCurve, minCurve, color, EditorGUI.kCurveBGColor, ranges);
				}
				EditorStyles.colorPickerBox.Draw(position2, GUIContent.none, controlID, false);
			}
		}

		public static void GUIMinMaxCurve(string label, SerializedMinMaxCurve mmCurve, params GUILayoutOption[] layoutOptions)
		{
			ModuleUI.GUIMinMaxCurve(GUIContent.Temp(label), mmCurve, layoutOptions);
		}

		public static void GUIMinMaxCurve(GUIContent label, SerializedMinMaxCurve mmCurve, params GUILayoutOption[] layoutOptions)
		{
			ModuleUI.GUIMinMaxCurve(label, mmCurve, null, layoutOptions);
		}

		public static void GUIMinMaxCurve(SerializedProperty editableLabel, SerializedMinMaxCurve mmCurve, params GUILayoutOption[] layoutOptions)
		{
			ModuleUI.GUIMinMaxCurve(null, mmCurve, editableLabel, layoutOptions);
		}

		internal static void GUIMinMaxCurve(GUIContent label, SerializedMinMaxCurve mmCurve, SerializedProperty editableLabel, params GUILayoutOption[] layoutOptions)
		{
			bool stateHasMultipleDifferentValues = mmCurve.stateHasMultipleDifferentValues;
			Rect rect = ModuleUI.GetControlRect(13, layoutOptions);
			label = EditorGUI.BeginProperty(rect, label, mmCurve.rootProperty);
			Rect popupRect = ModuleUI.GetPopupRect(rect);
			rect = ModuleUI.SubtractPopupWidth(rect);
			Rect rect2;
			if (editableLabel != null)
			{
				Rect rect3;
				rect2 = ModuleUI.FieldPosition(rect, out rect3);
				rect3.width -= 4f;
				float x = ParticleSystemStyles.Get().editableLabel.CalcSize(GUIContent.Temp(editableLabel.stringValue)).x;
				rect3.width = Mathf.Min(rect3.width, x + 4f);
				EditorGUI.BeginProperty(rect3, GUIContent.none, editableLabel);
				EditorGUI.BeginChangeCheck();
				string stringValue = EditorGUI.TextFieldInternal(GUIUtility.GetControlID(FocusType.Passive, rect3), rect3, editableLabel.stringValue, ParticleSystemStyles.Get().editableLabel);
				if (EditorGUI.EndChangeCheck())
				{
					editableLabel.stringValue = stringValue;
				}
				EditorGUI.EndProperty();
			}
			else
			{
				rect2 = ModuleUI.PrefixLabel(rect, label);
			}
			if (stateHasMultipleDifferentValues)
			{
				ModuleUI.Label(rect2, GUIContent.Temp("-"));
			}
			else
			{
				MinMaxCurveState state = mmCurve.state;
				if (state == MinMaxCurveState.k_Scalar)
				{
					EditorGUI.BeginChangeCheck();
					float a = ModuleUI.FloatDraggable(rect, mmCurve.scalar, mmCurve.m_RemapValue, EditorGUIUtility.labelWidth);
					if (EditorGUI.EndChangeCheck() && !mmCurve.signedRange)
					{
						mmCurve.scalar.floatValue = Mathf.Max(a, 0f);
					}
				}
				else if (state == MinMaxCurveState.k_TwoScalars)
				{
					Rect rect4 = rect2;
					rect4.width = (rect2.width - 20f) * 0.5f;
					Rect rect5 = rect4;
					rect5.xMin -= 20f;
					EditorGUI.BeginChangeCheck();
					float a2 = ModuleUI.FloatDraggable(rect5, mmCurve.minScalar, mmCurve.m_RemapValue, 20f, "g5");
					if (EditorGUI.EndChangeCheck() && !mmCurve.signedRange)
					{
						mmCurve.minScalar.floatValue = Mathf.Max(a2, 0f);
					}
					rect5.x += rect4.width + 20f;
					EditorGUI.BeginChangeCheck();
					float a3 = ModuleUI.FloatDraggable(rect5, mmCurve.scalar, mmCurve.m_RemapValue, 20f, "g5");
					if (EditorGUI.EndChangeCheck() && !mmCurve.signedRange)
					{
						mmCurve.scalar.floatValue = Mathf.Max(a3, 0f);
					}
				}
				else
				{
					Rect ranges = (!mmCurve.signedRange) ? ModuleUI.kUnsignedRange : ModuleUI.kSignedRange;
					SerializedProperty minCurve = (state != MinMaxCurveState.k_TwoCurves) ? null : mmCurve.minCurve;
					ModuleUI.GUICurveField(rect2, mmCurve.maxCurve, minCurve, ModuleUI.GetColor(mmCurve), ranges, new ModuleUI.CurveFieldMouseDownCallback(mmCurve.OnCurveAreaMouseDown));
				}
			}
			ModuleUI.GUIMMCurveStateList(popupRect, mmCurve);
			EditorGUI.EndProperty();
		}

		public static Rect GUIMinMaxCurveInline(Rect rect, SerializedMinMaxCurve mmCurve, float dragWidth)
		{
			EditorGUI.BeginProperty(rect, GUIContent.none, mmCurve.rootProperty);
			bool stateHasMultipleDifferentValues = mmCurve.stateHasMultipleDifferentValues;
			if (stateHasMultipleDifferentValues)
			{
				ModuleUI.Label(rect, GUIContent.Temp("-"));
			}
			else
			{
				MinMaxCurveState state = mmCurve.state;
				if (state == MinMaxCurveState.k_Scalar)
				{
					EditorGUI.BeginChangeCheck();
					float a = ModuleUI.FloatDraggable(rect, mmCurve.scalar, mmCurve.m_RemapValue, dragWidth, "n0");
					if (EditorGUI.EndChangeCheck() && !mmCurve.signedRange)
					{
						mmCurve.scalar.floatValue = Mathf.Max(a, 0f);
					}
				}
				else if (state == MinMaxCurveState.k_TwoScalars)
				{
					Rect rect2 = rect;
					rect2.width = rect.width * 0.5f;
					Rect rect3 = rect2;
					EditorGUI.BeginChangeCheck();
					float a2 = ModuleUI.FloatDraggable(rect3, mmCurve.minScalar, mmCurve.m_RemapValue, dragWidth, "n0");
					if (EditorGUI.EndChangeCheck() && !mmCurve.signedRange)
					{
						mmCurve.minScalar.floatValue = Mathf.Max(a2, 0f);
					}
					rect3.x += rect2.width;
					EditorGUI.BeginChangeCheck();
					float a3 = ModuleUI.FloatDraggable(rect3, mmCurve.scalar, mmCurve.m_RemapValue, dragWidth, "n0");
					if (EditorGUI.EndChangeCheck() && !mmCurve.signedRange)
					{
						mmCurve.scalar.floatValue = Mathf.Max(a3, 0f);
					}
				}
				else
				{
					Rect ranges = (!mmCurve.signedRange) ? ModuleUI.kUnsignedRange : ModuleUI.kSignedRange;
					SerializedProperty minCurve = (state != MinMaxCurveState.k_TwoCurves) ? null : mmCurve.minCurve;
					ModuleUI.GUICurveField(rect, mmCurve.maxCurve, minCurve, ModuleUI.GetColor(mmCurve), ranges, new ModuleUI.CurveFieldMouseDownCallback(mmCurve.OnCurveAreaMouseDown));
				}
			}
			rect.width += 13f;
			Rect popupRect = ModuleUI.GetPopupRect(rect);
			ModuleUI.GUIMMCurveStateList(popupRect, mmCurve);
			EditorGUI.EndProperty();
			return rect;
		}

		public void GUIMinMaxGradient(GUIContent label, SerializedMinMaxGradient minMaxGradient, bool hdr, params GUILayoutOption[] layoutOptions)
		{
			this.GUIMinMaxGradient(label, minMaxGradient, null, hdr, layoutOptions);
		}

		public void GUIMinMaxGradient(SerializedProperty editableLabel, SerializedMinMaxGradient minMaxGradient, bool hdr, params GUILayoutOption[] layoutOptions)
		{
			this.GUIMinMaxGradient(null, minMaxGradient, editableLabel, hdr, layoutOptions);
		}

		internal void GUIMinMaxGradient(GUIContent label, SerializedMinMaxGradient minMaxGradient, SerializedProperty editableLabel, bool hdr, params GUILayoutOption[] layoutOptions)
		{
			bool stateHasMultipleDifferentValues = minMaxGradient.stateHasMultipleDifferentValues;
			MinMaxGradientState state = minMaxGradient.state;
			bool flag = !stateHasMultipleDifferentValues && (state == MinMaxGradientState.k_RandomBetweenTwoColors || state == MinMaxGradientState.k_RandomBetweenTwoGradients);
			Rect rect = GUILayoutUtility.GetRect(0f, (float)((!flag) ? 13 : 26), layoutOptions);
			label = EditorGUI.BeginProperty(rect, label, minMaxGradient.m_RootProperty);
			Rect popupRect = ModuleUI.GetPopupRect(rect);
			rect = ModuleUI.SubtractPopupWidth(rect);
			Rect rect2;
			if (editableLabel != null)
			{
				Rect rect3;
				rect2 = ModuleUI.FieldPosition(rect, out rect3);
				rect3.width -= 4f;
				EditorGUI.BeginProperty(rect3, GUIContent.none, editableLabel);
				EditorGUI.BeginChangeCheck();
				string stringValue = EditorGUI.TextFieldInternal(GUIUtility.GetControlID(FocusType.Passive, rect3), rect3, editableLabel.stringValue, ParticleSystemStyles.Get().editableLabel);
				if (EditorGUI.EndChangeCheck())
				{
					editableLabel.stringValue = stringValue;
				}
				EditorGUI.EndProperty();
			}
			else
			{
				rect2 = ModuleUI.PrefixLabel(rect, label);
			}
			rect2.height = 13f;
			if (stateHasMultipleDifferentValues)
			{
				ModuleUI.Label(rect2, GUIContent.Temp("-"));
			}
			else
			{
				switch (state)
				{
				case MinMaxGradientState.k_Color:
					ModuleUI.GUIColor(rect2, minMaxGradient.m_MaxColor, hdr);
					break;
				case MinMaxGradientState.k_Gradient:
				case MinMaxGradientState.k_RandomColor:
					EditorGUI.BeginProperty(rect2, GUIContent.none, minMaxGradient.m_MaxGradient);
					EditorGUI.GradientField(rect2, minMaxGradient.m_MaxGradient, hdr);
					EditorGUI.EndProperty();
					break;
				case MinMaxGradientState.k_RandomBetweenTwoColors:
					ModuleUI.GUIColor(rect2, minMaxGradient.m_MaxColor, hdr);
					rect2.y += rect2.height;
					ModuleUI.GUIColor(rect2, minMaxGradient.m_MinColor, hdr);
					break;
				case MinMaxGradientState.k_RandomBetweenTwoGradients:
					EditorGUI.BeginProperty(rect2, GUIContent.none, minMaxGradient.m_MaxGradient);
					EditorGUI.GradientField(rect2, minMaxGradient.m_MaxGradient, hdr);
					EditorGUI.EndProperty();
					rect2.y += rect2.height;
					EditorGUI.BeginProperty(rect2, GUIContent.none, minMaxGradient.m_MinGradient);
					EditorGUI.GradientField(rect2, minMaxGradient.m_MinGradient, hdr);
					EditorGUI.EndProperty();
					break;
				}
			}
			ModuleUI.GUIMMGradientPopUp(popupRect, minMaxGradient);
			EditorGUI.EndProperty();
		}

		private static void GUIColor(Rect rect, SerializedProperty colorProp)
		{
			ModuleUI.GUIColor(rect, colorProp, false);
		}

		private static void GUIColor(Rect rect, SerializedProperty colorProp, bool hdr)
		{
			EditorGUI.BeginProperty(rect, GUIContent.none, colorProp);
			EditorGUI.BeginChangeCheck();
			Color colorValue = EditorGUI.ColorField(rect, GUIContent.none, colorProp.colorValue, false, true, hdr);
			if (EditorGUI.EndChangeCheck())
			{
				colorProp.colorValue = colorValue;
			}
			EditorGUI.EndProperty();
		}

		public void GUITripleMinMaxCurve(GUIContent label, GUIContent x, SerializedMinMaxCurve xCurve, GUIContent y, SerializedMinMaxCurve yCurve, GUIContent z, SerializedMinMaxCurve zCurve, SerializedProperty randomizePerFrame, params GUILayoutOption[] layoutOptions)
		{
			using (new ModuleUI.PropertyGroupScope(new SerializedProperty[]
			{
				xCurve.rootProperty,
				yCurve.rootProperty,
				zCurve.rootProperty
			}))
			{
				bool stateHasMultipleDifferentValues = xCurve.stateHasMultipleDifferentValues;
				MinMaxCurveState state = xCurve.state;
				bool flag = label != GUIContent.none;
				int num = (!flag) ? 1 : 2;
				if (state == MinMaxCurveState.k_TwoScalars)
				{
					num++;
				}
				Rect rect = ModuleUI.GetControlRect(13 * num, layoutOptions);
				Rect popupRect = ModuleUI.GetPopupRect(rect);
				rect = ModuleUI.SubtractPopupWidth(rect);
				Rect rect2 = rect;
				float num2 = 2f;
				float[] array = new float[]
				{
					ParticleSystemStyles.Get().label.CalcSize(GUIContent.Temp(x.text)).x + num2,
					ParticleSystemStyles.Get().label.CalcSize(GUIContent.Temp(y.text)).x + num2,
					ParticleSystemStyles.Get().label.CalcSize(GUIContent.Temp(z.text)).x + num2
				};
				float num3 = (rect.width - array[0] - array[1] - array[2]) / 3f;
				if (num > 1)
				{
					rect2.height = 13f;
				}
				if (flag)
				{
					ModuleUI.PrefixLabel(rect, label);
					rect2.y += rect2.height;
				}
				GUIContent[] array2 = new GUIContent[]
				{
					x,
					y,
					z
				};
				SerializedMinMaxCurve[] array3 = new SerializedMinMaxCurve[]
				{
					xCurve,
					yCurve,
					zCurve
				};
				if (stateHasMultipleDifferentValues)
				{
					rect2.width = num3 + array[0];
					ModuleUI.Label(rect2, GUIContent.Temp("-"));
				}
				else if (state == MinMaxCurveState.k_Scalar)
				{
					for (int i = 0; i < array3.Length; i++)
					{
						rect2.width = num3 + array[i] - num2 * 2f;
						EditorGUI.BeginProperty(rect2, array2[i], array3[i].scalar);
						ModuleUI.Label(rect2, array2[i]);
						EditorGUI.BeginChangeCheck();
						float a = ModuleUI.FloatDraggable(rect2, array3[i].scalar, array3[i].m_RemapValue, array[i]);
						if (EditorGUI.EndChangeCheck() && !array3[i].signedRange)
						{
							array3[i].scalar.floatValue = Mathf.Max(a, 0f);
						}
						rect2.x += num3 + array[i] + num2;
						EditorGUI.EndProperty();
					}
				}
				else if (state == MinMaxCurveState.k_TwoScalars)
				{
					for (int j = 0; j < array3.Length; j++)
					{
						rect2.width = num3 + array[j] - num2 * 2f;
						ModuleUI.Label(rect2, array2[j]);
						float num4 = array3[j].minConstant;
						float num5 = array3[j].maxConstant;
						EditorGUI.BeginChangeCheck();
						num5 = ModuleUI.FloatDraggable(rect2, num5, array3[j].m_RemapValue, array[j], "g5");
						if (EditorGUI.EndChangeCheck())
						{
							array3[j].maxConstant = num5;
						}
						rect2.y += 13f;
						EditorGUI.BeginChangeCheck();
						num4 = ModuleUI.FloatDraggable(rect2, num4, array3[j].m_RemapValue, array[j], "g5");
						if (EditorGUI.EndChangeCheck())
						{
							array3[j].minConstant = num4;
						}
						rect2.x += num3 + array[j] + num2;
						rect2.y -= 13f;
					}
				}
				else
				{
					Rect ranges = (!xCurve.signedRange) ? ModuleUI.kUnsignedRange : ModuleUI.kSignedRange;
					for (int k = 0; k < array3.Length; k++)
					{
						rect2.width = num3 + array[k] - num2 * 2f;
						SerializedProperty serializedProperty = (state != MinMaxCurveState.k_TwoCurves) ? null : array3[k].minCurve;
						using ((serializedProperty != null) ? new ModuleUI.PropertyGroupScope(new SerializedProperty[]
						{
							array3[k].maxCurve,
							serializedProperty
						}) : new EditorGUI.PropertyScope(rect2, array2[k], array3[k].maxCurve))
						{
							ModuleUI.Label(rect2, array2[k]);
							Rect position = rect2;
							position.xMin += array[k];
							ModuleUI.GUICurveField(position, array3[k].maxCurve, serializedProperty, ModuleUI.GetColor(array3[k]), ranges, new ModuleUI.CurveFieldMouseDownCallback(array3[k].OnCurveAreaMouseDown));
							rect2.x += num3 + array[k] + num2;
						}
					}
				}
				ModuleUI.GUIMMCurveStateList(popupRect, array3);
			}
		}

		private static void SelectMinMaxCurveStateCallback(object obj)
		{
			ModuleUI.CurveStateCallbackData curveStateCallbackData = (ModuleUI.CurveStateCallbackData)obj;
			SerializedMinMaxCurve[] minMaxCurves = curveStateCallbackData.minMaxCurves;
			for (int i = 0; i < minMaxCurves.Length; i++)
			{
				SerializedMinMaxCurve serializedMinMaxCurve = minMaxCurves[i];
				serializedMinMaxCurve.state = curveStateCallbackData.selectedState;
			}
		}

		public static void GUIMMCurveStateList(Rect rect, SerializedMinMaxCurve minMaxCurves)
		{
			SerializedMinMaxCurve[] minMaxCurves2 = new SerializedMinMaxCurve[]
			{
				minMaxCurves
			};
			ModuleUI.GUIMMCurveStateList(rect, minMaxCurves2);
		}

		public static void GUIMMCurveStateList(Rect rect, SerializedMinMaxCurve[] minMaxCurves)
		{
			if (EditorGUI.DropdownButton(rect, GUIContent.none, FocusType.Passive, ParticleSystemStyles.Get().minMaxCurveStateDropDown))
			{
				if (minMaxCurves.Length != 0)
				{
					GUIContent[] array = new GUIContent[]
					{
						EditorGUIUtility.TrTextContent("Constant", null, null),
						EditorGUIUtility.TrTextContent("Curve", null, null),
						EditorGUIUtility.TrTextContent("Random Between Two Constants", null, null),
						EditorGUIUtility.TrTextContent("Random Between Two Curves", null, null)
					};
					MinMaxCurveState[] array2 = new MinMaxCurveState[]
					{
						MinMaxCurveState.k_Scalar,
						MinMaxCurveState.k_Curve,
						MinMaxCurveState.k_TwoScalars,
						MinMaxCurveState.k_TwoCurves
					};
					bool[] array3 = new bool[]
					{
						minMaxCurves[0].m_AllowConstant,
						minMaxCurves[0].m_AllowCurves,
						minMaxCurves[0].m_AllowRandom,
						minMaxCurves[0].m_AllowRandom && minMaxCurves[0].m_AllowCurves
					};
					bool flag = !minMaxCurves[0].stateHasMultipleDifferentValues;
					GenericMenu genericMenu = new GenericMenu();
					for (int i = 0; i < array.Length; i++)
					{
						if (array3[i])
						{
							GenericMenu arg_12B_0 = genericMenu;
							GUIContent arg_12B_1 = array[i];
							bool arg_12B_2 = flag && minMaxCurves[0].state == array2[i];
							if (ModuleUI.<>f__mg$cache0 == null)
							{
								ModuleUI.<>f__mg$cache0 = new GenericMenu.MenuFunction2(ModuleUI.SelectMinMaxCurveStateCallback);
							}
							arg_12B_0.AddItem(arg_12B_1, arg_12B_2, ModuleUI.<>f__mg$cache0, new ModuleUI.CurveStateCallbackData(array2[i], minMaxCurves));
						}
					}
					genericMenu.DropDown(rect);
					Event.current.Use();
				}
			}
		}

		private static void SelectMinMaxGradientStateCallback(object obj)
		{
			ModuleUI.GradientCallbackData gradientCallbackData = (ModuleUI.GradientCallbackData)obj;
			gradientCallbackData.gradientProp.state = gradientCallbackData.selectedState;
		}

		public static void GUIMMGradientPopUp(Rect rect, SerializedMinMaxGradient gradientProp)
		{
			if (EditorGUI.DropdownButton(rect, GUIContent.none, FocusType.Passive, ParticleSystemStyles.Get().minMaxCurveStateDropDown))
			{
				GUIContent[] array = new GUIContent[]
				{
					EditorGUIUtility.TrTextContent("Color", null, null),
					EditorGUIUtility.TrTextContent("Gradient", null, null),
					EditorGUIUtility.TrTextContent("Random Between Two Colors", null, null),
					EditorGUIUtility.TrTextContent("Random Between Two Gradients", null, null),
					EditorGUIUtility.TrTextContent("Random Color", null, null)
				};
				MinMaxGradientState[] array2 = new MinMaxGradientState[]
				{
					MinMaxGradientState.k_Color,
					MinMaxGradientState.k_Gradient,
					MinMaxGradientState.k_RandomBetweenTwoColors,
					MinMaxGradientState.k_RandomBetweenTwoGradients,
					MinMaxGradientState.k_RandomColor
				};
				bool[] array3 = new bool[]
				{
					gradientProp.m_AllowColor,
					gradientProp.m_AllowGradient,
					gradientProp.m_AllowRandomBetweenTwoColors,
					gradientProp.m_AllowRandomBetweenTwoGradients,
					gradientProp.m_AllowRandomColor
				};
				bool flag = !gradientProp.stateHasMultipleDifferentValues;
				GenericMenu genericMenu = new GenericMenu();
				for (int i = 0; i < array.Length; i++)
				{
					if (array3[i])
					{
						GenericMenu arg_11A_0 = genericMenu;
						GUIContent arg_11A_1 = array[i];
						bool arg_11A_2 = flag && gradientProp.state == array2[i];
						if (ModuleUI.<>f__mg$cache1 == null)
						{
							ModuleUI.<>f__mg$cache1 = new GenericMenu.MenuFunction2(ModuleUI.SelectMinMaxGradientStateCallback);
						}
						arg_11A_0.AddItem(arg_11A_1, arg_11A_2, ModuleUI.<>f__mg$cache1, new ModuleUI.GradientCallbackData(array2[i], gradientProp));
					}
				}
				genericMenu.ShowAsContext();
				Event.current.Use();
			}
		}

		private static void SelectMinMaxColorStateCallback(object obj)
		{
			ModuleUI.ColorCallbackData colorCallbackData = (ModuleUI.ColorCallbackData)obj;
			colorCallbackData.boolProp.boolValue = colorCallbackData.selectedState;
		}

		public static void GUIMMColorPopUp(Rect rect, SerializedProperty boolProp)
		{
			if (EditorGUI.DropdownButton(rect, GUIContent.none, FocusType.Passive, ParticleSystemStyles.Get().minMaxCurveStateDropDown))
			{
				GenericMenu genericMenu = new GenericMenu();
				GUIContent[] array = new GUIContent[]
				{
					EditorGUIUtility.TrTextContent("Constant Color", null, null),
					EditorGUIUtility.TrTextContent("Random Between Two Colors", null, null)
				};
				bool[] array2 = new bool[]
				{
					default(bool),
					true
				};
				for (int i = 0; i < array.Length; i++)
				{
					GenericMenu arg_90_0 = genericMenu;
					GUIContent arg_90_1 = array[i];
					bool arg_90_2 = boolProp.boolValue == array2[i];
					if (ModuleUI.<>f__mg$cache2 == null)
					{
						ModuleUI.<>f__mg$cache2 = new GenericMenu.MenuFunction2(ModuleUI.SelectMinMaxColorStateCallback);
					}
					arg_90_0.AddItem(arg_90_1, arg_90_2, ModuleUI.<>f__mg$cache2, new ModuleUI.ColorCallbackData(array2[i], boolProp));
				}
				genericMenu.ShowAsContext();
				Event.current.Use();
			}
		}
	}
}
