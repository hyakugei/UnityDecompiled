using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

namespace UnityEditor
{
	internal class RendererEditorBase : Editor
	{
		internal class Probes
		{
			private SerializedProperty m_LightProbeUsage;

			private SerializedProperty m_LightProbeVolumeOverride;

			private SerializedProperty m_ReflectionProbeUsage;

			private SerializedProperty m_ProbeAnchor;

			private SerializedProperty m_ReceiveShadows;

			private GUIContent m_LightProbeUsageStyle = EditorGUIUtility.TrTextContent("Light Probes", "Specifies how Light Probes will handle the interpolation of lighting and occlusion. Disabled if the object is set to Lightmap Static.", null);

			private GUIContent m_LightProbeVolumeOverrideStyle = EditorGUIUtility.TrTextContent("Proxy Volume Override", "If set, the Renderer will use the Light Probe Proxy Volume component from another GameObject.", null);

			private GUIContent m_ReflectionProbeUsageStyle = EditorGUIUtility.TrTextContent("Reflection Probes", "Specifies if or how the object is affected by reflections in the Scene.  This property cannot be disabled in deferred rendering modes.", null);

			private GUIContent m_ProbeAnchorStyle = EditorGUIUtility.TrTextContent("Anchor Override", "Specifies the Transform position that will be used for sampling the light probes and reflection probes.", null);

			private GUIContent m_DeferredNote = EditorGUIUtility.TrTextContent("In Deferred Shading, all objects receive shadows and get per-pixel reflection probes.", null, null);

			private GUIContent m_LightProbeVolumeNote = EditorGUIUtility.TrTextContent("A valid Light Probe Proxy Volume component could not be found.", null, null);

			private GUIContent m_LightProbeVolumeUnsupportedNote = EditorGUIUtility.TrTextContent("The Light Probe Proxy Volume feature is unsupported by the current graphics hardware or API configuration. Simple 'Blend Probes' mode will be used instead.", null, null);

			private GUIContent m_LightProbeVolumeUnsupportedOnTreesNote = EditorGUIUtility.TrTextContent("The Light Probe Proxy Volume feature is not supported on tree rendering. Simple 'Blend Probes' mode will be used instead.", null, null);

			private GUIContent m_LightProbeCustomNote = EditorGUIUtility.TrTextContent("The Custom Provided mode requires SH properties to be sent via MaterialPropertyBlock.", null, null);

			private GUIContent[] m_ReflectionProbeUsageOptions = (from x in (from x in Enum.GetNames(typeof(ReflectionProbeUsage))
			select ObjectNames.NicifyVariableName(x)).ToArray<string>()
			select new GUIContent(x)).ToArray<GUIContent>();

			private List<ReflectionProbeBlendInfo> m_BlendInfo = new List<ReflectionProbeBlendInfo>();

			internal void Initialize(SerializedObject serializedObject)
			{
				this.m_LightProbeUsage = serializedObject.FindProperty("m_LightProbeUsage");
				this.m_LightProbeVolumeOverride = serializedObject.FindProperty("m_LightProbeVolumeOverride");
				this.m_ReflectionProbeUsage = serializedObject.FindProperty("m_ReflectionProbeUsage");
				this.m_ProbeAnchor = serializedObject.FindProperty("m_ProbeAnchor");
				this.m_ReceiveShadows = serializedObject.FindProperty("m_ReceiveShadows");
			}

			internal bool IsUsingLightProbeProxyVolume(int selectionCount)
			{
				return (selectionCount == 1 && this.m_LightProbeUsage.intValue == 2) || (selectionCount > 1 && !this.m_LightProbeUsage.hasMultipleDifferentValues && this.m_LightProbeUsage.intValue == 2);
			}

			internal bool HasValidLightProbeProxyVolumeOverride(Renderer renderer, int selectionCount)
			{
				LightProbeProxyVolume lightProbeProxyVolume = (!(renderer.lightProbeProxyVolumeOverride != null)) ? null : renderer.lightProbeProxyVolumeOverride.GetComponent<LightProbeProxyVolume>();
				return this.IsUsingLightProbeProxyVolume(selectionCount) && (lightProbeProxyVolume == null || lightProbeProxyVolume.boundingBoxMode != LightProbeProxyVolume.BoundingBoxMode.AutomaticLocal);
			}

