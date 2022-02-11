using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Diploma.Models;

namespace Diploma.Services.Mapper
{
    public class MapperService : IMapperService
    {
        private TaskCompletionSource<IMapper> _mapperCompletionSource;

        public MapperService()
        {
            _mapperCompletionSource = new TaskCompletionSource<IMapper>();
            Task.Run(ConfigMapper);
        }

        #region -- IMapperService Implementation --

        public Task<IMapper> GetMapperAsync() => _mapperCompletionSource.Task;

        public async Task<OutT> MapAsync<InT, OutT>(InT source)
        {
            var mapper = await _mapperCompletionSource.Task;

            return mapper.Map<InT, OutT>(source);
        }

        public async Task<OutT> MapAsync<InT, OutT>(InT source, Action<InT, OutT> afterMap)
        {
            var mapper = await _mapperCompletionSource.Task;

            return mapper.Map<InT, OutT>(source, opt => opt.AfterMap(afterMap));
        }

        public async Task<IEnumerable<OutT>> MapRangeAsync<InT, OutT>(IEnumerable<InT> items)
        {
            var mapper = await _mapperCompletionSource.Task;

            return items.Select(x => mapper.Map<InT, OutT>(x));
        }

        public async Task<IEnumerable<OutT>> MapRangeAsync<InT, OutT>(IEnumerable<InT> items, Action<InT, OutT> afterMap)
        {
            var mapper = await _mapperCompletionSource.Task;

            return items.Select(x => mapper.Map<InT, OutT>(x, opt => opt.AfterMap(afterMap)));
        }

        public async Task<T> MapAsync<T>(object source)
        {
            var mapper = await _mapperCompletionSource.Task;

            return mapper.Map<T>(source);
        }

        public async Task<T> MapAsync<T>(object source, Action<object, T> afterMap)
        {
            var mapper = await _mapperCompletionSource.Task;

            return mapper.Map<T>(source, opt => opt.AfterMap(afterMap));
        }

        public async Task<IEnumerable<T>> MapRangeAsync<T>(IEnumerable<object> items)
        {
            var mapper = await _mapperCompletionSource.Task;

            return items.Select(x => mapper.Map<T>(x));
        }

        public async Task<IEnumerable<T>> MapRangeAsync<T>(IEnumerable<object> items, Action<object, T> afterMap)
        {
            var mapper = await _mapperCompletionSource.Task;

            return items.Select(x => mapper.Map<T>(x, opt => opt.AfterMap(afterMap)));
        }

        #endregion

        #region -- Private Helpers --

        private void ConfigMapper()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NewsModel, NewsBindableModel>().ReverseMap();
            });

            _mapperCompletionSource.TrySetResult(mapperConfiguration.CreateMapper());
        }

        #endregion
    }
}
