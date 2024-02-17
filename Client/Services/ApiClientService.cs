using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BiDegree.Services
{
    public sealed class ApiClientService : IApiClientService
    {
        private readonly HttpClient _http;
        private readonly ILogger<ApiClientService> _logger;

        public ApiClientService(HttpClient http,
            ILogger<ApiClientService> logger)
        {
            _http = http;
            _logger = logger;
        }

        /// <summary>
        /// GET.
        /// </summary>
        /// <typeparam name="T">The Response Type.</typeparam>
        /// <param name="endpointUri">Endpoint URL.</param>
        /// <param name="silentWhenFail">Do not show the toast message if the response is an error.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string endpointUri, bool silentWhenFail = false, CancellationToken cancellationToken = default)
            => await ProcessRequestAsync<T>(()
                => _http.GetAsync(endpointUri, cancellationToken), silentWhenFail: silentWhenFail);

        /// <summary>
        /// POST.
        /// </summary>
        /// <typeparam name="S">The Request Type.</typeparam>
        /// <typeparam name="T">The Response Type.</typeparam>
        /// <param name="endpointUri">Endpoint URL.</param>
        /// <param name="request">The request object with properties parameters.</param>
        /// <param name="successMessage">A message to show if the operation was successful.</param>
        /// <param name="silentWhenFail">Do not show the toast message if the response is an error.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<T> PostAsync<S, T>(string endpointUri, S request, string successMessage = null, bool silentWhenFail = false, CancellationToken cancellationToken = default)
        {
            HttpContent httpContent = JsonContent.Create(request);
            return await ProcessRequestAsync<T>(()
                   => _http.PostAsync(endpointUri, httpContent, cancellationToken), successMessage: successMessage, silentWhenFail: silentWhenFail);
        }

        /// <summary>
        /// PATCH.
        /// </summary>
        /// <typeparam name="S">The Request Type.</typeparam>
        /// <typeparam name="T">The Response Type.</typeparam>
        /// <param name="endpointUri">Endpoint URL.</param>
        /// <param name="request">The request object with properties parameters.</param>
        /// <param name="successMessage">A message to show if the operation was successful.</param>
        /// <param name="silentWhenFail">Do not show the toast message if the response is an error.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<T> PatchAsync<S, T>(string endpointUri, S request, string successMessage = null, bool silentWhenFail = false, CancellationToken cancellationToken = default)
        {
            HttpContent httpContent = request is null ? null : JsonContent.Create(request);
            return await ProcessRequestAsync<T>(()
                   => _http.PatchAsync(endpointUri, httpContent, cancellationToken), successMessage: successMessage, silentWhenFail: silentWhenFail);
        }

        /// <summary>
        /// PUT.
        /// </summary>
        /// <typeparam name="S">The Request Type.</typeparam>
        /// <typeparam name="T">The Response Type.</typeparam>
        /// <param name="endpointUri">Endpoint URL.</param>
        /// <param name="request">The request object with properties parameters.</param>
        /// <param name="successMessage">A message to show if the operation was successful.</param>
        /// <param name="silentWhenFail">Do not show the toast message if the response is an error.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<T> PutAsync<S, T>(string endpointUri, S request, string successMessage = null, bool silentWhenFail = false, CancellationToken cancellationToken = default)
        {
            HttpContent httpContent = JsonContent.Create(request);
            return await ProcessRequestAsync<T>(()
                   => _http.PutAsync(endpointUri, httpContent, cancellationToken), successMessage: successMessage, silentWhenFail: silentWhenFail);
        }

        /// <summary>
        /// PUT (With parameters in the route).
        /// </summary>
        /// <typeparam name="T">The Response Type.</typeparam>
        /// <param name="endpointUri">Endpoint URL.</param>
        /// <param name="silentWhenFail">Do not show the toast message if the response is an error.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<T> PutAsync<T>(string endpointUri, string successMessage = null, bool silentWhenFail = false, CancellationToken cancellationToken = default)
            => await ProcessRequestAsync<T>(()
                => _http.PutAsync(endpointUri, null, cancellationToken), successMessage: successMessage, silentWhenFail: silentWhenFail);


        /// <summary>
        /// DELETE.
        /// </summary>
        /// <typeparam name="T">The Response Type.</typeparam>
        /// <param name="endpointUri">Endpoint URL.</param>
        /// <param name="silentWhenFail">Do not show the toast message if the response is an error.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<T> DeleteAsync<T>(string endpointUri, string successMessage = null, bool silentWhenFail = false, CancellationToken cancellationToken = default)
            => await ProcessRequestAsync<T>(()
                => _http.DeleteAsync(endpointUri, cancellationToken), successMessage: successMessage, silentWhenFail: silentWhenFail);


        public void LogClientAppEvent(string message, string heading, bool silent = false)
        {
            // TODO: Log client app error on the server
        }

        private async Task<T> ProcessRequestAsync<T>(Func<Task<HttpResponseMessage>> httpFunc, string successMessage = null, bool silentWhenFail = false)
        {
            T result = default;

            try
            {
                var response = await httpFunc();

                if (!response.IsSuccessStatusCode)
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.InternalServerError)
                        {
                            _logger.LogError($"Error on {response.RequestMessage.RequestUri.AbsoluteUri}", response.RequestMessage.Headers);

                            return result;
                        }

                        if (response.StatusCode == HttpStatusCode.NoContent)
                        {
                            _logger.LogDebug("Response got no content.");
                            return result;
                        }

                        return result;
                    }

                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        _logger.LogWarning("Resource Not Found.", response);

                        return result;
                    }

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        _logger.LogWarning("Unauthorized.", response);

                        return result;
                    }

                    if (response.StatusCode == HttpStatusCode.Forbidden)
                    {
                        _logger.LogWarning("Cannot be processed.", response);

                        return result;
                    }

                    _logger.LogError("Couldn't get a processed error. Null BadResponse.");

                    return result;

                }

            }
            catch (NotSupportedException ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogInformation("Operation canceled.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return default;
            }

            return result;
        }

    }

}
