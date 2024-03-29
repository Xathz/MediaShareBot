﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace MediaShareBot.Utilities {

    public static class Http {

        /// <summary>
        /// Static http client.
        /// </summary>
        public static readonly HttpClient Client = new HttpClient();

        /// <summary>
        /// Send a http request.
        /// </summary>
        /// <param name="url">Url to download.</param>
        /// <param name="headers">Headers for the request. (name, value)</param>
        /// <param name="parameters">Parameters for the request. (key, value)</param>
        /// <param name="method">Http method for the request.</param>
        /// <param name="timeout">Timeout in seconds.</param>
        /// <returns>Response content as a string. This may be empty based on <see cref="Method"/>.</returns>
        public static async Task<string> SendRequestAsync(string url, Dictionary<string, string> headers = null, Dictionary<string, string> parameters = null, Method method = Method.Get, int timeout = 20) {
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod(method.ToString()), url);

            if (headers != null) {
                foreach (KeyValuePair<string, string> header in headers) {
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            if (parameters != null) {
                request.Content = new FormUrlEncodedContent(parameters);
            }

            using (CancellationTokenSource cancellation = new CancellationTokenSource(timeout * 1000)) {
                using (HttpResponseMessage response = await Client.SendAsync(request, cancellation.Token)) {
                    if (response.IsSuccessStatusCode) {
                        using (HttpContent content = response.Content) {
                            return await content.ReadAsStringAsync();
                        }
                    } else {
                        throw new HttpRequestException($"There was an error; ({(int)response.StatusCode}) {response.ReasonPhrase}");
                    }
                }
            }
        }

        /// <summary>
        /// Download data as a stream.
        /// </summary>
        /// <param name="url">Url to download.</param>
        /// <param name="headers">Headers for the request. (name, value)</param>
        /// <param name="method">Http method for the request.</param>
        /// <param name="timeout">Timeout in seconds.</param>
        /// <returns>The requested data as a <see cref="MemoryStream"/></returns>
        public static async Task<MemoryStream> GetStreamAsync(string url, Dictionary<string, string> headers = null, Method method = Method.Get, int timeout = 20) {
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod(method.ToString()), url);

            if (headers != null) {
                foreach (KeyValuePair<string, string> header in headers) {
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            using (CancellationTokenSource cancellation = new CancellationTokenSource(timeout * 1000)) {
                using (HttpResponseMessage response = await Client.SendAsync(request, cancellation.Token)) {
                    if (response.IsSuccessStatusCode) {
                        using (HttpContent content = response.Content) {
                            using (Stream stream = await content.ReadAsStreamAsync()) {
                                MemoryStream copyStream = new MemoryStream(256);
                                stream.CopyTo(copyStream);
                                copyStream.Seek(0, SeekOrigin.Begin);

                                return copyStream;
                            }
                        }
                    } else {
                        throw new HttpRequestException($"There was an error; ({(int)response.StatusCode}) {response.ReasonPhrase}");
                    }
                }
            }
        }

        /// <summary>
        /// Deserializes a json string to a type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json">Json to deserialize.</param>
        public static T DeserializeJson<T>(string json) {
            try {
                return JsonConvert.DeserializeObject<T>(json);
            } catch (Exception ex) {
                LoggingManager.Log.Error(ex);
                return default;
            }
        }

        /// <summary>
        /// Parses a url for a parameters value.
        /// </summary>
        /// <param name="url">Url to parse</param>
        /// <param name="parameter">Parameter to get</param>
        public static string ParseUrlForParameter(string url, string parameter) {
            Uri myUri = new Uri(url);
            return HttpUtility.ParseQueryString(myUri.Query).Get(parameter);
        }

        /// <summary>
        /// Http request method.
        /// </summary>
        public enum Method {
            Delete,
            Get,
            Head,
            Options,
            Patch,
            Post,
            Put
        }

    }

}

