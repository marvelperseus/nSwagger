//nSwaggerVersion:0.0.6
// This file was automatically generated by nSwagger. Changes made to this file will be lost if nSwagger is run again. See https://github.com/rmaclean/nswagger for more information.
// This file was last generated at: 2016-04-08T12:38:04.4353452Z
namespace nSwagger
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class DevConfRatings
    {
        private readonly string url;
        private readonly ISwaggerHTTPClient httpClient;
        public DevConfRatings(string url = null, ISwaggerHTTPClient httpClient = null)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                this.url = url;
            }
            else
            {
                this.url = "https://localhost:24110";
            }

            if (httpClient == null)
            {
                this.httpClient = new SwaggerHTTPClient();
            }
            else
            {
                this.httpClient = httpClient;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("", "RECS0001:Class is declared partial but has only one part", Justification = "This is partial to allow the file to extended in a seperate file if needed. Changes to this file would be lost when the code is regenerated and so supporting a seperate file for this is ideal.")]
        public partial class GetRatings
        {
            public string Email
            {
                get;
                set;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("", "RECS0001:Class is declared partial but has only one part", Justification = "This is partial to allow the file to extended in a seperate file if needed. Changes to this file would be lost when the code is regenerated and so supporting a seperate file for this is ideal.")]
        public partial class RatingSession
        {
            public string Comment
            {
                get;
                set;
            }

            public int Order
            {
                get;
                set;
            }

            public int Rating
            {
                get;
                set;
            }

            public int SessionId
            {
                get;
                set;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("", "RECS0001:Class is declared partial but has only one part", Justification = "This is partial to allow the file to extended in a seperate file if needed. Changes to this file would be lost when the code is regenerated and so supporting a seperate file for this is ideal.")]
        public partial class Rating
        {
            public string Email
            {
                get;
                set;
            }

            public RatingSession Session1
            {
                get;
                set;
            }

            public RatingSession Session2
            {
                get;
                set;
            }

            public RatingSession Session3
            {
                get;
                set;
            }

            public RatingSession Session4
            {
                get;
                set;
            }

            public RatingSession Session5
            {
                get;
                set;
            }

            public RatingSession Session6
            {
                get;
                set;
            }

            public RatingSession Session7
            {
                get;
                set;
            }

            public RatingSession Session8
            {
                get;
                set;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("", "RECS0001:Class is declared partial but has only one part", Justification = "This is partial to allow the file to extended in a seperate file if needed. Changes to this file would be lost when the code is regenerated and so supporting a seperate file for this is ideal.")]
        public partial class TimeSlot
        {
            public string End
            {
                get;
                set;
            }

            public int Order
            {
                get;
                set;
            }

            public Session[] Sessions
            {
                get;
                set;
            }

            public string Start
            {
                get;
                set;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("", "RECS0001:Class is declared partial but has only one part", Justification = "This is partial to allow the file to extended in a seperate file if needed. Changes to this file would be lost when the code is regenerated and so supporting a seperate file for this is ideal.")]
        public partial class Session
        {
            public int Id
            {
                get;
                set;
            }

            public string Presenter
            {
                get;
                set;
            }

            public string Title
            {
                get;
                set;
            }

            public string Track
            {
                get;
                set;
            }
        }

        //<summary>
        // Gets previously entered reviews
        //</summary>
        //<returns>
        // Success
        //</returns>
        //<param name="request">email or token for lookup</param>
        public async Task<APIResponse<RatingSession[]>> Rating_PostGetRatingAsync(GetRatings request)
        {
            {
                var response = await httpClient.PostAsync(new Uri(url + "/api/rating/history", UriKind.Absolute), new SwaggerHTTPClientOptions(TimeSpan.FromSeconds(30)), new StringContent(JsonConvert.SerializeObject(request)));
                if (response == null)
                {
                    return new APIResponse<RatingSession[]>(false);
                }

                switch ((int)response.StatusCode)
                {
                    case 200:
                    {
                        var data = JsonConvert.DeserializeObject<RatingSession[]>(await response.Content.ReadAsStringAsync());
                        return new APIResponse<RatingSession[]>(data, response.StatusCode);
                    }

                    case 400:
                    {
                        return new APIResponse<RatingSession[]>(response.StatusCode);
                    }

                    case 404:
                    {
                        return new APIResponse<RatingSession[]>(response.StatusCode);
                    }

                    default:
                    {
                        return new APIResponse<RatingSession[]>(response.StatusCode);
                    }
                }
            }
        }

        //<summary>
        // Used to add a rating or update a rating in the system
        //</summary>
        //<returns>
        // Success
        //</returns>
        //<param name="rating">The rating value</param>
        public async Task<APIResponse<string>> Rating_PostAddRatingAsync(Rating rating)
        {
            {
                var response = await httpClient.PostAsync(new Uri(url + "/api/Rating", UriKind.Absolute), new SwaggerHTTPClientOptions(TimeSpan.FromSeconds(30)), new StringContent(JsonConvert.SerializeObject(rating)));
                if (response == null)
                {
                    return new APIResponse<string>(false);
                }

                switch ((int)response.StatusCode)
                {
                    case 400:
                    {
                        return new APIResponse<string>(response.StatusCode);
                    }

                    case 401:
                    {
                        return new APIResponse<string>(response.StatusCode);
                    }

                    case 204:
                    {
                        var data = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
                        return new APIResponse<string>(data, response.StatusCode);
                    }

                    default:
                    {
                        return new APIResponse<string>(response.StatusCode);
                    }
                }
            }
        }

        //<summary>
        // Gets all the sessions
        //</summary>
        //<returns>
        // Success
        //</returns>
        public async Task<APIResponse<TimeSlot[]>> Session_GetSessionsAsync()
        {
            {
                var response = await httpClient.GetAsync(new Uri(url + "/api/Session", UriKind.Absolute), new SwaggerHTTPClientOptions(TimeSpan.FromSeconds(30)));
                if (response == null)
                {
                    return new APIResponse<TimeSlot[]>(false);
                }

                switch ((int)response.StatusCode)
                {
                    case 200:
                    {
                        var data = JsonConvert.DeserializeObject<TimeSlot[]>(await response.Content.ReadAsStringAsync());
                        return new APIResponse<TimeSlot[]>(data, response.StatusCode);
                    }

                    default:
                    {
                        return new APIResponse<TimeSlot[]>(response.StatusCode);
                    }
                }
            }
        }
    }     public class SwaggerHTTPClientOptions
    {
        public TimeSpan Timeout { get; }
    
        public SwaggerHTTPClientOptions(TimeSpan timeout)
        {
            Timeout = timeout;
        }
    }
    
    public interface ISwaggerHTTPClient
    {
        Task<HttpResponseMessage> PutAsync(Uri uri, SwaggerHTTPClientOptions httpOptions, HttpContent content = null, string token = null);
        Task<HttpResponseMessage> PostAsync(Uri uri, SwaggerHTTPClientOptions httpOptions, HttpContent content = null, string token = null);
        Task<HttpResponseMessage> HeadAsync(Uri uri, SwaggerHTTPClientOptions httpOptions, string token = null);
        Task<HttpResponseMessage> OptionsAsync(Uri uri, SwaggerHTTPClientOptions httpOptions, string token = null);
        Task<HttpResponseMessage> PatchAsync(Uri uri, SwaggerHTTPClientOptions httpOptions, HttpContent content, string token = null);
        Task<HttpResponseMessage> DeleteAsync(Uri uri, SwaggerHTTPClientOptions httpOptions, string token = null);
        Task<HttpResponseMessage> GetAsync(Uri uri, SwaggerHTTPClientOptions httpOptions, string token = null);
    }
    
    class SwaggerHTTPClient : ISwaggerHTTPClient
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "This is done interntionally as each place that calls this will dispose it")]
        private static HttpClient CreateClient()
        {
            var cookieJar = new CookieContainer();
            var httpHandler = new HttpClientHandler
            {
                CookieContainer = cookieJar,
                AllowAutoRedirect = true,
                UseCookies = true
            };
    
            var client = new HttpClient(httpHandler, true);
            client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
            {
                NoCache = true,
                NoStore = true,
                Private = true,
                ProxyRevalidate = true,
                MustRevalidate = true
            };
    
            return client;
        }
    
        public async Task<HttpResponseMessage> PutAsync(Uri uri, SwaggerHTTPClientOptions httpOptions, HttpContent content = null, string token = null) => await HTTPCallAsync("put", uri, httpOptions, content, token);
    
        public async Task<HttpResponseMessage> PostAsync(Uri uri, SwaggerHTTPClientOptions httpOptions, HttpContent content = null, string token = null) => await HTTPCallAsync("post", uri, httpOptions, content, token);
    
        private static async Task<HttpResponseMessage> HTTPCallAsync(string method, Uri uri, SwaggerHTTPClientOptions options, HttpContent content = null, string token = null)
        {
            using (var client = CreateClient())
            {
                using (var cancellationTokenSource = new CancellationTokenSource(options.Timeout))
                {
                    var errorMessage = string.Empty;
                    try
                    {
                        if (content != null)
                        {
                            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        }
    
                        if (!string.IsNullOrWhiteSpace(token))
                        {
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                        }
    
                        var response = default(HttpResponseMessage);
                        switch (method.ToUpperInvariant())
                        {
                            case "DELETE":
                                {
                                    response = await client.DeleteAsync(uri, cancellationTokenSource.Token);
                                    break;
                                }
                            case "POST":
                                {
                                    response = await client.PostAsync(uri, content, cancellationTokenSource.Token);
                                    break;
                                }
                            case "PUT":
                                {
                                    response = await client.PutAsync(uri, content, cancellationTokenSource.Token);
                                    break;
                                }
                            case "GET":
                                {
                                    response = await client.GetAsync(uri, HttpCompletionOption.ResponseContentRead, cancellationTokenSource.Token);
                                    break;
                                }
                            case "HEAD":
                                {
                                    response = await client.SendAsync(new HttpRequestMessage
                                    {
                                        Method = new HttpMethod(method),
                                        RequestUri = uri
                                    }, HttpCompletionOption.ResponseHeadersRead, cancellationTokenSource.Token);
    
                                    break;
                                }
                            case "OPTIONS":
                                {
                                    response = await client.SendAsync(new HttpRequestMessage
                                    {
                                        Method = new HttpMethod(method),
                                        RequestUri = uri
                                    }, HttpCompletionOption.ResponseContentRead, cancellationTokenSource.Token);
    
                                    break;
                                }
                            case "PATCH":
                                {
                                    response = await client.SendAsync(new HttpRequestMessage
                                    {
                                        Method = new HttpMethod(method),
                                        RequestUri = uri,
                                        Content = content
                                    }, HttpCompletionOption.ResponseContentRead, cancellationTokenSource.Token);
    
                                    break;
                                }
                        }
    
    #if DEBUG
                                    Debug.WriteLine($"HTTP {method} to {uri} returned {response.StatusCode} with content {await response.Content?.ReadAsStringAsync()}");
    #endif
                        return response;
                    }
                    catch (FileNotFoundException) { errorMessage = $"HTTP {method} exception - file not found exception"; /* this can happen if WP cannot resolve the server */ }
                    catch (WebException) { errorMessage = $"HTTP {method} exception - web exception"; }
                    catch (HttpRequestException) { errorMessage = $"HTTP {method} exception - http exception"; }
                    catch (TaskCanceledException) { errorMessage = $"HTTP {method} exception - task cancelled exception"; }
                    catch (UnauthorizedAccessException) { errorMessage = $"HTTP {method} exception - unauth exception"; }
    
    #if DEBUG
                                Debug.WriteLine(errorMessage);
    #endif
                }
            } 
    
            return null;
        }
    
        public async Task<HttpResponseMessage> HeadAsync(Uri uri, SwaggerHTTPClientOptions httpOptions, string token = null) => await HTTPCallAsync("head", uri, httpOptions, token: token);
    
        public async Task<HttpResponseMessage> OptionsAsync(Uri uri, SwaggerHTTPClientOptions httpOptions, string token = null) => await HTTPCallAsync("options", uri, httpOptions, token: token);
    
        public async Task<HttpResponseMessage> PatchAsync(Uri uri, SwaggerHTTPClientOptions httpOptions, HttpContent content, string token = null) => await HTTPCallAsync("patch", uri, httpOptions, content, token: token);
    
        public async Task<HttpResponseMessage> DeleteAsync(Uri uri, SwaggerHTTPClientOptions httpOptions, string token = null) => await HTTPCallAsync("delete", uri, httpOptions, token: token);
    
        public async Task<HttpResponseMessage> GetAsync(Uri uri, SwaggerHTTPClientOptions httpOptions, string token = null) => await HTTPCallAsync("get", uri, httpOptions, token: token);
    }
    
    public class APIResponse<T>
    {
        public APIResponse(dynamic data, HttpStatusCode statusCode) : this(statusCode)
        {
            Data = data;
        }
    
        public APIResponse(T successData, HttpStatusCode statusCode) : this(statusCode)
        {
            SuccessData = successData;
            SuccessDataAvailable = true;
        }
    
        public bool Success { get; }
    
        public APIResponse(bool success)
        {
            Success = success;
        }
    
        public APIResponse(HttpStatusCode statusCode) : this((int)statusCode >= 200 && (int)statusCode <= 299)
        {
            HTTPStatusCode = statusCode;
        }
    
        public dynamic Data { get; }
    
        public T SuccessData { get; }
    
        public HttpStatusCode? HTTPStatusCode { get; }
    
        public bool SuccessDataAvailable { get; }
    }

}