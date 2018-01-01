using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEditor.Experimental.UIElements;
using UnityEditor.StyleSheets;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleSheets;
using UnityEngine.Scripting;

namespace UnityEditor
{
	[UsedByNativeCode]
	[StructLayout(LayoutKind.Sequential)]
	internal class GUIView : View
	{
		private Panel m_Panel = null;

		private EditorCursorManager m_CursorManager = new EditorCursorManager();

		private static EditorContextualMenuManager s_ContextualMenuManager;

		private int m_DepthBufferBits = 0;

		private EventInterests m_EventInterests;

		private bool m_AutoRepaintOnSceneChange = false;

		private bool m_BackgroundValid = false;

		[CompilerGenerated]
		private static LoadResourceFunction <>f__mg$cache0;

		[CompilerGenerated]
		private static StyleSheetApplicator.CreateDefaultCursorStyleFunction <>f__mg$cache1;

		internal static event Action<GUIView> positionChanged
		{
			add
			{
				Action<GUIView> action = GUIView.positionChanged;
				Action<GUIView> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<GUIView>>(ref GUIView.positionChanged, (Action<GUIView>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<GUIView> action = GUIView.positionChanged;
				Action<GUIView> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<GUIView>>(ref GUIView.positionChanged, (Action<GUIView>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		public static extern GUIView current
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern GUIView focusedView
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public static extern GUIView mouseOverView
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		public extern bool hasFocus
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		internal extern bool mouseRayInvisible
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		internal extern bool disableInputEvents
		{
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[GeneratedByOldBindingsGenerator]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		protected Panel panel
		{
			get
			{
				if (this.m_Panel == null)
				{
					UXMLEditorFactories.RegisterAll();
					this.m_Panel = UIElementsUtility.FindOrCreatePanel(this, ContextType.Editor, DataWatchService.sharedInstance);
					this.m_Panel.cursorManager = this.m_CursorManager;
					this.m_Panel.contextualMenuManager = GUIView.s_ContextualMenuManager;
				}
				return this.m_Panel;
			}
		}

		public VisualElement visualTree
		{
			get
			{
				return this.panel.visualTree;
			}
		}

		protected IMGUIContainer imguiContainer
		{
			get;
			private set;
		}

		public EventInterests eventInterests
		{
			get
			{
				return this.m_EventInterests;
			}
			set
			{
				this.m_EventInterests = value;
				this.panel.IMGUIEventInterests = this.m_EventInterests;
				this.Internal_SetWantsMouseMove(this.wantsMouseMove);
				this.Internal_SetWantsMouseEnterLeaveWindow(this.wantsMouseEnterLeaveWindow);
			}
		}

		public bool wantsMouseMove
		{
			get
			{
				return this.m_EventInterests.wantsMouseMove;
			}
			set
			{
				this.m_EventInterests.wantsMouseMove = value;
				this.panel.IMGUIEventInterests = this.m_EventInterests;
				this.Internal_SetWantsMouseMove(this.wantsMouseMove);
			}
		}

		public bool wantsMouseEnterLeaveWindow
		{
			get
			{
				return this.m_EventInterests.wantsMouseEnterLeaveWindow;
			}
			set
			{
				this.m_EventInterests.wantsMouseEnterLeaveWindow = value;
				this.panel.IMGUIEventInterests = this.m_EventInterests;
				this.Internal_SetWantsMouseEnterLeaveWindow(this.wantsMouseEnterLeaveWindow);
			}
		}

		internal bool backgroundValid
		{
			get
			{
				return this.m_BackgroundValid;
			}
			set
			{
				this.m_BackgroundValid = value;
			}
		}

		public bool autoRepaintOnSceneChange
		{
			get
			{
				return this.m_AutoRepaintOnSceneChange;
			}
			set
			{
				this.m_AutoRepaintOnSceneChange = value;
				this.Internal_SetAutoRepaint(this.m_AutoRepaintOnSceneChange);
			}
		}

		public int depthBufferBits
		{
			get
			{
				return this.m_DepthBufferBits;
			}
			set
			{
				this.m_DepthBufferBits = value;
			}
		}

		[Obsolete("AA is not supported on GUIViews", false)]
		public int antiAlias
		{
			get
			{
				return 1;
			}
			set
			{
			}
		}

		static GUIView()
		{
			GUIView.positionChanged = null;
			GUIView.s_ContextualMenuManager = new EditorContextualMenuManager();
			if (GUIView.<>f__mg$cache0 == null)
			{
				GUIView.<>f__mg$cache0 = new LoadResourceFunction(StyleSheetResourceUtil.LoadResource);
			}
			Panel.loadResourceFunc = GUIView.<>f__mg$cache0;
			if (GUIView.<>f__mg$cache1 == null)
			{
				GUIView.<>f__mg$cache1 = new StyleSheetApplicator.CreateDefaultCursorStyleFunction(UIElementsEditorUtility.CreateDefaultCursorStyle);
			}
			StyleSheetApplicator.createDefaultCursorStyleFunc = GUIView.<>f__mg$cache1;
			Panel.TimeSinceStartup = (() => (long)(EditorApplication.timeSinceStartup * 1000.0));
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetTitle(string title);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_Init(int depthBits);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_Recreate(int depthBits);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_Close();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool Internal_SendEvent(Event e);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void AddToAuxWindowList();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void RemoveFromAuxWindowList();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		protected extern void Internal_SetAsActiveWindow();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetWantsMouseMove(bool wantIt);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetWantsMouseEnterLeaveWindow(bool wantIt);

		public void SetInternalGameViewDimensions(Rect rect, Rect clippedRect, Vector2 targetSize)
		{
			GUIView.INTERNAL_CALL_SetInternalGameViewDimensions(this, ref rect, ref clippedRect, ref targetSize);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetInternalGameViewDimensions(GUIView self, ref Rect rect, ref Rect clippedRect, ref Vector2 targetSize);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetAsStartView();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ClearStartView();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetAutoRepaint(bool doit);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetWindow(ContainerWindow win);

		private void Internal_SetPosition(Rect windowPosition)
		{
			GUIView.INTERNAL_CALL_Internal_SetPosition(this, ref windowPosition);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Internal_SetPosition(GUIView self, ref Rect windowPosition);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Focus();

		[GeneratedByOldBindingsGenerator, ThreadAndSerializationSafe]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Repaint();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RepaintImmediately();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void CaptureRenderDoc();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void MakeVistaDWMHappyDance();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void StealMouseCapture();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void ClearKeyboardControl();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetKeyboardControl(int id);

		internal void GrabPixels(RenderTexture rd, Rect rect)
		{
			GUIView.INTERNAL_CALL_GrabPixels(this, rd, ref rect);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_GrabPixels(GUIView self, RenderTexture rd, ref Rect rect);

		internal bool SendEvent(Event e)
		{
			int num = SavedGUIState.Internal_GetGUIDepth();
			bool result;
			if (num > 0)
			{
				SavedGUIState savedGUIState = SavedGUIState.Create();
				result = this.Internal_SendEvent(e);
				savedGUIState.ApplyAndForget();
			}
			else
			{
				result = this.Internal_SendEvent(e);
			}
			return result;
		}

		protected override void SetWindow(ContainerWindow win)
		{
			base.SetWindow(win);
			this.Internal_Init(this.m_DepthBufferBits);
			if (win)
			{
				this.Internal_SetWindow(win);
			}
			this.Internal_SetAutoRepaint(this.m_AutoRepaintOnSceneChange);
			this.Internal_SetPosition(base.windowPosition);
			this.Internal_SetWantsMouseMove(this.m_EventInterests.wantsMouseMove);
			this.Internal_SetWantsMouseEnterLeaveWindow(this.m_EventInterests.wantsMouseMove);
			this.panel.visualTree.SetSize(base.windowPosition.size);
			this.m_BackgroundValid = false;
		}

		internal void RecreateContext()
		{
			this.Internal_Recreate(this.m_DepthBufferBits);
			this.m_BackgroundValid = false;
		}

		protected virtual void OnEnable()
		{
			this.imguiContainer = new IMGUIContainer(new Action(this.OldOnGUI))
			{
				useOwnerObjectGUIState = true
			};
			this.imguiContainer.StretchToParentSize();
			this.imguiContainer.persistenceKey = "Dockarea";
			this.visualTree.Insert(0, this.imguiContainer);
		}

		protected virtual void OnDisable()
		{
			if (this.imguiContainer.HasMouseCapture())
			{
				MouseCaptureController.ReleaseMouseCapture();
			}
			this.visualTree.Remove(this.imguiContainer);
		}

		protected virtual void OldOnGUI()
		{
		}

		protected virtual void OnGUI()
		{
		}

		protected override void SetPosition(Rect newPos)
		{
			Rect windowPosition = base.windowPosition;
			base.SetPosition(newPos);
			if (windowPosition == base.windowPosition)
			{
				this.Internal_SetPosition(base.windowPosition);
			}
			else
			{
				this.Internal_SetPosition(base.windowPosition);
				this.m_BackgroundValid = false;
				this.panel.visualTree.SetSize(base.windowPosition.size);
				if (GUIView.positionChanged != null)
				{
					GUIView.positionChanged(this);
				}
				this.Repaint();
			}
		}

		protected override void OnDestroy()
		{
			this.Internal_Close();
			base.OnDestroy();
		}

		internal void DoWindowDecorationStart()
		{
			if (base.window != null)
			{
				base.window.HandleWindowDecorationStart(base.windowPosition);
			}
		}

		internal void DoWindowDecorationEnd()
		{
			if (base.window != null)
			{
				base.window.HandleWindowDecorationEnd(base.windowPosition);
			}
		}
	}
}
