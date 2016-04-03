using System;
using System.Collections.Generic;

namespace Gwen.Renderer
{
	internal class MultiKeyDictionary<TKey1, TKey2, TValue> : Dictionary<TKey1, Dictionary<TKey2, TValue>>
	{
		public new Dictionary<TKey2, TValue> this[TKey1 key]
		{
			get
			{
				Dictionary<TKey2, TValue> value;
				if (!TryGetValue(key, out value))
				{
					value = new Dictionary<TKey2, TValue>();
					Add(key, value);
				}

				return value;
			}
		}

		public IEnumerable<TValue> AllValues
		{
			get
			{
				foreach (var value1 in Values)
				{
					foreach (var value2 in value1.Values)
					{
						yield return value2;
					}
				}
			}
		}

		public new int Count
		{
			get
			{
				int count = 0;
				foreach (var value1 in Values)
				{
					count += value1.Count;
				}
				return count;
			}
		}

		public bool ContainsKey(TKey1 key1, TKey2 key2)
		{
			Dictionary<TKey2, TValue> innerDict;
			if (TryGetValue(key1, out innerDict))
			{
				return innerDict.ContainsKey(key2);
			}

			return false;
		}

		public bool TryGetValue(TKey1 key1, TKey2 key2, out TValue value)
		{
			Dictionary<TKey2, TValue> innerDict;
			if (TryGetValue(key1, out innerDict))
			{
				return innerDict.TryGetValue(key2, out value);
			}

			value = default(TValue);
			return false;
		}
	}

	internal class MultiKeyDictionary<TKey1, TKey2, TKey3, TValue> : Dictionary<TKey1, MultiKeyDictionary<TKey2, TKey3, TValue>>
	{
		public new MultiKeyDictionary<TKey2, TKey3, TValue> this[TKey1 key]
		{
			get
			{
				MultiKeyDictionary<TKey2, TKey3, TValue> value;
				if (!TryGetValue(key, out value))
				{
					value = new MultiKeyDictionary<TKey2, TKey3, TValue>();
					Add(key, value);
				}

				return value;
			}
		}

		public IEnumerable<TValue> AllValues
		{
			get
			{
				foreach (var value1 in Values)
				{
					foreach (var value2 in value1.Values)
					{
						foreach (var value3 in value2.Values)
						{
							yield return value3;
						}
					}
				}
			}
		}

		public new int Count
		{
			get
			{
				int count = 0;
				foreach (var value1 in Values)
				{
					count += value1.Count;
				}
				return count;
			}
		}

		public bool ContainsKey(TKey1 key1, TKey2 key2, TKey3 key3)
		{
			MultiKeyDictionary<TKey2, TKey3, TValue> innerDict;
			if (TryGetValue(key1, out innerDict))
			{
				return innerDict.ContainsKey(key2, key3);
			}

			return false;
		}

		public bool TryGetValue(TKey1 key1, TKey2 key2, TKey3 key3, out TValue value)
		{
			MultiKeyDictionary<TKey2, TKey3, TValue> innerDict;
			if (TryGetValue(key1, out innerDict))
			{
				return innerDict.TryGetValue(key2, key3, out value);
			}

			value = default(TValue);
			return false;
		}
	}
}
