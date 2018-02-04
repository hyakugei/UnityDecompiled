using System;
using Unity.Collections;
using UnityEditor.Experimental.U2D;
using UnityEditor.Sprites;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Experimental.U2D;

namespace UnityEditor
{
	[CanEditMultipleObjects, CustomEditor(typeof(Sprite))]
	internal class SpriteInspector : Editor
	{
		private static class Styles
		{
			public static readonly GUIContent[] spriteAlignmentOptions = new GUIContent[]
			{
				EditorGUIUtility.TrTextContent("Center", null, null),
				EditorGUIUtility.TrTextContent("Top Left", null, null),
				EditorGUIUtility.TrTextContent("Top", null, null),
				EditorGUIUtility.TrTextContent("Top Right", null, null),
				EditorGUIUtility.TrTextContent("Left", null, null),
				EditorGUIUtility.TrTextContent("Right", null, null),
				EditorGUIUtility.TrTextContent("Bottom Left", null, null),
				EditorGUIUtility.TrTextContent("Bottom", null, null),
				EditorGUIUtility.TrTextContent("Bottom Right", null, null),
				EditorGUIUtility.TrTextContent("Custom", null, null)
			};

			public static readonly GUIContent spriteAlignment = EditorGUIUtility.TrTextContent("Pivot", "Sprite pivot point in its localspace. May be used for syncing animation frames of different sizes.", null);
		}

		private Sprite sprite
		{
			get
			{
				return base.target as Sprite;
			}
		}

		private SpriteMetaData GetMetaData(string name)
		{
			string assetPath = AssetDatabase.GetAssetPath(this.sprite);
			TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
			SpriteMetaData result;
			if (textureImporter != null)
			{
				if (textureImporter.spriteImportMode == SpriteImportMode.Single)
				{
					result = SpriteInspector.GetMetaDataInSingleMode(name, textureImporter);
				}
				else
				{
					result = SpriteInspector.GetMetaDataInMultipleMode(name, textureImporter);
				}
			}
			else
			{
				result = default(SpriteMetaData);
			}
			return result;
		}

		private static SpriteMetaData GetMetaDataInMultipleMode(string name, TextureImporter textureImporter)
		{
			SpriteMetaData[] spritesheet = textureImporter.spritesheet;
			SpriteMetaData result;
			for (int i = 0; i < spritesheet.Length; i++)
			{
				if (spritesheet[i].name.Equals(name))
				{
					result = spritesheet[i];
					return result;
				}
			}
			result = default(SpriteMetaData);
			return result;
		}

		private static SpriteMetaData GetMetaDataInSingleMode(string name, TextureImporter textureImporter)
		{
			SpriteMetaData result = default(SpriteMetaData);
			result.border = textureImporter.spriteBorder;
			result.name = name;
			result.pivot = textureImporter.spritePivot;
			result.rect = new Rect(0f, 0f, 1f, 1f);
			TextureImporterSettings textureImporterSettings = new TextureImporterSettings();
			textureImporter.ReadTextureSettings(textureImporterSettings);
			result.alignment = textureImporterSettings.spriteAlignment;
			return result;
		}

		public override void OnInspectorGUI()
		{
			bool flag;
			bool flag2;
			bool flag3;
			this.UnifiedValues(out flag, out flag2, out flag3);
			if (flag)
			{
				EditorGUILayout.LabelField("Name", this.sprite.name, new GUILayoutOption[0]);
			}
			else
			{
				EditorGUILayout.LabelField("Name", "-", new GUILayoutOption[0]);
			}
			if (flag2)
			{
				int alignment = this.GetMetaData(this.sprite.name).alignment;
				EditorGUILayout.LabelField(SpriteInspector.Styles.spriteAlignment, SpriteInspector.Styles.spriteAlignmentOptions[alignment], new GUILayoutOption[0]);
			}
			else
			{
				EditorGUILayout.LabelField(SpriteInspector.Styles.spriteAlignment.text, "-", new GUILayoutOption[0]);
			}
			if (flag3)
			{
				Vector4 border = this.GetMetaData(this.sprite.name).border;
				EditorGUILayout.LabelField("Border", string.Format("L:{0} B:{1} R:{2} T:{3}", new object[]
				{
					border.x,
					border.y,
					border.z,
					border.w
				}), new GUILayoutOption[0]);
			}
			else
			{
				EditorGUILayout.LabelField("Border", "-", new GUILayoutOption[0]);
			}
		}

