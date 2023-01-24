using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCollectionsBenchmarks
{
	internal class SortedListLocalWithClassEnumerator<TKey, TValue> : SortedListLocalBase<TKey, TValue>
	{
		public SortedListLocalWithClassEnumerator() : base() { }

		public SortedListLocalWithClassEnumerator(IDictionary<TKey, TValue> dictionary) : base(dictionary) { }

		// public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		public Enumerator GetEnumerator()
		{
			return new Enumerator(this, Enumerator.KeyValuePair);
		}

		public class Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDictionaryEnumerator
		{
			private readonly SortedListLocalBase<TKey, TValue> _sortedList;
			private TKey? _key;
			private TValue? _value;
			private int _index;
			private readonly int _version;
			private readonly int _getEnumeratorRetType;  // What should Enumerator.Current return?

			internal const int KeyValuePair = 1;
			internal const int DictEntry = 2;

			internal Enumerator(SortedListLocalBase<TKey, TValue> sortedList, int getEnumeratorRetType)
			{
				_sortedList = sortedList;
				_index = 0;
				_version = _sortedList.version;
				_getEnumeratorRetType = getEnumeratorRetType;
				_key = default;
				_value = default;
			}

			public void Dispose()
			{
				_index = 0;
				_key = default;
				_value = default;
			}

			object IDictionaryEnumerator.Key
			{
				get
				{
					if (_index == 0 || (_index == _sortedList.Count + 1))
					{
						throw new InvalidOperationException(SR.InvalidOperation_EnumOpCantHappen);
					}

					return _key!;
				}
			}

			public bool MoveNext()
			{
				if (_version != _sortedList.version) throw new InvalidOperationException(SR.InvalidOperation_EnumFailedVersion);

				if ((uint)_index < (uint)_sortedList.Count)
				{
					_key = _sortedList.keys[_index];
					_value = _sortedList.values[_index];
					_index++;
					return true;
				}

				_index = _sortedList.Count + 1;
				_key = default;
				_value = default;
				return false;
			}

			DictionaryEntry IDictionaryEnumerator.Entry
			{
				get
				{
					if (_index == 0 || (_index == _sortedList.Count + 1))
					{
						throw new InvalidOperationException(SR.InvalidOperation_EnumOpCantHappen);
					}

					return new DictionaryEntry(_key!, _value);
				}
			}

			public KeyValuePair<TKey, TValue> Current => new KeyValuePair<TKey, TValue>(_key!, _value!);

			object? IEnumerator.Current
			{
				get
				{
					if (_index == 0 || (_index == _sortedList.Count + 1))
					{
						throw new InvalidOperationException(SR.InvalidOperation_EnumOpCantHappen);
					}

					if (_getEnumeratorRetType == DictEntry)
					{
						return new DictionaryEntry(_key!, _value);
					}
					else
					{
						return new KeyValuePair<TKey, TValue>(_key!, _value!);
					}
				}
			}

			object? IDictionaryEnumerator.Value
			{
				get
				{
					if (_index == 0 || (_index == _sortedList.Count + 1))
					{
						throw new InvalidOperationException(SR.InvalidOperation_EnumOpCantHappen);
					}

					return _value;
				}
			}

			void IEnumerator.Reset()
			{
				if (_version != _sortedList.version)
				{
					throw new InvalidOperationException(SR.InvalidOperation_EnumFailedVersion);
				}

				_index = 0;
				_key = default;
				_value = default;
			}
		}
	}
}