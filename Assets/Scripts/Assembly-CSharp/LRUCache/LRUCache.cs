using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LRUCache
{
	public class LRUCache<K, V>
	{
		private int capacity;

		private Dictionary<K, LinkedListNode<LRUCacheItem<K, V>>> cacheMap = new Dictionary<K, LinkedListNode<LRUCacheItem<K, V>>>();

		private LinkedList<LRUCacheItem<K, V>> lruList = new LinkedList<LRUCacheItem<K, V>>();

		public LRUCache(int capacity)
		{
			this.capacity = capacity;
		}

		[MethodImpl(32)]
		public bool get(K key, ref V retVal)
		{
			LinkedListNode<LRUCacheItem<K, V>> value;
			if (cacheMap.TryGetValue(key, out value))
			{
				V value2 = value.Value.value;
				lruList.Remove(value);
				lruList.AddLast(value);
				retVal = value2;
				return true;
			}
			retVal = default(V);
			return false;
		}

		[MethodImpl(32)]
		public void add(K key, V val)
		{
			LinkedListNode<LRUCacheItem<K, V>> value;
			if (cacheMap.TryGetValue(key, out value))
			{
				lruList.Remove(value);
				cacheMap.Remove(key);
			}
			if (cacheMap.Count >= capacity)
			{
				removeFirst();
			}
			LRUCacheItem<K, V> value2 = new LRUCacheItem<K, V>(key, val);
			LinkedListNode<LRUCacheItem<K, V>> linkedListNode = new LinkedListNode<LRUCacheItem<K, V>>(value2);
			lruList.AddLast(linkedListNode);
			cacheMap.Add(key, linkedListNode);
		}

		protected void removeFirst()
		{
			LinkedListNode<LRUCacheItem<K, V>> first = lruList.First;
			lruList.RemoveFirst();
			cacheMap.Remove(first.Value.key);
		}
	}
}
