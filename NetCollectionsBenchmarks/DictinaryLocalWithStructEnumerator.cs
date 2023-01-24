using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCollectionsBenchmarks
{
	internal class DictinaryLocalWithStructEnumerator<TKey, TValue> : DictionaryLocalBase<TKey, TValue>
	{
		public DictinaryLocalWithStructEnumerator() : base (0, null) { }

		public DictinaryLocalWithStructEnumerator(IEnumerable<KeyValuePair<TKey, TValue>> collection) : base(collection, null) { }

		public Enumerator GetEnumerator() => new Enumerator(this, Enumerator.KeyValuePair);

		public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDictionaryEnumerator
		{
			private readonly DictionaryLocalBase<TKey, TValue> _dictionary;
			private readonly int _version;
			private int _index;
			private KeyValuePair<TKey, TValue> _current;
			private readonly int _getEnumeratorRetType;  // What should Enumerator.Current return?

			internal const int DictEntry = 1;
			internal const int KeyValuePair = 2;

			internal Enumerator(DictionaryLocalBase<TKey, TValue> dictionary, int getEnumeratorRetType)
			{
				_dictionary = dictionary;
				_version = dictionary._version;
				_index = 0;
				_getEnumeratorRetType = getEnumeratorRetType;
				_current = default;
			}

			public bool MoveNext()
			{
				if (_version != _dictionary._version)
				{
					ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion();
				}

				// Use unsigned comparison since we set index to dictionary.count+1 when the enumeration ends.
				// dictionary.count+1 could be negative if dictionary.count is int.MaxValue
				while ((uint)_index < (uint)_dictionary._count)
				{
					ref Entry entry = ref _dictionary._entries![_index++];

					if (entry.next >= -1)
					{
						_current = new KeyValuePair<TKey, TValue>(entry.key, entry.value);
						return true;
					}
				}

				_index = _dictionary._count + 1;
				_current = default;
				return false;
			}

			public KeyValuePair<TKey, TValue> Current => _current;

			public void Dispose() { }

			object? IEnumerator.Current
			{
				get
				{
					if (_index == 0 || (_index == _dictionary._count + 1))
					{
						ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumOpCantHappen();
					}

					if (_getEnumeratorRetType == DictEntry)
					{
						return new DictionaryEntry(_current.Key, _current.Value);
					}

					return new KeyValuePair<TKey, TValue>(_current.Key, _current.Value);
				}
			}

			void IEnumerator.Reset()
			{
				if (_version != _dictionary._version)
				{
					ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion();
				}

				_index = 0;
				_current = default;
			}

			DictionaryEntry IDictionaryEnumerator.Entry
			{
				get
				{
					if (_index == 0 || (_index == _dictionary._count + 1))
					{
						ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumOpCantHappen();
					}

					return new DictionaryEntry(_current.Key, _current.Value);
				}
			}

			object IDictionaryEnumerator.Key
			{
				get
				{
					if (_index == 0 || (_index == _dictionary._count + 1))
					{
						ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumOpCantHappen();
					}

					return _current.Key;
				}
			}

			object? IDictionaryEnumerator.Value
			{
				get
				{
					if (_index == 0 || (_index == _dictionary._count + 1))
					{
						ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumOpCantHappen();
					}

					return _current.Value;
				}
			}
		}
	}
}
