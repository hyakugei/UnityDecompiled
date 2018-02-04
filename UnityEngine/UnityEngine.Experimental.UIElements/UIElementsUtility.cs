using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UnityEngine.Experimental.UIElements
{
	internal class UIElementsUtility
	{
		private static Stack<IMGUIContainer> s_ContainerStack;

		private static Dictionary<int, Panel> s_UIElementsCache;

		private static Event s_EventInstance;

		private static EventDispatcher s_EventDispatcher;

		internal static Action<IMGUIContainer> s_BeginContainerCallback;

		internal static Action<IMGUIContainer> s_EndContainerCallback;

		/*
		[CompilerGenerated]
		private static Action <>f__mg$cache0;

		[CompilerGenerated]
		private static Action <>f__mg$cache1;

		[CompilerGenerated]
		private static Func<int, IntPtr, bool> <>f__mg$cache2;

		[CompilerGenerated]
		private static Action <>f__mg$cache3;

		[CompilerGenerated]
		private static Func<Exception, bool> <>f__mg$cache4;
		*/

		internal static IEventDispatcher eventDispatcher
		{
			get
			{
				if (UIElementsUtility.s_EventDispatcher == null)
				{
					UIElementsUtility.s_EventDispatcher = new EventDispatcher();
				}
				return UIElementsUtility.s_EventDispatcher;
			}
		}

		static UIElementsUtility()
		{
			/*
			UIElementsUtility.s_ContainerStack = new Stack<IMGUIContainer>();
			UIElementsUtility.s_UIElementsCache = new Dictionary<int, Panel>();
			UIElementsUtility.s_EventInstance = new Event();
			Delegate arg_41_0 = GUIUtility.takeCapture;
			if (UIElementsUtility.<>f__mg$cache0 == null)
			{
				UIElementsUtility.<>f__mg$cache0 = new Action(UIElementsUtility.TakeCapture);
			}
			GUIUtility.takeCapture = (Action)Delegate.Combine(arg_41_0, UIElementsUtility.<>f__mg$cache0);
			Delegate arg_72_0 = GUIUtility.releaseCapture;
			if (UIElementsUtility.<>f__mg$cache1 == null)
			{
				UIElementsUtility.<>f__mg$cache1 = new Action(UIElementsUtility.ReleaseCapture);
			}
			GUIUtility.releaseCapture = (Action)Delegate.Combine(arg_72_0, UIElementsUtility.<>f__mg$cache1);
			Delegate arg_A3_0 = GUIUtility.processEvent;
			if (UIElementsUtility.<>f__mg$cache2 == null)
			{
				UIElementsUtility.<>f__mg$cache2 = new Func<int, IntPtr, bool>(UIElementsUtility.ProcessEvent);
			}
			GUIUtility.processEvent = (Func<int, IntPtr, bool>)Delegate.Combine(arg_A3_0, UIElementsUtility.<>f__mg$cache2);
			Delegate arg_D4_0 = GUIUtility.cleanupRoots;
			if (UIElementsUtility.<>f__mg$cache3 == null)
			{
				UIElementsUtility.<>f__mg$cache3 = new Action(UIElementsUtility.CleanupRoots);
			}
			GUIUtility.cleanupRoots = (Action)Delegate.Combine(arg_D4_0, UIElementsUtility.<>f__mg$cache3);
			Delegate arg_105_0 = GUIUtility.endContainerGUIFromException;
			if (UIElementsUtility.<>f__mg$cache4 == null)
			{
				UIElementsUtility.<>f__mg$cache4 = new Func<Exception, bool>(UIElementsUtility.EndContainerGUIFromException);
			}
			GUIUtility.endContainerGUIFromException = (Func<Exception, bool>)Delegate.Combine(arg_105_0, UIElementsUtility.<>f__mg$cache4);
			*/
		}

		internal static void ClearDispatcher()
		{
			UIElementsUtility.s_EventDispatcher = null;
		}

		private static void TakeCapture()
		{
			if (UIElementsUtility.s_ContainerStack.Count > 0)
			{
				IMGUIContainer iMGUIContainer = UIElementsUtility.s_ContainerStack.Peek();
				if (iMGUIContainer.GUIDepth == GUIUtility.Internal_GetGUIDepth())
				{
					if (MouseCaptureController.IsMouseCaptureTaken() && !iMGUIContainer.HasMouseCapture())
					{
						Debug.Log("Should not grab hot control with an active capture");
					}
					iMGUIContainer.TakeMouseCapture();
				}
			}
		}

		private static void ReleaseCapture()
		{
			MouseCaptureController.ReleaseMouseCapture();
		}

		private static bool ProcessEvent(int instanceID, IntPtr nativeEventPtr)
		{
			Panel panel;
			bool result;
			if (nativeEventPtr != IntPtr.Zero && UIElementsUtility.s_UIElementsCache.TryGetValue(instanceID, out panel))
			{
				UIElementsUtility.s_EventInstance.CopyFromPtr(nativeEventPtr);
				result = UIElementsUtility.DoDispatch(panel);
			}
			else
			{
				result = false;
			}
			return result;
		}

		private static void CleanupRoots()
		{
			UIElementsUtility.s_EventInstance = null;
			UIElementsUtility.s_EventDispatcher = null;
			UIElementsUtility.s_UIElementsCache = null;
			UIElementsUtility.s_ContainerStack = null;
			UIElementsUtility.s_BeginContainerCallback = null;
			UIElementsUtility.s_EndContainerCallback = null;
		}

		private static bool EndContainerGUIFromException(Exception exception)
		{
			if (UIElementsUtility.s_ContainerStack.Count > 0)
			{
				GUIUtility.EndContainer();
				UIElementsUtility.s_ContainerStack.Pop();
			}
			return GUIUtility.ShouldRethrowException(exception);
		}

		internal static void BeginContainerGUI(GUILayoutUtility.LayoutCache cache, Event evt, IMGUIContainer container)
		{
			if (container.useOwnerObjectGUIState)
			{
				GUIUtility.BeginContainerFromOwner(container.elementPanel.ownerObject);
			}
			else
			{
				GUIUtility.BeginContainer(container.guiState);
			}
			UIElementsUtility.s_ContainerStack.Push(container);
			GUIUtility.s_SkinMode = (int)container.contextType;
			GUIUtility.s_OriginalID = container.elementPanel.ownerObject.GetInstanceID();
			Event.current = evt;
			if (UIElementsUtility.s_BeginContainerCallback != null)
			{
				UIElementsUtility.s_BeginContainerCallback(container);
			}
			GUI.enabled = container.enabledInHierarchy;
			GUILayoutUtility.BeginContainer(cache);
			GUIUtility.ResetGlobalState();
		}

		internal static void EndContainerGUI()
		{
			if (Event.current.type == EventType.Layout && UIElementsUtility.s_ContainerStack.Count > 0)
			{
				Rect layout = UIElementsUtility.s_ContainerStack.Peek().layout;
				GUILayoutUtility.LayoutFromContainer(layout.width, layout.height);
			}
			GUILayoutUtility.SelectIDList(GUIUtility.s_OriginalID, false);
			GUIContent.ClearStaticCache();
			if (UIElementsUtility.s_ContainerStack.Count > 0)
			{
				IMGUIContainer obj = UIElementsUtility.s_ContainerStack.Peek();
				if (UIElementsUtility.s_EndContainerCallback != null)
				{
					UIElementsUtility.s_EndContainerCallback(obj);
				}
				GUIUtility.EndContainer();
				UIElementsUtility.s_ContainerStack.Pop();
			}
		}

		internal static ContextType GetGUIContextType()
		{
			return (GUIUtility.s_SkinMode != 0) ? ContextType.Editor : ContextType.Player;
		}

		internal static EventBase CreateEvent(Event systemEvent)
		{
			EventType type = systemEvent.type;
			EventBase pooled;
			switch (type)
			{
			case EventType.MouseDown:
				pooled = MouseEventBase<MouseDownEvent>.GetPooled(systemEvent);
				break;
			case EventType.MouseUp:
				pooled = MouseEventBase<MouseUpEvent>.GetPooled(systemEvent);
				break;
			case EventType.MouseMove:
				pooled = MouseEventBase<MouseMoveEvent>.GetPooled(systemEvent);
				break;
			case EventType.MouseDrag:
				pooled = MouseEventBase<MouseMoveEvent>.GetPooled(systemEvent);
				break;
			case EventType.KeyDown:
				pooled = KeyboardEventBase<KeyDownEvent>.GetPooled(systemEvent);
				break;
			case EventType.KeyUp:
				pooled = KeyboardEventBase<KeyUpEvent>.GetPooled(systemEvent);
				break;
			case EventType.ScrollWheel:
				pooled = WheelEvent.GetPooled(systemEvent);
				break;
			default:
				if (type != EventType.MouseEnterWindow)
				{
					if (type != EventType.MouseLeaveWindow)
					{
						pooled = IMGUIEvent.GetPooled(systemEvent);
					}
					else
					{
						pooled = MouseEventBase<MouseLeaveWindowEvent>.GetPooled(systemEvent);
					}
				}
				else
				{
					pooled = MouseEventBase<MouseEnterWindowEvent>.GetPooled(systemEvent);
				}
				break;
			}
			return pooled;
		}

		private static bool DoDispatch(BaseVisualElementPanel panel)
		{
			bool result;
			if (UIElementsUtility.s_EventInstance.type == EventType.Repaint)
			{
				bool sRGBWrite = GL.sRGBWrite;
				if (sRGBWrite)
				{
					GL.sRGBWrite = false;
				}
				panel.Repaint(UIElementsUtility.s_EventInstance);
				if (sRGBWrite)
				{
					GL.sRGBWrite = true;
				}
				result = (panel.IMGUIContainersCount > 0);
			}
			else
			{
				panel.ValidateLayout();
				using (EventBase eventBase = UIElementsUtility.CreateEvent(UIElementsUtility.s_EventInstance))
				{
					UIElementsUtility.s_EventDispatcher.DispatchEvent(eventBase, panel);
					UIElementsUtility.s_EventInstance.mousePosition = eventBase.originalMousePosition;
					if (eventBase.isPropagationStopped)
					{
						panel.visualTree.Dirty(ChangeType.Repaint);
					}
					result = eventBase.isPropagationStopped;
				}
			}
			return result;
		}

		internal static Dictionary<int, Panel>.Enumerator GetPanelsIterator()
		{
			return UIElementsUtility.s_UIElementsCache.GetEnumerator();
		}

		internal static Panel FindOrCreatePanel(ScriptableObject ownerObject, ContextType contextType, IDataWatchService dataWatch = null)
		{
			Panel panel;
			if (!UIElementsUtility.s_UIElementsCache.TryGetValue(ownerObject.GetInstanceID(), out panel))
			{
				panel = new Panel(ownerObject, contextType, dataWatch, UIElementsUtility.eventDispatcher);
				UIElementsUtility.s_UIElementsCache.Add(ownerObject.GetInstanceID(), panel);
			}
			else
			{
				Debug.Assert(contextType == panel.contextType, "Context type mismatch");
			}
			return panel;
		}

		internal static Panel FindOrCreatePanel(ScriptableObject ownerObject)
		{
			return UIElementsUtility.FindOrCreatePanel(ownerObject, UIElementsUtility.GetGUIContextType(), null);
		}
	}
}
