using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements
{
	public class ObjectField : VisualElement, INotifyValueChanged<UnityEngine.Object>
	{
		private class ObjectFieldDisplay : VisualElement
		{
			private readonly ObjectField m_ObjectField;

			private readonly Image m_ObjectIcon;

			private readonly Label m_ObjectLabel;

			public ObjectFieldDisplay(ObjectField objectField)
			{
				this.m_ObjectIcon = new Image
				{
					scaleMode = ScaleMode.ScaleAndCrop,
					pickingMode = PickingMode.Ignore
				};
				this.m_ObjectLabel = new Label
				{
					pickingMode = PickingMode.Ignore
				};
				this.m_ObjectField = objectField;
				this.Update();
				base.Add(this.m_ObjectIcon);
				base.Add(this.m_ObjectLabel);
			}

			public void Update()
			{
				GUIContent gUIContent = EditorGUIUtility.ObjectContent(this.m_ObjectField.value, this.m_ObjectField.objectType);
				this.m_ObjectIcon.image = gUIContent.image;
				this.m_ObjectLabel.text = gUIContent.text;
			}

			protected internal override void ExecuteDefaultAction(EventBase evt)
			{
				base.ExecuteDefaultAction(evt);
				if (evt.GetEventTypeId() == EventBase<MouseDownEvent>.TypeId())
				{
					this.OnMouseDown();
				}
				else if (evt.GetEventTypeId() == EventBase<IMGUIEvent>.TypeId())
				{
					this.OnIMGUI(evt);
				}
				else if (evt.GetEventTypeId() == EventBase<MouseLeaveEvent>.TypeId())
				{
					this.OnMouseLeave();
				}
			}

			private void OnMouseLeave()
			{
				base.RemoveFromClassList("acceptDrop");
			}

			private void OnMouseDown()
			{
				UnityEngine.Object @object = this.m_ObjectField.value;
				Component component = @object as Component;
				if (component)
				{
					@object = component.gameObject;
				}
				if (Event.current.clickCount == 1)
				{
					this.PingObject(@object);
				}
				else if (Event.current.clickCount == 2)
				{
					if (@object)
					{
						AssetDatabase.OpenAsset(@object);
						GUIUtility.ExitGUI();
					}
				}
			}

			private void OnIMGUI(EventBase evt)
			{
				if (evt.imguiEvent.type == EventType.DragUpdated || evt.imguiEvent.type == EventType.DragPerform)
				{
					UnityEngine.Object[] objectReferences = DragAndDrop.objectReferences;
					UnityEngine.Object @object = EditorGUI.ValidateObjectFieldAssignment(objectReferences, this.m_ObjectField.objectType, null, EditorGUI.ObjectFieldValidatorOptions.None);
					if (@object != null)
					{
						if (!this.m_ObjectField.allowSceneObjects && !EditorUtility.IsPersistent(@object))
						{
							@object = null;
						}
					}
					if (@object != null)
					{
						DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
						if (evt.imguiEvent.type == EventType.DragPerform)
						{
							this.m_ObjectField.SetValueAndNotify(@object);
							DragAndDrop.AcceptDrag();
							base.RemoveFromClassList("acceptDrop");
						}
						else
						{
							base.AddToClassList("acceptDrop");
						}
					}
				}
			}

			private void PingObject(UnityEngine.Object targetObject)
			{
				if (!(targetObject == null))
				{
					Event current = Event.current;
					if (!current.shift && !current.control)
					{
						EditorGUIUtility.PingObject(targetObject);
					}
				}
			}
		}

		private class ObjectFieldSelector : VisualElement
		{
			private readonly ObjectField m_ObjectField;

			public ObjectFieldSelector(ObjectField objectField)
			{
				this.m_ObjectField = objectField;
			}

			protected internal override void ExecuteDefaultAction(EventBase evt)
			{
				base.ExecuteDefaultAction(evt);
				if (evt.GetEventTypeId() == EventBase<MouseDownEvent>.TypeId())
				{
					this.OnMouseDown();
				}
			}

			private void OnMouseDown()
			{
				ObjectSelector.get.Show(this.m_ObjectField.value, this.m_ObjectField.objectType, null, this.m_ObjectField.allowSceneObjects, null, new Action<UnityEngine.Object>(this.m_ObjectField.OnObjectChanged), new Action<UnityEngine.Object>(this.m_ObjectField.OnObjectChanged));
			}
		}

		private UnityEngine.Object m_Value;

		private Type m_objectType;

		private readonly ObjectField.ObjectFieldDisplay m_ObjectFieldDisplay;

		public UnityEngine.Object value
		{
			get
			{
				return this.m_Value;
			}
			set
			{
				if (this.m_Value != value)
				{
					this.m_Value = value;
					this.m_ObjectFieldDisplay.Update();
				}
			}
		}

		public Type objectType
		{
			get
			{
				return this.m_objectType;
			}
			set
			{
				if (this.m_objectType != value)
				{
					this.m_objectType = value;
					this.m_ObjectFieldDisplay.Update();
				}
			}
		}

		public bool allowSceneObjects
		{
			get;
			set;
		}

		public ObjectField()
		{
			this.allowSceneObjects = true;
			this.m_ObjectFieldDisplay = new ObjectField.ObjectFieldDisplay(this)
			{
				focusIndex = 0
			};
			ObjectField.ObjectFieldSelector child = new ObjectField.ObjectFieldSelector(this);
			base.Add(this.m_ObjectFieldDisplay);
			base.Add(child);
		}

		public void SetValueAndNotify(UnityEngine.Object newValue)
		{
			if (newValue != this.value)
			{
				using (ChangeEvent<UnityEngine.Object> pooled = ChangeEvent<UnityEngine.Object>.GetPooled(this.value, newValue))
				{
					pooled.target = this;
					this.value = newValue;
					UIElementsUtility.eventDispatcher.DispatchEvent(pooled, base.panel);
				}
			}
		}

		public void OnValueChanged(EventCallback<ChangeEvent<UnityEngine.Object>> callback)
		{
			base.RegisterCallback<ChangeEvent<UnityEngine.Object>>(callback, Capture.NoCapture);
		}

		protected internal override void ExecuteDefaultAction(EventBase evt)
		{
			base.ExecuteDefaultAction(evt);
			if (evt.GetEventTypeId() == EventBase<FocusEvent>.TypeId())
			{
				this.m_ObjectFieldDisplay.Focus();
			}
		}

		private void OnObjectChanged(UnityEngine.Object obj)
		{
			this.SetValueAndNotify(obj);
		}
	}
}
