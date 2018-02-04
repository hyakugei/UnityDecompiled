using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

namespace UnityEditorInternal.VR
{
	public class HolographicEmulationWindow : EditorWindow
	{
		private bool m_InPlayMode = false;

		private bool m_OperatingSystemChecked = false;

		private bool m_OperatingSystemValid = false;

		private HolographicStreamerConnectionState m_LastConnectionState = HolographicStreamerConnectionState.Disconnected;

		[SerializeField]
		private EmulationMode m_Mode = EmulationMode.None;

		[SerializeField]
		private int m_RoomIndex = 0;

		[SerializeField]
		private GestureHand m_Hand = GestureHand.Right;

		[SerializeField]
		private string m_RemoteMachineAddress = "";

		[SerializeField]
		private bool m_EnableVideo = true;

		[SerializeField]
		private bool m_EnableAudio = true;

		[SerializeField]
		private int m_MaxBitrateKbps = 99999;

		private string[] m_RemoteMachineHistory;

		private static int s_MaxHistoryLength = 4;

		private static GUIContent s_ConnectionStatusText = EditorGUIUtility.TrTextContent("Connection Status", null, null);

		private static GUIContent s_EmulationModeText = EditorGUIUtility.TrTextContent("Emulation Mode", null, null);

		private static GUIContent s_RoomText = EditorGUIUtility.TrTextContent("Room", null, null);

		private static GUIContent s_HandText = EditorGUIUtility.TrTextContent("Gesture Hand", null, null);

		private static GUIContent s_RemoteMachineText = EditorGUIUtility.TrTextContent("Remote Machine", null, null);

		private static GUIContent s_EnableVideoText = EditorGUIUtility.TrTextContent("Enable Video", null, null);

		private static GUIContent s_EnableAudioText = EditorGUIUtility.TrTextContent("Enable Audio", null, null);

		private static GUIContent s_MaxBitrateText = EditorGUIUtility.TrTextContent("Max Bitrate (kbps)", null, null);

		private static GUIContent s_ConnectionButtonConnectText = EditorGUIUtility.TrTextContent("Connect", null, null);

		private static GUIContent s_ConnectionButtonDisconnectText = EditorGUIUtility.TrTextContent("Disconnect", null, null);

		private static GUIContent s_ConnectionStateDisconnectedText = EditorGUIUtility.TrTextContent("Disconnected", null, null);

		private static GUIContent s_ConnectionStateConnectingText = EditorGUIUtility.TrTextContent("Connecting", null, null);

		private static GUIContent s_ConnectionStateConnectedText = EditorGUIUtility.TrTextContent("Connected", null, null);

		private static Texture2D s_ConnectedTexture = null;

		private static Texture2D s_ConnectingTexture = null;

		private static Texture2D s_DisconnectedTexture = null;

		private static GUIContent[] s_ModeStrings = new GUIContent[]
		{
			EditorGUIUtility.TrTextContent("None", null, null),
			EditorGUIUtility.TrTextContent("Remote to Device", null, null),
			EditorGUIUtility.TrTextContent("Simulate in Editor", null, null)
		};

		private static GUIContent[] s_RoomStrings = new GUIContent[]
		{
			EditorGUIUtility.TrTextContent("None", null, null),
			EditorGUIUtility.TrTextContent("DefaultRoom", null, null),
			EditorGUIUtility.TrTextContent("Bedroom1", null, null),
			EditorGUIUtility.TrTextContent("Bedroom2", null, null),
			EditorGUIUtility.TrTextContent("GreatRoom", null, null),
			EditorGUIUtility.TrTextContent("LivingRoom", null, null)
		};

		private static GUIContent[] s_HandStrings = new GUIContent[]
		{
			EditorGUIUtility.TrTextContent("Left Hand", null, null),
			EditorGUIUtility.TrTextContent("Right Hand", null, null)
		};

		public EmulationMode emulationMode
		{
			get
			{
				return this.m_Mode;
			}
			set
			{
				this.m_Mode = value;
				base.Repaint();
			}
		}

		private bool RemoteMachineNameSpecified
		{
			get
			{
				return !string.IsNullOrEmpty(this.m_RemoteMachineAddress);
			}
		}

		internal static void Init()
		{
			EditorWindow.GetWindow<HolographicEmulationWindow>(false);
		}

