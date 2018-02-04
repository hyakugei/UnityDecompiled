using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Playables;

namespace UnityEditor
{
	[CanEditMultipleObjects, CustomEditor(typeof(PlayableDirector))]
	internal class DirectorEditor : Editor
	{
		private static class Styles
		{
			public static readonly GUIContent PlayableText = EditorGUIUtility.TrTextContent("Playable", null, null);

			public static readonly GUIContent InitialTimeContent = EditorGUIUtility.TrTextContent("Initial Time", "The time at which the Playable will begin playing", null);

			public static readonly GUIContent TimeContent = EditorGUIUtility.TrTextContent("Current Time", "The current Playable time", null);

			public static readonly GUIContent InitialStateContent = EditorGUIUtility.TrTextContent("Play On Awake", "Whether the Playable should be playing after it loads", null);

			public static readonly GUIContent UpdateMethod = EditorGUIUtility.TrTextContent("Update Method", "Controls how the Playable updates every frame", null);

			public static readonly GUIContent WrapModeContent = EditorGUIUtility.TrTextContent("Wrap Mode", "Controls the behaviour of evaluating the Playable outside its duration", null);

			public static readonly GUIContent NoBindingsContent = EditorGUIUtility.TrTextContent("This channel will not playback because it is not currently assigned", null, null);

			public static readonly GUIContent BindingsTitleContent = EditorGUIUtility.TrTextContent("Bindings", null, null);
		}

		private struct BindingPropertyPair
		{
			public PlayableBinding binding;

			public SerializedProperty property;
		}

		private SerializedProperty m_PlayableAsset;

		private SerializedProperty m_InitialState;

		private SerializedProperty m_WrapMode;

		private SerializedProperty m_InitialTime;

		private SerializedProperty m_UpdateMethod;

		private SerializedProperty m_SceneBindings;

		private GUIContent m_AnimatorContent;

		private GUIContent m_AudioContent;

		private GUIContent m_VideoContent;

		private GUIContent m_ScriptContent;

		private Texture m_DefaultScriptContentTexture;

		private List<DirectorEditor.BindingPropertyPair> m_BindingPropertiesCache = new List<DirectorEditor.BindingPropertyPair>();

		private PlayableBinding[] m_SynchedPlayableBindings = null;

		public void OnEnable()
		{
			this.m_PlayableAsset = base.serializedObject.FindProperty("m_PlayableAsset");
			this.m_InitialState = base.serializedObject.FindProperty("m_InitialState");
			this.m_WrapMode = base.serializedObject.FindProperty("m_WrapMode");
			this.m_UpdateMethod = base.serializedObject.FindProperty("m_DirectorUpdateMode");
			this.m_InitialTime = base.serializedObject.FindProperty("m_InitialTime");
			this.m_SceneBindings = base.serializedObject.FindProperty("m_SceneBindings");
			this.m_AnimatorContent = new GUIContent(AssetPreview.GetMiniTypeThumbnail(typeof(Animator)));
			this.m_AudioContent = new GUIContent(AssetPreview.GetMiniTypeThumbnail(typeof(AudioSource)));
			this.m_VideoContent = new GUIContent(AssetPreview.GetMiniTypeThumbnail(typeof(RenderTexture)));
			this.m_ScriptContent = new GUIContent(EditorGUIUtility.FindTexture(typeof(ScriptableObject)));
			this.m_DefaultScriptContentTexture = this.m_ScriptContent.image;
		}

		public override void OnInspectorGUI()
		{
			if (this.PlayableAssetOutputsChanged())
			{
				this.SynchSceneBindings();
			}
			base.serializedObject.Update();
			if (DirectorEditor.PropertyFieldAsObject(this.m_PlayableAsset, DirectorEditor.Styles.PlayableText, typeof(PlayableAsset), false, false))
			{
				base.serializedObject.ApplyModifiedProperties();
				this.SynchSceneBindings();
				InternalEditorUtility.RepaintAllViews();
			}
			EditorGUILayout.PropertyField(this.m_UpdateMethod, DirectorEditor.Styles.UpdateMethod, new GUILayoutOption[0]);
			Rect controlRect = EditorGUILayout.GetControlRect(true, new GUILayoutOption[0]);
			GUIContent label = EditorGUI.BeginProperty(controlRect, DirectorEditor.Styles.InitialStateContent, this.m_InitialState);
			bool flag = this.m_InitialState.enumValueIndex != 0;
			EditorGUI.BeginChangeCheck();
			flag = EditorGUI.Toggle(controlRect, label, flag);
			if (EditorGUI.EndChangeCheck())
			{
				this.m_InitialState.enumValueIndex = ((!flag) ? 0 : 1);
			}
			EditorGUI.EndProperty();
			EditorGUILayout.PropertyField(this.m_WrapMode, DirectorEditor.Styles.WrapModeContent, new GUILayoutOption[0]);
			DirectorEditor.PropertyFieldAsFloat(this.m_InitialTime, DirectorEditor.Styles.InitialTimeContent);
			if (Application.isPlaying)
			{
				this.CurrentTimeField();
			}
			if (base.targets.Length == 1)
			{
				PlayableAsset x = this.m_PlayableAsset.objectReferenceValue as PlayableAsset;
				if (x != null)
				{
					this.DoDirectorBindingInspector();
				}
			}
			base.serializedObject.ApplyModifiedProperties();
		}

