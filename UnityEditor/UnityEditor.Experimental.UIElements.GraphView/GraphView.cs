using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	public abstract class GraphView : DataWatchContainer, ISelection
	{
		private class Layer : VisualElement
		{
		}

		public delegate GraphViewChange GraphViewChanged(GraphViewChange graphViewChange);

		public delegate void GroupNodeTitleChanged(GroupNode groupNode, string title);

		public delegate void ElementAddedToGroupNode(GroupNode groupNode, GraphElement element);

		public delegate void ElementRemovedFromGroupNode(GroupNode groupNode, GraphElement element);

		public delegate void ElementResized(VisualElement visualElement);

		public delegate void ViewTransformChanged(GraphView graphView);

		[Serializable]
		private class PersistedSelection : IGraphViewSelection
		{
			[SerializeField]
			private int m_Version;

			[SerializeField]
			private List<string> m_SelectedElements;

			public int version
			{
				get
				{
					return this.m_Version;
				}
				set
				{
					this.m_Version = value;
				}
			}

			public List<string> selectedElements
			{
				get
				{
					return this.m_SelectedElements;
				}
				set
				{
					this.m_SelectedElements = value;
				}
			}
		}

		private class ContentViewContainer : VisualElement
		{
			public override bool Overlaps(Rect r)
			{
				return true;
			}
		}

		public enum FrameType
		{
			All,
			Selection,
			Origin
		}

		[Serializable]
		private class PersistedViewTransform
		{
			public Vector3 position = Vector3.zero;

			public Vector3 scale = Vector3.one;
		}

		public enum AskUser
		{
			AskUser,
			DontAskUser
		}

		public delegate string SerializeGraphElementsDelegate(IEnumerable<GraphElement> elements);

		public delegate bool CanPasteSerializedDataDelegate(string data);

		public delegate void UnserializeAndPasteDelegate(string operationName, string data);

		public delegate void DeleteSelectionDelegate(string operationName, GraphView.AskUser askUser);

		private GraphViewPresenter m_Presenter;

		private GraphViewChange m_GraphViewChange;

		private List<GraphElement> m_ElementsToRemove;

		private const string k_SelectionUndoRedoLabel = "Change GraphView Selection";

		private int m_SavedSelectionVersion;

		private GraphView.PersistedSelection m_PersistedSelection;

		private GraphViewUndoRedoSelection m_GraphViewUndoRedoSelection;

		private bool m_FontsOverridden = false;

		private bool m_FrameAnimate = false;

		private readonly int k_FrameBorder = 30;

		private readonly float k_ContentViewWidth = 10000f;

		private readonly Dictionary<int, GraphView.Layer> m_ContainerLayers = new Dictionary<int, GraphView.Layer>();

		private IVisualElementScheduledItem m_OnTimerTicker;

		public UQuery.QueryState<Port> ports;

		private GraphView.PersistedViewTransform m_PersistedViewTransform;

		private ContentZoomer m_Zoomer;

		private int m_ZoomerMaxElementCountWithPixelCacheRegen = 100;

		private float m_MinScale = ContentZoomer.DefaultMinScale;

		private float m_MaxScale = ContentZoomer.DefaultMaxScale;

		private float m_ScaleStep = ContentZoomer.DefaultScaleStep;

		private float m_ReferenceScale = ContentZoomer.DefaultReferenceScale;

		internal bool m_UseInternalClipboard = false;

		private string m_Clipboard = string.Empty;

		private const string m_SerializedDataMimeType = "application/vnd.unity.graphview.elements";

		[CompilerGenerated]
		private static Func<EventBase, ContextualMenu.MenuAction.StatusFlags> <>f__mg$cache0;

		public Action<NodeCreationContext> nodeCreationRequest
		{
			get;
			set;
		}

		public GraphView.GraphViewChanged graphViewChanged
		{
			get;
			set;
		}

		public GraphView.GroupNodeTitleChanged groupNodeTitleChanged
		{
			get;
			set;
		}

		public GraphView.ElementAddedToGroupNode elementAddedToGroupNode
		{
			get;
			set;
		}

		public GraphView.ElementRemovedFromGroupNode elementRemovedFromGroupNode
		{
			get;
			set;
		}

		public GraphView.ElementResized elementResized
		{
			get;
			set;
		}

		public GraphView.ViewTransformChanged viewTransformChanged
		{
			get;
			set;
		}

		public GraphViewPresenter presenter
		{
			get
			{
				return this.m_Presenter;
			}
			set
			{
				if (!(this.m_Presenter == value))
				{
					base.RemoveWatch();
					this.m_Presenter = value;
					this.OnDataChanged();
					base.AddWatch();
				}
			}
		}

		protected GraphViewTypeFactory typeFactory
		{
			get;
			set;
		}

		public VisualElement contentViewContainer
		{
			get;
			private set;
		}

		public VisualElement viewport
		{
			get
			{
				return this;
			}
		}

		public ITransform viewTransform
		{
			get
			{
				return this.contentViewContainer.transform;
			}
		}

		public bool isReframable
		{
			get;
			set;
		}

		public UQuery.QueryState<GraphElement> graphElements
		{
			get;
			private set;
		}

		public UQuery.QueryState<Node> nodes
		{
			get;
			private set;
		}

		public UQuery.QueryState<Edge> edges
		{
			get;
			private set;
		}

		public float minScale
		{
			get
			{
				return this.m_MinScale;
			}
		}

		public float maxScale
		{
			get
			{
				return this.m_MaxScale;
			}
		}

		public float scaleStep
		{
			get
			{
				return this.m_ScaleStep;
			}
		}

		public float referenceScale
		{
			get
			{
				return this.m_ReferenceScale;
			}
		}

		public float scale
		{
			get
			{
				return this.viewTransform.scale.x;
			}
		}

		public int zoomerMaxElementCountWithPixelCacheRegen
		{
			get
			{
				return this.m_ZoomerMaxElementCountWithPixelCacheRegen;
			}
			set
			{
				if (this.m_ZoomerMaxElementCountWithPixelCacheRegen != value)
				{
					this.m_ZoomerMaxElementCountWithPixelCacheRegen = value;
					if (this.m_Presenter != null)
					{
						this.m_Zoomer.keepPixelCacheOnZoom = (this.m_Presenter.elements.Count<GraphElementPresenter>() > this.m_ZoomerMaxElementCountWithPixelCacheRegen);
					}
				}
			}
		}

		protected override UnityEngine.Object[] toWatch
		{
			get
			{
				object arg_27_0;
				if (this.presenter == null)
				{
					arg_27_0 = null;
				}
				else
				{
					(arg_27_0 = new UnityEngine.Object[1])[0] = this.presenter;
				}
				return arg_27_0;
			}
		}

		public List<ISelectable> selection
		{
			get;
			protected set;
		}

		internal string clipboard
		{
			get
			{
				string result;
				if (this.m_UseInternalClipboard)
				{
					result = this.m_Clipboard;
				}
				else
				{
					result = EditorGUIUtility.systemCopyBuffer;
				}
				return result;
			}
			set
			{
				if (this.m_UseInternalClipboard)
				{
					this.m_Clipboard = value;
				}
				else
				{
					EditorGUIUtility.systemCopyBuffer = value;
				}
			}
		}

		protected internal virtual bool canCopySelection
		{
			get
			{
				return this.selection.OfType<Node>().Any<Node>() || this.selection.OfType<GroupNode>().Any<GroupNode>();
			}
		}

		protected internal virtual bool canCutSelection
		{
			get
			{
				return this.selection.Count > 0;
			}
		}

		protected internal virtual bool canPaste
		{
			get
			{
				return this.CanPasteSerializedData(this.clipboard);
			}
		}

		protected internal virtual bool canDuplicateSelection
		{
			get
			{
				return this.canCopySelection;
			}
		}

		protected internal virtual bool canDeleteSelection
		{
			get
			{
				return (from GraphElement e in this.selection
				where e != null && (e.capabilities & Capabilities.Deletable) != (Capabilities)0
				select e).Any<GraphElement>();
			}
		}

		public GraphView.SerializeGraphElementsDelegate serializeGraphElements
		{
			get;
			set;
		}

		public GraphView.CanPasteSerializedDataDelegate canPasteSerializedData
		{
			get;
			set;
		}

		public GraphView.UnserializeAndPasteDelegate unserializeAndPaste
		{
			get;
			set;
		}

		public GraphView.DeleteSelectionDelegate deleteSelection
		{
			get;
			set;
		}

		protected GraphView()
		{
			this.selection = new List<ISelectable>();
			base.clippingOptions = VisualElement.ClippingOptions.ClipContents;
			this.contentViewContainer = new GraphView.ContentViewContainer
			{
				name = "contentViewContainer",
				clippingOptions = VisualElement.ClippingOptions.NoClipping,
				pickingMode = PickingMode.Ignore,
				style = 
				{
					width = this.k_ContentViewWidth
				}
			};
			base.Add(this.contentViewContainer);
			this.typeFactory = new GraphViewTypeFactory();
			this.typeFactory[typeof(EdgePresenter)] = typeof(Edge);
			base.AddStyleSheetPath("StyleSheets/GraphView/GraphView.uss");
			this.graphElements = this.contentViewContainer.Query().Children<GraphView.Layer>(null, null).Children<GraphElement>(null, null).Build();
			this.nodes = this.Query(null, null).Children<Node>(null, null).Build();
			this.edges = this.Query(null, null).Children<Edge>(null, null).Build();
			this.ports = this.contentViewContainer.Query().Children<GraphView.Layer>(null, null).Descendents<Port>(null, null).Build();
			this.m_ElementsToRemove = new List<GraphElement>();
			this.m_GraphViewChange.elementsToRemove = this.m_ElementsToRemove;
			this.isReframable = true;
			base.focusIndex = 0;
			base.RegisterCallback<IMGUIEvent>(new EventCallback<IMGUIEvent>(this.OnValidateCommand), Capture.NoCapture);
			base.RegisterCallback<IMGUIEvent>(new EventCallback<IMGUIEvent>(this.OnExecuteCommand), Capture.NoCapture);
			base.RegisterCallback<AttachToPanelEvent>(new EventCallback<AttachToPanelEvent>(this.OnEnterPanel), Capture.NoCapture);
			base.RegisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.OnLeavePanel), Capture.NoCapture);
			base.RegisterCallback<ContextualMenuPopulateEvent>(new EventCallback<ContextualMenuPopulateEvent>(this.OnContextualMenu), Capture.NoCapture);
			if (!this.m_FontsOverridden && (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor))
			{
				Font font = EditorGUIUtility.LoadRequired("GraphView/DummyFont(LucidaGrande).ttf") as Font;
				if (Application.platform == RuntimePlatform.WindowsEditor)
				{
					font.fontNames = new string[]
					{
						"Segoe UI",
						"Helvetica Neue",
						"Helvetica",
						"Arial",
						"Verdana"
					};
				}
				else if (Application.platform == RuntimePlatform.OSXEditor)
				{
					font.fontNames = new string[]
					{
						"Helvetica Neue",
						"Lucida Grande"
					};
				}
				this.m_FontsOverridden = true;
			}
		}

		public T GetPresenter<T>() where T : GraphViewPresenter
		{
			return this.presenter as T;
		}

		public void UpdateViewTransform(Vector3 newPosition, Vector3 newScale)
		{
			this.contentViewContainer.transform.position = newPosition;
			this.contentViewContainer.transform.scale = newScale;
			if (this.m_Presenter != null)
			{
				this.m_Presenter.position = newPosition;
				this.m_Presenter.scale = newScale;
			}
			this.UpdatePersistedViewTransform();
			if (this.viewTransformChanged != null)
			{
				this.viewTransformChanged(this);
			}
		}

		internal override void ChangePanel(BaseVisualElementPanel p)
		{
			if (p != base.panel)
			{
				if (p == null)
				{
					Undo.ClearUndo(this.m_GraphViewUndoRedoSelection);
					Undo.undoRedoPerformed = (Undo.UndoRedoCallback)Delegate.Remove(Undo.undoRedoPerformed, new Undo.UndoRedoCallback(this.UndoRedoPerformed));
					UnityEngine.Object.DestroyImmediate(this.m_GraphViewUndoRedoSelection);
					this.m_GraphViewUndoRedoSelection = null;
					if (!EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
					{
						this.ClearSavedSelection();
					}
				}
				else if (base.panel == null)
				{
					Undo.undoRedoPerformed = (Undo.UndoRedoCallback)Delegate.Combine(Undo.undoRedoPerformed, new Undo.UndoRedoCallback(this.UndoRedoPerformed));
					this.m_GraphViewUndoRedoSelection = ScriptableObject.CreateInstance<GraphViewUndoRedoSelection>();
					this.m_GraphViewUndoRedoSelection.selectedElements = new List<string>();
					this.m_GraphViewUndoRedoSelection.hideFlags = HideFlags.HideAndDontSave;
				}
				base.ChangePanel(p);
			}
		}

		private void ClearSavedSelection()
		{
			if (this.m_PersistedSelection != null)
			{
				this.m_PersistedSelection.selectedElements.Clear();
				base.SavePersistentData();
			}
		}

		private bool ShouldRecordUndo()
		{
			return this.m_GraphViewUndoRedoSelection != null && this.m_PersistedSelection != null && this.m_SavedSelectionVersion == this.m_GraphViewUndoRedoSelection.version;
		}

		private void RestoreSavedSelection(IGraphViewSelection graphViewSelection)
		{
			if (graphViewSelection.version != this.m_SavedSelectionVersion)
			{
				this.m_GraphViewUndoRedoSelection.version = graphViewSelection.version;
				this.m_PersistedSelection.version = graphViewSelection.version;
				this.ClearSelection();
				List<string> list = null;
				foreach (string current in graphViewSelection.selectedElements)
				{
					GraphElement elementByGuid = this.GetElementByGuid(current);
					if (elementByGuid == null)
					{
						if (list == null)
						{
							list = new List<string>();
						}
						list.Add(current);
					}
					else
					{
						this.AddToSelection(elementByGuid);
					}
				}
				if (list != null)
				{
					foreach (string current2 in list)
					{
						graphViewSelection.selectedElements.Remove(current2);
					}
				}
				this.m_SavedSelectionVersion = graphViewSelection.version;
				IGraphViewSelection graphViewSelection2 = this.m_GraphViewUndoRedoSelection;
				if (graphViewSelection is GraphViewUndoRedoSelection)
				{
					graphViewSelection2 = this.m_PersistedSelection;
				}
				graphViewSelection2.selectedElements.Clear();
				graphViewSelection2.selectedElements.AddRange(graphViewSelection.selectedElements);
			}
		}

		private void UndoRedoPerformed()
		{
			this.RestoreSavedSelection(this.m_GraphViewUndoRedoSelection);
		}

		private void RecordSelectionUndoPre()
		{
			if (!(this.m_GraphViewUndoRedoSelection == null))
			{
				Undo.RegisterCompleteObjectUndo(this.m_GraphViewUndoRedoSelection, "Change GraphView Selection");
			}
		}

		private void RecordSelectionUndoPost()
		{
			this.m_GraphViewUndoRedoSelection.version++;
			this.m_SavedSelectionVersion = this.m_GraphViewUndoRedoSelection.version;
			this.m_PersistedSelection.version++;
			if (this.m_OnTimerTicker == null)
			{
				this.m_OnTimerTicker = base.schedule.Execute(new Action(this.DelayPersistentDataSave));
			}
			this.m_OnTimerTicker.ExecuteLater(1L);
		}

		private void DelayPersistentDataSave()
		{
			this.m_OnTimerTicker = null;
			base.SavePersistentData();
		}

		public void AddLayer(int index)
		{
			GraphView.Layer layer = new GraphView.Layer
			{
				clippingOptions = VisualElement.ClippingOptions.NoClipping,
				pickingMode = PickingMode.Ignore
			};
			this.m_ContainerLayers.Add(index, layer);
			int index2 = (from t in this.m_ContainerLayers
			orderby t.Key
			select t.Value).ToList<GraphView.Layer>().IndexOf(layer);
			this.contentViewContainer.Insert(index2, layer);
		}

		private VisualElement GetLayer(int index)
		{
			return this.m_ContainerLayers[index];
		}

		internal void ChangeLayer(GraphElement element)
		{
			if (!this.m_ContainerLayers.ContainsKey(element.layer))
			{
				this.AddLayer(element.layer);
			}
			this.GetLayer(element.layer).Add(element);
		}

		public GraphElement GetElementByGuid(string guid)
		{
			return this.graphElements.ToList().FirstOrDefault((GraphElement e) => e.persistenceKey == guid);
		}

		public Node GetNodeByGuid(string guid)
		{
			return this.nodes.ToList().FirstOrDefault((Node e) => e.persistenceKey == guid);
		}

		public Port GetPortByGuid(string guid)
		{
			return this.ports.ToList().FirstOrDefault((Port e) => e.persistenceKey == guid);
		}

		public Edge GetEdgeByGuid(string guid)
		{
			return this.graphElements.ToList().OfType<Edge>().FirstOrDefault((Edge e) => e.persistenceKey == guid);
		}

		public void SetupZoom(float minScaleSetup, float maxScaleSetup)
		{
			this.SetupZoom(minScaleSetup, maxScaleSetup, this.m_ScaleStep, this.m_ReferenceScale);
		}

		public void SetupZoom(float minScaleSetup, float maxScaleSetup, float scaleStepSetup, float referenceScaleSetup)
		{
			this.m_MinScale = minScaleSetup;
			this.m_MaxScale = maxScaleSetup;
			this.m_ScaleStep = scaleStepSetup;
			this.m_ReferenceScale = referenceScaleSetup;
			this.UpdateContentZoomer();
		}

		private void UpdatePersistedViewTransform()
		{
			if (this.m_PersistedViewTransform != null)
			{
				this.m_PersistedViewTransform.position = this.contentViewContainer.transform.position;
				this.m_PersistedViewTransform.scale = this.contentViewContainer.transform.scale;
				base.SavePersistentData();
			}
		}

		public override void OnPersistentDataReady()
		{
			base.OnPersistentDataReady();
			string fullHierarchicalPersistenceKey = base.GetFullHierarchicalPersistenceKey();
			this.m_PersistedViewTransform = base.GetOrCreatePersistentData<GraphView.PersistedViewTransform>(this.m_PersistedViewTransform, fullHierarchicalPersistenceKey);
			this.m_PersistedSelection = base.GetOrCreatePersistentData<GraphView.PersistedSelection>(this.m_PersistedSelection, fullHierarchicalPersistenceKey);
			if (this.m_PersistedSelection.selectedElements == null)
			{
				this.m_PersistedSelection.selectedElements = new List<string>();
			}
			this.UpdateViewTransform(this.m_PersistedViewTransform.position, this.m_PersistedViewTransform.scale);
			this.RestoreSavedSelection(this.m_PersistedSelection);
		}

		private void UpdateContentZoomer()
		{
			if (this.m_MinScale != this.m_MaxScale)
			{
				if (this.m_Zoomer == null)
				{
					this.m_Zoomer = new ContentZoomer();
					this.AddManipulator(this.m_Zoomer);
				}
				this.m_Zoomer.minScale = this.m_MinScale;
				this.m_Zoomer.maxScale = this.m_MaxScale;
				this.m_Zoomer.scaleStep = this.m_ScaleStep;
				this.m_Zoomer.referenceScale = this.m_ReferenceScale;
			}
			else if (this.m_Zoomer != null)
			{
				this.RemoveManipulator(this.m_Zoomer);
			}
			this.ValidateTransform();
		}

		protected void ValidateTransform()
		{
			if (this.contentViewContainer != null)
			{
				Vector3 scale = this.viewTransform.scale;
				scale.x = Mathf.Clamp(scale.x, this.minScale, this.maxScale);
				scale.y = Mathf.Clamp(scale.y, this.minScale, this.maxScale);
				this.viewTransform.scale = scale;
			}
		}

		public override void OnDataChanged()
		{
			if (!(this.m_Presenter == null))
			{
				this.contentViewContainer.transform.position = this.m_Presenter.position;
				this.contentViewContainer.transform.scale = ((!(this.m_Presenter.scale != Vector3.zero)) ? Vector3.one : this.m_Presenter.scale);
				this.ValidateTransform();
				this.UpdatePersistedViewTransform();
				this.UpdateContentZoomer();
				List<GraphElement> list = this.graphElements.ToList();
				foreach (GraphElement current in list)
				{
					if (current.dependsOnPresenter && !this.m_Presenter.elements.Contains(current.presenter))
					{
						this.selection.Remove(current);
						this.RemoveElement(current);
					}
				}
				int num = 0;
				foreach (GraphElementPresenter current2 in this.m_Presenter.elements)
				{
					num++;
					bool flag = false;
					if (!current2.isFloating)
					{
						foreach (GraphElement current3 in list)
						{
							if (current3 != null && current3.presenter == current2)
							{
								flag = true;
								break;
							}
						}
					}
					else
					{
						foreach (VisualElement current4 in base.Children())
						{
							if (current4 != this.contentViewContainer)
							{
								GraphElement graphElement = current4 as GraphElement;
								if (graphElement != null)
								{
									if (graphElement.presenter == current2)
									{
										flag = true;
										break;
									}
								}
							}
						}
					}
					if (!flag)
					{
						this.InstantiateElement(current2);
					}
				}
				this.m_Zoomer.keepPixelCacheOnZoom = (num > this.m_ZoomerMaxElementCountWithPixelCacheRegen);
			}
		}

		public virtual void AddToSelection(ISelectable selectable)
		{
			GraphElement graphElement = selectable as GraphElement;
			if (graphElement != null)
			{
				graphElement.selected = true;
				if (graphElement.presenter != null)
				{
					graphElement.presenter.selected = true;
				}
				this.selection.Add(selectable);
				graphElement.OnSelected();
				this.contentViewContainer.Dirty(ChangeType.Repaint);
				if (this.ShouldRecordUndo())
				{
					this.RecordSelectionUndoPre();
					this.m_GraphViewUndoRedoSelection.selectedElements.Add(graphElement.persistenceKey);
					this.m_PersistedSelection.selectedElements.Add(graphElement.persistenceKey);
					this.RecordSelectionUndoPost();
				}
			}
		}

		public virtual void RemoveFromSelection(ISelectable selectable)
		{
			GraphElement graphElement = selectable as GraphElement;
			if (graphElement != null)
			{
				graphElement.selected = false;
				if (graphElement.presenter != null)
				{
					graphElement.presenter.selected = false;
				}
				this.selection.Remove(selectable);
				graphElement.OnUnselected();
				this.contentViewContainer.Dirty(ChangeType.Repaint);
				if (this.ShouldRecordUndo())
				{
					this.RecordSelectionUndoPre();
					this.m_GraphViewUndoRedoSelection.selectedElements.Remove(graphElement.persistenceKey);
					this.m_PersistedSelection.selectedElements.Remove(graphElement.persistenceKey);
					this.RecordSelectionUndoPost();
				}
			}
		}

		public virtual void ClearSelection()
		{
			foreach (GraphElement current in this.selection.OfType<GraphElement>())
			{
				current.selected = false;
				if (current.presenter != null)
				{
					current.presenter.selected = false;
				}
				current.OnUnselected();
			}
			bool flag = this.selection.Any<ISelectable>();
			this.selection.Clear();
			this.contentViewContainer.Dirty(ChangeType.Repaint);
			if (this.ShouldRecordUndo() && flag)
			{
				this.RecordSelectionUndoPre();
				this.m_GraphViewUndoRedoSelection.selectedElements.Clear();
				this.m_PersistedSelection.selectedElements.Clear();
				this.RecordSelectionUndoPost();
			}
		}

		public virtual void BuildContextualMenu(ContextualMenuPopulateEvent evt)
		{
			if (evt.target is GraphView)
			{
				ContextualMenu arg_46_0 = evt.menu;
				string arg_46_1 = "Create Node";
				Action<EventBase> arg_46_2 = new Action<EventBase>(this.OnContextMenuNodeCreate);
				if (GraphView.<>f__mg$cache0 == null)
				{
					GraphView.<>f__mg$cache0 = new Func<EventBase, ContextualMenu.MenuAction.StatusFlags>(ContextualMenu.MenuAction.AlwaysEnabled);
				}
				arg_46_0.AppendAction(arg_46_1, arg_46_2, GraphView.<>f__mg$cache0);
				evt.menu.AppendSeparator();
			}
			if (evt.target is GraphView || evt.target is Node || evt.target is GroupNode || evt.target is Edge)
			{
				evt.menu.AppendAction("Cut", delegate(EventBase e)
				{
					this.CutSelectionCallback();
				}, (EventBase e) => (!this.canCutSelection) ? ContextualMenu.MenuAction.StatusFlags.Disabled : ContextualMenu.MenuAction.StatusFlags.Normal);
			}
			if (evt.target is GraphView || evt.target is Node || evt.target is GroupNode)
			{
				evt.menu.AppendAction("Copy", delegate(EventBase e)
				{
					this.CopySelectionCallback();
				}, (EventBase e) => (!this.canCopySelection) ? ContextualMenu.MenuAction.StatusFlags.Disabled : ContextualMenu.MenuAction.StatusFlags.Normal);
			}
			if (evt.target is GraphView)
			{
				evt.menu.AppendAction("Paste", delegate(EventBase e)
				{
					this.PasteCallback();
				}, (EventBase e) => (!this.canPaste) ? ContextualMenu.MenuAction.StatusFlags.Disabled : ContextualMenu.MenuAction.StatusFlags.Normal);
			}
		}

		private void OnContextMenuNodeCreate(EventBase evt)
		{
			if (this.nodeCreationRequest != null)
			{
				GUIView gUIView = base.elementPanel.ownerObject as GUIView;
				if (!(gUIView == null))
				{
					Vector2 b;
					if (evt is IMouseEvent)
					{
						b = (evt as IMouseEvent).mousePosition;
					}
					else
					{
						b = Vector2.zero;
					}
					Vector2 screenMousePosition = gUIView.screenPosition.position + b;
					this.nodeCreationRequest(new NodeCreationContext
					{
						screenMousePosition = screenMousePosition
					});
				}
			}
		}

		protected internal override void ExecuteDefaultActionAtTarget(EventBase evt)
		{
			base.ExecuteDefaultActionAtTarget(evt);
			if (base.elementPanel != null && base.elementPanel.contextualMenuManager != null)
			{
				base.elementPanel.contextualMenuManager.DisplayMenuIfEventMatches(evt, this);
			}
		}

		private void OnContextualMenu(ContextualMenuPopulateEvent evt)
		{
			this.BuildContextualMenu(evt);
		}

		private void OnEnterPanel(AttachToPanelEvent e)
		{
			UIElementsEditorUtility.ForceDarkStyleSheet(this);
			if (this.isReframable)
			{
				base.panel.visualTree.RegisterCallback<KeyDownEvent>(new EventCallback<KeyDownEvent>(this.OnKeyDownShortcut), Capture.NoCapture);
			}
		}

		private void OnLeavePanel(DetachFromPanelEvent e)
		{
			if (this.isReframable)
			{
				base.panel.visualTree.UnregisterCallback<KeyDownEvent>(new EventCallback<KeyDownEvent>(this.OnKeyDownShortcut), Capture.NoCapture);
			}
		}

		private void OnKeyDownShortcut(KeyDownEvent evt)
		{
			if (this.isReframable)
			{
				if (!MouseCaptureController.IsMouseCaptureTaken())
				{
					EventPropagation eventPropagation = EventPropagation.Continue;
					char character = evt.character;
					switch (character)
					{
					case '[':
						eventPropagation = this.FramePrev();
						goto IL_83;
					case '\\':
						IL_3E:
						if (character == 'a')
						{
							eventPropagation = this.FrameAll();
							goto IL_83;
						}
						if (character != 'o')
						{
							goto IL_83;
						}
						eventPropagation = this.FrameOrigin();
						goto IL_83;
					case ']':
						eventPropagation = this.FrameNext();
						goto IL_83;
					}
					goto IL_3E;
					IL_83:
					if (eventPropagation == EventPropagation.Stop)
					{
						evt.StopPropagation();
						if (evt.imguiEvent != null)
						{
							evt.imguiEvent.Use();
						}
					}
				}
			}
		}

		private void OnValidateCommand(IMGUIEvent evt)
		{
			Event imguiEvent = evt.imguiEvent;
			if (imguiEvent != null && imguiEvent.type == EventType.ValidateCommand)
			{
				if ((imguiEvent.commandName == "Copy" && this.canCopySelection) || (imguiEvent.commandName == "Paste" && this.canPaste) || (imguiEvent.commandName == "Duplicate" && this.canDuplicateSelection) || (imguiEvent.commandName == "Cut" && this.canCutSelection) || ((imguiEvent.commandName == "Delete" || imguiEvent.commandName == "SoftDelete") && this.canDeleteSelection))
				{
					evt.StopPropagation();
					imguiEvent.Use();
				}
				else if (imguiEvent.commandName == "FrameSelected")
				{
					evt.StopPropagation();
					imguiEvent.Use();
				}
			}
		}

		private void OnExecuteCommand(IMGUIEvent evt)
		{
			Event imguiEvent = evt.imguiEvent;
			if (imguiEvent != null && imguiEvent.type == EventType.ExecuteCommand)
			{
				if (imguiEvent.commandName == "Copy")
				{
					this.CopySelectionCallback();
					evt.StopPropagation();
				}
				else if (imguiEvent.commandName == "Paste")
				{
					this.PasteCallback();
					evt.StopPropagation();
				}
				else if (imguiEvent.commandName == "Duplicate")
				{
					this.DuplicateSelectionCallback();
					evt.StopPropagation();
				}
				else if (imguiEvent.commandName == "Cut")
				{
					this.CutSelectionCallback();
					evt.StopPropagation();
				}
				else if (imguiEvent.commandName == "Delete")
				{
					this.DeleteSelectionCallback(GraphView.AskUser.DontAskUser);
					evt.StopPropagation();
				}
				else if (imguiEvent.commandName == "SoftDelete")
				{
					this.DeleteSelectionCallback(GraphView.AskUser.AskUser);
					evt.StopPropagation();
				}
				else if (imguiEvent.commandName == "FrameSelected")
				{
					this.FrameSelection();
					evt.StopPropagation();
				}
				if (evt.isPropagationStopped)
				{
					imguiEvent.Use();
				}
			}
		}

		protected internal void CopySelectionCallback()
		{
			string text = this.SerializeGraphElements(this.selection.OfType<GraphElement>());
			if (!string.IsNullOrEmpty(text))
			{
				this.clipboard = text;
			}
		}

		protected internal void CutSelectionCallback()
		{
			this.CopySelectionCallback();
			this.DeleteSelectionOperation("Cut", GraphView.AskUser.DontAskUser);
		}

		protected internal void PasteCallback()
		{
			this.UnserializeAndPasteOperation("Paste", this.clipboard);
		}

		protected internal void DuplicateSelectionCallback()
		{
			string data = this.SerializeGraphElements(this.selection.OfType<GraphElement>());
			this.UnserializeAndPasteOperation("Duplicate", data);
		}

		protected internal void DeleteSelectionCallback(GraphView.AskUser askUser)
		{
			this.DeleteSelectionOperation("Delete", askUser);
		}

		protected string SerializeGraphElements(IEnumerable<GraphElement> elements)
		{
			string result;
			if (this.serializeGraphElements != null)
			{
				string text = this.serializeGraphElements(elements);
				if (!string.IsNullOrEmpty(text))
				{
					text = "application/vnd.unity.graphview.elements " + text;
				}
				result = text;
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}

		protected bool CanPasteSerializedData(string data)
		{
			bool result;
			if (this.canPasteSerializedData != null)
			{
				if (data.StartsWith("application/vnd.unity.graphview.elements"))
				{
					result = this.canPasteSerializedData(data.Substring("application/vnd.unity.graphview.elements".Length + 1));
				}
				else
				{
					result = this.canPasteSerializedData(data);
				}
			}
			else
			{
				result = data.StartsWith("application/vnd.unity.graphview.elements");
			}
			return result;
		}

		protected void UnserializeAndPasteOperation(string operationName, string data)
		{
			if (this.unserializeAndPaste != null)
			{
				if (data.StartsWith("application/vnd.unity.graphview.elements"))
				{
					this.unserializeAndPaste(operationName, data.Substring("application/vnd.unity.graphview.elements".Length + 1));
				}
				else
				{
					this.unserializeAndPaste(operationName, data);
				}
			}
		}

		protected void DeleteSelectionOperation(string operationName, GraphView.AskUser askUser)
		{
			if (this.deleteSelection != null)
			{
				this.deleteSelection(operationName, askUser);
			}
			else
			{
				this.DeleteSelection();
			}
		}

		public virtual List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
		{
			return (from nap in this.ports.ToList()
			where nap.IsConnectable() && nap.orientation == startPort.orientation && nap.direction != startPort.direction && nodeAdapter.GetAdapter(nap.source, startPort.source) != null
			select nap).ToList<Port>();
		}

		public void AddElement(GraphElement graphElement)
		{
			if (graphElement.IsResizable())
			{
				graphElement.Add(new Resizer());
				graphElement.style.borderBottom = 6f;
			}
			int layer = graphElement.layer;
			if (!this.m_ContainerLayers.ContainsKey(layer))
			{
				this.AddLayer(layer);
			}
			this.GetLayer(layer).Add(graphElement);
			if (graphElement.presenter != null)
			{
				graphElement.OnDataChanged();
			}
		}

		public void RemoveElement(GraphElement graphElement)
		{
			graphElement.RemoveFromHierarchy();
		}

		protected void InstantiateElement(GraphElementPresenter elementPresenter)
		{
			GraphElement graphElement = this.typeFactory.Create(elementPresenter);
			if (graphElement != null)
			{
				graphElement.SetPosition(elementPresenter.position);
				graphElement.presenter = elementPresenter;
				if (elementPresenter.isFloating)
				{
					base.Add(graphElement);
				}
				else
				{
					this.AddElement(graphElement);
				}
			}
		}

		private EventPropagation DeleteSelectedPresenters()
		{
			EventPropagation result;
			if (this.presenter == null)
			{
				result = EventPropagation.Stop;
			}
			else
			{
				HashSet<GraphElementPresenter> hashSet = new HashSet<GraphElementPresenter>();
				foreach (GraphElement current in from GraphElement e in this.selection
				where e != null && e.presenter != null
				select e)
				{
					if ((current.presenter.capabilities & Capabilities.Deletable) != (Capabilities)0)
					{
						hashSet.Add(current.presenter);
						NodePresenter presenter = current.GetPresenter<NodePresenter>();
						if (!(presenter == null))
						{
							hashSet.UnionWith(presenter.inputPorts.SelectMany((PortPresenter c) => c.connections).Where((EdgePresenter d) => (d.capabilities & Capabilities.Deletable) != (Capabilities)0).Cast<GraphElementPresenter>());
							hashSet.UnionWith(presenter.outputPorts.SelectMany((PortPresenter c) => c.connections).Where((EdgePresenter d) => (d.capabilities & Capabilities.Deletable) != (Capabilities)0).Cast<GraphElementPresenter>());
						}
					}
				}
				foreach (GraphElementPresenter current2 in hashSet)
				{
					this.presenter.RemoveElement(current2);
				}
				foreach (EdgePresenter current3 in hashSet.OfType<EdgePresenter>())
				{
					current3.output = null;
					current3.input = null;
					if (current3.output != null)
					{
						current3.output.Disconnect(current3);
					}
					if (current3.input != null)
					{
						current3.input.Disconnect(current3);
					}
				}
				result = ((hashSet.Count <= 0) ? EventPropagation.Continue : EventPropagation.Stop);
			}
			return result;
		}

		public EventPropagation DeleteSelection()
		{
			EventPropagation result;
			if (this.presenter != null)
			{
				result = this.DeleteSelectedPresenters();
			}
			else
			{
				HashSet<GraphElement> hashSet = new HashSet<GraphElement>();
				foreach (GraphElement current in from GraphElement e in this.selection
				where e != null
				select e)
				{
					if ((current.capabilities & Capabilities.Deletable) != (Capabilities)0)
					{
						hashSet.Add(current);
						Node node = current as Node;
						if (node != null)
						{
							hashSet.UnionWith(node.inputContainer.Children().OfType<Port>().SelectMany((Port c) => c.connections).Where((Edge d) => (d.capabilities & Capabilities.Deletable) != (Capabilities)0).Cast<GraphElement>());
							hashSet.UnionWith(node.outputContainer.Children().OfType<Port>().SelectMany((Port c) => c.connections).Where((Edge d) => (d.capabilities & Capabilities.Deletable) != (Capabilities)0).Cast<GraphElement>());
						}
					}
				}
				this.DeleteElements(hashSet);
				this.selection.Clear();
				result = ((hashSet.Count <= 0) ? EventPropagation.Continue : EventPropagation.Stop);
			}
			return result;
		}

		public void DeleteElements(IEnumerable<GraphElement> elementsToRemove)
		{
			this.m_ElementsToRemove.Clear();
			foreach (GraphElement current in elementsToRemove)
			{
				this.m_ElementsToRemove.Add(current);
			}
			List<GraphElement> elementsToRemove2 = this.m_ElementsToRemove;
			if (this.graphViewChanged != null)
			{
				elementsToRemove2 = this.graphViewChanged(this.m_GraphViewChange).elementsToRemove;
			}
			foreach (Edge current2 in elementsToRemove2.OfType<Edge>())
			{
				if (current2.output != null)
				{
					current2.output.Disconnect(current2);
				}
				if (current2.input != null)
				{
					current2.input.Disconnect(current2);
				}
				current2.output = null;
				current2.input = null;
			}
			foreach (GraphElement current3 in elementsToRemove2)
			{
				this.RemoveElement(current3);
			}
		}

		public EventPropagation FrameAll()
		{
			return this.Frame(GraphView.FrameType.All);
		}

		public EventPropagation FrameSelection()
		{
			return this.Frame(GraphView.FrameType.Selection);
		}

		public EventPropagation FrameOrigin()
		{
			return this.Frame(GraphView.FrameType.Origin);
		}

		public EventPropagation FramePrev()
		{
			EventPropagation result;
			if (this.contentViewContainer.childCount == 0)
			{
				result = EventPropagation.Continue;
			}
			else
			{
				List<GraphElement> childrenList = (from e in this.graphElements.ToList()
				where e.IsSelectable() && !(e is Edge)
				orderby e.controlid descending
				select e).ToList<GraphElement>();
				result = this.FramePrevNext(childrenList);
			}
			return result;
		}

		public EventPropagation FrameNext()
		{
			EventPropagation result;
			if (this.contentViewContainer.childCount == 0)
			{
				result = EventPropagation.Continue;
			}
			else
			{
				List<GraphElement> childrenList = (from e in this.graphElements.ToList()
				where e.IsSelectable() && !(e is Edge)
				orderby e.controlid
				select e).ToList<GraphElement>();
				result = this.FramePrevNext(childrenList);
			}
			return result;
		}

		private EventPropagation FramePrevNext(List<GraphElement> childrenList)
		{
			GraphElement graphElement = null;
			if (this.selection.Count != 0)
			{
				graphElement = (this.selection[0] as GraphElement);
			}
			int num = childrenList.IndexOf(graphElement);
			EventPropagation result;
			if (num < 0)
			{
				result = EventPropagation.Continue;
			}
			else
			{
				if (num < childrenList.Count - 1)
				{
					graphElement = childrenList[num + 1];
				}
				else
				{
					graphElement = childrenList[0];
				}
				this.ClearSelection();
				this.AddToSelection(graphElement);
				result = this.Frame(GraphView.FrameType.Selection);
			}
			return result;
		}

		private EventPropagation Frame(GraphView.FrameType frameType)
		{
			Rect rect = this.contentViewContainer.layout;
			Vector3 zero = Vector3.zero;
			Vector3 one = Vector3.one;
			if (frameType == GraphView.FrameType.Selection)
			{
				if (this.selection.Count != 0)
				{
					if (this.selection.Any((ISelectable e) => e.IsSelectable() && !(e is Edge)))
					{
						goto IL_60;
					}
				}
				frameType = GraphView.FrameType.All;
			}
			IL_60:
			if (frameType == GraphView.FrameType.Selection)
			{
				GraphElement graphElement = this.selection[0] as GraphElement;
				if (graphElement != null)
				{
					rect = graphElement.localBound;
				}
				rect = this.selection.OfType<GraphElement>().Aggregate(rect, (Rect current, GraphElement e) => RectUtils.Encompass(current, e.localBound));
				GraphView.CalculateFrameTransform(rect, base.layout, this.k_FrameBorder, out zero, out one);
			}
			else if (frameType == GraphView.FrameType.All)
			{
				rect = this.CalculateRectToFitAll(this.contentViewContainer);
				GraphView.CalculateFrameTransform(rect, base.layout, this.k_FrameBorder, out zero, out one);
			}
			if (!this.m_FrameAnimate)
			{
				Matrix4x4.TRS(zero, Quaternion.identity, one);
				this.UpdateViewTransform(zero, one);
			}
			this.contentViewContainer.Dirty(ChangeType.Repaint);
			this.UpdatePersistedViewTransform();
			return EventPropagation.Stop;
		}

		public virtual Rect CalculateRectToFitAll(VisualElement container)
		{
			Rect rectToFit = container.layout;
			bool reachedFirstChild = false;
			this.graphElements.ForEach(delegate(GraphElement ge)
			{
				if (!(ge is Edge))
				{
					if (!reachedFirstChild)
					{
						rectToFit = ge.localBound;
						reachedFirstChild = true;
					}
					else
					{
						rectToFit = RectUtils.Encompass(rectToFit, ge.localBound);
					}
				}
			});
			return rectToFit;
		}

		public static void CalculateFrameTransform(Rect rectToFit, Rect clientRect, int border, out Vector3 frameTranslation, out Vector3 frameScaling)
		{
			Rect screenRect = new Rect
			{
				xMin = (float)border,
				xMax = clientRect.width - (float)border,
				yMin = (float)border,
				yMax = clientRect.height - (float)border
			};
			Matrix4x4 matrix = GUI.matrix;
			GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
			Rect rect = GUIUtility.ScreenToGUIRect(screenRect);
			float num = Math.Min(rect.width / rectToFit.width, rect.height / rectToFit.height);
			num = Mathf.Clamp(num, ContentZoomer.DefaultMinScale, 1f);
			Matrix4x4 matrix4x = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(num, num, 1f));
			Vector2 max = new Vector2(clientRect.width, clientRect.height);
			Vector2 min = new Vector2(0f, 0f);
			Rect rect2 = new Rect
			{
				min = min,
				max = max
			};
			Vector3 vector = new Vector3(matrix4x.GetColumn(0).magnitude, matrix4x.GetColumn(1).magnitude, matrix4x.GetColumn(2).magnitude);
			Vector2 vector2 = rect2.center - rectToFit.center * vector.x;
			frameTranslation = new Vector3(vector2.x, vector2.y, 0f);
			frameScaling = vector;
			GUI.matrix = matrix;
		}
	}
}
