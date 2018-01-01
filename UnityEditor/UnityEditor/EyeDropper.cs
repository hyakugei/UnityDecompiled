using System;
using UnityEditorInternal;
using UnityEngine;

namespace UnityEditor
{
	internal class EyeDropper : GUIView
	{
		private static class Styles
		{
			public static readonly GUIStyle eyeDropperHorizontalLine = "EyeDropperHorizontalLine";

			public static readonly GUIStyle eyeDropperVerticalLine = "EyeDropperVerticalLine";

			public static readonly GUIStyle eyeDropperPickedPixel = "EyeDropperPickedPixel";
		}

		private const int kPixelSize = 10;

		private const int kDummyWindowSize = 8192;

		internal static Color s_LastPickedColor;

		private GUIView m_DelegateView;

		private Texture2D m_Preview;

		private static EyeDropper s_Instance;

		private static Vector2 s_PickCoordinates = Vector2.zero;

		private bool m_Focused = false;

		private Action<Color> m_ColorPickedCallback;

		private static EyeDropper instance
		{
			get
			{
				if (!EyeDropper.s_Instance)
				{
					ScriptableObject.CreateInstance<EyeDropper>();
				}
				return EyeDropper.s_Instance;
			}
		}

		private EyeDropper()
		{
			EyeDropper.s_Instance = this;
		}

		public static void Start(GUIView viewToUpdate, bool stealFocus = true)
		{
			EyeDropper.Start(viewToUpdate, null, stealFocus);
		}

		public static void Start(Action<Color> colorPickedCallback, bool stealFocus = true)
		{
			EyeDropper.Start(null, colorPickedCallback, stealFocus);
		}

		private static void Start(GUIView viewToUpdate, Action<Color> colorPickedCallback, bool stealFocus)
		{
			EyeDropper.instance.m_DelegateView = viewToUpdate;
			EyeDropper.instance.m_ColorPickedCallback = colorPickedCallback;
			ContainerWindow containerWindow = ScriptableObject.CreateInstance<ContainerWindow>();
			containerWindow.m_DontSaveToLayout = true;
			containerWindow.title = "EyeDropper";
			containerWindow.hideFlags = HideFlags.DontSave;
			containerWindow.rootView = EyeDropper.instance;
			containerWindow.Show(ShowMode.PopupMenu, true, false);
			EyeDropper.instance.AddToAuxWindowList();
			containerWindow.SetInvisible();
			EyeDropper.instance.SetMinMaxSizes(new Vector2(0f, 0f), new Vector2(8192f, 8192f));
			containerWindow.position = new Rect(-4096f, -4096f, 8192f, 8192f);
			EyeDropper.instance.wantsMouseMove = true;
			EyeDropper.instance.StealMouseCapture();
			if (stealFocus)
			{
				EyeDropper.instance.Focus();
			}
		}

		public static void End()
		{
			if (EyeDropper.s_Instance != null)
			{
				EyeDropper.s_Instance.window.Close();
			}
		}

		public static Color GetPickedColor()
		{
			return InternalEditorUtility.ReadScreenPixel(EyeDropper.s_PickCoordinates, 1, 1)[0];
		}

		public static Color GetLastPickedColor()
		{
			return EyeDropper.s_LastPickedColor;
		}

		public static void DrawPreview(Rect position)
		{
			if (Event.current.type == EventType.Repaint)
			{
				Texture2D texture2D = EyeDropper.instance.m_Preview;
				int num = (int)Mathf.Ceil(position.width / 10f);
				int num2 = (int)Mathf.Ceil(position.height / 10f);
				if (texture2D == null)
				{
					texture2D = (EyeDropper.instance.m_Preview = ColorPicker.MakeTexture(num, num2));
					texture2D.filterMode = FilterMode.Point;
				}
				if (texture2D.width != num || texture2D.height != num2)
				{
					texture2D.Resize(num, num2);
				}
				Vector2 a = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
				Vector2 pixelPos = a - new Vector2((float)(num / 2), (float)(num2 / 2));
				texture2D.SetPixels(InternalEditorUtility.ReadScreenPixel(pixelPos, num, num2), 0);
				texture2D.Apply(true);
				Graphics.DrawTexture(position, texture2D);
				float num3 = position.width / (float)num;
				GUIStyle gUIStyle = EyeDropper.Styles.eyeDropperVerticalLine;
				for (float num4 = position.x; num4 < position.xMax; num4 += num3)
				{
					Rect position2 = new Rect(Mathf.Round(num4), position.y, num3, position.height);
					gUIStyle.Draw(position2, false, false, false, false);
				}
				float num5 = position.height / (float)num2;
				gUIStyle = EyeDropper.Styles.eyeDropperHorizontalLine;
				for (float num6 = position.y; num6 < position.yMax; num6 += num5)
				{
					Rect position3 = new Rect(position.x, Mathf.Floor(num6), position.width, num5);
					gUIStyle.Draw(position3, false, false, false, false);
				}
				Rect position4 = new Rect((a.x - pixelPos.x) * num3 + position.x, (a.y - pixelPos.y) * num5 + position.y, num3, num5);
				EyeDropper.Styles.eyeDropperPickedPixel.Draw(position4, false, false, false, false);
			}
		}

		protected override void OldOnGUI()
		{
			EventType type = Event.current.type;
			if (type != EventType.MouseMove)
			{
				if (type != EventType.MouseDown)
				{
					if (type == EventType.KeyDown)
					{
						if (Event.current.keyCode == KeyCode.Escape)
						{
							base.window.Close();
							Event.current.Use();
							this.SendEvent("EyeDropperCancelled", true, true);
						}
					}
				}
				else if (Event.current.button == 0)
				{
					EyeDropper.s_PickCoordinates = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
					base.window.Close();
					EyeDropper.s_LastPickedColor = EyeDropper.GetPickedColor();
					Event.current.Use();
					this.SendEvent("EyeDropperClicked", true, true);
					if (this.m_ColorPickedCallback != null)
					{
						this.m_ColorPickedCallback(EyeDropper.s_LastPickedColor);
					}
				}
			}
			else
			{
				EyeDropper.s_PickCoordinates = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
				base.StealMouseCapture();
				this.SendEvent("EyeDropperUpdate", true, false);
			}
		}

		private void SendEvent(string eventName, bool exitGUI, bool focusOther = true)
		{
			if (this.m_DelegateView != null)
			{
				Event e = EditorGUIUtility.CommandEvent(eventName);
				if (focusOther)
				{
					this.m_DelegateView.Focus();
				}
				this.m_DelegateView.SendEvent(e);
				if (exitGUI)
				{
					GUIUtility.ExitGUI();
				}
			}
		}

		public new void OnDestroy()
		{
			if (this.m_Preview)
			{
				UnityEngine.Object.DestroyImmediate(this.m_Preview);
			}
			if (!this.m_Focused)
			{
				this.SendEvent("EyeDropperCancelled", false, true);
			}
			base.OnDestroy();
		}

		protected override bool OnFocus()
		{
			this.m_Focused = true;
			return base.OnFocus();
		}
	}
}
