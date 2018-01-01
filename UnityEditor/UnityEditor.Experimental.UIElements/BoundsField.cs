using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements
{
	public class BoundsField : BaseValueField<Bounds>
	{
		private Vector3Field m_CenterField;

		private Vector3Field m_ExtentsField;

		public BoundsField()
		{
			this.m_CenterField = new Vector3Field();
			this.m_CenterField.OnValueChanged(delegate(ChangeEvent<Vector3> e)
			{
				Bounds value = base.value;
				value.center = e.newValue;
				base.SetValueAndNotify(value);
			});
			VisualElement visualElement = new VisualElement();
			visualElement.AddToClassList("group");
			visualElement.Add(new Label("Center"));
			visualElement.Add(this.m_CenterField);
			base.shadow.Add(visualElement);
			this.m_ExtentsField = new Vector3Field();
			this.m_ExtentsField.OnValueChanged(delegate(ChangeEvent<Vector3> e)
			{
				Bounds value = base.value;
				value.extents = e.newValue;
				base.SetValueAndNotify(value);
			});
			VisualElement visualElement2 = new VisualElement();
			visualElement2.AddToClassList("group");
			visualElement2.contentContainer.Add(new Label("Extents"));
			visualElement2.contentContainer.Add(this.m_ExtentsField);
			base.shadow.Add(visualElement2);
		}

		protected override void UpdateDisplay()
		{
			this.m_CenterField.value = this.m_Value.center;
			this.m_ExtentsField.value = this.m_Value.extents;
		}
	}
}
