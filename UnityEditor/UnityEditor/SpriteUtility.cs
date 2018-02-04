using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.U2D;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.U2D.Interface;

namespace UnityEditor
{
	internal static class SpriteUtility
	{
		private static class SpriteUtilityStrings
		{
			public static readonly GUIContent saveAnimDialogMessage = EditorGUIUtility.TrTextContent("Create a new animation for the game object '{0}':", null, null);

			public static readonly GUIContent saveAnimDialogTitle = EditorGUIUtility.TrTextContent("Create New Animation", null, null);

			public static readonly GUIContent saveAnimDialogName = EditorGUIUtility.TrTextContent("New Animation", null, null);

			public static readonly GUIContent unableToFindSpriteRendererWarning = EditorGUIUtility.TrTextContent("There should be a SpriteRenderer in dragged object", null, null);

			public static readonly GUIContent unableToAddSpriteRendererWarning = EditorGUIUtility.TrTextContent("Unable to add SpriteRenderer into Gameobject.", null, null);

			public static readonly GUIContent failedToCreateAnimationError = EditorGUIUtility.TrTextContent("Failed to create animation for dragged object", null, null);
		}

		private enum DragType
		{
			NotInitialized,
			SpriteAnimation,
			CreateMultiple
		}

		public delegate string ShowFileDialogDelegate(string title, string defaultName, string extension, string message, string defaultPath);

		private static Material s_PreviewSpriteDefaultMaterial;

		private static List<UnityEngine.Object> s_SceneDragObjects;

		private static SpriteUtility.DragType s_DragType;

		[CompilerGenerated]
		private static SpriteUtility.ShowFileDialogDelegate <>f__mg$cache0;

		[CompilerGenerated]
		private static SpriteUtility.ShowFileDialogDelegate <>f__mg$cache1;

		internal static Material previewSpriteDefaultMaterial
		{
			get
			{
				if (SpriteUtility.s_PreviewSpriteDefaultMaterial == null)
				{
					Shader shader = Shader.Find("Sprites/Default");
					SpriteUtility.s_PreviewSpriteDefaultMaterial = new Material(shader);
				}
				return SpriteUtility.s_PreviewSpriteDefaultMaterial;
			}
		}

		public static void OnSceneDrag(SceneView sceneView)
		{
			IEvent arg_2E_1 = new UnityEngine.U2D.Interface.Event();
			UnityEngine.Object[] arg_2E_2 = DragAndDrop.objectReferences;
			string[] arg_2E_3 = DragAndDrop.paths;
			if (SpriteUtility.<>f__mg$cache0 == null)
			{
				SpriteUtility.<>f__mg$cache0 = new SpriteUtility.ShowFileDialogDelegate(EditorUtility.SaveFilePanelInProject);
			}
			SpriteUtility.HandleSpriteSceneDrag(sceneView, arg_2E_1, arg_2E_2, arg_2E_3, SpriteUtility.<>f__mg$cache0);
		}

