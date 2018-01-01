using System;
using System.Collections.Generic;

namespace UnityEngine.CSSLayout
{
	internal class LockDictionary<TKey, TValue>
	{
		private object _cacheLock = new object();

		private Dictionary<TKey, TValue> _cacheItemDictionary = new Dictionary<TKey, TValue>();

		public void Set(TKey key, TValue value)
		{
			object cacheLock = this._cacheLock;
			lock (cacheLock)
			{
				this._cacheItemDictionary[key] = value;
			}
		}

		public bool TryGetValue(TKey key, out TValue cacheItem)
		{
			object cacheLock = this._cacheLock;
			bool flag;
			lock (cacheLock)
			{
				flag = this._cacheItemDictionary.TryGetValue(key, out cacheItem);
			}
			if (!flag)
			{
				cacheItem = default(TValue);
			}
			return flag;
		}

		public bool ContainsKey(TKey key)
		{
			bool result = false;
			object cacheLock = this._cacheLock;
			lock (cacheLock)
			{
				result = this._cacheItemDictionary.ContainsKey(key);
			}
			return result;
		}

		public void Remove(TKey key)
		{
			object cacheLock = this._cacheLock;
			lock (cacheLock)
			{
				this._cacheItemDictionary.Remove(key);
			}
		}
	}
}
