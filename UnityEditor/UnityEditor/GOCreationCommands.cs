using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

namespace UnityEditor
{
	internal static class GOCreationCommands
	{
		internal static GameObject CreateGameObject(GameObject parent, string name, params Type[] types)
		{
			return ObjectFactory.CreateGameObject(GameObjectUtility.GetUniqueNameForSibling((!(parent != null)) ? null : parent.transform, name), types);
		}

		internal static void Place(GameObject go, GameObject parent)
		{
			if (parent != null)
			{
				Transform transform = go.transform;
				transform.SetParent(parent.transform, false);
				transform.localPosition = Vector3.zero;
				transform.localRotation = Quaternion.identity;
				transform.localScale = Vector3.one;
				go.layer = parent.layer;
				if (parent.GetComponent<RectTransform>())
				{
					ObjectFactory.AddComponent<RectTransform>(go);
				}
			}
			SceneView.PlaceGameObjectInFrontOfSceneView(go);
			EditorWindow.FocusWindowIfItsOpen<SceneHierarchyWindow>();
			Selection.activeGameObject = go;
		}

		[MenuItem("GameObject/Create Empty %#n", priority = 0)]
		private static void CreateEmpty(MenuCommand menuCommand)
		{
			GameObject parent = menuCommand.context as GameObject;
			GOCreationCommands.Place(GOCreationCommands.CreateGameObject(parent, "GameObject", new Type[0]), parent);
		}

		[MenuItem("GameObject/Create Empty Child &#n", priority = 0)]
		private static void CreateEmptyChild(MenuCommand menuCommand)
		{
			GameObject gameObject = menuCommand.context as GameObject;
			if (gameObject == null)
			{
				gameObject = Selection.activeGameObject;
			}
			GameObject go = GOCreationCommands.CreateGameObject(gameObject, "GameObject", new Type[0]);
			GOCreationCommands.Place(go, gameObject);
		}

		private static void CreateAndPlacePrimitive(PrimitiveType type, GameObject parent)
		{
			string uniqueNameForSibling = GameObjectUtility.GetUniqueNameForSibling((!(parent != null)) ? null : parent.transform, type.ToString());
			GameObject gameObject = ObjectFactory.CreatePrimitive(type);
			gameObject.name = uniqueNameForSibling;
			GOCreationCommands.Place(gameObject, parent);
		}

		[MenuItem("GameObject/3D Object/Cube", priority = 1)]
		private static void CreateCube(MenuCommand menuCommand)
		{
			GOCreationCommands.CreateAndPlacePrimitive(PrimitiveType.Cube, menuCommand.context as GameObject);
		}

		[MenuItem("GameObject/3D Object/Sphere", priority = 2)]
		private static void CreateSphere(MenuCommand menuCommand)
		{
			GOCreationCommands.CreateAndPlacePrimitive(PrimitiveType.Sphere, menuCommand.context as GameObject);
		}

		[MenuItem("GameObject/3D Object/Capsule", priority = 3)]
		private static void CreateCapsule(MenuCommand menuCommand)
		{
			GOCreationCommands.CreateAndPlacePrimitive(PrimitiveType.Capsule, menuCommand.context as GameObject);
		}

		[MenuItem("GameObject/3D Object/Cylinder", priority = 4)]
		private static void CreateCylinder(MenuCommand menuCommand)
		{
			GOCreationCommands.CreateAndPlacePrimitive(PrimitiveType.Cylinder, menuCommand.context as GameObject);
		}

		[MenuItem("GameObject/3D Object/Plane", priority = 5)]
		private static void CreatePlane(MenuCommand menuCommand)
		{
			GOCreationCommands.CreateAndPlacePrimitive(PrimitiveType.Plane, menuCommand.context as GameObject);
		}

		[MenuItem("GameObject/3D Object/Quad", priority = 6)]
		private static void CreateQuad(MenuCommand menuCommand)
		{
			GOCreationCommands.CreateAndPlacePrimitive(PrimitiveType.Quad, menuCommand.context as GameObject);
		}