		private void OnEnable()
		{
			base.titleContent = EditorGUIUtility.TrTextContent("Holographic", null, null);
			EditorApplication.playModeStateChanged += new Action<PlayModeStateChange>(this.OnPlayModeStateChanged);
			this.m_InPlayMode = EditorApplication.isPlayingOrWillChangePlaymode;
			this.m_RemoteMachineHistory = EditorPrefs.GetString("HolographicRemoting.RemoteMachineHistory").Split(new char[]
			{
				','
			});
		}

		private void OnDisable()
		{
			EditorApplication.playModeStateChanged -= new Action<PlayModeStateChange>(this.OnPlayModeStateChanged);
		}

		private void LoadCurrentRoom()
		{
			if (this.m_RoomIndex != 0)
			{
				string str = EditorApplication.applicationContentsPath + "/UnityExtensions/Unity/VR/HolographicSimulation/Rooms/";
				HolographicEmulation.LoadRoom(str + HolographicEmulationWindow.s_RoomStrings[this.m_RoomIndex].text + ".xef");
			}
		}

		private void InitializeSimulation()
		{
			this.Disconnect();
			HolographicEmulation.Initialize();
			this.LoadCurrentRoom();
		}

		private void OnPlayModeStateChanged(PlayModeStateChange state)
		{
			if (this.IsWindowsMixedRealityCurrentTarget())
			{
				bool inPlayMode = this.m_InPlayMode;
				this.m_InPlayMode = EditorApplication.isPlayingOrWillChangePlaymode;
				if (this.m_InPlayMode && !inPlayMode)
				{
					HolographicEmulation.SetEmulationMode(this.m_Mode);
					EmulationMode mode = this.m_Mode;
					if (mode != EmulationMode.Simulated)
					{
						if (mode != EmulationMode.RemoteDevice)
						{
						}
					}
					else
					{
						this.InitializeSimulation();
					}
				}
				else if (!this.m_InPlayMode && inPlayMode)
				{
					EmulationMode mode2 = this.m_Mode;
					if (mode2 != EmulationMode.Simulated)
					{
						if (mode2 != EmulationMode.RemoteDevice)
						{
						}
					}
					else
					{
						HolographicEmulation.Shutdown();
					}
				}
			}
		}

		private void Connect()
		{
			PerceptionRemotingPlugin.SetVideoEncodingParameters(this.m_MaxBitrateKbps);
			PerceptionRemotingPlugin.SetEnableVideo(this.m_EnableVideo);
			PerceptionRemotingPlugin.SetEnableAudio(this.m_EnableAudio);
			PerceptionRemotingPlugin.Connect(this.m_RemoteMachineAddress);
		}

		private void Disconnect()
		{
			if (PerceptionRemotingPlugin.GetConnectionState() != HolographicStreamerConnectionState.Disconnected)
			{
				PerceptionRemotingPlugin.Disconnect();
			}
		}

		private bool IsConnectedToRemoteDevice()
		{
			return PerceptionRemotingPlugin.GetConnectionState() == HolographicStreamerConnectionState.Connected;
		}

		private void HandleButtonPress()
		{
			if (EditorApplication.isPlayingOrWillChangePlaymode)
			{
				Debug.LogError("Unable to connect / disconnect remoting while playing.");
			}
			else
			{
				HolographicStreamerConnectionState connectionState = PerceptionRemotingPlugin.GetConnectionState();
				if (connectionState == HolographicStreamerConnectionState.Connecting || connectionState == HolographicStreamerConnectionState.Connected)
				{
					this.Disconnect();
				}
				else if (this.RemoteMachineNameSpecified)
				{
					this.Connect();
				}
				else
				{
					Debug.LogError("Cannot connect without a remote machine address specified");
				}
			}
		}

		private void UpdateRemoteMachineHistory()
		{
			List<string> list = new List<string>(this.m_RemoteMachineHistory);
			int i = 0;
			while (i < this.m_RemoteMachineHistory.Length)
			{
				if (this.m_RemoteMachineHistory[i].Equals(this.m_RemoteMachineAddress, StringComparison.CurrentCultureIgnoreCase))
				{
					if (i == 0)
					{
						return;
					}
					list.RemoveAt(i);
					break;
				}
				else
				{
					i++;
				}
			}
			list.Insert(0, this.m_RemoteMachineAddress);
			if (list.Count > HolographicEmulationWindow.s_MaxHistoryLength)
			{
				list.RemoveRange(HolographicEmulationWindow.s_MaxHistoryLength, list.Count - HolographicEmulationWindow.s_MaxHistoryLength);
			}
			this.m_RemoteMachineHistory = list.ToArray();
			EditorPrefs.SetString("HolographicRemoting.RemoteMachineHistory", string.Join(",", this.m_RemoteMachineHistory));
		}

