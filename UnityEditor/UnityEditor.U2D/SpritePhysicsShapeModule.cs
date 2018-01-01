using System;
using System.Collections.Generic;
using UnityEditor.Experimental.U2D;
using UnityEditor.U2D.Interface;
using UnityEngine;
using UnityEngine.U2D.Interface;

namespace UnityEditor.U2D
{
	[RequireSpriteDataProvider(new Type[]
	{
		typeof(ISpritePhysicsOutlineDataProvider),
		typeof(ITextureDataProvider)
	})]
	internal class SpritePhysicsShapeModule : SpriteOutlineModule
	{
		private readonly float kDefaultPhysicsTessellationDetail = 0.25f;

		private readonly byte kDefaultPhysicsAlphaTolerance = 200;

		public override string moduleName
		{
			get
			{
				return "Edit Physics Shape";
			}
		}

		private ISpriteEditor spriteEditorWindow
		{
			get;
			set;
		}

		public SpritePhysicsShapeModule(ISpriteEditor sem, IEventSystem ege, IUndoSystem us, IAssetDatabase ad, IGUIUtility gu, IShapeEditorFactory sef, ITexture2D outlineTexture) : base(sem, ege, us, ad, gu, sef, outlineTexture)
		{
			this.spriteEditorWindow = sem;
		}

		public override bool ApplyRevert(bool apply)
		{
			if (this.m_Outline != null)
			{
				if (apply)
				{
					ISpritePhysicsOutlineDataProvider dataProvider = this.spriteEditorWindow.GetDataProvider<ISpritePhysicsOutlineDataProvider>();
					for (int i = 0; i < this.m_Outline.Count; i++)
					{
						dataProvider.SetOutlines(this.m_Outline[i].spriteID, this.m_Outline[i].ToListVector());
						dataProvider.SetTessellationDetail(this.m_Outline[i].spriteID, this.m_Outline[i].tessellationDetail);
					}
				}
				UnityEngine.Object.DestroyImmediate(this.m_Outline);
				this.m_Outline = null;
			}
			return true;
		}

		protected override void LoadOutline()
		{
			this.m_Outline = ScriptableObject.CreateInstance<SpriteOutlineModel>();
			ISpriteEditorDataProvider dataProvider = this.spriteEditorWindow.GetDataProvider<ISpriteEditorDataProvider>();
			ISpritePhysicsOutlineDataProvider dataProvider2 = this.spriteEditorWindow.GetDataProvider<ISpritePhysicsOutlineDataProvider>();
			SpriteRect[] spriteRects = dataProvider.GetSpriteRects();
			for (int i = 0; i < spriteRects.Length; i++)
			{
				SpriteRect spriteRect = spriteRects[i];
				List<Vector2[]> outlines = dataProvider2.GetOutlines(spriteRect.spriteID);
				this.m_Outline.AddListVector2(spriteRect.spriteID, outlines);
				this.m_Outline[this.m_Outline.Count - 1].tessellationDetail = dataProvider2.GetTessellationDetail(spriteRect.spriteID);
			}
		}

		protected override void SetupShapeEditorOutline(SpriteRect spriteRect)
		{
			SpriteOutlineList spriteOutlineList = this.m_Outline[spriteRect.spriteID];
			if (spriteOutlineList.spriteOutlines == null || spriteOutlineList.spriteOutlines.Count == 0)
			{
				List<SpriteOutline> spriteOutlines = SpriteOutlineModule.GenerateSpriteRectOutline(spriteRect.rect, (Math.Abs(spriteOutlineList.tessellationDetail - -1f) >= Mathf.Epsilon) ? spriteOutlineList.tessellationDetail : this.kDefaultPhysicsTessellationDetail, this.kDefaultPhysicsAlphaTolerance, this.m_TextureDataProvider);
				this.spriteEditorWindow.SetDataModified();
				this.m_Outline[spriteRect.spriteID].spriteOutlines = spriteOutlines;
			}
		}
	}
}
