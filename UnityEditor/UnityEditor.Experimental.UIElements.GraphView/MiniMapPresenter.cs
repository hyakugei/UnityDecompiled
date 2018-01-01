using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityEditor.Experimental.UIElements.GraphView
{
	[Serializable]
	public class MiniMapPresenter : GraphElementPresenter
	{
		public float maxHeight;

		public float maxWidth;

		[SerializeField]
		private bool m_Anchored;

		public bool anchored
		{
			get
			{
				return this.m_Anchored;
			}
			set
			{
				this.m_Anchored = value;
			}
		}

		public override bool isFloating
		{
			[CompilerGenerated]
			get
			{
				return true;
			}
		}

		protected MiniMapPresenter()
		{
			this.maxWidth = 200f;
			this.maxHeight = 180f;
		}

		protected new void OnEnable()
		{
			base.OnEnable();
			base.capabilities = Capabilities.Movable;
		}
	}
}
