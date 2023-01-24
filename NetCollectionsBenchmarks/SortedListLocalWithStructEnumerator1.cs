using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCollectionsBenchmarks
{
	internal class SortedListLocalWithStructEnumerator1<TKey, TValue> : SortedListLocalWithStructEnumerator<TKey, TValue>
	{
		public SortedListLocalWithStructEnumerator1() : base() { }

		public SortedListLocalWithStructEnumerator1(IDictionary<TKey, TValue> dictionary) : base(dictionary) { }

		// public IEnumeratorStruct<KeyValuePair<TKey, TValue>> GetEnumerator()
		public Enumerator GetEnumerator()
		{
			return new Enumerator(this, Enumerator.KeyValuePair);
		}
	}
}
