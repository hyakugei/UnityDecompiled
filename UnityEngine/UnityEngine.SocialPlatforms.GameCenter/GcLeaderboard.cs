using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.SocialPlatforms.Impl;

namespace UnityEngine.SocialPlatforms.GameCenter
{
	[StructLayout(LayoutKind.Sequential)]
	internal class GcLeaderboard
	{
		private IntPtr m_InternalLeaderboard;

		private Leaderboard m_GenericLeaderboard;

		internal GcLeaderboard(Leaderboard board)
		{
			this.m_GenericLeaderboard = board;
		}

		~GcLeaderboard()
		{
			this.Dispose();
		}

		internal bool Contains(Leaderboard board)
		{
			return this.m_GenericLeaderboard == board;
		}

		internal void SetScores(GcScoreData[] scoreDatas)
		{
			if (this.m_GenericLeaderboard != null)
			{
				Score[] array = new Score[scoreDatas.Length];
				for (int i = 0; i < scoreDatas.Length; i++)
				{
					array[i] = scoreDatas[i].ToScore();
				}
				this.m_GenericLeaderboard.SetScores(array);
			}
		}

		internal void SetLocalScore(GcScoreData scoreData)
		{
			if (this.m_GenericLeaderboard != null)
			{
				this.m_GenericLeaderboard.SetLocalUserScore(scoreData.ToScore());
			}
		}

		internal void SetMaxRange(uint maxRange)
		{
			if (this.m_GenericLeaderboard != null)
			{
				this.m_GenericLeaderboard.SetMaxRange(maxRange);
			}
		}

		internal void SetTitle(string title)
		{
			if (this.m_GenericLeaderboard != null)
			{
				this.m_GenericLeaderboard.SetTitle(title);
			}
		}

		internal void Internal_LoadScores(string category, int from, int count, string[] userIDs, int playerScope, int timeScope, object callback)
		{
			this.m_InternalLeaderboard = GcLeaderboard.GcLeaderboard_LoadScores(this, category, from, count, userIDs, playerScope, timeScope, callback);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr GcLeaderboard_LoadScores(object self, string category, int from, int count, string[] userIDs, int playerScope, int timeScope, object callback);

		internal bool Loading()
		{
			return GcLeaderboard.GcLeaderboard_Loading(this.m_InternalLeaderboard);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GcLeaderboard_Loading(IntPtr leaderboard);

		internal void Dispose()
		{
			GcLeaderboard.GcLeaderboard_Dispose(this.m_InternalLeaderboard);
			this.m_InternalLeaderboard = IntPtr.Zero;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GcLeaderboard_Dispose(IntPtr leaderboard);
	}
}
