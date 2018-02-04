using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEditor
{
	internal sealed class EditorConnectionInternal : IPlayerEditorConnectionNative
	{
		void IPlayerEditorConnectionNative.SendMessage(Guid messageId, byte[] data, int playerId)
		{
			if (messageId == Guid.Empty)
			{
				throw new ArgumentException("messageId must not be empty");
			}
			EditorConnectionInternal.SendMessage(messageId.ToString("N"), data, playerId);
		}

		void IPlayerEditorConnectionNative.Poll()
		{
			EditorConnectionInternal.PollInternal();
		}

		void IPlayerEditorConnectionNative.RegisterInternal(Guid messageId)
		{
			EditorConnectionInternal.RegisterInternal(messageId.ToString("N"));
		}

		void IPlayerEditorConnectionNative.UnregisterInternal(Guid messageId)
		{
			EditorConnectionInternal.UnregisterInternal(messageId.ToString("N"));
		}

		void IPlayerEditorConnectionNative.Initialize()
		{
			EditorConnectionInternal.Initialize();
		}

		void IPlayerEditorConnectionNative.DisconnectAll()
		{
			EditorConnectionInternal.DisconnectAll();
		}

		public bool IsConnected()
		{
			throw new NotSupportedException("Check the connected players list instead");
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Initialize();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void UnregisterInternal(string messageId);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void RegisterInternal(string messageId);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SendMessage(string messageId, byte[] data, int playerId);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void PollInternal();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int Connect(string IP, int port);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DisconnectAll();
	}
}