		[MenuItem("GameObject/2D Object/Sprite", priority = 1)]
		private static void CreateSprite(MenuCommand menuCommand)
		{
			GameObject parent = menuCommand.context as GameObject;
			GameObject gameObject = GOCreationCommands.CreateGameObject(parent, "New Sprite", new Type[]
			{
				typeof(SpriteRenderer)
			});
			Sprite sprite = Selection.activeObject as Sprite;
			if (sprite == null)
			{
				Texture2D texture2D = Selection.activeObject as Texture2D;
				if (texture2D)
				{
					string assetPath = AssetDatabase.GetAssetPath(texture2D);
					sprite = AssetDatabase.LoadAllAssetsAtPath(assetPath).OfType<Sprite>().First<Sprite>();
					if (sprite == null)
					{
						TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
						if (textureImporter != null && textureImporter.textureType != TextureImporterType.Sprite)
						{
							EditorUtility.DisplayDialog("Sprite could not be assigned!", "Can not assign a Sprite to the new SpriteRenderer because the selected Texture is not configured to generate Sprites.", "OK");
						}
					}
				}
			}
			if (sprite != null)
			{
				gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
			}
			GOCreationCommands.Place(gameObject, parent);
		}

		[MenuItem("GameObject/Light/Directional Light", priority = 1)]
		private static void CreateDirectionalLight(MenuCommand menuCommand)
		{
			GameObject parent = menuCommand.context as GameObject;
			GameObject gameObject = GOCreationCommands.CreateGameObject(null, "Directional Light", new Type[]
			{
				typeof(Light)
			});
			gameObject.GetComponent<Light>().type = LightType.Directional;
			gameObject.GetComponent<Light>().intensity = 1f;
			gameObject.GetComponent<Transform>().SetLocalEulerAngles(new Vector3(50f, -30f, 0f), RotationOrder.OrderZXY);
			GOCreationCommands.Place(gameObject, parent);
		}

		[MenuItem("GameObject/Light/Point Light", priority = 2)]
		private static void CreatePointLight(MenuCommand menuCommand)
		{
			GameObject parent = menuCommand.context as GameObject;
			GameObject gameObject = GOCreationCommands.CreateGameObject(null, "Point Light", new Type[]
			{
				typeof(Light)
			});
			gameObject.GetComponent<Light>().type = LightType.Point;
			GOCreationCommands.Place(gameObject, parent);
		}

		[MenuItem("GameObject/Light/Spotlight", priority = 3)]
		private static void CreateSpotLight(MenuCommand menuCommand)
		{
			GameObject parent = menuCommand.context as GameObject;
			GameObject gameObject = GOCreationCommands.CreateGameObject(null, "Spot Light", new Type[]
			{
				typeof(Light)
			});
			gameObject.GetComponent<Light>().type = LightType.Spot;
			gameObject.GetComponent<Transform>().SetLocalEulerAngles(new Vector3(90f, 0f, 0f), RotationOrder.OrderZXY);
			GOCreationCommands.Place(gameObject, parent);
		}

		[MenuItem("GameObject/Light/Area Light", priority = 4)]
		private static void CreateAreaLight(MenuCommand menuCommand)
		{
			GameObject parent = menuCommand.context as GameObject;
			GameObject gameObject = GOCreationCommands.CreateGameObject(null, "Area Light", new Type[]
			{
				typeof(Light)
			});
			gameObject.GetComponent<Light>().type = LightType.Area;
			gameObject.GetComponent<Transform>().SetLocalEulerAngles(new Vector3(90f, 0f, 0f), RotationOrder.OrderZXY);
			GOCreationCommands.Place(gameObject, parent);
		}

		[MenuItem("GameObject/Light/Reflection Probe", priority = 20)]
		private static void CreateReflectionProbe(MenuCommand menuCommand)
		{
			GameObject parent = menuCommand.context as GameObject;
			GOCreationCommands.Place(GOCreationCommands.CreateGameObject(parent, "Reflection Probe", new Type[]
			{
				typeof(ReflectionProbe)
			}), parent);
		}