			internal void RenderLightProbeProxyVolumeWarningNote(Renderer renderer, int selectionCount)
			{
				if (this.IsUsingLightProbeProxyVolume(selectionCount))
				{
					if (LightProbeProxyVolume.isFeatureSupported && SupportedRenderingFeatures.active.rendererSupportsLightProbeProxyVolumes)
					{
						LightProbeProxyVolume component = renderer.GetComponent<LightProbeProxyVolume>();
						bool flag = renderer.lightProbeProxyVolumeOverride == null || renderer.lightProbeProxyVolumeOverride.GetComponent<LightProbeProxyVolume>() == null;
						if (component == null && flag && LightProbes.AreLightProbesAllowed(renderer))
						{
							EditorGUILayout.HelpBox(this.m_LightProbeVolumeNote.text, MessageType.Warning);
						}
					}
					else
					{
						EditorGUILayout.HelpBox(this.m_LightProbeVolumeUnsupportedNote.text, MessageType.Warning);
					}
				}
			}

			internal void RenderReflectionProbeUsage(bool useMiniStyle, bool isDeferredRenderingPath, bool isDeferredReflections)
			{
				if (SupportedRenderingFeatures.active.rendererSupportsReflectionProbes)
				{
					using (new EditorGUI.DisabledScope(isDeferredRenderingPath))
					{
						if (!useMiniStyle)
						{
							if (isDeferredReflections)
							{
								EditorGUILayout.EnumPopup(this.m_ReflectionProbeUsageStyle, (this.m_ReflectionProbeUsage.intValue == 0) ? ReflectionProbeUsage.Off : ReflectionProbeUsage.Simple, new GUILayoutOption[0]);
							}
							else
							{
								EditorGUILayout.Popup(this.m_ReflectionProbeUsage, this.m_ReflectionProbeUsageOptions, this.m_ReflectionProbeUsageStyle, new GUILayoutOption[0]);
							}
						}
						else if (isDeferredReflections)
						{
							ModuleUI.GUIPopup(this.m_ReflectionProbeUsageStyle, 3, this.m_ReflectionProbeUsageOptions, new GUILayoutOption[0]);
						}
						else
						{
							ModuleUI.GUIPopup(this.m_ReflectionProbeUsageStyle, this.m_ReflectionProbeUsage, this.m_ReflectionProbeUsageOptions, new GUILayoutOption[0]);
						}
					}
				}
			}

			internal void RenderLightProbeUsage(int selectionCount, Renderer renderer, bool useMiniStyle, bool lightProbeAllowed)
			{
				using (new EditorGUI.DisabledScope(!lightProbeAllowed))
				{
					if (lightProbeAllowed)
					{
						if (useMiniStyle)
						{
							EditorGUI.BeginChangeCheck();
							Enum @enum = ModuleUI.GUIEnumPopup(this.m_LightProbeUsageStyle, (LightProbeUsage)this.m_LightProbeUsage.intValue, this.m_LightProbeUsage, new GUILayoutOption[0]);
							if (EditorGUI.EndChangeCheck())
							{
								this.m_LightProbeUsage.intValue = (int)((LightProbeUsage)@enum);
							}
						}
						else
						{
							Rect controlRect = EditorGUILayout.GetControlRect(true, 16f, EditorStyles.popup, new GUILayoutOption[0]);
							EditorGUI.BeginProperty(controlRect, this.m_LightProbeUsageStyle, this.m_LightProbeUsage);
							EditorGUI.BeginChangeCheck();
							Enum enum2 = EditorGUI.EnumPopup(controlRect, this.m_LightProbeUsageStyle, (LightProbeUsage)this.m_LightProbeUsage.intValue);
							if (EditorGUI.EndChangeCheck())
							{
								this.m_LightProbeUsage.intValue = (int)((LightProbeUsage)enum2);
							}
							EditorGUI.EndProperty();
						}
						if (!this.m_LightProbeUsage.hasMultipleDifferentValues)
						{
							if (this.m_LightProbeUsage.intValue == 2 && SupportedRenderingFeatures.active.rendererSupportsLightProbeProxyVolumes)
							{
								EditorGUI.indentLevel++;
								if (useMiniStyle)
								{
									ModuleUI.GUIObject(this.m_LightProbeVolumeOverrideStyle, this.m_LightProbeVolumeOverride, new GUILayoutOption[0]);
								}
								else
								{
									EditorGUILayout.PropertyField(this.m_LightProbeVolumeOverride, this.m_LightProbeVolumeOverrideStyle, new GUILayoutOption[0]);
								}
								EditorGUI.indentLevel--;
							}
							else if (this.m_LightProbeUsage.intValue == 4)
							{
								EditorGUI.indentLevel++;
								if (!Application.isPlaying)
								{
									EditorGUILayout.HelpBox(this.m_LightProbeCustomNote.text, MessageType.Info);
								}
								else if (!renderer.HasPropertyBlock())
								{
									EditorGUILayout.HelpBox(this.m_LightProbeCustomNote.text, MessageType.Error);
								}
								EditorGUI.indentLevel--;
							}
						}
					}
					else if (useMiniStyle)
					{
						ModuleUI.GUIEnumPopup(this.m_LightProbeUsageStyle, LightProbeUsage.Off, this.m_LightProbeUsage, new GUILayoutOption[0]);
					}
					else
					{
						EditorGUILayout.EnumPopup(this.m_LightProbeUsageStyle, LightProbeUsage.Off, new GUILayoutOption[0]);
					}
				}
				Tree component = renderer.GetComponent<Tree>();
				if (component != null && this.m_LightProbeUsage.intValue == 2)
				{
					EditorGUI.indentLevel++;
					EditorGUILayout.HelpBox(this.m_LightProbeVolumeUnsupportedOnTreesNote.text, MessageType.Warning);
					EditorGUI.indentLevel--;
				}
			}