		private void RemotingPreferencesOnGUI()
		{
			EditorGUI.BeginChangeCheck();
			this.m_RemoteMachineAddress = EditorGUILayout.DelayedTextFieldDropDown(HolographicEmulationWindow.s_RemoteMachineText, this.m_RemoteMachineAddress, this.m_RemoteMachineHistory);
			if (EditorGUI.EndChangeCheck())
			{
				this.UpdateRemoteMachineHistory();
			}
			this.m_EnableVideo = EditorGUILayout.Toggle(HolographicEmulationWindow.s_EnableVideoText, this.m_EnableVideo, new GUILayoutOption[0]);
			this.m_EnableAudio = EditorGUILayout.Toggle(HolographicEmulationWindow.s_EnableAudioText, this.m_EnableAudio, new GUILayoutOption[0]);
			this.m_MaxBitrateKbps = EditorGUILayout.IntSlider(HolographicEmulationWindow.s_MaxBitrateText, this.m_MaxBitrateKbps, 1024, 99999, new GUILayoutOption[0]);
		}

		private void ConnectionStateGUI()
		{
			if (HolographicEmulationWindow.s_ConnectedTexture == null)
			{
				HolographicEmulationWindow.s_ConnectedTexture = EditorGUIUtility.LoadIconRequired("sv_icon_dot3_sml");
				HolographicEmulationWindow.s_ConnectingTexture = EditorGUIUtility.LoadIconRequired("sv_icon_dot4_sml");
				HolographicEmulationWindow.s_DisconnectedTexture = EditorGUIUtility.LoadIconRequired("sv_icon_dot6_sml");
			}
			Texture2D image;
			GUIContent label;
			GUIContent content;
			switch (PerceptionRemotingPlugin.GetConnectionState())
			{
			case HolographicStreamerConnectionState.Disconnected:
				IL_5D:
				image = HolographicEmulationWindow.s_DisconnectedTexture;
				label = HolographicEmulationWindow.s_ConnectionStateDisconnectedText;
				content = HolographicEmulationWindow.s_ConnectionButtonConnectText;
				goto IL_A2;
			case HolographicStreamerConnectionState.Connecting:
				image = HolographicEmulationWindow.s_ConnectingTexture;
				label = HolographicEmulationWindow.s_ConnectionStateConnectingText;
				content = HolographicEmulationWindow.s_ConnectionButtonDisconnectText;
				goto IL_A2;
			case HolographicStreamerConnectionState.Connected:
				image = HolographicEmulationWindow.s_ConnectedTexture;
				label = HolographicEmulationWindow.s_ConnectionStateConnectedText;
				content = HolographicEmulationWindow.s_ConnectionButtonDisconnectText;
				goto IL_A2;
			}
			goto IL_5D;
			IL_A2:
			EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
			EditorGUILayout.PrefixLabel(HolographicEmulationWindow.s_ConnectionStatusText, "Button");
			float singleLineHeight = EditorGUIUtility.singleLineHeight;
			Rect rect = GUILayoutUtility.GetRect(singleLineHeight, singleLineHeight, new GUILayoutOption[]
			{
				GUILayout.ExpandWidth(false)
			});
			GUI.DrawTexture(rect, image);
			EditorGUILayout.LabelField(label, new GUILayoutOption[0]);
			EditorGUILayout.EndHorizontal();
			EditorGUI.BeginDisabledGroup(this.m_InPlayMode);
			bool flag = EditorGUILayout.DropdownButton(content, FocusType.Passive, EditorStyles.miniButton, new GUILayoutOption[0]);
			EditorGUI.EndDisabledGroup();
			if (flag)
			{
				if (EditorGUIUtility.editingTextField)
				{
					EditorGUIUtility.editingTextField = false;
					GUIUtility.keyboardControl = 0;
				}
				EditorApplication.CallDelayed(delegate
				{
					this.HandleButtonPress();
				}, 0f);
			}
		}