		public static void HandleSpriteSceneDrag(SceneView sceneView, IEvent evt, UnityEngine.Object[] objectReferences, string[] paths, SpriteUtility.ShowFileDialogDelegate saveFileDialog)
		{
			if (evt.type == EventType.DragUpdated || evt.type == EventType.DragPerform || evt.type == EventType.DragExited)
			{
				if (!objectReferences.Any((UnityEngine.Object obj) => obj == null))
				{
					if (objectReferences.Length == 1 && objectReferences[0] as UnityEngine.Texture2D != null)
					{
						GameObject gameObject = HandleUtility.PickGameObject(evt.mousePosition, true);
						if (gameObject != null)
						{
							Renderer component = gameObject.GetComponent<Renderer>();
							if (component != null && !(component is SpriteRenderer))
							{
								SpriteUtility.CleanUp(true);
								return;
							}
						}
					}
					EventType type = evt.type;
					if (type != EventType.DragUpdated)
					{
						if (type != EventType.DragPerform)
						{
							if (type == EventType.DragExited)
							{
								if (SpriteUtility.s_SceneDragObjects != null)
								{
									SpriteUtility.CleanUp(true);
									evt.Use();
								}
							}
						}
						else
						{
							List<Sprite> spriteFromPathsOrObjects = SpriteUtility.GetSpriteFromPathsOrObjects(objectReferences, paths, evt.type);
							if (spriteFromPathsOrObjects.Count > 0 && SpriteUtility.s_SceneDragObjects != null)
							{
								int currentGroup = Undo.GetCurrentGroup();
								if (SpriteUtility.s_SceneDragObjects.Count == 0)
								{
									SpriteUtility.CreateSceneDragObjects(spriteFromPathsOrObjects);
									SpriteUtility.PositionSceneDragObjects(SpriteUtility.s_SceneDragObjects, sceneView, evt.mousePosition);
								}
								using (List<UnityEngine.Object>.Enumerator enumerator = SpriteUtility.s_SceneDragObjects.GetEnumerator())
								{
									while (enumerator.MoveNext())
									{
										GameObject gameObject2 = (GameObject)enumerator.Current;
										gameObject2.hideFlags = HideFlags.None;
										Undo.RegisterCreatedObjectUndo(gameObject2, "Create Sprite");
										EditorUtility.SetDirty(gameObject2);
									}
								}
								bool flag = true;
								if (SpriteUtility.s_DragType == SpriteUtility.DragType.SpriteAnimation && spriteFromPathsOrObjects.Count > 1)
								{
									UsabilityAnalytics.Event("Sprite Drag and Drop", "Drop multiple sprites to scene", "null", 1);
									flag = SpriteUtility.AddAnimationToGO((GameObject)SpriteUtility.s_SceneDragObjects[0], spriteFromPathsOrObjects.ToArray(), saveFileDialog);
								}
								else
								{
									UsabilityAnalytics.Event("Sprite Drag and Drop", "Drop single sprite to scene", "null", 1);
								}
								if (flag)
								{
									Selection.objects = SpriteUtility.s_SceneDragObjects.ToArray();
								}
								else
								{
									Undo.RevertAllDownToGroup(currentGroup);
								}
								SpriteUtility.CleanUp(!flag);
								evt.Use();
							}
						}
					}
					else
					{
						SpriteUtility.DragType dragType = (!evt.alt) ? SpriteUtility.DragType.SpriteAnimation : SpriteUtility.DragType.CreateMultiple;
						if (SpriteUtility.s_DragType != dragType || SpriteUtility.s_SceneDragObjects == null)
						{
							if (!SpriteUtility.ExistingAssets(objectReferences) && SpriteUtility.PathsAreValidTextures(paths))
							{
								DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
								SpriteUtility.s_SceneDragObjects = new List<UnityEngine.Object>();
								SpriteUtility.s_DragType = dragType;
							}
							else
							{
								List<Sprite> spriteFromPathsOrObjects2 = SpriteUtility.GetSpriteFromPathsOrObjects(objectReferences, paths, evt.type);
								if (spriteFromPathsOrObjects2.Count == 0)
								{
									return;
								}
								if (SpriteUtility.s_DragType != SpriteUtility.DragType.NotInitialized)
								{
									SpriteUtility.CleanUp(true);
								}
								SpriteUtility.s_DragType = dragType;
								SpriteUtility.CreateSceneDragObjects(spriteFromPathsOrObjects2);
								SpriteUtility.IgnoreForRaycasts(SpriteUtility.s_SceneDragObjects);
							}
						}
						SpriteUtility.PositionSceneDragObjects(SpriteUtility.s_SceneDragObjects, sceneView, evt.mousePosition);
						DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
						evt.Use();
					}
				}
			}
		}

		private static void IgnoreForRaycasts(List<UnityEngine.Object> objects)
		{
			List<Transform> list = new List<Transform>();
			using (List<UnityEngine.Object>.Enumerator enumerator = objects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject gameObject = (GameObject)enumerator.Current;
					list.AddRange(gameObject.GetComponentsInChildren<Transform>());
				}
			}
			HandleUtility.ignoreRaySnapObjects = list.ToArray();
		}

