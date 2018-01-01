using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Experimental.UIElements
{
	public abstract class BaseCompoundField<T> : BaseValueField<T>
	{
		public class FieldDescription
		{
			public delegate void WriteDelegate(ref T val, double fieldValue);

			internal readonly string name;

			internal readonly Func<T, double> read;

			internal readonly BaseCompoundField<T>.FieldDescription.WriteDelegate write;

			public FieldDescription(string name, Func<T, double> read, BaseCompoundField<T>.FieldDescription.WriteDelegate write)
			{
				this.name = name;
				this.read = read;
				this.write = write;
			}
		}

		protected static BaseCompoundField<T>.FieldDescription[] s_FieldDescriptions;

		protected List<DoubleField> m_Fields;

		public override VisualElement contentContainer
		{
			get
			{
				return null;
			}
		}

		protected BaseCompoundField()
		{
			base.AddToClassList("compositeField");
			if (BaseCompoundField<T>.s_FieldDescriptions == null)
			{
				BaseCompoundField<T>.s_FieldDescriptions = this.DescribeFields();
			}
			if (BaseCompoundField<T>.s_FieldDescriptions == null)
			{
				Debug.LogError("Describe fields MUST return a non null array of field descriptions");
			}
			else
			{
				this.m_Fields = new List<DoubleField>(BaseCompoundField<T>.s_FieldDescriptions.Length);
				BaseCompoundField<T>.FieldDescription[] array = BaseCompoundField<T>.s_FieldDescriptions;
				for (int i = 0; i < array.Length; i++)
				{
					BaseCompoundField<T>.FieldDescription desc = array[i];
					BaseCompoundField<T> $this = this;
					VisualElement visualElement = new VisualElement();
					visualElement.AddToClassList("field");
					visualElement.Add(new Label(desc.name));
					DoubleField doubleField = new DoubleField();
					visualElement.Add(doubleField);
					doubleField.OnValueChanged(delegate(ChangeEvent<double> e)
					{
						T value = $this.value;
						desc.write(ref value, e.newValue);
						$this.SetValueAndNotify(value);
					});
					this.m_Fields.Add(doubleField);
					base.shadow.Add(visualElement);
				}
				this.UpdateDisplay();
			}
		}

		internal abstract BaseCompoundField<T>.FieldDescription[] DescribeFields();

		protected override void UpdateDisplay()
		{
			if (BaseCompoundField<T>.s_FieldDescriptions != null)
			{
				for (int i = 0; i < BaseCompoundField<T>.s_FieldDescriptions.Length; i++)
				{
					this.m_Fields[i].value = BaseCompoundField<T>.s_FieldDescriptions[i].read(this.m_Value);
				}
			}
		}

		private DoubleField AddDoubleField(EventCallback<ChangeEvent<double>> callback)
		{
			DoubleField doubleField = new DoubleField();
			base.shadow.Add(doubleField);
			doubleField.OnValueChanged(callback);
			return doubleField;
		}
	}
}
