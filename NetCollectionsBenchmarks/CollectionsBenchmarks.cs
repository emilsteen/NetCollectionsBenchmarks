
using System.Collections.ObjectModel;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace NetCollectionsBenchmarks
{
	//[SimpleJob(RuntimeMoniker.Net48)]
	//// [SimpleJob(RuntimeMoniker.NetCoreApp30)]
	//[SimpleJob(RuntimeMoniker.Net60, baseline: true)]
	//[SimpleJob(RuntimeMoniker.Net70)]
	[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
	[RankColumn]
	[MemoryDiagnoser]
	public class CollectionsBenchmarks
	{
		private Dictionary<int, int> DictionaryData = new();
		private SortedList<int, int> SortedListData = new();

		private Dictionary<int, int> DictionaryCheck = new();
		private SortedList<int, int> SortedListCheck = new();

		private ReadOnlyDictionary<int, int> ReadOnlyDictionaryData;


		private DictinaryLocalWithStructEnumerator<int, int> DictinaryLocalWithStructEnumeratorData = new();
		private DictinaryLocalWithClassEnumerator<int, int> DictinaryLocalWithClassEnumeratorData = new();

		private SortedListLocalWithStructEnumerator1<int, int> SortedListLocalWithStructEnumerator1Data = new();
		private SortedListLocalWithStructEnumerator2<int, int> SortedListLocalWithStructEnumerator2Data = new();
		private SortedListLocalWithClassEnumerator<int, int> SortedListLocalWithClassEnumeratorData = new();

		[GlobalSetup]
		public void Setup()
		{
			for (int x = 0; x < 15; x++)
				this.DictionaryData.Add(x, x);

			this.SortedListData = new SortedList<int, int>(this.DictionaryData);

			this.DictinaryLocalWithStructEnumeratorData = new DictinaryLocalWithStructEnumerator<int, int>(this.DictionaryData);
			this.DictinaryLocalWithClassEnumeratorData = new DictinaryLocalWithClassEnumerator<int, int>(this.DictionaryData);

			this.SortedListLocalWithStructEnumerator1Data = new SortedListLocalWithStructEnumerator1<int, int>(this.DictionaryData);
			this.SortedListLocalWithStructEnumerator2Data = new SortedListLocalWithStructEnumerator2<int, int>(this.DictionaryData);
			this.SortedListLocalWithClassEnumeratorData = new SortedListLocalWithClassEnumerator<int, int>(this.DictionaryData);

			this.DictionaryCheck = new Dictionary<int, int>(this.DictionaryData);
			this.SortedListCheck = new SortedList<int, int>(this.DictionaryData);

			this.ReadOnlyDictionaryData = new ReadOnlyDictionary<int, int>(this.DictionaryData);

		}

		[Benchmark]
		public long ForLoopDictionaryBenchmark()
		{
			var count = 0L;
			var res = 0L;
			for (int x = 0; x < 1_000_000; x++)
			{
				for (int i = 0; i < 15; i++)
				{
					if (this.DictionaryCheck.TryGetValue(x, out var value) || value < x)
						res += value;

					count++;
				}
			}

			return res;
		}

		[Benchmark]
		public long ForLoopSortedListBenchmark()
		{
			var res = 0L;
			for (int x = 0; x < 1_000_000; x++)
			{
				for (int i = 0; i < 15; i++)
				{
					if (this.SortedListCheck.TryGetValue(x, out var value) || value < x)
						res += value;
				}
			}

			return res;
		}

		[Benchmark]
		public long ForeachDictionaryBenchmark()
		{
			var res = 0L;
			for (int x = 0; x < 1_000_000; x++)
			{
				foreach (var needle in this.DictionaryData)
				{
					if (this.DictionaryCheck.TryGetValue(needle.Key, out var value) || value < needle.Value)
						res += value;
				}
			}

			return res;
		}

		[Benchmark]
		public long ForeachSortedListBenchmark()
		{
			var res = 0L;
			for (int x = 0; x < 1_000_000; x++)
			{
				foreach (var needle in this.SortedListData)
				{
					if (this.SortedListCheck.TryGetValue(needle.Key, out var value) || value < needle.Value)
						res += value;
				}
			}

			return res;
		}

		[Benchmark]
		public long ForeachNoTryGetValueDictionaryBenchmark()
		{
			var res = 0L;
			for (int x = 0; x < 1_000_000; x++)
			{
				foreach (var needle in this.DictionaryData)
				{
				}
			}

			return res;
		}

		[Benchmark]
		public long ForeachNoTryGetValueDictionaryLocalStructBenchmark()
		{
			var res = 0L;
			for (int x = 0; x < 1_000_000; x++)
			{
				foreach (var needle in this.DictinaryLocalWithStructEnumeratorData)
				{
				}
			}

			return res;
		}

		[Benchmark]
		public long ForeachNoTryGetValueDictionaryLocalClassBenchmark()
		{
			var res = 0L;
			for (int x = 0; x < 1_000_000; x++)
			{
				foreach (var needle in this.DictinaryLocalWithClassEnumeratorData)
				{
				}
			}

			return res;
		}

		[Benchmark]
		public long ForeachNoTryGetValueSortedListBenchmark()
		{
			var res = 0L;
			for (int x = 0; x < 1_000_000; x++)
			{
				foreach (var needle in this.SortedListData)
				{
				}
			}

			return res;
		}

		[Benchmark(Baseline = true)]
		public long ForeachNoTryGetValueSortedListLocalStruct1Benchmark()
		{
			var res = 0L;
			for (int x = 0; x < 1_000_000; x++)
			{
				foreach (var needle in this.SortedListLocalWithStructEnumerator1Data)
				{
				}
			}

			return res;
		}

		[Benchmark]
		public long ForeachNoTryGetValueSortedListLocalStruct2Benchmark()
		{
			var res = 0L;
			for (int x = 0; x < 1_000_000; x++)
			{
				foreach (var needle in this.SortedListLocalWithStructEnumerator2Data)
				{
				}
			}

			return res;
		}

		[Benchmark]
		public long ForeachNoTryGetValueSortedListLocalClassBenchmark()
		{
			var res = 0L;
			for (int x = 0; x < 1_000_000; x++)
			{
				foreach (var needle in this.SortedListLocalWithClassEnumeratorData)
				{
				}
			}

			return res;
		}

		[Benchmark]
		public long ForeachNoTryGetValueReadOnlyDictionaryBenchmark()
		{
			var res = 0L;
			for (int x = 0; x < 1_000_000; x++)
			{
				foreach (var needle in this.ReadOnlyDictionaryData)
				{
				}
			}

			return res;
		}
	}
}
