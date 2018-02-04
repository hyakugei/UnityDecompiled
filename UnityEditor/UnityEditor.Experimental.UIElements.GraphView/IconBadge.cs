using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	public class IconBadge : VisualElement
	{
		private VisualElement m_TipElement;

		private VisualElement m_IconElement;

		private Label m_TextElement;

		private const int kDefaultDistanceValue = 6;

		private StyleValue<int> m_Distance;

		private int m_CurrentTipAngle = 0;

		private Attacher m_Attacher = null;

		private bool m_IsAttached;

		private VisualElement m_originalParent;

		protected SpriteAlignment alignment
		{
			get;
			set;
		}

		protected VisualElement target
		{
			get;
			set;
		}

		public string badgeText
		{
			get
			{
				return (this.m_TextElement == null) ? string.Empty : this.m_TextElement.text;
			}
			set
			{
				if (this.m_TextElement != null)
				{
					this.m_TextElement.text = value;
				}
			}
		}

		public StyleValue<int> distance
		{
			get
			{
				return this.m_Distance;
			}
			set
			{
				int num = this.m_Distance;
				if (this.m_Distance.Apply(value, StylePropertyApplyMode.CopyIfEqualOrGreaterSpecificity))
				{
					if (num != this.m_Distance)
					{
						base.Dirty(ChangeType.Layout);
					}
				}
			}
		}

		public IconBadge()
		{
			this.m_IsAttached = false;
			VisualTreeAsset tpl = EditorGUIUtility.Load("GraphView/Badge/IconBadge.uxml") as VisualTreeAsset;
			this.LoadTemplate(tpl);
		}

		public IconBadge(VisualTreeAsset template)
		{
			this.LoadTemplate(template);
		}

		private void LoadTemplate(VisualTreeAsset tpl)
		{
			tpl.CloneTree(this, new Dictionary<string, VisualElement>());
			this.m_IconElement = this.Q("icon", null);
			this.m_TipElement = this.Q("tip", null);
			this.m_TextElement = this.Q("text", null);
			if (this.m_IconElement == null)
			{
				Debug.Log("IconBadge: Couldn't load icon element from template");
			}
			if (this.m_TipElement == null)
			{
				Debug.Log("IconBadge: Couldn't load tip element from template");
			}
			if (this.m_TextElement == null)
			{
				Debug.Log("IconBadge: Couldn't load text element from template");
			}
			if (this.m_TipElement != null)
			{
				VisualElement parent = this.m_TipElement.shadow.parent;
				this.m_TipElement.RemoveFromHierarchy();
				parent.shadow.Insert(0, this.m_TipElement);
			}
			base.name = "IconBadge";
			base.AddToClassList("iconBadge");
			base.AddStyleSheetPath("GraphView/Badge/IconBadge.uss");
			if (this.m_TextElement != null)
			{
				this.m_TextElement.RemoveFromHierarchy();
				this.m_TextElement.style.wordWrap = true;
				this.m_TextElement.RegisterCallback<PostLayoutEvent>(delegate(PostLayoutEvent evt)
				{
					this.ComputeTextSize();
				}, Capture.NoCapture);
				base.clippingOptions = VisualElement.ClippingOptions.NoClipping;
			}
		}

		public static IconBadge CreateError(string message)
		{
			IconBadge iconBadge = new IconBadge();
			iconBadge.AddToClassList("error");
			iconBadge.badgeText = message;
			return iconBadge;
		}

		public static IconBadge CreateComment(string message)
		{
			IconBadge iconBadge = new IconBadge();
			iconBadge.AddToClassList("comment");
			iconBadge.badgeText = message;
			return iconBadge;
		}

		public void AttachTo(VisualElement target, SpriteAlignment align)
		{
			this.Detach();
			this.alignment = align;
			this.target = target;
			this.m_IsAttached = true;
			target.RegisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.OnTargetDetachedFromPanel), Capture.NoCapture);
			this.CreateAttacher();
		}

		public void Detach()
		{
			if (this.m_IsAttached)
			{
				this.target.UnregisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.OnTargetDetachedFromPanel), Capture.NoCapture);
				this.m_IsAttached = false;
			}
			this.ReleaseAttacher();
			this.m_originalParent = null;
		}

		private void OnTargetDetachedFromPanel(DetachFromPanelEvent evt)
		{
			this.ReleaseAttacher();
			if (this.m_IsAttached)
			{
				this.m_originalParent = base.shadow.parent;
				base.RemoveFromHierarchy();
				this.target.UnregisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.OnTargetDetachedFromPanel), Capture.NoCapture);
				this.target.RegisterCallback<AttachToPanelEvent>(new EventCallback<AttachToPanelEvent>(this.OnTargetAttachedToPanel), Capture.NoCapture);
			}
		}

		private void OnTargetAttachedToPanel(AttachToPanelEvent evt)
		{
			if (this.m_IsAttached)
			{
				this.target.RegisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.OnTargetDetachedFromPanel), Capture.NoCapture);
				if (this.m_originalParent != null)
				{
					this.m_originalParent.shadow.Add(this);
				}
				if (this.m_Attacher != null)
				{
					this.ReleaseAttacher();
				}
				this.CreateAttacher();
			}
		}

		private void ReleaseAttacher()
		{
			if (this.m_Attacher != null)
			{
				this.m_Attacher.Detach();
				this.m_Attacher = null;
			}
		}

		private void CreateAttacher()
		{
			this.m_Attacher = new Attacher(this, this.target, this.alignment);
			this.m_Attacher.distance = (float)this.distance.GetSpecifiedValueOrDefault(6);
		}

		private void ComputeTextSize()
		{
			if (this.m_TextElement != null)
			{
				Vector2 vector = this.m_TextElement.DoMeasure(this.m_TextElement.style.maxWidth, VisualElement.MeasureMode.AtMost, 0f, VisualElement.MeasureMode.Undefined);
				this.m_TextElement.style.width = vector.x + this.m_TextElement.style.marginLeft + this.m_TextElement.style.marginRight + this.m_TextElement.style.borderLeft + this.m_TextElement.style.borderRight + this.m_TextElement.style.paddingLeft + this.m_TextElement.style.paddingRight;
				this.m_TextElement.style.height = vector.y + this.m_TextElement.style.marginTop + this.m_TextElement.style.marginBottom + this.m_TextElement.style.borderTop + this.m_TextElement.style.borderBottom + this.m_TextElement.style.paddingTop + this.m_TextElement.style.paddingBottom;
				this.PerformTipLayout();
			}
		}

		private void ShowText()
		{
			if (this.m_TextElement != null && this.m_TextElement.shadow.parent == null)
			{
				base.Add(this.m_TextElement);
				this.m_TextElement.ResetPositionProperties();
				this.ComputeTextSize();
			}
		}

		private void HideText()
		{
			if (this.m_TextElement != null && this.m_TextElement.shadow.parent != null)
			{
				this.m_TextElement.RemoveFromHierarchy();
			}
		}

		protected internal override void ExecuteDefaultAction(EventBase evt)
		{
			if (evt.GetEventTypeId() == EventBase<PostLayoutEvent>.TypeId())
			{
				if (this.m_Attacher != null)
				{
					this.PerformTipLayout();
				}
			}
			else if (evt.GetEventTypeId() == EventBase<DetachFromPanelEvent>.TypeId())
			{
				if (this.m_Attacher != null)
				{
					this.m_Attacher.Detach();
					this.m_Attacher = null;
				}
			}
			else if (evt.GetEventTypeId() == EventBase<MouseEnterEvent>.TypeId())
			{
				this.ShowText();
			}
			else if (evt.GetEventTypeId() == EventBase<MouseLeaveEvent>.TypeId())
			{
				this.HideText();
			}
			base.ExecuteDefaultAction(evt);
		}

		private void PerformTipLayout()
		{
			float num = base.style.width;
			float num2 = 0f;
			float num3 = 0f;
			if (this.m_TipElement != null)
			{
				num2 = this.m_TipElement.style.width;
				num3 = this.m_TipElement.style.height;
			}
			float num4 = (this.m_IconElement == null) ? 0f : this.m_IconElement.style.width.GetSpecifiedValueOrDefault(num - num3);
			float num5 = Mathf.Floor((num4 - num2) * 0.5f);
			Rect layout = new Rect(0f, 0f, num4, num4);
			float num6 = Mathf.Floor((num - num4) * 0.5f);
			Rect layout2 = new Rect(0f, 0f, num2, num3);
			int num7 = 0;
			Vector2 zero = Vector2.zero;
			bool flag = true;
			switch (this.alignment)
			{
			case SpriteAlignment.TopCenter:
				layout.x = num6;
				layout.y = 0f;
				layout2.x = num6 + num5;
				layout2.y = layout.height;
				zero = new Vector2(num2, num3);
				num7 = 180;
				goto IL_202;
			case SpriteAlignment.LeftCenter:
				layout.y = num6;
				layout2.x = layout.width;
				layout2.y = num6 + num5;
				zero = new Vector2(num3, 0f);
				num7 = 90;
				goto IL_202;
			case SpriteAlignment.RightCenter:
				layout.y = num6;
				layout.x += num3;
				layout2.y = num6 + num5;
				zero = new Vector2(0f, num2);
				num7 = 270;
				goto IL_202;
			case SpriteAlignment.BottomCenter:
				layout.x = num6;
				layout.y = num3;
				layout2.x = num6 + num5;
				zero = new Vector2(0f, 0f);
				num7 = 0;
				goto IL_202;
			}
			flag = false;
			IL_202:
			if (num7 != this.m_CurrentTipAngle)
			{
				if (this.m_TipElement != null)
				{
					this.m_TipElement.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, (float)num7));
					this.m_TipElement.transform.position = new Vector3(zero.x, zero.y, 0f);
				}
				this.m_CurrentTipAngle = num7;
			}
			if (this.m_IconElement != null)
			{
				this.m_IconElement.layout = layout;
			}
			if (this.m_TipElement != null)
			{
				this.m_TipElement.layout = layout2;
				if (this.m_TipElement.visible != flag)
				{
					this.m_TipElement.visible = flag;
				}
			}
			if (this.m_TextElement != null)
			{
				this.m_TextElement.style.positionType = PositionType.Absolute;
				this.m_TextElement.style.positionLeft = layout.xMax;
				this.m_TextElement.style.positionTop = layout.y;
			}
		}

		protected override void OnStyleResolved(ICustomStyle elementStyle)
		{
			base.OnStyleResolved(elementStyle);
			elementStyle.ApplyCustomProperty("distance", ref this.m_Distance);
		}
	}
}
