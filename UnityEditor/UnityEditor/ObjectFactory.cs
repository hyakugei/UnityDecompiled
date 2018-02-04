using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Bindings;

namespace UnityEditor
{
	public static class ObjectFactory
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern UnityEngine.Object CreateDefaultInstance([NotNull] Type type);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Component AddDefaultComponent([NotNull] GameObject gameObject, [NotNull] Type type);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern GameObject CreateDefaultGameObject(string name);

		private static void CheckTypeValidity(Type type)
		{
			if (type.IsAbstract)
			{
				throw new ArgumentException("Abstract types can't be used in the ObjectFactory : " + type.FullName);
			}
			if (Attribute.GetCustomAttribute(type, typeof(ExcludeFromObjectFactoryAttribute)) != null)
			{
				throw new ArgumentException("The type " + type.FullName + " is not supported by the ObjectFactory.");
			}
		}

		public static T CreateInstance<T>() where T : UnityEngine.Object
		{
			return (T)((object)ObjectFactory.CreateInstance(typeof(T)));
		}

		public static UnityEngine.Object CreateInstance(Type type)
		{
			ObjectFactory.CheckTypeValidity(type);
			if (type == typeof(GameObject))
			{
				throw new ArgumentException("GameObject type must be created using ObjectFactory.CreateGameObject instead : " + type.FullName);
			}
			if (type.IsSubclassOf(typeof(Component)))
			{
				throw new ArgumentException("Component type must be created using ObjectFactory.AddComponent instead : " + type.FullName);
			}
			if (type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null) != null)
			{
				throw new ArgumentException(type.FullName + " constructor is not accessible which prevent this type from being used in ObjectFactory.");
			}
			return ObjectFactory.CreateDefaultInstance(type);
		}

		public static T AddComponent<T>(GameObject gameObject) where T : Component
		{
			return (T)((object)ObjectFactory.AddComponent(gameObject, typeof(T)));
		}

		public static Component AddComponent(GameObject gameObject, Type type)
		{
			ObjectFactory.CheckTypeValidity(type);
			if (!type.IsSubclassOf(typeof(Component)))
			{
				throw new ArgumentException("Non-Component type must use ObjectFactory.CreateInstance instead : " + type.FullName);
			}
			return ObjectFactory.AddDefaultComponent(gameObject, type);
		}

		public static GameObject CreateGameObject(string name, params Type[] types)
		{
			GameObject gameObject = ObjectFactory.CreateDefaultGameObject(name);
			gameObject.SetActive(false);
			for (int i = 0; i < types.Length; i++)
			{
				Type type = types[i];
				ObjectFactory.AddComponent(gameObject, type);
			}
			gameObject.SetActive(true);
			return gameObject;
		}

		public static GameObject CreatePrimitive(PrimitiveType type)
		{
			GameObject gameObject = ObjectFactory.CreateGameObject(type.ToString(), new Type[]
			{
				typeof(MeshFilter),
				typeof(MeshRenderer)
			});
			gameObject.SetActive(false);
			switch (type)
			{
			case PrimitiveType.Sphere:
				gameObject.GetComponent<MeshFilter>().sharedMesh = Resources.GetBuiltinResource<Mesh>("New-Sphere.fbx");
				ObjectFactory.AddComponent<SphereCollider>(gameObject);
				break;
			case PrimitiveType.Capsule:
			{
				gameObject.GetComponent<MeshFilter>().sharedMesh = Resources.GetBuiltinResource<Mesh>("New-Capsule.fbx");
				CapsuleCollider capsuleCollider = ObjectFactory.AddComponent<CapsuleCollider>(gameObject);
				capsuleCollider.height = 2f;
				break;
			}
			case PrimitiveType.Cylinder:
			{
				gameObject.GetComponent<MeshFilter>().sharedMesh = Resources.GetBuiltinResource<Mesh>("New-Cylinder.fbx");
				CapsuleCollider capsuleCollider2 = ObjectFactory.AddComponent<CapsuleCollider>(gameObject);
				capsuleCollider2.height = 2f;
				break;
			}
			case PrimitiveType.Cube:
				gameObject.GetComponent<MeshFilter>().sharedMesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
				ObjectFactory.AddComponent<BoxCollider>(gameObject);
				break;
			case PrimitiveType.Plane:
				gameObject.GetComponent<MeshFilter>().sharedMesh = Resources.GetBuiltinResource<Mesh>("New-Plane.fbx");
				ObjectFactory.AddComponent<MeshCollider>(gameObject);
				break;
			case PrimitiveType.Quad:
				gameObject.GetComponent<MeshFilter>().sharedMesh = Resources.GetBuiltinResource<Mesh>("Quad.fbx");
				ObjectFactory.AddComponent<MeshCollider>(gameObject);
				break;
			}
			Renderer component = gameObject.GetComponent<Renderer>();
			component.material = Material.GetDefaultMaterial();
			gameObject.SetActive(true);
			return gameObject;
		}
	}
}
