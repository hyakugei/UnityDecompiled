using System;
using System.Collections.Generic;
using UnityEditor.Experimental.U2D;
using UnityEditor.Sprites;
using UnityEditor.U2D.Interface;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.U2D.Interface;

namespace UnityEditor
{
	[RequireSpriteDataProvider(new Type[]
	{
		typeof(ISpriteOutlineDataProvider),
		typeof(ITextureDataProvider)
	})]
	internal class SpritePolygonModeModule : SpriteFrameModuleBase
	{
		private static class SpritePolygonModeStyles
		{
			public static readonly GUIContent changeShapeLabel = EditorGUIUtility.TrTextContent("Change Shape", null, null);

			public static readonly GUIContent sidesLabel = EditorGUIUtility.TrTextContent("Sides", null, null);

			public static readonly GUIContent polygonChangeShapeHelpBoxContent = EditorGUIUtility.TrTextContent("Sides can only be either 0 or anything between 3 and 128", null, null);

			public static readonly GUIContent changeButtonLabel = EditorGUIUtility.TrTextContent("Change", "Change to the new number of sides", null);
		}

		private List<List<Vector2[]>> m_Outline;

		private const int k_PolygonChangeShapeWindowMargin = 17;

		private const int k_PolygonChangeShapeWindowWidth = 150;

		private const int k_PolygonChangeShapeWindowHeight = 45;

		private const int k_PolygonChangeShapeWindowWarningHeight = 65;

		private Rect m_PolygonChangeShapeWindowRect = new Rect(0f, 17f, 150f, 45f);

		private bool polygonSprite
		{
			get
			{
				return base.spriteImportMode == SpriteImportMode.Polygon;
			}
		}

		public int polygonSides
		{
			get;
			set;
		}

		private bool isSidesValid
		{
			get
			{
				return this.polygonSides == 0 || (this.polygonSides >= 3 && this.polygonSides <= 128);
			}
		}

		public bool showChangeShapeWindow
		{
			get;
			set;
		}

		public SpritePolygonModeModule(ISpriteEditor sw, IEventSystem es, IUndoSystem us, IAssetDatabase ad) : base("Sprite Polygon Mode Editor", sw, es, us, ad)
		{
		}

		public override void OnModuleActivate()
		{
			base.OnModuleActivate();
			this.m_Outline = new List<List<Vector2[]>>();
			for (int i = 0; i < this.m_RectsCache.spriteRects.Count; i++)
			{
				SpriteRect spriteRect = this.m_RectsCache.spriteRects[i];
				this.m_Outline.Add(base.spriteEditor.GetDataProvider<ISpriteOutlineDataProvider>().GetOutlines(spriteRect.spriteID));
			}
			this.showChangeShapeWindow = this.polygonSprite;
			if (this.polygonSprite)
			{
				this.DeterminePolygonSides();
			}
		}

		public override bool CanBeActivated()
		{
			return SpriteUtility.GetSpriteImportMode(base.spriteEditor.GetDataProvider<ISpriteEditorDataProvider>()) == SpriteImportMode.Polygon;
		}

		private void DeterminePolygonSides()
		{
			if (this.polygonSprite && this.m_RectsCache.spriteRects.Count == 1 && this.m_Outline.Count == 1 && this.m_Outline[0].Count == 1)
			{
				this.polygonSides = this.m_Outline[0][0].Length;
			}
			else
			{
				this.polygonSides = 0;
			}
		}

		public int GetPolygonSideCount()
		{
			this.DeterminePolygonSides();
			return this.polygonSides;
		}

		public List<Vector2[]> GetSpriteOutlineAt(int i)
		{
			return this.m_Outline[i];
		}

		public void GeneratePolygonOutline()
		{
			for (int i = 0; i < this.m_RectsCache.spriteRects.Count; i++)
			{
				SpriteRect spriteRect = this.m_RectsCache.spriteRects[i];
				Vector2[] item = UnityEditor.Sprites.SpriteUtility.GeneratePolygonOutlineVerticesOfSize(this.polygonSides, (int)spriteRect.rect.width, (int)spriteRect.rect.height);
				this.m_Outline.Clear();
				List<Vector2[]> list = new List<Vector2[]>();
				list.Add(item);
				this.m_Outline.Add(list);
				base.spriteEditor.SetDataModified();
			}
			base.Repaint();
		}

		public override void DoPostGUI()
		{
			this.DoPolygonChangeShapeWindow();
			base.DoPostGUI();
		}

