using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityEditorInternal
{
	internal class AnimationPropertyContextualMenu
	{
		public static AnimationPropertyContextualMenu Instance = new AnimationPropertyContextualMenu();

		private IAnimationContextualResponder m_Responder;

		private static GUIContent addKeyContent = EditorGUIUtility.TrTextContent("Add Key", null, null);

		private static GUIContent updateKeyContent = EditorGUIUtility.TrTextContent("Update Key", null, null);

		private static GUIContent removeKeyContent = EditorGUIUtility.TrTextContent("Remove Key", null, null);

		private static GUIContent removeCurveContent = EditorGUIUtility.TrTextContent("Remove All Keys", null, null);

		private static GUIContent goToPreviousKeyContent = EditorGUIUtility.TrTextContent("Go to Previous Key", null, null);

		private static GUIContent goToNextKeyContent = EditorGUIUtility.TrTextContent("Go to Next Key", null, null);

		private static GUIContent addCandidatesContent = EditorGUIUtility.TrTextContent("Key All Modified", null, null);

		private static GUIContent addAnimatedContent = EditorGUIUtility.TrTextContent("Key All Animated", null, null);

		public AnimationPropertyContextualMenu()
		{
			EditorApplication.contextualPropertyMenu = (EditorApplication.SerializedPropertyCallbackFunction)Delegate.Combine(EditorApplication.contextualPropertyMenu, new EditorApplication.SerializedPropertyCallbackFunction(this.OnPropertyContextMenu));
			MaterialEditor.contextualPropertyMenu = (MaterialEditor.MaterialPropertyCallbackFunction)Delegate.Combine(MaterialEditor.contextualPropertyMenu, new MaterialEditor.MaterialPropertyCallbackFunction(this.OnPropertyContextMenu));
		}

		public void SetResponder(IAnimationContextualResponder responder)
		{
			this.m_Responder = responder;
		}

		public bool IsResponder(IAnimationContextualResponder responder)
		{
			return responder == this.m_Responder;
		}

		private void OnPropertyContextMenu(GenericMenu menu, SerializedProperty property)
		{
			if (this.m_Responder != null)
			{
				PropertyModification[] modifications = AnimationWindowUtility.SerializedPropertyToPropertyModifications(property);
				bool flag = this.m_Responder.IsAnimatable(modifications);
				if (flag)
				{
					UnityEngine.Object targetObject = property.serializedObject.targetObject;
					if (this.m_Responder.IsEditable(targetObject))
					{
						this.OnPropertyContextMenu(menu, modifications);
					}
					else
					{
						this.OnDisabledPropertyContextMenu(menu);
					}
				}
			}
		}

		private void OnPropertyContextMenu(GenericMenu menu, MaterialProperty property, Renderer[] renderers)
		{
			if (this.m_Responder != null)
			{
				if (property.targets != null && property.targets.Length != 0)
				{
					if (renderers != null && renderers.Length != 0)
					{
						List<PropertyModification> list = new List<PropertyModification>();
						for (int i = 0; i < renderers.Length; i++)
						{
							Renderer target = renderers[i];
							list.AddRange(MaterialAnimationUtility.MaterialPropertyToPropertyModifications(property, target));
						}
						if (this.m_Responder.IsEditable(renderers[0]))
						{
							this.OnPropertyContextMenu(menu, list.ToArray());
						}
						else
						{
							this.OnDisabledPropertyContextMenu(menu);
						}
					}
				}
			}
		}

		private void OnPropertyContextMenu(GenericMenu menu, PropertyModification[] modifications)
		{
			bool flag = this.m_Responder.KeyExists(modifications);
			bool flag2 = this.m_Responder.CandidateExists(modifications);
			bool flag3 = flag || this.m_Responder.CurveExists(modifications);
			bool flag4 = this.m_Responder.HasAnyCandidates();
			bool flag5 = this.m_Responder.HasAnyCurves();
			menu.AddItem((!flag || !flag2) ? AnimationPropertyContextualMenu.addKeyContent : AnimationPropertyContextualMenu.updateKeyContent, false, delegate
			{
				this.m_Responder.AddKey(modifications);
			});
			if (flag)
			{
				menu.AddItem(AnimationPropertyContextualMenu.removeKeyContent, false, delegate
				{
					this.m_Responder.RemoveKey(modifications);
				});
			}
			else
			{
				menu.AddDisabledItem(AnimationPropertyContextualMenu.removeKeyContent);
			}
			if (flag3)
			{
				menu.AddItem(AnimationPropertyContextualMenu.removeCurveContent, false, delegate
				{
					this.m_Responder.RemoveCurve(modifications);
				});
			}
			else
			{
				menu.AddDisabledItem(AnimationPropertyContextualMenu.removeCurveContent);
			}
			menu.AddSeparator(string.Empty);
			if (flag4)
			{
				menu.AddItem(AnimationPropertyContextualMenu.addCandidatesContent, false, delegate
				{
					this.m_Responder.AddCandidateKeys();
				});
			}
			else
			{
				menu.AddDisabledItem(AnimationPropertyContextualMenu.addCandidatesContent);
			}
			if (flag5)
			{
				menu.AddItem(AnimationPropertyContextualMenu.addAnimatedContent, false, delegate
				{
					this.m_Responder.AddAnimatedKeys();
				});
			}
			else
			{
				menu.AddDisabledItem(AnimationPropertyContextualMenu.addAnimatedContent);
			}
			menu.AddSeparator(string.Empty);
			if (flag3)
			{
				menu.AddItem(AnimationPropertyContextualMenu.goToPreviousKeyContent, false, delegate
				{
					this.m_Responder.GoToPreviousKeyframe(modifications);
				});
				menu.AddItem(AnimationPropertyContextualMenu.goToNextKeyContent, false, delegate
				{
					this.m_Responder.GoToNextKeyframe(modifications);
				});
			}
			else
			{
				menu.AddDisabledItem(AnimationPropertyContextualMenu.goToPreviousKeyContent);
				menu.AddDisabledItem(AnimationPropertyContextualMenu.goToNextKeyContent);
			}
		}

		private void OnDisabledPropertyContextMenu(GenericMenu menu)
		{
			menu.AddDisabledItem(AnimationPropertyContextualMenu.addKeyContent);
			menu.AddDisabledItem(AnimationPropertyContextualMenu.removeKeyContent);
			menu.AddDisabledItem(AnimationPropertyContextualMenu.removeCurveContent);
			menu.AddSeparator(string.Empty);
			menu.AddDisabledItem(AnimationPropertyContextualMenu.addCandidatesContent);
			menu.AddDisabledItem(AnimationPropertyContextualMenu.addAnimatedContent);
			menu.AddSeparator(string.Empty);
			menu.AddDisabledItem(AnimationPropertyContextualMenu.goToPreviousKeyContent);
			menu.AddDisabledItem(AnimationPropertyContextualMenu.goToNextKeyContent);
		}
	}
}
