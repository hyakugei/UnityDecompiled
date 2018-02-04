using System;
using UnityEngine;

namespace UnityEditor
{
	internal class CurveWrapper
	{
		public delegate Vector2 GetAxisScalarsCallback();

		public delegate void SetAxisScalarsCallback(Vector2 newAxisScalars);

		public delegate void PreProcessKeyMovement(ref Keyframe key);

		internal enum SelectionMode
		{
			None,
			Selected,
			SemiSelected
		}

		private CurveRenderer m_Renderer;

		private ISelectionBinding m_SelectionBinding;

		public int id;

		public EditorCurveBinding binding;

		public int groupId;

		public int regionId;

		public Color color;

		public Color wrapColorMultiplier = Color.white;

		public bool readOnly;

		public bool hidden;

		public CurveWrapper.GetAxisScalarsCallback getAxisUiScalarsCallback;

		public CurveWrapper.SetAxisScalarsCallback setAxisUiScalarsCallback;

		public CurveWrapper.PreProcessKeyMovement preProcessKeyMovementDelegate;

		public CurveWrapper.SelectionMode selected;

		public int listIndex;

		private bool m_Changed;

		public float vRangeMin = float.NegativeInfinity;

		public float vRangeMax = float.PositiveInfinity;

		public CurveRenderer renderer
		{
			get
			{
				return this.m_Renderer;
			}
			set
			{
				this.m_Renderer = value;
			}
		}

		public AnimationCurve curve
		{
			get
			{
				return this.renderer.GetCurve();
			}
		}

		public GameObject rootGameObjet
		{
			get
			{
				return (this.m_SelectionBinding == null) ? null : this.m_SelectionBinding.rootGameObject;
			}
		}

		public AnimationClip animationClip
		{
			get
			{
				return (this.m_SelectionBinding == null) ? null : this.m_SelectionBinding.animationClip;
			}
		}

		public bool clipIsEditable
		{
			get
			{
				return this.m_SelectionBinding == null || this.m_SelectionBinding.clipIsEditable;
			}
		}

		public bool animationIsEditable
		{
			get
			{
				return this.m_SelectionBinding == null || this.m_SelectionBinding.animationIsEditable;
			}
		}

		public int selectionID
		{
			get
			{
				return (this.m_SelectionBinding == null) ? 0 : this.m_SelectionBinding.id;
			}
		}

		public ISelectionBinding selectionBindingInterface
		{
			get
			{
				return this.m_SelectionBinding;
			}
			set
			{
				this.m_SelectionBinding = value;
			}
		}

		public Bounds bounds
		{
			get
			{
				return this.renderer.GetBounds();
			}
		}

		public bool changed
		{
			get
			{
				return this.m_Changed;
			}
			set
			{
				this.m_Changed = value;
				if (value && this.renderer != null)
				{
					this.renderer.FlushCache();
				}
			}
		}

		public CurveWrapper()
		{
			this.id = 0;
			this.groupId = -1;
			this.regionId = -1;
			this.hidden = false;
			this.readOnly = false;
			this.listIndex = -1;
			this.getAxisUiScalarsCallback = null;
			this.setAxisUiScalarsCallback = null;
		}

		public int AddKey(Keyframe key)
		{
			this.PreProcessKey(ref key);
			return this.curve.AddKey(key);
		}

		public void PreProcessKey(ref Keyframe key)
		{
			if (this.preProcessKeyMovementDelegate != null)
			{
				this.preProcessKeyMovementDelegate(ref key);
			}
		}

		public int MoveKey(int index, ref Keyframe key)
		{
			this.PreProcessKey(ref key);
			return this.curve.MoveKey(index, key);
		}
	}
}