		private bool PlayableAssetOutputsChanged()
		{
			PlayableAsset playableAsset = this.m_PlayableAsset.objectReferenceValue as PlayableAsset;
			bool result;
			if (this.m_SynchedPlayableBindings == null)
			{
				result = (playableAsset != null);
			}
			else
			{
				result = (playableAsset == null || playableAsset.outputs.Count<PlayableBinding>() != this.m_SynchedPlayableBindings.Length || playableAsset.outputs.Where((PlayableBinding t, int i) => t.sourceObject != this.m_SynchedPlayableBindings[i].sourceObject).Any<PlayableBinding>());
			}
			return result;
		}

		private void BindingInspector(SerializedProperty bindingProperty, PlayableBinding binding)
		{
			if (!(binding.sourceObject == null))
			{
				UnityEngine.Object objectReferenceValue = bindingProperty.objectReferenceValue;
				if (binding.streamType == DataStreamType.Audio)
				{
					this.m_AudioContent.text = binding.streamName;
					this.m_AudioContent.tooltip = ((!(objectReferenceValue == null)) ? string.Empty : DirectorEditor.Styles.NoBindingsContent.text);
					DirectorEditor.PropertyFieldAsObject(bindingProperty, this.m_AudioContent, typeof(AudioSource), true, false);
				}
				else if (binding.streamType == DataStreamType.Animation)
				{
					this.m_AnimatorContent.text = binding.streamName;
					this.m_AnimatorContent.tooltip = ((!(objectReferenceValue is GameObject)) ? string.Empty : DirectorEditor.Styles.NoBindingsContent.text);
					DirectorEditor.PropertyFieldAsObject(bindingProperty, this.m_AnimatorContent, typeof(Animator), true, true);
				}
				if (binding.streamType == DataStreamType.Texture)
				{
					this.m_VideoContent.text = binding.streamName;
					this.m_VideoContent.tooltip = ((!(objectReferenceValue == null)) ? string.Empty : DirectorEditor.Styles.NoBindingsContent.text);
					DirectorEditor.PropertyFieldAsObject(bindingProperty, this.m_VideoContent, typeof(RenderTexture), false, false);
				}
				else if (binding.streamType == DataStreamType.None)
				{
					this.m_ScriptContent.text = binding.streamName;
					this.m_ScriptContent.tooltip = ((!(objectReferenceValue == null)) ? string.Empty : DirectorEditor.Styles.NoBindingsContent.text);
					this.m_ScriptContent.image = (AssetPreview.GetMiniTypeThumbnail(binding.sourceBindingType) ?? this.m_DefaultScriptContentTexture);
					if (binding.sourceBindingType != null && typeof(UnityEngine.Object).IsAssignableFrom(binding.sourceBindingType))
					{
						DirectorEditor.PropertyFieldAsObject(bindingProperty, this.m_ScriptContent, binding.sourceBindingType, true, false);
					}
				}
			}
		}

		private void DoDirectorBindingInspector()
		{
			if (this.m_BindingPropertiesCache.Any<DirectorEditor.BindingPropertyPair>())
			{
				this.m_SceneBindings.isExpanded = EditorGUILayout.Foldout(this.m_SceneBindings.isExpanded, DirectorEditor.Styles.BindingsTitleContent);
				if (this.m_SceneBindings.isExpanded)
				{
					EditorGUI.indentLevel++;
					foreach (DirectorEditor.BindingPropertyPair current in this.m_BindingPropertiesCache)
					{
						this.BindingInspector(current.property, current.binding);
					}
					EditorGUI.indentLevel--;
				}
			}
		}