		private static void PositionSceneDragObjects(List<UnityEngine.Object> objects, SceneView sceneView, Vector2 mousePosition)
		{
			Vector3 vector = Vector3.zero;
			vector = HandleUtility.GUIPointToWorldRay(mousePosition).GetPoint(10f);
			if (sceneView.in2DMode)
			{
				vector.z = 0f;
			}
			else
			{
				DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
				object obj = HandleUtility.RaySnap(HandleUtility.GUIPointToWorldRay(mousePosition));
				if (obj != null)
				{
					vector = ((RaycastHit)obj).point;
				}
			}
			if (Selection.activeGameObject != null)
			{
				Grid componentInParent = Selection.activeGameObject.GetComponentInParent<Grid>();
				if (componentInParent != null)
				{
					Vector3Int position = componentInParent.WorldToCell(vector);
					vector = componentInParent.GetCellCenterWorld(position);
				}
			}
			using (List<UnityEngine.Object>.Enumerator enumerator = objects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject gameObject = (GameObject)enumerator.Current;
					gameObject.transform.position = vector;
				}
			}
		}

		private static void CreateSceneDragObjects(List<Sprite> sprites)
		{
			if (SpriteUtility.s_SceneDragObjects == null)
			{
				SpriteUtility.s_SceneDragObjects = new List<UnityEngine.Object>();
			}
			if (SpriteUtility.s_DragType == SpriteUtility.DragType.CreateMultiple)
			{
				foreach (Sprite current in sprites)
				{
					SpriteUtility.s_SceneDragObjects.Add(SpriteUtility.CreateDragGO(current, Vector3.zero));
				}
			}
			else
			{
				SpriteUtility.s_SceneDragObjects.Add(SpriteUtility.CreateDragGO(sprites[0], Vector3.zero));
			}
		}

		private static void CleanUp(bool deleteTempSceneObject)
		{
			if (SpriteUtility.s_SceneDragObjects != null)
			{
				if (deleteTempSceneObject)
				{
					using (List<UnityEngine.Object>.Enumerator enumerator = SpriteUtility.s_SceneDragObjects.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							GameObject obj = (GameObject)enumerator.Current;
							UnityEngine.Object.DestroyImmediate(obj, false);
						}
					}
				}
				SpriteUtility.s_SceneDragObjects.Clear();
				SpriteUtility.s_SceneDragObjects = null;
			}
			HandleUtility.ignoreRaySnapObjects = null;
			SpriteUtility.s_DragType = SpriteUtility.DragType.NotInitialized;
		}

		private static bool CreateAnimation(GameObject gameObject, UnityEngine.Object[] frames, SpriteUtility.ShowFileDialogDelegate saveFileDialog)
		{
			SpriteUtility.ShowFileDialogDelegate arg_26_0;
			if ((arg_26_0 = saveFileDialog) == null)
			{
				if (SpriteUtility.<>f__mg$cache1 == null)
				{
					SpriteUtility.<>f__mg$cache1 = new SpriteUtility.ShowFileDialogDelegate(EditorUtility.SaveFilePanelInProject);
				}
				arg_26_0 = SpriteUtility.<>f__mg$cache1;
			}
			saveFileDialog = arg_26_0;
			Array.Sort<UnityEngine.Object>(frames, (UnityEngine.Object a, UnityEngine.Object b) => EditorUtility.NaturalCompare(a.name, b.name));
			Animator animator = (!AnimationWindowUtility.EnsureActiveAnimationPlayer(gameObject)) ? null : AnimationWindowUtility.GetClosestAnimatorInParents(gameObject.transform);
			bool flag = animator != null;
			bool result;
			if (animator != null)
			{
				string message = string.Format(SpriteUtility.SpriteUtilityStrings.saveAnimDialogMessage.text, gameObject.name);
				string activeFolderPath = ProjectWindowUtil.GetActiveFolderPath();
				string text = saveFileDialog(SpriteUtility.SpriteUtilityStrings.saveAnimDialogTitle.text, SpriteUtility.SpriteUtilityStrings.saveAnimDialogName.text, "anim", message, activeFolderPath);
				if (string.IsNullOrEmpty(text))
				{
					UnityEngine.Object.DestroyImmediate(animator);
					result = false;
					return result;
				}
				AnimationClip animationClip = AnimationWindowUtility.CreateNewClipAtPath(text);
				if (animationClip != null)
				{
					SpriteUtility.AddSpriteAnimationToClip(animationClip, frames);
					flag = AnimationWindowUtility.AddClipToAnimatorComponent(animator, animationClip);
				}
			}
			if (!flag)
			{
				Debug.LogError(SpriteUtility.SpriteUtilityStrings.failedToCreateAnimationError.text);
			}
			result = flag;
			return result;
		}

		private static void AddSpriteAnimationToClip(AnimationClip newClip, UnityEngine.Object[] frames)
		{
			newClip.frameRate = 12f;
			ObjectReferenceKeyframe[] array = new ObjectReferenceKeyframe[frames.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = default(ObjectReferenceKeyframe);
				array[i].value = SpriteUtility.RemapObjectToSprite(frames[i]);
				array[i].time = (float)i / newClip.frameRate;
			}
			EditorCurveBinding binding = EditorCurveBinding.PPtrCurve("", typeof(SpriteRenderer), "m_Sprite");
			AnimationUtility.SetObjectReferenceCurve(newClip, binding, array);
		}

		public static List<Sprite> GetSpriteFromPathsOrObjects(UnityEngine.Object[] objects, string[] paths, EventType currentEventType)
		{
			List<Sprite> list = new List<Sprite>();
			for (int i = 0; i < objects.Length; i++)
			{
				UnityEngine.Object @object = objects[i];
				if (AssetDatabase.Contains(@object))
				{
					if (@object is Sprite)
					{
						list.Add(@object as Sprite);
					}
					else if (@object is UnityEngine.Texture2D)
					{
						list.AddRange(SpriteUtility.TextureToSprites(@object as UnityEngine.Texture2D));
					}
				}
			}
			if (!SpriteUtility.ExistingAssets(objects) && currentEventType == EventType.DragPerform && EditorSettings.defaultBehaviorMode == EditorBehaviorMode.Mode2D)
			{
				SpriteUtility.HandleExternalDrag(paths, true, ref list);
			}
			return list;
		}

		public static bool ExistingAssets(UnityEngine.Object[] objects)
		{
			bool result;
			for (int i = 0; i < objects.Length; i++)
			{
				UnityEngine.Object obj = objects[i];
				if (AssetDatabase.Contains(obj))
				{
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}

		private static void HandleExternalDrag(string[] paths, bool perform, ref List<Sprite> result)
		{
			for (int i = 0; i < paths.Length; i++)
			{
				string text = paths[i];
				if (SpriteUtility.ValidPathForTextureAsset(text))
				{
					DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
					if (perform)
					{
						string text2 = AssetDatabase.GenerateUniqueAssetPath(Path.Combine("Assets", FileUtil.GetLastPathNameComponent(text)));
						if (text2.Length > 0)
						{
							FileUtil.CopyFileOrDirectory(text, text2);
							SpriteUtility.ForcedImportFor(text2);
							Sprite sprite = SpriteUtility.GenerateDefaultSprite(AssetDatabase.LoadMainAssetAtPath(text2) as UnityEngine.Texture2D);
							if (sprite != null)
							{
								result.Add(sprite);
							}
						}
					}
				}
			}
		}

		private static bool PathsAreValidTextures(string[] paths)
		{
			bool result;
			if (paths == null || paths.Length == 0)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < paths.Length; i++)
				{
					string path = paths[i];
					if (!SpriteUtility.ValidPathForTextureAsset(path))
					{
						result = false;
						return result;
					}
				}
				result = true;
			}
			return result;
		}

		private static void ForcedImportFor(string newPath)
		{
			try
			{
				AssetDatabase.StartAssetEditing();
				AssetDatabase.ImportAsset(newPath);
			}
			finally
			{
				AssetDatabase.StopAssetEditing();
			}
		}

		private static Sprite GenerateDefaultSprite(UnityEngine.Texture2D texture)
		{
			string assetPath = AssetDatabase.GetAssetPath(texture);
			TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
			Sprite result;
			if (textureImporter == null)
			{
				result = null;
			}
			else if (textureImporter.textureType != TextureImporterType.Sprite)
			{
				result = null;
			}
			else
			{
				if (textureImporter.spriteImportMode == SpriteImportMode.None)
				{
					textureImporter.spriteImportMode = SpriteImportMode.Single;
					AssetDatabase.WriteImportSettingsIfDirty(assetPath);
					SpriteUtility.ForcedImportFor(assetPath);
				}
				UnityEngine.Object @object = AssetDatabase.LoadAllAssetsAtPath(assetPath).FirstOrDefault((UnityEngine.Object t) => t is Sprite);
				result = (@object as Sprite);
			}
			return result;
		}

		public static GameObject CreateDragGO(Sprite frame, Vector3 position)
		{
			string name = (!string.IsNullOrEmpty(frame.name)) ? frame.name : "Sprite";
			name = GameObjectUtility.GetUniqueNameForSibling(null, name);
			GameObject gameObject = new GameObject(name);
			SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
			spriteRenderer.sprite = frame;
			gameObject.transform.position = position;
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			return gameObject;
		}

		public static bool AddAnimationToGO(GameObject go, Sprite[] frames, SpriteUtility.ShowFileDialogDelegate saveFileDialog)
		{
			SpriteRenderer spriteRenderer = go.GetComponent<SpriteRenderer>();
			bool result;
			if (spriteRenderer == null)
			{
				Debug.LogWarning(SpriteUtility.SpriteUtilityStrings.unableToFindSpriteRendererWarning.text);
				spriteRenderer = go.AddComponent<SpriteRenderer>();
				if (spriteRenderer == null)
				{
					Debug.LogWarning(SpriteUtility.SpriteUtilityStrings.unableToAddSpriteRendererWarning.text);
					result = false;
					return result;
				}
			}
			spriteRenderer.sprite = frames[0];
			result = SpriteUtility.CreateAnimation(go, frames, saveFileDialog);
			return result;
		}

		public static GameObject DropSpriteToSceneToCreateGO(Sprite sprite, Vector3 position)
		{
			GameObject gameObject = new GameObject((!string.IsNullOrEmpty(sprite.name)) ? sprite.name : "Sprite");
			SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
			spriteRenderer.sprite = sprite;
			gameObject.transform.position = position;
			Selection.activeObject = gameObject;
			return gameObject;
		}

		public static Sprite RemapObjectToSprite(UnityEngine.Object obj)
		{
			Sprite result;
			if (obj is Sprite)
			{
				result = (Sprite)obj;
			}
			else
			{
				if (obj is UnityEngine.Texture2D)
				{
					UnityEngine.Object[] array = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(obj));
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i].GetType() == typeof(Sprite))
						{
							result = (array[i] as Sprite);
							return result;
						}
					}
				}
				result = null;
			}
			return result;
		}

		public static List<Sprite> TextureToSprites(UnityEngine.Texture2D tex)
		{
			UnityEngine.Object[] array = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(tex));
			List<Sprite> list = new List<Sprite>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].GetType() == typeof(Sprite))
				{
					list.Add(array[i] as Sprite);
				}
			}
			List<Sprite> result;
			if (list.Count > 0)
			{
				result = list;
			}
			else
			{
				Sprite sprite = SpriteUtility.GenerateDefaultSprite(tex);
				if (sprite != null)
				{
					list.Add(sprite);
				}
				result = list;
			}
			return result;
		}

		public static Sprite TextureToSprite(UnityEngine.Texture2D tex)
		{
			List<Sprite> list = SpriteUtility.TextureToSprites(tex);
			Sprite result;
			if (list.Count > 0)
			{
				result = list[0];
			}
			else
			{
				result = null;
			}
			return result;
		}

		private static bool ValidPathForTextureAsset(string path)
		{
			string a = FileUtil.GetPathExtension(path).ToLower();
			return a == "jpg" || a == "jpeg" || a == "tif" || a == "tiff" || a == "tga" || a == "gif" || a == "png" || a == "psd" || a == "bmp" || a == "iff" || a == "pict" || a == "pic" || a == "pct" || a == "exr" || a == "hdr";
		}

		public static UnityEngine.Texture2D RenderStaticPreview(Sprite sprite, Color color, int width, int height)
		{
			return SpriteUtility.RenderStaticPreview(sprite, color, width, height, Matrix4x4.identity);
		}

		public static UnityEngine.Texture2D RenderStaticPreview(Sprite sprite, Color color, int width, int height, Matrix4x4 transform)
		{
			UnityEngine.Texture2D result;
			if (sprite == null)
			{
				result = null;
			}
			else
			{
				PreviewHelpers.AdjustWidthAndHeightForStaticPreview((int)sprite.rect.width, (int)sprite.rect.height, ref width, ref height);
				SavedRenderTargetState savedRenderTargetState = new SavedRenderTargetState();
				RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
				RenderTexture.active = temporary;
				GL.Clear(true, true, new Color(0f, 0f, 0f, 0.1f));
				SpriteUtility.previewSpriteDefaultMaterial.mainTexture = sprite.texture;
				SpriteUtility.previewSpriteDefaultMaterial.SetPass(0);
				SpriteUtility.RenderSpriteImmediate(sprite, color, transform);
				UnityEngine.Texture2D texture2D = new UnityEngine.Texture2D(width, height, TextureFormat.ARGB32, false);
				texture2D.hideFlags = HideFlags.HideAndDontSave;
				texture2D.ReadPixels(new Rect(0f, 0f, (float)width, (float)height), 0, 0);
				texture2D.Apply();
				RenderTexture.ReleaseTemporary(temporary);
				savedRenderTargetState.Restore();
				result = texture2D;
			}
			return result;
		}

		internal static void RenderSpriteImmediate(Sprite sprite, Color color, Matrix4x4 transform)
		{
			float width = sprite.rect.width;
			float height = sprite.rect.height;
			float num = sprite.rect.width / sprite.bounds.size.x;
			Vector2[] vertices = sprite.vertices;
			Vector2[] uv = sprite.uv;
			ushort[] triangles = sprite.triangles;
			Vector2 pivot = sprite.pivot;
			GL.PushMatrix();
			GL.LoadOrtho();
			GL.Begin(4);
			for (int i = 0; i < sprite.triangles.Length; i++)
			{
				ushort num2 = triangles[i];
				Vector2 vector = vertices[(int)num2];
				Vector2 vector2 = uv[(int)num2];
				Vector3 point = new Vector3(vector.x, vector.y, 0f);
				point = transform.MultiplyPoint(point);
				point.x = (point.x * num + pivot.x) / width;
				point.y = (point.y * num + pivot.y) / height;
				GL.Color(color);
				GL.TexCoord(new Vector3(vector2.x, vector2.y, 0f));
				GL.Vertex3(point.x, point.y, point.z);
			}
			GL.End();
			GL.PopMatrix();
		}

		public static UnityEngine.Texture2D CreateTemporaryDuplicate(UnityEngine.Texture2D original, int width, int height)
		{
			UnityEngine.Texture2D result;
			if (!ShaderUtil.hardwareSupportsRectRenderTexture || !original)
			{
				result = null;
			}
			else
			{
				RenderTexture active = RenderTexture.active;
				Rect rawViewportRect = ShaderUtil.rawViewportRect;
				RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.sRGB);
				Graphics.Blit(original, temporary, EditorGUIUtility.GUITextureBlit2SRGBMaterial);
				RenderTexture.active = temporary;
				bool flag = width >= SystemInfo.maxTextureSize || height >= SystemInfo.maxTextureSize;
				UnityEngine.Texture2D texture2D = new UnityEngine.Texture2D(width, height, TextureFormat.RGBA32, original.mipmapCount > 1 || flag);
				texture2D.ReadPixels(new Rect(0f, 0f, (float)width, (float)height), 0, 0);
				texture2D.Apply();
				RenderTexture.ReleaseTemporary(temporary);
				EditorGUIUtility.SetRenderTextureNoViewport(active);
				ShaderUtil.rawViewportRect = rawViewportRect;
				texture2D.alphaIsTransparency = original.alphaIsTransparency;
				result = texture2D;
			}
			return result;
		}

		public static SpriteImportMode GetSpriteImportMode(ISpriteEditorDataProvider dataProvider)
		{
			return (dataProvider != null) ? dataProvider.spriteImportMode : SpriteImportMode.None;
		}
	}
}