			internal bool RenderProbeAnchor(bool useMiniStyle)
			{
				bool flag = !this.m_ReflectionProbeUsage.hasMultipleDifferentValues && this.m_ReflectionProbeUsage.intValue != 0;
				bool flag2 = !this.m_LightProbeUsage.hasMultipleDifferentValues && this.m_LightProbeUsage.intValue != 0;
				bool flag3 = flag || flag2;
				if (flag3)
				{
					if (!useMiniStyle)
					{
						EditorGUILayout.PropertyField(this.m_ProbeAnchor, this.m_ProbeAnchorStyle, new GUILayoutOption[0]);
					}
					else
					{
						ModuleUI.GUIObject(this.m_ProbeAnchorStyle, this.m_ProbeAnchor, new GUILayoutOption[0]);
					}
				}
				return flag3;
			}

			internal void OnGUI(UnityEngine.Object[] selection, Renderer renderer, bool useMiniStyle)
			{
				int selectionCount = 1;
				bool flag = SceneView.IsUsingDeferredRenderingPath();
				bool flag2 = flag && GraphicsSettings.GetShaderMode(BuiltinShaderType.DeferredReflections) != BuiltinShaderMode.Disabled;
				bool lightProbeAllowed = true;
				if (selection != null)
				{
					for (int i = 0; i < selection.Length; i++)
					{
						UnityEngine.Object @object = selection[i];
						if (!LightProbes.AreLightProbesAllowed((Renderer)@object))
						{
							lightProbeAllowed = false;
							break;
						}
					}
					selectionCount = selection.Length;
				}
				this.RenderLightProbeUsage(selectionCount, renderer, useMiniStyle, lightProbeAllowed);
				this.RenderLightProbeProxyVolumeWarningNote(renderer, selectionCount);
				this.RenderReflectionProbeUsage(useMiniStyle, flag, flag2);
				bool flag3 = this.RenderProbeAnchor(useMiniStyle);
				if (flag3)
				{
					bool flag4 = !this.m_ReflectionProbeUsage.hasMultipleDifferentValues && this.m_ReflectionProbeUsage.intValue != 0;
					if (flag4)
					{
						if (!flag2)
						{
							renderer.GetClosestReflectionProbes(this.m_BlendInfo);
							RendererEditorBase.Probes.ShowClosestReflectionProbes(this.m_BlendInfo);
						}
					}
				}
				bool flag5 = !this.m_ReceiveShadows.hasMultipleDifferentValues && this.m_ReceiveShadows.boolValue;
				if ((flag && flag5) || (flag2 && flag3))
				{
					EditorGUILayout.HelpBox(this.m_DeferredNote.text, MessageType.Info);
				}
			}

			internal static void ShowClosestReflectionProbes(List<ReflectionProbeBlendInfo> blendInfos)
			{
				float num = 20f;
				float num2 = 70f;
				using (new EditorGUI.DisabledScope(true))
				{
					for (int i = 0; i < blendInfos.Count; i++)
					{
						Rect rect = GUILayoutUtility.GetRect(0f, 16f);
						rect = EditorGUI.IndentedRect(rect);
						float width = rect.width - num - num2;
						Rect position = rect;
						position.width = num;
						GUI.Label(position, "#" + i, EditorStyles.miniLabel);
						position.x += position.width;
						position.width = width;
						EditorGUI.ObjectField(position, blendInfos[i].probe, typeof(ReflectionProbe), true);
						position.x += position.width;
						position.width = num2;
						GUI.Label(position, "Weight " + blendInfos[i].weight.ToString("f2"), EditorStyles.miniLabel);
					}
				}
			}

			internal static string[] GetFieldsStringArray()
			{
				return new string[]
				{
					"m_LightProbeUsage",
					"m_LightProbeVolumeOverride",
					"m_ReflectionProbeUsage",
					"m_ProbeAnchor"
				};
			}
		}

