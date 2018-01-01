using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Experimental.UIElements.StyleSheets;
using UnityEngine.Profiling;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.UIElements
{
	internal class Panel : BaseVisualElementPanel
	{
		private StyleContext m_StyleContext;

		private VisualElement m_RootContainer;

		private IDataWatchService m_DataWatch;

		private TimerEventScheduler m_Scheduler;

		internal static LoadResourceFunction loadResourceFunc = null;

		private static TimeMsFunction s_TimeSinceStartup;

		private bool m_KeepPixelCacheOnWorldBoundChange;

		private const int kMaxValidatePersistentDataCount = 5;

		private const int kMaxValidateLayoutCount = 5;

		[CompilerGenerated]
		private static TimeMsFunction <>f__mg$cache0;

		public override VisualElement visualTree
		{
			get
			{
				return this.m_RootContainer;
			}
		}

		public override IEventDispatcher dispatcher
		{
			get;
			protected set;
		}

		internal override IDataWatchService dataWatch
		{
			get
			{
				return this.m_DataWatch;
			}
		}

		public TimerEventScheduler timerEventScheduler
		{
			get
			{
				TimerEventScheduler arg_1C_0;
				if ((arg_1C_0 = this.m_Scheduler) == null)
				{
					arg_1C_0 = (this.m_Scheduler = new TimerEventScheduler());
				}
				return arg_1C_0;
			}
		}

		internal override IScheduler scheduler
		{
			get
			{
				return this.timerEventScheduler;
			}
		}

		internal StyleContext styleContext
		{
			get
			{
				return this.m_StyleContext;
			}
		}

		public override ScriptableObject ownerObject
		{
			get;
			protected set;
		}

		public bool allowPixelCaching
		{
			get;
			set;
		}

		public override ContextType contextType
		{
			get;
			protected set;
		}

		public override SavePersistentViewData savePersistentViewData
		{
			get;
			set;
		}

		public override GetViewDataDictionary getViewDataDictionary
		{
			get;
			set;
		}

		public override FocusController focusController
		{
			get;
			set;
		}

		public override EventInterests IMGUIEventInterests
		{
			get;
			set;
		}

		internal static TimeMsFunction TimeSinceStartup
		{
			get
			{
				return Panel.s_TimeSinceStartup;
			}
			set
			{
				if (value == null)
				{
					if (Panel.<>f__mg$cache0 == null)
					{
						Panel.<>f__mg$cache0 = new TimeMsFunction(Panel.DefaultTimeSinceStartupMs);
					}
					value = Panel.<>f__mg$cache0;
				}
				Panel.s_TimeSinceStartup = value;
			}
		}

		public override bool keepPixelCacheOnWorldBoundChange
		{
			get
			{
				return this.m_KeepPixelCacheOnWorldBoundChange;
			}
			set
			{
				if (this.m_KeepPixelCacheOnWorldBoundChange != value)
				{
					this.m_KeepPixelCacheOnWorldBoundChange = value;
					if (!value)
					{
						this.m_RootContainer.Dirty(ChangeType.Transform | ChangeType.Repaint);
					}
				}
			}
		}

		public override int IMGUIContainersCount
		{
			get;
			set;
		}

		public Panel(ScriptableObject ownerObject, ContextType contextType, IDataWatchService dataWatch = null, IEventDispatcher dispatcher = null)
		{
			this.ownerObject = ownerObject;
			this.contextType = contextType;
			this.m_DataWatch = dataWatch;
			this.dispatcher = dispatcher;
			this.stylePainter = new StylePainter();
			this.cursorManager = new CursorManager();
			this.contextualMenuManager = null;
			this.m_RootContainer = new VisualElement();
			this.m_RootContainer.name = VisualElementUtils.GetUniqueName("PanelContainer");
			this.m_RootContainer.persistenceKey = "PanelContainer";
			this.visualTree.ChangePanel(this);
			this.focusController = new FocusController(new VisualElementFocusRing(this.visualTree, VisualElementFocusRing.DefaultFocusOrder.ChildOrder));
			this.m_StyleContext = new StyleContext(this.m_RootContainer);
			this.allowPixelCaching = true;
		}

		public static long TimeSinceStartupMs()
		{
			return (Panel.s_TimeSinceStartup != null) ? Panel.s_TimeSinceStartup() : Panel.DefaultTimeSinceStartupMs();
		}

		internal static long DefaultTimeSinceStartupMs()
		{
			return (long)(Time.realtimeSinceStartup * 1000f);
		}

		private VisualElement PickAll(VisualElement root, Vector2 point, List<VisualElement> picked = null)
		{
			VisualElement result;
			if ((root.pseudoStates & PseudoStates.Invisible) == PseudoStates.Invisible)
			{
				result = null;
			}
			else
			{
				Vector3 v = root.WorldToLocal(point);
				bool flag = root.ContainsPoint(v);
				if (!flag && root.clippingOptions != VisualElement.ClippingOptions.NoClipping)
				{
					result = null;
				}
				else
				{
					if (picked != null && root.enabledInHierarchy && root.pickingMode == PickingMode.Position)
					{
						picked.Add(root);
					}
					VisualElement visualElement = null;
					for (int i = root.shadow.childCount - 1; i >= 0; i--)
					{
						VisualElement root2 = root.shadow[i];
						VisualElement visualElement2 = this.PickAll(root2, point, picked);
						if (visualElement == null && visualElement2 != null)
						{
							visualElement = visualElement2;
						}
					}
					if (visualElement != null)
					{
						result = visualElement;
					}
					else
					{
						PickingMode pickingMode = root.pickingMode;
						if (pickingMode != PickingMode.Position)
						{
							if (pickingMode != PickingMode.Ignore)
							{
							}
						}
						else if (flag && root.enabledInHierarchy)
						{
							result = root;
							return result;
						}
						result = null;
					}
				}
			}
			return result;
		}

		public override VisualElement LoadTemplate(string path, Dictionary<string, VisualElement> slots = null)
		{
			VisualTreeAsset visualTreeAsset = Panel.loadResourceFunc(path, typeof(VisualTreeAsset)) as VisualTreeAsset;
			VisualElement result;
			if (visualTreeAsset == null)
			{
				result = null;
			}
			else
			{
				result = visualTreeAsset.CloneTree(slots);
			}
			return result;
		}

		public override VisualElement PickAll(Vector2 point, List<VisualElement> picked)
		{
			this.ValidateLayout();
			if (picked != null)
			{
				picked.Clear();
			}
			return this.PickAll(this.visualTree, point, picked);
		}

		public override VisualElement Pick(Vector2 point)
		{
			this.ValidateLayout();
			return this.PickAll(this.visualTree, point, null);
		}

		private void ValidatePersistentData()
		{
			int num = 0;
			while (this.visualTree.AnyDirty(ChangeType.PersistentData | ChangeType.PersistentDataPath))
			{
				this.ValidatePersistentDataOnSubTree(this.visualTree, true);
				num++;
				if (num > 5)
				{
					Debug.LogError("UIElements: Too many children recursively added that rely on persistent data: " + this.visualTree);
					break;
				}
			}
		}

		private void ValidatePersistentDataOnSubTree(VisualElement root, bool enablePersistence)
		{
			if (!root.IsPersitenceSupportedOnChildren())
			{
				enablePersistence = false;
			}
			if (root.IsDirty(ChangeType.PersistentData))
			{
				root.OnPersistentDataReady(enablePersistence);
				root.ClearDirty(ChangeType.PersistentData);
			}
			if (root.IsDirty(ChangeType.PersistentDataPath))
			{
				for (int i = 0; i < root.shadow.childCount; i++)
				{
					this.ValidatePersistentDataOnSubTree(root.shadow[i], enablePersistence);
				}
				root.ClearDirty(ChangeType.PersistentDataPath);
			}
		}

		private void ValidateStyling()
		{
			Profiler.BeginSample("Panel.ValidateStyling");
			if (this.m_RootContainer.AnyDirty(ChangeType.Styles | ChangeType.StylesPath))
			{
				this.m_StyleContext.ApplyStyles();
			}
			Profiler.EndSample();
		}

		public override void ValidateLayout()
		{
			Profiler.BeginSample("Panel.ValidateLayout");
			this.ValidateStyling();
			int num = 0;
			while (this.visualTree.cssNode.IsDirty)
			{
				this.visualTree.cssNode.CalculateLayout();
				this.ValidateSubTree(this.visualTree);
				if (num++ >= 5)
				{
					Debug.LogError("ValidateLayout is struggling to process current layout (consider simplifying to avoid recursive layout): " + this.visualTree);
					break;
				}
			}
			Profiler.EndSample();
		}

		private bool ValidateSubTree(VisualElement root)
		{
			if (root.renderData.lastLayout != new Rect(root.cssNode.LayoutX, root.cssNode.LayoutY, root.cssNode.LayoutWidth, root.cssNode.LayoutHeight))
			{
				root.Dirty(ChangeType.Transform);
				root.renderData.lastLayout = new Rect(root.cssNode.LayoutX, root.cssNode.LayoutY, root.cssNode.LayoutWidth, root.cssNode.LayoutHeight);
			}
			bool hasNewLayout = root.cssNode.HasNewLayout;
			if (hasNewLayout)
			{
				for (int i = 0; i < root.shadow.childCount; i++)
				{
					this.ValidateSubTree(root.shadow[i]);
				}
			}
			using (PostLayoutEvent pooled = PostLayoutEvent.GetPooled(hasNewLayout))
			{
				pooled.target = root;
				UIElementsUtility.eventDispatcher.DispatchEvent(pooled, this);
			}
			root.ClearDirty(ChangeType.Layout);
			root.cssNode.MarkLayoutSeen();
			return hasNewLayout;
		}

		private Rect ComputeAAAlignedBound(Rect position, Matrix4x4 mat)
		{
			Rect rect = position;
			Vector3 vector = mat.MultiplyPoint3x4(new Vector3(rect.x, rect.y, 0f));
			Vector3 vector2 = mat.MultiplyPoint3x4(new Vector3(rect.x + rect.width, rect.y, 0f));
			Vector3 vector3 = mat.MultiplyPoint3x4(new Vector3(rect.x, rect.y + rect.height, 0f));
			Vector3 vector4 = mat.MultiplyPoint3x4(new Vector3(rect.x + rect.width, rect.y + rect.height, 0f));
			return Rect.MinMaxRect(Mathf.Min(vector.x, Mathf.Min(vector2.x, Mathf.Min(vector3.x, vector4.x))), Mathf.Min(vector.y, Mathf.Min(vector2.y, Mathf.Min(vector3.y, vector4.y))), Mathf.Max(vector.x, Mathf.Max(vector2.x, Mathf.Max(vector3.x, vector4.x))), Mathf.Max(vector.y, Mathf.Max(vector2.y, Mathf.Max(vector3.y, vector4.y))));
		}

		private bool ShouldUsePixelCache(VisualElement root)
		{
			return (root.panel.panelDebug == null || !root.panel.panelDebug.RecordRepaint(root)) && this.allowPixelCaching && root.clippingOptions == VisualElement.ClippingOptions.ClipAndCacheContents && root.worldBound.size.magnitude > Mathf.Epsilon;
		}

		private void PaintSubTree(Event e, VisualElement root, Matrix4x4 offset, Rect currentGlobalClip)
		{
			if (root != null && root.panel == this)
			{
				if ((root.pseudoStates & PseudoStates.Invisible) != PseudoStates.Invisible && root.style.opacity.GetSpecifiedValueOrDefault(1f) >= Mathf.Epsilon)
				{
					if (root.clippingOptions != VisualElement.ClippingOptions.NoClipping)
					{
						Rect rect = this.ComputeAAAlignedBound(root.rect, offset * root.worldTransform);
						if (!rect.Overlaps(currentGlobalClip))
						{
							return;
						}
						float num = Mathf.Max(rect.x, currentGlobalClip.x);
						float num2 = Mathf.Min(rect.x + rect.width, currentGlobalClip.x + currentGlobalClip.width);
						float num3 = Mathf.Max(rect.y, currentGlobalClip.y);
						float num4 = Mathf.Min(rect.y + rect.height, currentGlobalClip.y + currentGlobalClip.height);
						currentGlobalClip = new Rect(num, num3, num2 - num, num4 - num3);
					}
					if (this.ShouldUsePixelCache(root))
					{
						IStylePainter stylePainter = this.stylePainter;
						Rect worldBound = root.worldBound;
						int num5 = Mathf.RoundToInt(worldBound.xMax) - Mathf.RoundToInt(worldBound.xMin);
						int num6 = Mathf.RoundToInt(worldBound.yMax) - Mathf.RoundToInt(worldBound.yMin);
						int num7 = Mathf.RoundToInt((float)num5 * GUIUtility.pixelsPerPoint);
						int num8 = Mathf.RoundToInt((float)num6 * GUIUtility.pixelsPerPoint);
						num7 = Math.Max(num7, 1);
						num8 = Math.Max(num8, 1);
						RenderTexture renderTexture = root.renderData.pixelCache;
						if (renderTexture != null && (renderTexture.width != num7 || renderTexture.height != num8) && (!this.keepPixelCacheOnWorldBoundChange || root.IsDirty(ChangeType.Repaint)))
						{
							UnityEngine.Object.DestroyImmediate(renderTexture);
							renderTexture = (root.renderData.pixelCache = null);
						}
						if (root.IsDirty(ChangeType.Repaint) || root.renderData.pixelCache == null || !root.renderData.pixelCache.IsCreated())
						{
							if (renderTexture == null)
							{
								renderTexture = (root.renderData.pixelCache = new RenderTexture(num7, num8, 32, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB));
							}
							bool flag = root.style.borderTopLeftRadius > 0f || root.style.borderTopRightRadius > 0f || root.style.borderBottomLeftRadius > 0f || root.style.borderBottomRightRadius > 0f;
							RenderTexture renderTexture2 = null;
							RenderTexture active = RenderTexture.active;
							try
							{
								if (flag)
								{
									renderTexture = (renderTexture2 = RenderTexture.GetTemporary(num7, num8, 32, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB));
								}
								RenderTexture.active = renderTexture;
								GL.Clear(true, true, new Color(0f, 0f, 0f, 0f));
								Matrix4x4 matrix4x = Matrix4x4.Translate(new Vector3((float)(-(float)Mathf.RoundToInt(worldBound.x)), (float)(-(float)Mathf.RoundToInt(worldBound.y)), 0f));
								Matrix4x4 matrix4x2 = matrix4x * root.worldTransform;
								Rect rect2 = new Rect(0f, 0f, (float)num5, (float)num6);
								stylePainter.currentTransform = matrix4x2;
								bool enabled = SystemInfo.graphicsDeviceType != GraphicsDeviceType.Metal;
								using (new GUIUtility.ManualTex2SRGBScope(enabled))
								{
									using (new GUIClip.ParentClipScope(stylePainter.currentTransform, rect2))
									{
										stylePainter.currentWorldClip = rect2;
										root.DoRepaint(stylePainter);
										root.ClearDirty(ChangeType.Repaint);
										this.PaintSubTreeChildren(e, root, matrix4x, rect2);
									}
								}
								if (flag)
								{
									RenderTexture.active = root.renderData.pixelCache;
									stylePainter.currentTransform = Matrix4x4.identity;
									using (new GUIUtility.ManualTex2SRGBScope(enabled))
									{
										using (new GUIClip.ParentClipScope(stylePainter.currentTransform, rect2))
										{
											GL.Clear(true, true, new Color(0f, 0f, 0f, 0f));
											TextureStylePainterParameters defaultTextureParameters = stylePainter.GetDefaultTextureParameters(root);
											defaultTextureParameters.texture = renderTexture;
											defaultTextureParameters.scaleMode = ScaleMode.StretchToFill;
											defaultTextureParameters.rect = rect2;
											defaultTextureParameters.border.SetWidth(0f);
											Vector4 vector = new Vector4(1f, 0f, 0f, 0f);
											float x = (matrix4x2 * vector).x;
											defaultTextureParameters.border.SetRadius(defaultTextureParameters.border.topLeftRadius * x, defaultTextureParameters.border.topRightRadius * x, defaultTextureParameters.border.bottomRightRadius * x, defaultTextureParameters.border.bottomLeftRadius * x);
											defaultTextureParameters.usePremultiplyAlpha = true;
											stylePainter.DrawTexture(defaultTextureParameters);
										}
									}
									stylePainter.currentTransform = matrix4x2;
									using (new GUIUtility.ManualTex2SRGBScope(enabled))
									{
										using (new GUIClip.ParentClipScope(stylePainter.currentTransform, rect2))
										{
											stylePainter.DrawBorder(root);
										}
									}
								}
							}
							finally
							{
								renderTexture = null;
								if (renderTexture2 != null)
								{
									RenderTexture.ReleaseTemporary(renderTexture2);
								}
								RenderTexture.active = active;
							}
						}
						stylePainter.currentWorldClip = currentGlobalClip;
						stylePainter.currentTransform = offset * root.worldTransform;
						TextureStylePainterParameters painterParams = new TextureStylePainterParameters
						{
							rect = root.rect,
							uv = new Rect(0f, 0f, 1f, 1f),
							texture = root.renderData.pixelCache,
							color = Color.white,
							scaleMode = ScaleMode.ScaleAndCrop,
							usePremultiplyAlpha = true
						};
						using (new GUIClip.ParentClipScope(stylePainter.currentTransform, currentGlobalClip))
						{
							stylePainter.DrawTexture(painterParams);
						}
					}
					else
					{
						this.stylePainter.currentTransform = offset * root.worldTransform;
						using (new GUIClip.ParentClipScope(this.stylePainter.currentTransform, currentGlobalClip))
						{
							this.stylePainter.currentWorldClip = currentGlobalClip;
							this.stylePainter.mousePosition = root.worldTransform.inverse.MultiplyPoint3x4(e.mousePosition);
							this.stylePainter.opacity = root.style.opacity.GetSpecifiedValueOrDefault(1f);
							root.DoRepaint(this.stylePainter);
							this.stylePainter.opacity = 1f;
							root.ClearDirty(ChangeType.Repaint);
							this.PaintSubTreeChildren(e, root, offset, currentGlobalClip);
						}
					}
				}
			}
		}

		private void PaintSubTreeChildren(Event e, VisualElement root, Matrix4x4 offset, Rect textureClip)
		{
			int childCount = root.shadow.childCount;
			for (int i = 0; i < childCount; i++)
			{
				VisualElement root2 = root.shadow[i];
				this.PaintSubTree(e, root2, offset, textureClip);
				if (childCount != root.shadow.childCount)
				{
					throw new NotImplementedException("Visual tree is read-only during repaint");
				}
			}
		}

		public override void Repaint(Event e)
		{
			Debug.Assert(GUIClip.Internal_GetCount() == 0, "UIElement is not compatible with IMGUI GUIClips, only GUIClip.ParentClipScope");
			if (!Mathf.Approximately(this.m_StyleContext.currentPixelsPerPoint, GUIUtility.pixelsPerPoint))
			{
				this.m_RootContainer.Dirty(ChangeType.Styles);
				this.m_StyleContext.currentPixelsPerPoint = GUIUtility.pixelsPerPoint;
			}
			Profiler.BeginSample("Panel Repaint");
			this.ValidatePersistentData();
			this.ValidateLayout();
			this.stylePainter.repaintEvent = e;
			Rect currentGlobalClip = (this.visualTree.clippingOptions == VisualElement.ClippingOptions.NoClipping) ? GUIClip.topmostRect : this.visualTree.layout;
			Profiler.BeginSample("Panel Root PaintSubTree");
			this.PaintSubTree(e, this.visualTree, Matrix4x4.identity, currentGlobalClip);
			Profiler.EndSample();
			Profiler.EndSample();
			if (base.panelDebug != null)
			{
				if (base.panelDebug.EndRepaint())
				{
					this.visualTree.Dirty(ChangeType.Repaint);
				}
			}
		}
	}
}
