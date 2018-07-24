using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ExeSpy.Service.Common.Services
{
    public class NoContentResult
    {
    }

    public abstract class BaseHttpService
    {
        string baseApiUri;

        protected HttpClient client;

        protected BaseHttpService(string baseApiUri)
        {
            client = new HttpClient();
            this.baseApiUri = baseApiUri.EndsWith("/") ? baseApiUri : $"{baseApiUri}/";
        }

        protected Task<T> DeleteAsync<T>(string requestUri, Action<HttpRequestMessage> modifyRequestAction = null)
        {
            return SendWithRetryAsync<T>(HttpMethod.Delete, requestUri, modifyRequestAction);
        }

        protected Task<T> GetAsync<T>(string requestUri, Action<HttpRequestMessage> modifyRequestAction = null)
        {
            return SendWithRetryAsync<T>(HttpMethod.Get, requestUri, modifyRequestAction);
        }

        protected Task<T> PutAsync<T>(string requestUri, Action<HttpRequestMessage> modifyRequestAction = null)
        {
            return SendWithRetryAsync<T>(HttpMethod.Put, requestUri, modifyRequestAction);
        }

        protected async Task<T> PutAsync<T, K>(string requestUri, K obj, Action<HttpRequestMessage> modifyRequestAction = null) // where object
        {
            var jsonRequest = await SerializeObjectAsync<K>(obj).ConfigureAwait(false);
            return await SendWithRetryAsync<T>(HttpMethod.Put, requestUri, modifyRequestAction, jsonRequest);
        }

        protected async Task PostAsync(string requestUri, Action<HttpRequestMessage> modifyRequestAction = null)
        {
            await SendWithRetryAsync<NoContentResult>(HttpMethod.Post, requestUri, modifyRequestAction);
        }

        protected Task<T> PostAsync<T>(string requestUri, Action<HttpRequestMessage> modifyRequestAction = null)
        {
            return SendWithRetryAsync<T>(HttpMethod.Post, requestUri, modifyRequestAction);
        }

        protected async Task<T> PostAsync<T, K>(string requestUri, K obj, Action<HttpRequestMessage> modifyRequestAction = null) // where object
        {
            var jsonRequest = await SerializeObjectAsync<K>(obj).ConfigureAwait(false);
            return await SendWithRetryAsync<T>(HttpMethod.Post, requestUri, modifyRequestAction, jsonRequest);
        }

        protected async Task<T> PostAsync<T>(string requestUri, byte[] dataContent, Action<HttpRequestMessage> modifyRequestAction = null) // where object
        {
            T result = default(T);

            using (var content = new ByteArrayContent(dataContent))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result = await PostAsync<T>(requestUri, content, modifyRequestAction);
            }

            return result;
        }

        protected async Task<T> PostAsync<T>(string requestUri, ByteArrayContent dataContent, Action<HttpRequestMessage> modifyRequestAction = null) // where object
        {
            return await SendWithRetryAsync<T>(HttpMethod.Post, requestUri, modifyRequestAction, dataContent);
        }

        private async Task<string> SerializeObjectAsync<T>(T obj)
        {
            var serializationResult = await Task.Run(() =>
            {
                return !obj.Equals(default(T)) ? JsonConvert.SerializeObject(obj) : null;
            });

            return serializationResult;
        }

        async Task<T> SendWithRetryAsync<T>(HttpMethod requestType, string requestUri, Action<HttpRequestMessage> modifyRequestAction, ByteArrayContent dataContent)
        {
            T result = default(T);
            result = await Retry.Exponential<Task<T>>(async () => { return await SendAsync<T>(requestType, requestUri, modifyRequestAction, null, dataContent); });

            return result;
        }

        async Task<T> SendWithRetryAsync<T>(HttpMethod requestType, string requestUri, Action<HttpRequestMessage> modifyRequestAction, string jsonRequest = null)
        {
            T result = default(T);
            result = await Retry.Exponential<Task<T>>(async () => { return await SendAsync<T>(requestType, requestUri, modifyRequestAction, jsonRequest); });

            return result;
        }

        async Task<T> SendAsync<T>(HttpMethod requestType, string requestUri, Action<HttpRequestMessage> modifyRequestAction, string jsonRequest = null, ByteArrayContent dataContent = null)
        {
            T result = default(T);

            var request = new HttpRequestMessage(requestType, new Uri($"{baseApiUri}{requestUri}"));

            await Task.Run(() =>
            {
                modifyRequestAction?.Invoke(request);
            });

            if (jsonRequest != null)
            {
                request.Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            }
            else if (dataContent != null)
            {
                request.Content = dataContent;
            }

            HttpResponseMessage response = null;

            try
            {
                response = await client.SendAsync(request).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new HttpServiceException(response?.StatusCode ?? HttpStatusCode.ServiceUnavailable, ex);
            }

            if (response == null)
            {
                return result;
            }

            // TODO: Handle return types other than json
            using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
            {
                using (var reader = new StreamReader(stream))
                {
                    var responseContent = reader.ReadToEnd();

                    try
                    {
                        result = JsonConvert.DeserializeObject<T>(responseContent);
                    }
                    catch (Exception ex)
                    {
                        var x = 0;
                    }

                    return result;
                }
            }
        }
    }
}