		private bool IsWindowsMixedRealityCurrentTarget()
		{
			return VREditor.GetVREnabledOnTargetGroup(BuildTargetGroup.WSA) && Array.IndexOf<string>(XRSettings.supportedDevices, "WindowsMR") >= 0;
		}

		private void DrawRemotingMode()
		{
			EditorGUI.BeginChangeCheck();
			this.m_Mode = (EmulationMode)EditorGUILayout.Popup(HolographicEmulationWindow.s_EmulationModeText, (int)this.m_Mode, HolographicEmulationWindow.s_ModeStrings, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck() && this.m_Mode != EmulationMode.RemoteDevice)
			{
				this.Disconnect();
			}
		}

		private bool CheckOperatingSystem()
		{
			if (!this.m_OperatingSystemChecked)
			{
				this.m_OperatingSystemValid = (Environment.OSVersion.Version.Build >= 14318);
				this.m_OperatingSystemChecked = true;
			}
			return this.m_OperatingSystemValid;
		}

		private void OnGUI()
		{
			if (!this.CheckOperatingSystem())
			{
				EditorGUILayout.HelpBox("You must be running Windows build 14318 or later to use Holographic Simulation or Remoting.", MessageType.Warning);
			}
			else if (!this.IsWindowsMixedRealityCurrentTarget())
			{
				EditorGUILayout.HelpBox("You must enable Virtual Reality support in settings and add Windows Mixed Reality to the devices to use Holographic Emulation.", MessageType.Warning);
			}
			else
			{
				EditorGUILayout.Space();
				EditorGUI.BeginDisabledGroup(this.m_InPlayMode);
				this.DrawRemotingMode();
				EditorGUI.EndDisabledGroup();
				EmulationMode mode = this.m_Mode;
				if (mode != EmulationMode.RemoteDevice)
				{
					if (mode == EmulationMode.Simulated)
					{
						EditorGUI.BeginChangeCheck();
						this.m_RoomIndex = EditorGUILayout.Popup(HolographicEmulationWindow.s_RoomText, this.m_RoomIndex, HolographicEmulationWindow.s_RoomStrings, new GUILayoutOption[0]);
						if (EditorGUI.EndChangeCheck() && this.m_InPlayMode)
						{
							this.LoadCurrentRoom();
						}
						EditorGUI.BeginChangeCheck();
						this.m_Hand = (GestureHand)EditorGUILayout.Popup(HolographicEmulationWindow.s_HandText, (int)this.m_Hand, HolographicEmulationWindow.s_HandStrings, new GUILayoutOption[0]);
						if (EditorGUI.EndChangeCheck())
						{
							HolographicEmulation.SetGestureHand(this.m_Hand);
						}
					}
				}
				else
				{
					EditorGUI.BeginDisabledGroup(this.IsConnectedToRemoteDevice());
					this.RemotingPreferencesOnGUI();
					EditorGUI.EndDisabledGroup();
					this.ConnectionStateGUI();
				}
			}
		}

		private void Update()
		{
			EmulationMode mode = this.m_Mode;
			if (mode != EmulationMode.Simulated)
			{
				if (mode == EmulationMode.RemoteDevice)
				{
					HolographicStreamerConnectionState connectionState = PerceptionRemotingPlugin.GetConnectionState();
					if (connectionState != this.m_LastConnectionState)
					{
						base.Repaint();
					}
					HolographicStreamerConnectionFailureReason holographicStreamerConnectionFailureReason = PerceptionRemotingPlugin.CheckForDisconnect();
					if (holographicStreamerConnectionFailureReason == HolographicStreamerConnectionFailureReason.Unreachable || holographicStreamerConnectionFailureReason == HolographicStreamerConnectionFailureReason.ConnectionLost)
					{
						Debug.LogWarning("Disconnected with failure reason " + holographicStreamerConnectionFailureReason + ", attempting to reconnect.");
						this.Connect();
					}
					else if (holographicStreamerConnectionFailureReason != HolographicStreamerConnectionFailureReason.None)
					{
						Debug.LogError("Disconnected with error " + holographicStreamerConnectionFailureReason);
					}
					this.m_LastConnectionState = connectionState;
				}
			}
			else
			{
				HolographicEmulation.SetGestureHand(this.m_Hand);
			}
		}
	}
}
