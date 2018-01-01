using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor.PlatformSupport
{
	internal class ReorderableIconLayerList
	{
		public delegate void ChangedCallbackDelegate(ReorderableIconLayerList list);

		private ReorderableList m_List;

		public ReorderableIconLayerList.ChangedCallbackDelegate onChangedCallback = null;

		public string headerString = "";

		private const int kSlotSize = 86;

		private const int kIconSpacing = 6;

		public int m_ImageWidth = 20;

		public int m_ImageHeight = 20;

		public int minItems = 1;

		public int maxItems = 5;

		private bool m_useCustomLayerLabel;

		private string[] m_layerLabels;

		public List<Texture2D> textures
		{
			get
			{
				return (List<Texture2D>)this.m_List.list;
			}
			set
			{
				this.m_List.list = value;
			}
		}

		public List<Texture2D> previewTextures
		{
			get;
			set;
		}

		public ReorderableIconLayerList(bool draggable = true, bool showControls = true)
		{
			this.m_List = new ReorderableList(new List<Texture2D>(), typeof(Texture2D), draggable, true, showControls, showControls);
			this.m_List.onAddCallback = new ReorderableList.AddCallbackDelegate(this.OnAdd);
			this.m_List.onRemoveCallback = new ReorderableList.RemoveCallbackDelegate(this.OnRemove);
			this.m_List.onReorderCallback = new ReorderableList.ReorderCallbackDelegate(this.OnChange);
			this.m_List.drawElementCallback = new ReorderableList.ElementCallbackDelegate(this.OnElementDraw);
			this.m_List.drawHeaderCallback = new ReorderableList.HeaderCallbackDelegate(this.OnHeaderDraw);
			this.m_List.onCanAddCallback = new ReorderableList.CanAddCallbackDelegate(this.OnCanAdd);
			this.m_List.onCanRemoveCallback = new ReorderableList.CanRemoveCallbackDelegate(this.OnCanRemove);
			this.UpdateElementHeight();
		}

		public void SetElementLabels(params string[] labels)
		{
			this.m_useCustomLayerLabel = true;
			this.m_layerLabels = labels;
		}

		private string GetElementLabel(int index)
		{
			string result;
			if (this.m_useCustomLayerLabel)
			{
				result = this.m_layerLabels[index];
			}
			else
			{
				string localizedString = LocalizationDatabase.GetLocalizedString("Layer {0}");
				string text = string.Format(localizedString, index);
				result = text;
			}
			return result;
		}

		public void SetImageSize(int width, int height)
		{
			this.m_ImageWidth = width;
			this.m_ImageHeight = height;
			this.UpdateElementHeight();
		}

		private void UpdateElementHeight()
		{
			this.m_List.elementHeight = 86f * ((float)this.m_ImageHeight / (float)this.m_ImageWidth);
		}

		private bool OnCanAdd(ReorderableList list)
		{
			return list.count < this.maxItems;
		}

		private bool OnCanRemove(ReorderableList list)
		{
			return list.count > this.minItems;
		}

		private void OnAdd(ReorderableList list)
		{
			this.textures.Add(null);
			this.m_List.index = this.textures.Count - 1;
			this.OnChange(list);
		}

		private void OnRemove(ReorderableList list)
		{
			this.textures.RemoveAt(list.index);
			list.index = 0;
			this.OnChange(list);
		}

		private void OnChange(ReorderableList list)
		{
			if (this.onChangedCallback != null)
			{
				this.onChangedCallback(this);
			}
		}

		private void OnElementDraw(Rect rect, int index, bool isActive, bool isFocused)
		{
			string elementLabel = this.GetElementLabel(index);
			float num = Mathf.Min(rect.width, EditorGUIUtility.labelWidth + 4f + 86f + 6f);
			GUI.Label(new Rect(rect.x, rect.y, num - 86f - 6f, 20f), elementLabel);
			int num2 = 86;
			int num3 = (int)((float)this.m_ImageHeight / (float)this.m_ImageWidth * 86f);
			Rect position = new Rect(rect.x + rect.width - (float)num2 - (float)num2 - 6f, rect.y, (float)num2, (float)num3);
			EditorGUI.BeginChangeCheck();
			this.textures[index] = (Texture2D)EditorGUI.ObjectField(position, this.textures[index], typeof(Texture2D), false);
			if (EditorGUI.EndChangeCheck())
			{
				this.OnChange(this.m_List);
			}
			Rect rect2 = new Rect(rect.x + rect.width - (float)num2, rect.y, (float)num2, (float)num3);
			GUI.Box(rect2, "");
			Texture2D x = this.previewTextures[index];
			if (x != null)
			{
				GUI.DrawTexture(PlatformIconField.GetContentRect(rect2, 1f, 1f), this.previewTextures[index]);
			}
		}

		private void OnHeaderDraw(Rect rect)
		{
			GUI.Label(rect, LocalizationDatabase.GetLocalizedString(this.headerString), EditorStyles.label);
		}

		public void DoLayoutList()
		{
			this.m_List.DoLayoutList();
		}
	}
}