		[MenuItem("GameObject/Light/Light Probe Group", priority = 21)]
		private static void CreateLightProbeGroup(MenuCommand menuCommand)
		{
			GameObject parent = menuCommand.context as GameObject;
			GOCreationCommands.Place(GOCreationCommands.CreateGameObject(parent, "Light Probe Group", new Type[]
			{
				typeof(LightProbeGroup)
			}), parent);
		}

		[MenuItem("GameObject/Audio/Audio Source", priority = 1)]
		private static void CreateAudioSource(MenuCommand menuCommand)
		{
			GameObject parent = menuCommand.context as GameObject;
			GOCreationCommands.Place(GOCreationCommands.CreateGameObject(parent, "Audio Source", new Type[]
			{
				typeof(AudioSource)
			}), parent);
		}

		[MenuItem("GameObject/Audio/Audio Reverb Zone", priority = 2)]
		private static void CreateAudioReverbZone(MenuCommand menuCommand)
		{
			GameObject parent = menuCommand.context as GameObject;
			GOCreationCommands.Place(GOCreationCommands.CreateGameObject(parent, "Audio Reverb Zone", new Type[]
			{
				typeof(AudioReverbZone)
			}), parent);
		}

		[MenuItem("GameObject/Video/Video Player", priority = 1)]
		private static void CreateVideoPlayer(MenuCommand menuCommand)
		{
			GameObject parent = menuCommand.context as GameObject;
			GOCreationCommands.Place(GOCreationCommands.CreateGameObject(parent, "Video Player", new Type[]
			{
				typeof(VideoPlayer)
			}), parent);
		}

		[MenuItem("GameObject/Effects/Particle System", priority = 1)]
		private static void CreateParticleSystem(MenuCommand menuCommand)
		{
			GameObject parent = menuCommand.context as GameObject;
			GameObject gameObject = GOCreationCommands.CreateGameObject(parent, "Particle System", new Type[]
			{
				typeof(ParticleSystem)
			});
			gameObject.GetComponent<Transform>().SetLocalEulerAngles(new Vector3(-90f, 0f, 0f), RotationOrder.OrderZXY);
			ParticleSystemRenderer component = gameObject.GetComponent<ParticleSystemRenderer>();
			Renderer arg_62_0 = component;
			Material[] expr_5A = new Material[2];
			expr_5A[0] = Material.GetDefaultParticleMaterial();
			arg_62_0.materials = expr_5A;
			GOCreationCommands.Place(gameObject, parent);
		}

		[MenuItem("GameObject/Effects/Trail", priority = 2)]
		private static void CreateTrail(MenuCommand menuCommand)
		{
			GameObject parent = menuCommand.context as GameObject;
			GameObject gameObject = GOCreationCommands.CreateGameObject(parent, "Trail", new Type[]
			{
				typeof(TrailRenderer)
			});
			gameObject.GetComponent<TrailRenderer>().material = Material.GetDefaultLineMaterial();
			GOCreationCommands.Place(gameObject, parent);
		}

		[MenuItem("GameObject/Effects/Line", priority = 3)]
		private static void CreateLine(MenuCommand menuCommand)
		{
			GameObject parent = menuCommand.context as GameObject;
			GameObject gameObject = GOCreationCommands.CreateGameObject(parent, "Line", new Type[]
			{
				typeof(LineRenderer)
			});
			LineRenderer component = gameObject.GetComponent<LineRenderer>();
			component.material = Material.GetDefaultLineMaterial();
			component.widthMultiplier = 0.1f;
			component.useWorldSpace = false;
			GOCreationCommands.Place(gameObject, parent);
		}

		[MenuItem("GameObject/Camera", priority = 11)]
		private static void CreateCamera(MenuCommand menuCommand)
		{
			GameObject parent = menuCommand.context as GameObject;
			GOCreationCommands.Place(GOCreationCommands.CreateGameObject(parent, "Camera", new Type[]
			{
				typeof(Camera),
				typeof(FlareLayer),
				typeof(AudioListener)
			}), parent);
		}
	}
}
