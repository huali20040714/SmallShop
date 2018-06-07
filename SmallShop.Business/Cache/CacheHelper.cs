using CacheManager.Core;
using CacheManager.Core.Internal;
using System;
using System.Text;

namespace SmallShop.BackStage.Business
{
    public sealed class CacheHelper
    {
        private static readonly object lockObj = new object();
        private volatile static CacheHelper _instance = null;
        private ICacheManager<object> cache = null;

        private CacheHelper()
        {
            cache = CacheFactory.Build("StartedCache", settings =>
            {
                settings.WithSystemRuntimeCacheHandle("HandleStartedCache")
                        .WithExpiration(ExpirationMode.Sliding, TimeSpan.FromSeconds(10))
                        .EnableStatistics()
                        .EnablePerformanceCounters();
            });
        }

        public static CacheHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObj)
                    {
                        if (_instance == null)
                            _instance = new CacheHelper();
                    }
                }

                return _instance;
            }
        }

        public void Add(string key, object value)
        {
            cache.Add(key, value);
        }

        public T Get<T>(string key)
        {
            return cache.Get<T>(key);
        }

        public string GetStatistic()
        {
            var sb = new StringBuilder();
            foreach (var handle in cache.CacheHandles)
            {
                var stats = handle.Stats;
                sb.AppendFormat(
                    "Items: {0}, Hits: {1}, Miss: {2}, Remove: {3}, ClearRegion: {4}, Clear: {5}, Adds: {6}, Puts: {7}, Gets: {8}",
                        stats.GetStatistic(CacheStatsCounterType.Items),
                        stats.GetStatistic(CacheStatsCounterType.Hits),
                        stats.GetStatistic(CacheStatsCounterType.Misses),
                        stats.GetStatistic(CacheStatsCounterType.RemoveCalls),
                        stats.GetStatistic(CacheStatsCounterType.ClearRegionCalls),
                        stats.GetStatistic(CacheStatsCounterType.ClearCalls),
                        stats.GetStatistic(CacheStatsCounterType.AddCalls),
                        stats.GetStatistic(CacheStatsCounterType.PutCalls),
                        stats.GetStatistic(CacheStatsCounterType.GetCalls)
                    );
            }

            return sb.ToString();
        }
    }
}
