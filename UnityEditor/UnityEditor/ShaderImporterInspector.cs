using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEditor
{
	[CustomEditor(typeof(ShaderImporter))]
	internal class ShaderImporterInspector : AssetImporterEditor
	{
		private class TextureProp
		{
			public string propertyName;

			public string displayName;

			public Texture texture;

			public TextureDimension dimension;

			public bool modifiable;
		}

		private List<ShaderImporterInspector.TextureProp> m_Properties = new List<ShaderImporterInspector.TextureProp>();

		private static string[] s_ShaderIncludePaths = null;

		internal override void OnHeaderControlsGUI()
		{
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Open...", EditorStyles.miniButton, new GUILayoutOption[0]))
			{
				AssetDatabase.OpenAsset(base.assetTarget);
				GUIUtility.ExitGUI();
			}
		}

		public override void OnEnable()
		{
			this.ResetValues();
		}

		private void ShowTextures()
		{
			if (this.m_Properties.Count != 0)
			{
				EditorGUILayout.LabelField("Default Maps", EditorStyles.boldLabel, new GUILayoutOption[0]);
				for (int i = 0; i < this.m_Properties.Count; i++)
				{
					if (this.m_Properties[i].modifiable)
					{
						this.DrawTextureField(this.m_Properties[i]);
					}
				}
				EditorGUILayout.LabelField("NonModifiable Maps", EditorStyles.boldLabel, new GUILayoutOption[0]);
				for (int j = 0; j < this.m_Properties.Count; j++)
				{
					if (!this.m_Properties[j].modifiable)
					{
						this.DrawTextureField(this.m_Properties[j]);
					}
				}
			}
		}

		private void DrawTextureField(ShaderImporterInspector.TextureProp prop)
		{
			Texture texture = prop.texture;
			Texture texture2 = null;
			EditorGUI.BeginChangeCheck();
			Type textureTypeFromDimension = MaterialEditor.GetTextureTypeFromDimension(prop.dimension);
			if (textureTypeFromDimension != null)
			{
				string t = (!string.IsNullOrEmpty(prop.displayName)) ? prop.displayName : ObjectNames.NicifyVariableName(prop.propertyName);
				texture2 = (EditorGUILayout.MiniThumbnailObjectField(GUIContent.Temp(t), texture, textureTypeFromDimension, new GUILayoutOption[0]) as Texture);
			}
			if (EditorGUI.EndChangeCheck())
			{
				prop.texture = texture2;
			}
		}

		public override bool HasModified()
		{
			bool result;
			if (base.HasModified())
			{
				result = true;
			}
			else
			{
				ShaderImporter shaderImporter = base.target as ShaderImporter;
				if (shaderImporter == null)
				{
					result = false;
				}
				else
				{
					Shader shader = shaderImporter.GetShader();
					if (shader == null)
					{
						result = false;
					}
					else
					{
						int propertyCount = ShaderUtil.GetPropertyCount(shader);
						for (int i = 0; i < propertyCount; i++)
						{
							string propertyName = ShaderUtil.GetPropertyName(shader, i);
							for (int j = 0; j < this.m_Properties.Count; j++)
							{
								if (this.m_Properties[j].propertyName == propertyName)
								{
									Texture y = (!this.m_Properties[j].modifiable) ? shaderImporter.GetNonModifiableTexture(propertyName) : shaderImporter.GetDefaultTexture(propertyName);
									if (this.m_Properties[j].texture != y)
									{
										result = true;
										return result;
									}
								}
							}
						}
						result = false;
					}
				}
			}
			return result;
		}

		protected override void ResetValues()
		{
			base.ResetValues();
			this.m_Properties.Clear();
			ShaderImporter shaderImporter = base.target as ShaderImporter;
			if (!(shaderImporter == null))
			{
				Shader shader = shaderImporter.GetShader();
				if (!(shader == null))
				{
					int propertyCount = ShaderUtil.GetPropertyCount(shader);
					for (int i = 0; i < propertyCount; i++)
					{
						if (ShaderUtil.GetPropertyType(shader, i) == ShaderUtil.ShaderPropertyType.TexEnv)
						{
							string propertyName = ShaderUtil.GetPropertyName(shader, i);
							string propertyDescription = ShaderUtil.GetPropertyDescription(shader, i);
							bool flag = !ShaderUtil.IsShaderPropertyNonModifiableTexureProperty(shader, i);
							Texture texture;
							if (!flag)
							{
								texture = shaderImporter.GetNonModifiableTexture(propertyName);
							}
							else
							{
								texture = shaderImporter.GetDefaultTexture(propertyName);
							}
							ShaderImporterInspector.TextureProp item = new ShaderImporterInspector.TextureProp
							{
								propertyName = propertyName,
								texture = texture,
								dimension = ShaderUtil.GetTexDim(shader, i),
								displayName = propertyDescription,
								modifiable = flag
							};
							this.m_Properties.Add(item);
						}
					}
				}
			}
		}

		protected override void Apply()
		{
			base.Apply();
			ShaderImporter shaderImporter = base.target as ShaderImporter;
			if (!(shaderImporter == null))
			{
				string[] name = (from x in this.m_Properties
				where x.modifiable
				select x.propertyName).ToArray<string>();
				Texture[] textures = (from x in this.m_Properties
				where x.modifiable
				select x.texture).ToArray<Texture>();
				shaderImporter.SetDefaultTextures(name, textures);
				string[] name2 = (from x in this.m_Properties
				where !x.modifiable
				select x.propertyName).ToArray<string>();
				Texture[] textures2 = (from x in this.m_Properties
				where !x.modifiable
				select x.texture).ToArray<Texture>();
				shaderImporter.SetNonModifiableTextures(name2, textures2);
				AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(shaderImporter));
			}
		}

		private static int GetNumberOfTextures(Shader shader)
		{
			int num = 0;
			int propertyCount = ShaderUtil.GetPropertyCount(shader);
			for (int i = 0; i < propertyCount; i++)
			{
				if (ShaderUtil.GetPropertyType(shader, i) == ShaderUtil.ShaderPropertyType.TexEnv)
				{
					num++;
				}
			}
			return num;
		}

		public override void OnInspectorGUI()
		{
			ShaderImporter shaderImporter = base.target as ShaderImporter;
			if (!(shaderImporter == null))
			{
				Shader shader = shaderImporter.GetShader();
				if (!(shader == null))
				{
					if (ShaderImporterInspector.GetNumberOfTextures(shader) != this.m_Properties.Count)
					{
						this.ResetValues();
					}
					this.ShowTextures();
					base.ApplyRevertGUI();
				}
			}
		}

		[RequiredByNativeCode]
		internal static string[] GetShaderIncludePaths()
		{
			if (ShaderImporterInspector.s_ShaderIncludePaths == null)
			{
				List<string> list = new List<string>();
				AttributeHelper.MethodInfoSorter methodsWithAttribute = AttributeHelper.GetMethodsWithAttribute<ShaderIncludePathAttribute>(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				foreach (AttributeHelper.MethodWithAttribute current in methodsWithAttribute.methodsWithAttributes)
				{
					if (current.info.ReturnType == typeof(string[]) && current.info.GetParameters().Length == 0)
					{
						string[] array = current.info.Invoke(null, new object[0]) as string[];
						if (array != null)
						{
							list.AddRange(array);
						}
					}
				}
				ShaderImporterInspector.s_ShaderIncludePaths = list.ToArray();
			}
			return ShaderImporterInspector.s_ShaderIncludePaths;
		}
	}
}
