using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	public class Attacher
	{
		private Rect m_LastTargetWorldBounds = Rect.zero;

		private Rect m_ElementSize = Rect.zero;

		private Vector2 m_Offset;

		private SpriteAlignment m_Alignment;

		private float m_Distance;

		private VisualElement m_CommonAncestor;

		public VisualElement target
		{
			get;
			private set;
		}

		public VisualElement element
		{
			get;
			private set;
		}

		public SpriteAlignment alignment
		{
			get
			{
				return this.m_Alignment;
			}
			set
			{
				if (this.m_Alignment != value)
				{
					this.m_Alignment = value;
					if (this.isAttached)
					{
						this.AlignOnTarget();
					}
				}
			}
		}

		public Vector2 offset
		{
			get
			{
				return this.m_Offset;
			}
			set
			{
				if (this.m_Offset != value)
				{
					this.m_Offset = value;
					if (this.isAttached)
					{
						this.AlignOnTarget();
					}
				}
			}
		}

		public float distance
		{
			get
			{
				return this.m_Distance;
			}
			set
			{
				if (this.m_Distance != value)
				{
					this.m_Distance = value;
					if (this.isAttached)
					{
						this.AlignOnTarget();
					}
				}
			}
		}

		private bool isAttached
		{
			get
			{
				return this.target != null && this.element != null && this.m_CommonAncestor != null;
			}
		}

		public Attacher(VisualElement anchored, VisualElement target, SpriteAlignment alignment)
		{
			this.distance = 6f;
			this.target = target;
			this.element = anchored;
			this.alignment = alignment;
			this.Reattach();
		}

		public void Detach()
		{
			this.UnregisterCallbacks();
			this.m_ElementSize = (this.m_LastTargetWorldBounds = Rect.zero);
		}

		public void Reattach()
		{
			this.RegisterCallbacks();
			this.UpdateTargetBounds();
			this.AlignOnTarget();
		}

		private void RegisterCallbacks()
		{
			if (this.m_CommonAncestor != null)
			{
				this.UnregisterCallbacks();
			}
			this.m_CommonAncestor = this.target.FindCommonAncestor(this.element);
			if (this.m_CommonAncestor == this.target)
			{
				Debug.Log("Attacher: Target is already parent of anchored element.");
				this.m_CommonAncestor = null;
			}
			else if (this.m_CommonAncestor == this.element)
			{
				Debug.Log("Attacher: An element can't be anchored to one of its descendants");
				this.m_CommonAncestor = null;
			}
			else if (this.m_CommonAncestor == null)
			{
				Debug.Log("Attacher: The element and its target must be in the same visual tree hierarchy");
			}
			else
			{
				this.m_CommonAncestor.RegisterCallback<PostLayoutEvent>(new EventCallback<PostLayoutEvent>(this.OnTargetLayout), Capture.NoCapture);
			}
		}

		private void UnregisterCallbacks()
		{
			if (this.m_CommonAncestor != null)
			{
				this.m_CommonAncestor.UnregisterCallback<PostLayoutEvent>(new EventCallback<PostLayoutEvent>(this.OnTargetLayout), Capture.NoCapture);
			}
		}

		private void OnTargetLayout(PostLayoutEvent evt)
		{
			if (this.UpdateTargetBounds() || this.UpdateElementSize())
			{
				this.AlignOnTarget();
			}
		}

		private bool UpdateTargetBounds()
		{
			Rect worldBound = this.target.worldBound;
			bool result;
			if (this.m_LastTargetWorldBounds == worldBound)
			{
				result = false;
			}
			else
			{
				this.m_LastTargetWorldBounds = worldBound;
				result = true;
			}
			return result;
		}

		private bool UpdateElementSize()
		{
			Rect worldBound = this.element.worldBound;
			worldBound.position = Vector2.zero;
			bool result;
			if (this.m_ElementSize == worldBound)
			{
				result = false;
			}
			else
			{
				this.m_ElementSize = worldBound;
				result = true;
			}
			return result;
		}

		private void AlignOnTarget()
		{
			Rect layout = new Rect(this.element.style.positionLeft, this.element.style.positionTop, this.element.style.width, this.element.style.height);
			Rect rect = this.target.rect;
			rect = this.target.ChangeCoordinatesTo(this.element.shadow.parent, rect);
			float y = 0f;
			switch (this.alignment)
			{
			case SpriteAlignment.Center:
			case SpriteAlignment.LeftCenter:
			case SpriteAlignment.RightCenter:
				y = rect.center.y;
				break;
			case SpriteAlignment.TopLeft:
			case SpriteAlignment.TopCenter:
			case SpriteAlignment.TopRight:
				y = rect.y - layout.height * 0.5f - this.distance;
				break;
			case SpriteAlignment.BottomLeft:
			case SpriteAlignment.BottomCenter:
			case SpriteAlignment.BottomRight:
				y = rect.yMax + layout.height * 0.5f + this.distance;
				break;
			}
			float x = 0f;
			switch (this.alignment)
			{
			case SpriteAlignment.Center:
			case SpriteAlignment.TopCenter:
			case SpriteAlignment.BottomCenter:
				x = rect.center.x;
				break;
			case SpriteAlignment.TopLeft:
			case SpriteAlignment.LeftCenter:
			case SpriteAlignment.BottomLeft:
				x = rect.x - layout.width * 0.5f - this.distance;
				break;
			case SpriteAlignment.TopRight:
			case SpriteAlignment.RightCenter:
			case SpriteAlignment.BottomRight:
				x = rect.xMax + layout.width * 0.5f + this.distance;
				break;
			}
			layout.center = new Vector2(x, y) + this.offset;
			this.m_ElementSize.width = layout.width;
			this.m_ElementSize.height = layout.height;
			if (layout.width > 0f)
			{
				this.element.layout = layout;
			}
			else
			{
				this.element.style.positionLeft = layout.xMin;
				this.element.style.positionTop = layout.yMin;
			}
			this.m_LastTargetWorldBounds = this.target.worldBound;
		}
	}
}
