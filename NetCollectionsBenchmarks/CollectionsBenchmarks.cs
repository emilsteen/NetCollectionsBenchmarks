
using BenchmarkDotNet.Attributes;

namespace NetCollectionsBenchmarks
{
	[MemoryDiagnoser]
	public class CollectionsBenchmarks
	{
		private Dictionary<int, int> DictionaryData = new();
		private SortedList<int, int> SortedListData = new();

		private Dictionary<int, int> DictionaryCheck = new();
		private SortedList<int, int> SortedListCheck = new();

		[GlobalSetup]
		public void Setup()
		{
			for (int x = 0; x < 15; x++)
				this.DictionaryData.Add(x, x);

			this.SortedListData = new SortedList<int, int>(this.DictionaryData);

			this.DictionaryCheck = new Dictionary<int, int>(this.DictionaryData);
			this.SortedListCheck = new SortedList<int, int>(this.DictionaryData);
		}

		[Benchmark(Baseline = true)]
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
	}
}
