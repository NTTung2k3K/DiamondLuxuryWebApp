using DiamondLuxurySolution.ViewModel.Common;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace DiamondLuxurySolution.AdminCrewApp.Services
{
    public class BaseApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        protected async Task<ApiResult<TResponse>> PostAsyncHasImage<TResponse>(string url, object obj)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.BaseAddress]);

            using (var multipartFormDataContent = new MultipartFormDataContent())
            {
                // Get the properties of the object
                var properties = obj.GetType().GetProperties();

                foreach (var property in properties)
                {
                    var propertyValue = property.GetValue(obj);

                    if (propertyValue is IFormFile formFile)
                    {
                        // Handle IFormFile
                        if (formFile.Length > 0)
                        {
                            using (var stream = new MemoryStream())
                            {
                                await formFile.CopyToAsync(stream);
                                var fileContent = new ByteArrayContent(stream.ToArray());
                                fileContent.Headers.ContentType = new MediaTypeHeaderValue(formFile.ContentType);
                                multipartFormDataContent.Add(fileContent, property.Name, formFile.FileName);
                            }
                        }
                    }
                    else
                    {
                        // Handle other properties
                        string stringContent = string.Empty;

                        if (propertyValue != null)
                        {
                            if (propertyValue is string)
                            {
                                // If the property value is already a string, use it directly
                                stringContent = propertyValue.ToString();
                            }
                            else
                            {
                                // Convert property value to JSON and then to plain text
                                stringContent = JsonConvert.SerializeObject(propertyValue);
                            }
                        }

                        var textContent = new StringContent(stringContent, Encoding.UTF8, "text/plain");
                        multipartFormDataContent.Add(textContent, property.Name);
                    }
                }

                // Post the content
                var response = await client.PostAsync(url, multipartFormDataContent);
                var body = await response.Content.ReadAsStringAsync();
                var objectResult = JsonConvert.DeserializeObject<ApiErrorResult<TResponse>>(body);
               
                if (objectResult.IsSuccessed == false)
                {
                    return objectResult;
                }
                else
                {
                    return JsonConvert.DeserializeObject<ApiSuccessResult<TResponse>>(body);
                }
            }
        }

        protected async Task<ApiResult<TResponse>> PostAsync<TResponse>(string url, Object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.BaseAddress]);
            var response = await client.PostAsync(url, httpContent);
            var body = await response.Content.ReadAsStringAsync();
            var objectResult = JsonConvert.DeserializeObject<ApiErrorResult<TResponse>>(body);
           
            if (objectResult.IsSuccessed == false)
            {
                return objectResult;
            }
            else
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<TResponse>>(body);
            }
        }
        protected async Task<ApiResult<TResponse>> GetAsync<TResponse>(string url)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.BaseAddress]);
            var response = await client.GetAsync(url);
            var body = await response.Content.ReadAsStringAsync();
            var objectResult = JsonConvert.DeserializeObject<ApiErrorResult<TResponse>>(body);
           
            if (objectResult.IsSuccessed == false)
            {
                return objectResult;
            }
            else
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<TResponse>>(body);
            }

           
        }
        protected async Task<ApiResult<TResponse>> PutAsync<TResponse>(string url, Object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.BaseAddress]);
            var response = await client.PutAsync(url, httpContent);
            var body = await response.Content.ReadAsStringAsync();
            var objectResult = JsonConvert.DeserializeObject<ApiErrorResult<TResponse>>(body);
           
            if (objectResult.IsSuccessed == false)
            {
                return objectResult;
            }
            else
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<TResponse>>(body);
            }
        }
        protected async Task<ApiResult<TResponse>> PutAsyncHasImage<TResponse>(string url, object obj)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.BaseAddress]);

            using (var multipartFormDataContent = new MultipartFormDataContent())
            {
                // Get the properties of the object
                var properties = obj.GetType().GetProperties();

                foreach (var property in properties)
                {
                    var propertyValue = property.GetValue(obj);

                    if (propertyValue is IFormFile formFile)
                    {
                        // Handle IFormFile
                        if (formFile.Length > 0)
                        {
                            using (var stream = new MemoryStream())
                            {
                                await formFile.CopyToAsync(stream);
                                var fileContent = new ByteArrayContent(stream.ToArray());
                                fileContent.Headers.ContentType = new MediaTypeHeaderValue(formFile.ContentType);
                                multipartFormDataContent.Add(fileContent, property.Name, formFile.FileName);
                            }
                        }
                    }
                    else
                    {
                        // Handle other properties
                        string stringContent = string.Empty;

                        if (propertyValue != null)
                        {
                            if (propertyValue is string)
                            {
                                // If the property value is already a string, use it directly
                                stringContent = propertyValue.ToString();
                            }
                            else
                            {
                                // Convert property value to JSON and then to plain text
                                stringContent = JsonConvert.SerializeObject(propertyValue);
                            }
                        }

                        var textContent = new StringContent(stringContent, Encoding.UTF8, "text/plain");
                        multipartFormDataContent.Add(textContent, property.Name);
                    }
                }

                // Put the content
                var response = await client.PutAsync(url, multipartFormDataContent);
                var body = await response.Content.ReadAsStringAsync();
                var objectResult = JsonConvert.DeserializeObject<ApiErrorResult<TResponse>>(body);
               
                if (objectResult.IsSuccessed == false)
                {
                    return objectResult;
                }
                else
                {
                    return JsonConvert.DeserializeObject<ApiSuccessResult<TResponse>>(body);
                }
            }
        }

        protected async Task<ApiResult<TResponse>> DeleteAsync<TResponse>(string url)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.BaseAddress]);
            var response = await client.DeleteAsync(url);
            var body = await response.Content.ReadAsStringAsync();
            var objectResult = JsonConvert.DeserializeObject<ApiErrorResult<TResponse>>(body);
           
            if (objectResult.IsSuccessed == false)
            {
                return objectResult;
            }
            else
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<TResponse>>(body);
            }
        }

        protected async Task<ApiResult<TResponse>> PatchAsync<TResponse>(string url, Object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.BaseAddress]);
            var response = await client.PatchAsync(url, httpContent);
            var body = await response.Content.ReadAsStringAsync();
            var objectResult = JsonConvert.DeserializeObject<ApiErrorResult<TResponse>>(body);
           
            if (objectResult.IsSuccessed == false)
            {
                return objectResult;
            }
            else
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<TResponse>>(body);
            }
        }


    }
}
