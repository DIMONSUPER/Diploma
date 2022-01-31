﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Diploma.Services.Settings;
using Newtonsoft.Json;

namespace Diploma.Services.Rest
{
    public class RestService : IRestService
    {
        private readonly ISettingsManager _settingsManager;
        private readonly JsonSerializer _serializer = new();
        private readonly HttpClient _client = new();

        public RestService(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        #region -- IRestService implementation --

        public async Task<T> GetAsync<T>(string requestUrl, Dictionary<string, string> additioalHeaders = null)
        {
            using (var response = await MakeRequestAsync(requestUrl, HttpMethod.Get, null, additioalHeaders).ConfigureAwait(false))
            {
                ThrowIfNotSuccess(response);

                using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                using var reader = new StreamReader(stream);
                using var json = new JsonTextReader(reader);

                return _serializer.Deserialize<T>(json);
            }
        }

        public async Task<T> PutAsync<T>(string requestUrl, object requestBody, Dictionary<string, string> additioalHeaders = null)
        {
            using (var response = await MakeRequestAsync(requestUrl, HttpMethod.Put, requestBody, additioalHeaders))
            {
                ThrowIfNotSuccess(response);

                using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                using var reader = new StreamReader(stream);
                using var json = new JsonTextReader(reader);

                return _serializer.Deserialize<T>(json);
            }
        }

        public async Task<T> DeleteAsync<T>(string requestUrl, Dictionary<string, string> additioalHeaders = null)
        {
            using (var response = await MakeRequestAsync(requestUrl, HttpMethod.Delete, null, additioalHeaders).ConfigureAwait(false))
            {
                ThrowIfNotSuccess(response);

                using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                using var reader = new StreamReader(stream);
                using var json = new JsonTextReader(reader);

                return _serializer.Deserialize<T>(json);
            }
        }

        public async Task<T> DeleteAsync<T>(string requestUrl, object requestBody, Dictionary<string, string> additioalHeaders = null)
        {
            using (var response = await MakeRequestAsync(requestUrl, HttpMethod.Get, requestBody, additioalHeaders).ConfigureAwait(false))
            {
                ThrowIfNotSuccess(response);

                using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                using var reader = new StreamReader(stream);
                using var json = new JsonTextReader(reader);

                return _serializer.Deserialize<T>(json);
            }
        }

        public async Task<T> PostAsync<T>(string requestUrl, object requestBody, Dictionary<string, string> additioalHeaders = null)
        {
            using (var response = await MakeRequestAsync(requestUrl, HttpMethod.Post, requestBody, additioalHeaders).ConfigureAwait(false))
            {
                ThrowIfNotSuccess(response);

                using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                using var reader = new StreamReader(stream);
                using var json = new JsonTextReader(reader);

                return _serializer.Deserialize<T>(json);
            }
        }

        public async Task<GetT> PostAsync<PostT, GetT>(string requestUrl, object requestBody, Dictionary<string, string> additioalHeaders = null)
        {
            using (var response = await MakeRequestAsync(requestUrl, HttpMethod.Post, requestBody, additioalHeaders).ConfigureAwait(false))
            {
                ThrowIfNotSuccess(response);

                using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                using var reader = new StreamReader(stream);
                using var json = new JsonTextReader(reader);

                return _serializer.Deserialize<GetT>(json);
            }
        }

        #endregion

        #region -- Private helpers --

        private static void ThrowIfNotSuccess(HttpResponseMessage response, object dataObj = null)
        {
            try
            {
                if (!response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal string BuildParametersString(Dictionary<string, string> parameters)
        {
            if (parameters is null || parameters.Count == 0)
                return string.Empty;

            var sb = new StringBuilder("?");
            bool needAddDivider = false;

            foreach (var item in parameters)
            {
                if (needAddDivider)
                    sb.Append('&');
                var encodedKey = WebUtility.UrlEncode(item.Key);
                var encodedVal = WebUtility.UrlEncode(item.Value);
                sb.Append($"{encodedKey}={encodedVal}");

                needAddDivider = true;
            }

            return sb.ToString();
        }

        private async Task<HttpResponseMessage> MakeRequestAsync(string requestUrl, HttpMethod method, object requestBody = null, Dictionary<string, string> additioalHeaders = null)
        {
            var request = new HttpRequestMessage(method, requestUrl);

            if (requestBody is not null)
            {
                var json = JsonConvert.SerializeObject(requestBody);

                if (requestBody is IEnumerable<KeyValuePair<string, string>> body)
                {
                    request.Content = new FormUrlEncodedContent(body);
                }
                else
                {
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }
            }

            if (additioalHeaders is not null)
            {
                foreach (var header in additioalHeaders)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            var res = await _client.SendAsync(request).ConfigureAwait(false);

            return res;
        }

        #endregion
    }
}
