using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCollectionsBenchmarks
{
	internal class SortedListLocalWithStructEnumerator2<TKey, TValue> : SortedListLocalWithStructEnumerator<TKey, TValue>
	{
		public SortedListLocalWithStructEnumerator2() : base() { }

		public SortedListLocalWithStructEnumerator2(IDictionary<TKey, TValue> dictionary) : base(dictionary) { }

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return new Enumerator(this, Enumerator.KeyValuePair);
		}
	}
}