		private GUIContent m_DynamicOccludeeLabel = EditorGUIUtility.TrTextContent("Dynamic Occluded", "Controls if dynamic occlusion culling should be performed for this renderer.", null);

		private static string[] m_LayerNames;

		private SerializedProperty m_SortingOrder;

		private SerializedProperty m_SortingLayerID;

		private SerializedProperty m_DynamicOccludee;

		private SerializedProperty m_RenderingLayerMask;

		private static GUIContent m_RenderingLayerMaskStyle = EditorGUIUtility.TrTextContent("Rendering Layer Mask", "Mask that can be used with SRP DrawRenderers command to filter renderers outside of the normal layering system.", null);

		protected RendererEditorBase.Probes m_Probes;

		private static string[] layerNames
		{
			get
			{
				if (RendererEditorBase.m_LayerNames == null)
				{
					RendererEditorBase.m_LayerNames = new string[32];
					for (int i = 0; i < RendererEditorBase.m_LayerNames.Length; i++)
					{
						RendererEditorBase.m_LayerNames[i] = string.Format("Layer{0}", i + 1);
					}
				}
				return RendererEditorBase.m_LayerNames;
			}
		}

		public virtual void OnEnable()
		{
			this.m_SortingOrder = base.serializedObject.FindProperty("m_SortingOrder");
			this.m_SortingLayerID = base.serializedObject.FindProperty("m_SortingLayerID");
			this.m_DynamicOccludee = base.serializedObject.FindProperty("m_DynamicOccludee");
			this.m_RenderingLayerMask = base.serializedObject.FindProperty("m_RenderingLayerMask");
		}

		protected void RenderSortingLayerFields()
		{
			EditorGUILayout.Space();
			SortingLayerEditorUtility.RenderSortingLayerFields(this.m_SortingOrder, this.m_SortingLayerID);
		}

		protected void InitializeProbeFields()
		{
			this.m_Probes = new RendererEditorBase.Probes();
			this.m_Probes.Initialize(base.serializedObject);
		}

		protected void RenderProbeFields()
		{
			this.m_Probes.OnGUI(base.targets, (Renderer)base.target, false);
		}

		protected void CullDynamicFieldGUI()
		{
			EditorGUILayout.PropertyField(this.m_DynamicOccludee, this.m_DynamicOccludeeLabel, new GUILayoutOption[0]);
		}

		protected void RenderRenderingLayer()
		{
			RendererEditorBase.RenderRenderingLayer(this.m_RenderingLayerMask, base.target as Renderer, base.targets.ToArray<UnityEngine.Object>(), false);
		}

		internal static void RenderRenderingLayer(SerializedProperty layerMask, Renderer target, UnityEngine.Object[] targets, bool useMiniStyle = false)
		{
			bool flag = GraphicsSettings.renderPipelineAsset != null;
			if (flag && !(target == null))
			{
				EditorGUI.showMixedValue = layerMask.hasMultipleDifferentValues;
				int num = (int)target.renderingLayerMask;
				EditorGUI.BeginChangeCheck();
				Rect rect = EditorGUILayout.GetControlRect(new GUILayoutOption[0]);
				EditorGUI.BeginProperty(rect, RendererEditorBase.m_RenderingLayerMaskStyle, layerMask);
				if (useMiniStyle)
				{
					rect = ModuleUI.PrefixLabel(rect, RendererEditorBase.m_RenderingLayerMaskStyle);
					num = EditorGUI.MaskField(rect, GUIContent.none, num, RendererEditorBase.layerNames, ParticleSystemStyles.Get().popup);
				}
				else
				{
					num = EditorGUI.MaskField(rect, RendererEditorBase.m_RenderingLayerMaskStyle, num, RendererEditorBase.layerNames);
				}
				EditorGUI.EndProperty();
				if (EditorGUI.EndChangeCheck())
				{
					Undo.RecordObjects(targets, "Set rendering layer mask");
					for (int i = 0; i < targets.Length; i++)
					{
						UnityEngine.Object @object = targets[i];
						Renderer renderer = @object as Renderer;
						if (renderer != null)
						{
							renderer.renderingLayerMask = (uint)num;
							EditorUtility.SetDirty(@object);
						}
					}
				}
				EditorGUI.showMixedValue = false;
			}
		}

		protected void RenderCommonProbeFields(bool useMiniStyle)
		{
			bool flag = SceneView.IsUsingDeferredRenderingPath();
			bool isDeferredReflections = flag && GraphicsSettings.GetShaderMode(BuiltinShaderType.DeferredReflections) != BuiltinShaderMode.Disabled;
			this.m_Probes.RenderReflectionProbeUsage(useMiniStyle, flag, isDeferredReflections);
			this.m_Probes.RenderProbeAnchor(useMiniStyle);
		}
	}
}