		private void UnifiedValues(out bool name, out bool alignment, out bool border)
		{
			name = true;
			alignment = true;
			border = true;
			if (base.targets.Length >= 2)
			{
				string assetPath = AssetDatabase.GetAssetPath(this.sprite);
				ISpriteEditorDataProvider spriteEditorDataProvider = AssetImporter.GetAtPath(assetPath) as ISpriteEditorDataProvider;
				if (spriteEditorDataProvider != null)
				{
					spriteEditorDataProvider.InitSpriteEditorDataProvider();
					SpriteRect[] spriteRects = spriteEditorDataProvider.GetSpriteRects();
					string text = null;
					int num = -1;
					Vector4? vector = null;
					for (int i = 0; i < base.targets.Length; i++)
					{
						Sprite sprite = base.targets[i] as Sprite;
						for (int j = 0; j < spriteRects.Length; j++)
						{
							if (spriteRects[j].name.Equals(sprite.name))
							{
								if (spriteRects[j].alignment != (SpriteAlignment)num && num > 0)
								{
									alignment = false;
								}
								else
								{
									num = (int)spriteRects[j].alignment;
								}
								if (spriteRects[j].name != text && text != null)
								{
									name = false;
								}
								else
								{
									text = spriteRects[j].name;
								}
								if (spriteRects[j].border != vector && vector.HasValue)
								{
									border = false;
								}
								else
								{
									vector = new Vector4?(spriteRects[j].border);
								}
							}
						}
					}
				}
			}
		}

		public static Texture2D BuildPreviewTexture(int width, int height, Sprite sprite, Material spriteRendererMaterial, bool isPolygon)
		{
			Texture2D result;
			if (!ShaderUtil.hardwareSupportsRectRenderTexture)
			{
				result = null;
			}
			else
			{
				float width2 = sprite.rect.width;
				float height2 = sprite.rect.height;
				Texture2D spriteTexture = UnityEditor.Sprites.SpriteUtility.GetSpriteTexture(sprite, false);
				if (!isPolygon)
				{
					PreviewHelpers.AdjustWidthAndHeightForStaticPreview((int)width2, (int)height2, ref width, ref height);
				}
				SavedRenderTargetState savedRenderTargetState = new SavedRenderTargetState();
				RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
				RenderTexture.active = temporary;
				GL.Clear(true, true, new Color(0f, 0f, 0f, 0f));
				Texture value = null;
				Vector4 vector = new Vector4(0f, 0f, 0f, 0f);
				bool flag = false;
				bool flag2 = false;
				if (spriteRendererMaterial != null)
				{
					flag = spriteRendererMaterial.HasProperty("_MainTex");
					flag2 = spriteRendererMaterial.HasProperty("_MainTex_TexelSize");
				}
				bool flag3 = sprite.HasVertexAttribute(VertexAttribute.Color);
				Material material = null;
				if (spriteRendererMaterial != null)
				{
					if (flag)
					{
						value = spriteRendererMaterial.GetTexture("_MainTex");
						spriteRendererMaterial.SetTexture("_MainTex", spriteTexture);
					}
					if (flag2)
					{
						vector = spriteRendererMaterial.GetVector("_MainTex_TexelSize");
						spriteRendererMaterial.SetVector("_MainTex_TexelSize", TextureUtil.GetTexelSizeVector(spriteTexture));
					}
					spriteRendererMaterial.SetPass(0);
				}
				else if (flag3)
				{
					SpriteUtility.previewSpriteDefaultMaterial.SetPass(0);
				}
				else if (spriteTexture != null)
				{
					material = new Material(Shader.Find("Hidden/BlitCopy"));
					material.mainTexture = spriteTexture;
					material.mainTextureScale = Vector2.one;
					material.mainTextureOffset = Vector2.zero;
					material.SetPass(0);
				}
				float num = sprite.rect.width / sprite.bounds.size.x;
				Vector2[] vertices = sprite.vertices;
				Vector2[] uv = sprite.uv;
				ushort[] triangles = sprite.triangles;
				Vector2 pivot = sprite.pivot;
				NativeSlice<Color32>? nativeSlice = null;
				if (flag3)
				{
					nativeSlice = new NativeSlice<Color32>?(sprite.GetVertexAttribute(VertexAttribute.Color));
				}
				GL.PushMatrix();
				GL.LoadOrtho();
				GL.Color(new Color(1f, 1f, 1f, 1f));
				GL.Begin(4);
				for (int i = 0; i < triangles.Length; i++)
				{
					ushort num2 = triangles[i];
					Vector2 vector2 = vertices[(int)num2];
					Vector2 vector3 = uv[(int)num2];
					GL.TexCoord(new Vector3(vector3.x, vector3.y, 0f));
					if (nativeSlice.HasValue)
					{
						GL.Color(nativeSlice.Value[(int)num2]);
					}
					GL.Vertex3((vector2.x * num + pivot.x) / width2, (vector2.y * num + pivot.y) / height2, 0f);
				}
				GL.End();
				GL.PopMatrix();
				if (spriteRendererMaterial != null)
				{
					if (flag)
					{
						spriteRendererMaterial.SetTexture("_MainTex", value);
					}
					if (flag2)
					{
						spriteRendererMaterial.SetVector("_MainTex_TexelSize", vector);
					}
				}
				RenderTexture temporary2 = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
				Graphics.Blit(temporary, temporary2, EditorGUIUtility.GUITextureBlit2SRGBMaterial);
				RenderTexture.active = temporary2;
				Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGBA32, false);
				texture2D.hideFlags = HideFlags.HideAndDontSave;
				texture2D.filterMode = ((!(spriteTexture != null)) ? FilterMode.Point : spriteTexture.filterMode);
				texture2D.anisoLevel = ((!(spriteTexture != null)) ? 0 : spriteTexture.anisoLevel);
				texture2D.wrapMode = ((!(spriteTexture != null)) ? TextureWrapMode.Clamp : spriteTexture.wrapMode);
				texture2D.ReadPixels(new Rect(0f, 0f, (float)width, (float)height), 0, 0);
				texture2D.Apply();
				RenderTexture.ReleaseTemporary(temporary);
				RenderTexture.ReleaseTemporary(temporary2);
				savedRenderTargetState.Restore();
				if (material != null)
				{
					UnityEngine.Object.DestroyImmediate(material);
				}
				result = texture2D;
			}
			return result;
		}

