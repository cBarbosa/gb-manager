using gb_manager.Infraestructure.ExternalServices.MercadoPago.Domain;
using gb_manager.Infraestructure.ExternalServices.MercadoPago.Domain.Resource;
using gb_manager.Infraestructure.ExternalServices.MercadoPago.Serialization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace gb_manager.Infraestructure.ExternalServices.MercadoPago
{
    public class MercadoPagoService : IMercadoPagoService
    {
        private readonly IHttpClientFactory clientFactory;
        private readonly ILogger<MercadoPagoService> logger;
        private readonly ISerializer iserializer;

        public MercadoPagoService(
            IHttpClientFactory _clientFactory,
            ILogger<MercadoPagoService> _logger,
            ISerializer _iserializer)
        {
            clientFactory = _clientFactory;
            logger = _logger;
            iserializer = _iserializer;
        }

        public async Task<IResource> GetByResourceId(string resourceId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"/checkout/preferences/{resourceId}");
                var client = clientFactory.CreateClient("MercadoPago");
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    string content = null;
                    using (var sr = new StreamReader(responseStream))
                    {
                        content = await sr.ReadToEndAsync();
                    }
                    return iserializer.DeserializeFromJson<Preference>(content);
                }
                return null;
            }
            catch (WebException e)
            {
                ThrowError(e);
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<IResource> GetCheckoutPreferencesSearch()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "/checkout/preferences/search");
                var client = clientFactory.CreateClient("MercadoPago");
                var response = await client.SendAsync(request);

                //if (response.IsSuccessStatusCode)
                //{
                //    using var responseStream = await response.Content.ReadAsStreamAsync();
                //    PullRequests = await JsonSerializer.DeserializeAsync<IEnumerable<GitHubPullRequest>>(responseStream);
                //}
                //else
                //{
                //    GetPullRequestsError = true;
                //    PullRequests = Array.Empty<GitHubPullRequest>();
                //}

                using var responseStream = await response.Content.ReadAsStreamAsync();
                string content = null;
                using (var sr = new StreamReader(responseStream))
                {
                    content = await sr.ReadToEndAsync().ConfigureAwait(false);
                }

                return iserializer.DeserializeFromJson<ElementsResourcesPage<Preference>>(content);

                //return await JsonSerializer.DeserializeAsync<object>(responseStream);
                //return await iserializer.DeserializeFromJson<Preference>(responseStream);
            }
            catch (WebException e)
            {
                ThrowError(e);
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }

        private void ThrowError(WebException e)
        {
            using WebResponse response = e.Response;
            HttpWebResponse httpResponse = (HttpWebResponse)response;
            logger.LogError($"Error code: {httpResponse.StatusCode}", httpResponse);
            using Stream data = response.GetResponseStream();
            using var reader = new StreamReader(data);
            string text = reader.ReadToEnd();
            logger.LogError($"Error text: {text}", data);
            throw new Exception(text);
        }
    }
}