		public override void DoMainGUI()
		{
			base.DoMainGUI();
			this.DrawGizmos();
			base.HandleGizmoMode();
			base.HandleBorderCornerScalingHandles();
			base.HandleBorderSidePointScalingSliders();
			base.HandleBorderSideScalingHandles();
			base.HandlePivotHandle();
			if (!base.MouseOnTopOfInspector())
			{
				base.spriteEditor.HandleSpriteSelection();
			}
		}

		public override void DoToolbarGUI(Rect toolbarRect)
		{
			using (new EditorGUI.DisabledScope(base.spriteEditor.editingDisabled))
			{
				GUIStyle toolbarPopup = EditorStyles.toolbarPopup;
				Rect rect = toolbarRect;
				rect.width = toolbarPopup.CalcSize(SpritePolygonModeModule.SpritePolygonModeStyles.changeShapeLabel).x;
				SpriteUtilityWindow.DrawToolBarWidget(ref rect, ref toolbarRect, delegate(Rect adjustedDrawArea)
				{
					this.showChangeShapeWindow = GUI.Toggle(adjustedDrawArea, this.showChangeShapeWindow, SpritePolygonModeModule.SpritePolygonModeStyles.changeShapeLabel, EditorStyles.toolbarButton);
				});
			}
		}

		private void DrawGizmos()
		{
			if (base.eventSystem.current.type == EventType.Repaint)
			{
				for (int i = 0; i < base.spriteCount; i++)
				{
					List<Vector2[]> spriteOutlineAt = this.GetSpriteOutlineAt(i);
					Vector2 b = base.GetSpriteRectAt(i).size * 0.5f;
					if (spriteOutlineAt.Count > 0)
					{
						SpriteEditorUtility.BeginLines(new Color(0.75f, 0.75f, 0.75f, 0.75f));
						for (int j = 0; j < spriteOutlineAt.Count; j++)
						{
							int k = 0;
							int num = spriteOutlineAt[j].Length - 1;
							while (k < spriteOutlineAt[j].Length)
							{
								SpriteEditorUtility.DrawLine(spriteOutlineAt[j][num] + b, spriteOutlineAt[j][k] + b);
								num = k;
								k++;
							}
						}
						SpriteEditorUtility.EndLines();
					}
				}
				base.DrawSpriteRectGizmos();
			}
		}

		private void DoPolygonChangeShapeWindow()
		{
			if (this.showChangeShapeWindow && !base.spriteEditor.editingDisabled)
			{
				bool flag = false;
				float labelWidth = EditorGUIUtility.labelWidth;
				EditorGUIUtility.labelWidth = 45f;
				GUILayout.BeginArea(this.m_PolygonChangeShapeWindowRect);
				GUILayout.BeginVertical(GUI.skin.box, new GUILayoutOption[0]);
				IEvent current = base.eventSystem.current;
				if (this.isSidesValid && current.type == EventType.KeyDown && current.keyCode == KeyCode.Return)
				{
					flag = true;
					current.Use();
				}
				EditorGUI.BeginChangeCheck();
				this.polygonSides = EditorGUILayout.IntField(SpritePolygonModeModule.SpritePolygonModeStyles.sidesLabel, this.polygonSides, new GUILayoutOption[0]);
				if (EditorGUI.EndChangeCheck())
				{
					this.m_PolygonChangeShapeWindowRect.height = (float)((!this.isSidesValid) ? 65 : 45);
				}
				GUILayout.FlexibleSpace();
				if (!this.isSidesValid)
				{
					EditorGUILayout.HelpBox(SpritePolygonModeModule.SpritePolygonModeStyles.polygonChangeShapeHelpBoxContent.text, MessageType.Warning, true);
				}
				else
				{
					GUILayout.BeginHorizontal(new GUILayoutOption[0]);
					GUILayout.FlexibleSpace();
					EditorGUI.BeginDisabledGroup(!this.isSidesValid);
					if (GUILayout.Button(SpritePolygonModeModule.SpritePolygonModeStyles.changeButtonLabel, new GUILayoutOption[0]))
					{
						flag = true;
					}
					EditorGUI.EndDisabledGroup();
					GUILayout.EndHorizontal();
				}
				GUILayout.EndVertical();
				if (flag)
				{
					if (this.isSidesValid)
					{
						this.GeneratePolygonOutline();
					}
					this.showChangeShapeWindow = false;
					GUIUtility.hotControl = 0;
					GUIUtility.keyboardControl = 0;
				}
				EditorGUIUtility.labelWidth = labelWidth;
				GUILayout.EndArea();
			}
		}
	}
}