		public override Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width, int height)
		{
			bool isPolygon = false;
			TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
			if (textureImporter != null)
			{
				isPolygon = (textureImporter.spriteImportMode == SpriteImportMode.Polygon);
			}
			return SpriteInspector.BuildPreviewTexture(width, height, this.sprite, null, isPolygon);
		}

		public override bool HasPreviewGUI()
		{
			Sprite sprite = base.target as Sprite;
			return sprite != null && UnityEditor.Sprites.SpriteUtility.GetSpriteTexture(sprite, false) != null;
		}

		public override void OnPreviewGUI(Rect r, GUIStyle background)
		{
			if (!(base.target == null))
			{
				if (Event.current.type == EventType.Repaint)
				{
					bool isPolygon = false;
					string assetPath = AssetDatabase.GetAssetPath(this.sprite);
					TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
					if (textureImporter != null)
					{
						isPolygon = (textureImporter.spriteImportMode == SpriteImportMode.Polygon);
					}
					SpriteInspector.DrawPreview(r, this.sprite, null, isPolygon);
				}
			}
		}

		public static void DrawPreview(Rect r, Sprite frame, Material spriteRendererMaterial, bool isPolygon)
		{
			if (!(frame == null))
			{
				float num = Mathf.Min(r.width / frame.rect.width, r.height / frame.rect.height);
				Rect position = new Rect(r.x, r.y, frame.rect.width * num, frame.rect.height * num);
				position.center = r.center;
				Texture2D texture2D = SpriteInspector.BuildPreviewTexture((int)position.width, (int)position.height, frame, spriteRendererMaterial, isPolygon);
				EditorGUI.DrawTextureTransparent(position, texture2D, ScaleMode.ScaleToFit);
				Vector4 border = frame.border;
				if (!Mathf.Approximately((border * num).sqrMagnitude, 0f))
				{
					SpriteEditorUtility.BeginLines(new Color(0f, 1f, 0f, 0.7f));
					SpriteEditorUtility.EndLines();
				}
				UnityEngine.Object.DestroyImmediate(texture2D);
			}
		}

		public override string GetInfoString()
		{
			string result;
			if (base.target == null)
			{
				result = "";
			}
			else
			{
				Sprite sprite = base.target as Sprite;
				result = string.Format("({0}x{1})", (int)sprite.rect.width, (int)sprite.rect.height);
			}
			return result;
		}
	}
}
