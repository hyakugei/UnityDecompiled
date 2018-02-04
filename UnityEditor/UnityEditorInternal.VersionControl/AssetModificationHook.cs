using System;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

namespace UnityEditorInternal.VersionControl
{
	public class AssetModificationHook
	{
		private enum CachedStatusMode
		{
			Sync,
			Async
		}

		private static Asset GetStatusCachedIfPossible(string from, AssetModificationHook.CachedStatusMode mode)
		{
			Asset asset = Provider.CacheStatus(from);
			if (asset == null || asset.IsState(Asset.States.Updating))
			{
				if (mode == AssetModificationHook.CachedStatusMode.Sync)
				{
					Task task = Provider.Status(from, false);
					task.Wait();
					if (task.success)
					{
						asset = Provider.CacheStatus(from);
					}
					else
					{
						asset = null;
					}
				}
			}
			return asset;
		}

		private static Asset GetStatusForceUpdate(string from)
		{
			Task task = Provider.Status(from);
			task.Wait();
			return (task.assetList.Count <= 0) ? null : task.assetList[0];
		}

		public static AssetMoveResult OnWillMoveAsset(string from, string to)
		{
			AssetMoveResult result;
			if (!Provider.enabled)
			{
				result = AssetMoveResult.DidNotMove;
			}
			else
			{
				Asset statusCachedIfPossible = AssetModificationHook.GetStatusCachedIfPossible(from, AssetModificationHook.CachedStatusMode.Sync);
				if (statusCachedIfPossible == null || !statusCachedIfPossible.IsUnderVersionControl)
				{
					result = AssetMoveResult.DidNotMove;
				}
				else if (statusCachedIfPossible.IsState(Asset.States.OutOfSync))
				{
					Debug.LogError("Cannot move version controlled file that is not up to date. Please get latest changes from server");
					result = AssetMoveResult.FailedMove;
				}
				else if (statusCachedIfPossible.IsState(Asset.States.DeletedRemote))
				{
					Debug.LogError("Cannot move version controlled file that is deleted on server. Please get latest changes from server");
					result = AssetMoveResult.FailedMove;
				}
				else if (statusCachedIfPossible.IsState(Asset.States.CheckedOutRemote))
				{
					Debug.LogError("Cannot move version controlled file that is checked out on server. Please get latest changes from server");
					result = AssetMoveResult.FailedMove;
				}
				else if (statusCachedIfPossible.IsState(Asset.States.LockedRemote))
				{
					Debug.LogError("Cannot move version controlled file that is locked on server. Please get latest changes from server");
					result = AssetMoveResult.FailedMove;
				}
				else
				{
					Task task = Provider.Move(from, to);
					task.Wait();
					result = (AssetMoveResult)((!task.success) ? 1 : task.resultCode);
				}
			}
			return result;
		}

		public static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions option)
		{
			AssetDeleteResult result;
			if (!Provider.enabled)
			{
				result = AssetDeleteResult.DidNotDelete;
			}
			else
			{
				Task task = Provider.Delete(assetPath);
				task.SetCompletionAction(CompletionAction.UpdatePendingWindow);
				task.Wait();
				result = ((!task.success) ? AssetDeleteResult.FailedDelete : AssetDeleteResult.DidNotDelete);
			}
			return result;
		}

		public static bool IsOpenForEdit(string assetPath, out string message, StatusQueryOptions statusOptions)
		{
			message = "";
			bool result;
			if (!Provider.enabled)
			{
				result = true;
			}
			else if (string.IsNullOrEmpty(assetPath))
			{
				result = true;
			}
			else
			{
				Asset asset;
				if (statusOptions == StatusQueryOptions.UseCachedIfPossible || statusOptions == StatusQueryOptions.UseCachedAsync)
				{
					AssetModificationHook.CachedStatusMode mode = (statusOptions != StatusQueryOptions.UseCachedAsync) ? AssetModificationHook.CachedStatusMode.Sync : AssetModificationHook.CachedStatusMode.Async;
					asset = AssetModificationHook.GetStatusCachedIfPossible(assetPath, mode);
				}
				else
				{
					asset = AssetModificationHook.GetStatusForceUpdate(assetPath);
				}
				if (asset == null)
				{
					if (Provider.onlineState == OnlineState.Offline && Provider.offlineReason != string.Empty)
					{
						message = Provider.offlineReason;
					}
					result = false;
				}
				else
				{
					result = Provider.IsOpenForEdit(asset);
				}
			}
			return result;
		}
	}
}