		private void SynchSceneBindings()
		{
			if (base.targets.Length <= 1)
			{
				PlayableDirector playableDirector = (PlayableDirector)base.target;
				PlayableAsset playableAsset = this.m_PlayableAsset.objectReferenceValue as PlayableAsset;
				this.m_BindingPropertiesCache.Clear();
				this.m_SynchedPlayableBindings = null;
				if (!(playableAsset == null))
				{
					IEnumerable<PlayableBinding> outputs = playableAsset.outputs;
					this.m_SynchedPlayableBindings = outputs.ToArray<PlayableBinding>();
					PlayableBinding[] synchedPlayableBindings = this.m_SynchedPlayableBindings;
					for (int i = 0; i < synchedPlayableBindings.Length; i++)
					{
						PlayableBinding playableBinding = synchedPlayableBindings[i];
						if (!playableDirector.HasGenericBinding(playableBinding.sourceObject))
						{
							playableDirector.SetGenericBinding(playableBinding.sourceObject, null);
						}
					}
					base.serializedObject.Update();
					SerializedProperty[] array = new SerializedProperty[this.m_SceneBindings.arraySize];
					for (int j = 0; j < this.m_SceneBindings.arraySize; j++)
					{
						array[j] = this.m_SceneBindings.GetArrayElementAtIndex(j);
					}
					PlayableBinding[] synchedPlayableBindings2 = this.m_SynchedPlayableBindings;
					for (int k = 0; k < synchedPlayableBindings2.Length; k++)
					{
						PlayableBinding binding = synchedPlayableBindings2[k];
						SerializedProperty[] array2 = array;
						for (int l = 0; l < array2.Length; l++)
						{
							SerializedProperty serializedProperty = array2[l];
							if (serializedProperty.FindPropertyRelative("key").objectReferenceValue == binding.sourceObject)
							{
								this.m_BindingPropertiesCache.Add(new DirectorEditor.BindingPropertyPair
								{
									binding = binding,
									property = serializedProperty.FindPropertyRelative("value")
								});
								break;
							}
						}
					}
				}
			}
		}

		public override bool RequiresConstantRepaint()
		{
			return Application.isPlaying;
		}

		private static void PropertyFieldAsFloat(SerializedProperty property, GUIContent title)
		{
			Rect controlRect = EditorGUILayout.GetControlRect(new GUILayoutOption[0]);
			title = EditorGUI.BeginProperty(controlRect, title, property);
			EditorGUI.BeginChangeCheck();
			float num = EditorGUI.FloatField(controlRect, title, (float)property.doubleValue);
			if (EditorGUI.EndChangeCheck())
			{
				property.doubleValue = (double)num;
			}
			EditorGUI.EndProperty();
		}

		private static bool PropertyFieldAsObject(SerializedProperty property, GUIContent title, Type objType, bool allowSceneObjects, bool useBehaviourGameObject = false)
		{
			Rect controlRect = EditorGUILayout.GetControlRect(new GUILayoutOption[0]);
			GUIContent label = EditorGUI.BeginProperty(controlRect, title, property);
			EditorGUI.BeginChangeCheck();
			UnityEngine.Object @object = EditorGUI.ObjectField(controlRect, label, property.objectReferenceValue, objType, allowSceneObjects);
			bool flag = EditorGUI.EndChangeCheck();
			if (flag)
			{
				if (useBehaviourGameObject)
				{
					Behaviour behaviour = @object as Behaviour;
					property.objectReferenceValue = ((!(behaviour != null)) ? null : behaviour.gameObject);
				}
				else
				{
					property.objectReferenceValue = @object;
				}
			}
			EditorGUI.EndProperty();
			return flag;
		}

		private void CurrentTimeField()
		{
			if (base.targets.Length == 1)
			{
				PlayableDirector playableDirector = (PlayableDirector)base.target;
				EditorGUI.BeginChangeCheck();
				float num = EditorGUILayout.FloatField(DirectorEditor.Styles.TimeContent, (float)playableDirector.time, new GUILayoutOption[0]);
				if (EditorGUI.EndChangeCheck())
				{
					playableDirector.time = (double)num;
				}
			}
			else
			{
				EditorGUILayout.TextField(DirectorEditor.Styles.TimeContent, EditorGUI.mixedValueContent.text, new GUILayoutOption[0]);
			}
		}
	}
}
