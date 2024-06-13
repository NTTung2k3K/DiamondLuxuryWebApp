using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models;
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
            client.Timeout = TimeSpan.FromMinutes(5);
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
                    else if (propertyValue is DateTime dateTime)
                    {
                        // Convert DateTime to string in a specific format (e.g., ISO 8601)
                        string stringContent = dateTime.ToString("o"); // "o" stands for the round-trip format, which is ISO 8601
                        multipartFormDataContent.Add(new StringContent(stringContent, Encoding.UTF8, "application/json"), property.Name);
                    }
                    else if (propertyValue is Guid || propertyValue is Guid?)
                    {
                        // Convert Guid to string
                        string stringContent = propertyValue.ToString();
                        multipartFormDataContent.Add(new StringContent(stringContent, Encoding.UTF8, "application/json"), property.Name);
                    }

                    else if (propertyValue is IEnumerable<Guid> guidList)
                    {
                        // Add each Guid in the list separately
                        foreach (var guid in guidList)
                        {
                            multipartFormDataContent.Add(new StringContent(guid.ToString(), Encoding.UTF8, "application/json"), $"{property.Name}[]");
                        }
                    }
                    else if (propertyValue is string strValue)
                    {
                        // Handle string values directly without JSON serialization
                        multipartFormDataContent.Add(new StringContent(strValue, Encoding.UTF8, "text/plain"), property.Name);
                    }
                    else
                    {
                        // Convert other property values to JSON and then to plain text
                        string stringContent = JsonConvert.SerializeObject(propertyValue);
                        multipartFormDataContent.Add(new StringContent(stringContent, Encoding.UTF8, "application/json"), property.Name);
                    }
                }
                // Post the content
                var response = await client.PostAsync(url, multipartFormDataContent);
                var body = await response.Content.ReadAsStringAsync();
                // Log the response content for debugging
                Console.WriteLine($"Response Status Code: {response.StatusCode}");
                Console.WriteLine($"Response Body: {body}");
                if (!response.IsSuccessStatusCode)
                {
                    var objectResult = JsonConvert.DeserializeObject<ApiErrorResult<TResponse>>(body);
                    return objectResult;
                }
                return JsonConvert.DeserializeObject<ApiSuccessResult<TResponse>>(body);
            }
        }


        protected async Task<ApiResult<TResponse>> PostAsyncHasImageAndListImage<TResponse>(string url, object obj, List<IFormFile> files)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.BaseAddress]);
            client.Timeout = TimeSpan.FromMinutes(5);
            using (var multipartFormDataContent = new MultipartFormDataContent())
            {
                // Get the properties of the object
                var properties = obj.GetType().GetProperties();

                // Add properties of the object to the multipart form data
                foreach (var property in properties)
                {
                    var propertyValue = property.GetValue(obj);
                    if (propertyValue != null)
                    {
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
                        else if (propertyValue is DateTime dateTime)
                        {
                            // Convert DateTime to string in a specific format (e.g., ISO 8601)
                            string stringContent = dateTime.ToString("o"); // "o" stands for the round-trip format, which is ISO 8601
                            multipartFormDataContent.Add(new StringContent(stringContent, Encoding.UTF8, "application/json"), property.Name);
                        }
                        else if (propertyValue is Guid || propertyValue is Guid?)
                        {
                            // Convert Guid to string
                            string stringContent = propertyValue.ToString();
                            multipartFormDataContent.Add(new StringContent(stringContent, Encoding.UTF8, "application/json"), property.Name);
                        }
                        else if (propertyValue is IEnumerable<Guid> guidList)
                        {
                            // Add each Guid in the list separately
                            foreach (var guid in guidList)
                            {
                                multipartFormDataContent.Add(new StringContent(guid.ToString(), Encoding.UTF8, "application/json"), $"{property.Name}[]");
                            }
                        }
                        else if (propertyValue is string strValue)
                        {
                            // Handle string values directly without JSON serialization
                            multipartFormDataContent.Add(new StringContent(strValue, Encoding.UTF8, "text/plain"), property.Name);
                        }
                        else
                        {

                            // Convert other property values to JSON and then to plain text
                            string stringContent = JsonConvert.SerializeObject(propertyValue);
                            multipartFormDataContent.Add(new StringContent(stringContent, Encoding.UTF8, "application/json"), property.Name);
                        }
                    }


                }

                // Add IFormFile to the multipart form data
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await file.CopyToAsync(stream);
                            var fileContent = new ByteArrayContent(stream.ToArray());
                            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                            multipartFormDataContent.Add(fileContent, "Images", file.FileName);
                        }
                    }
                }

                // Post the content
                var response = await client.PostAsync(url, multipartFormDataContent);
                var body = await response.Content.ReadAsStringAsync();

                // Log the response content for debugging
                Console.WriteLine($"Response Status Code: {response.StatusCode}");
                Console.WriteLine($"Response Body: {body}");

                if (!response.IsSuccessStatusCode)
                {
                    var objectResult = JsonConvert.DeserializeObject<ApiErrorResult<TResponse>>(body);
                    return objectResult;
                }

                return JsonConvert.DeserializeObject<ApiSuccessResult<TResponse>>(body);
            }
        }

        protected async Task<ApiResult<TResponse>> PostAsync<TResponse>(string url, Object obj)
        {
            var json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat, // Ensure DateTime is serialized in ISO 8601 format
                DateTimeZoneHandling = DateTimeZoneHandling.Utc // Handle DateTime in UTC format if necessary

            });

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromMinutes(5);

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

        protected async Task<ApiResult<TResponse>> PutAsyncHasImageAndListImage<TResponse>(string url, object obj, List<IFormFile>? files)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.BaseAddress]);
            client.Timeout = TimeSpan.FromMinutes(5);
            using (var multipartFormDataContent = new MultipartFormDataContent())
            {
                // Get the properties of the object
                var properties = obj.GetType().GetProperties();

                // Add properties of the object to the multipart form data
                foreach (var property in properties)
                {
                    var propertyValue = property.GetValue(obj);
                    if (propertyValue != null)
                    {
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
 if (propertyValue is DateTime dateTime)
                        {
                            // Convert DateTime to string in a specific format (e.g., ISO 8601)
                            string stringContent = dateTime.ToString("o"); // "o" stands for the round-trip format, which is ISO 8601
                            multipartFormDataContent.Add(new StringContent(stringContent, Encoding.UTF8, "application/json"), property.Name);
                        }
                        else if (propertyValue is Guid || propertyValue is Guid?)
                        {
                            // Convert Guid to string
                            string stringContent = propertyValue.ToString();
                            multipartFormDataContent.Add(new StringContent(stringContent, Encoding.UTF8, "application/json"), property.Name);
                        }
                        else if (propertyValue is IEnumerable<Guid> guidList)
                        {
                            // Add each Guid in the list separately
                            foreach (var guid in guidList)
                            {
                                multipartFormDataContent.Add(new StringContent(guid.ToString(), Encoding.UTF8, "application/json"), $"{property.Name}[]");
                            }
                        }
                        else if (propertyValue is string strValue)
                        {
                            // Handle string values directly without JSON serialization
                            multipartFormDataContent.Add(new StringContent(strValue, Encoding.UTF8, "text/plain"), property.Name);
                        }
                        else if (propertyValue is IEnumerable<string> stringList)
                        {
                            // Handle list of strings
                            foreach (var str in stringList)
                            {
                                multipartFormDataContent.Add(new StringContent(str, Encoding.UTF8, "text/plain"), $"{property.Name}[]");
                            }
                        }
                        else
                        {

                            // Convert other property values to JSON and then to plain text
                            string stringContent = JsonConvert.SerializeObject(propertyValue);
                            multipartFormDataContent.Add(new StringContent(stringContent, Encoding.UTF8, "application/json"), property.Name);
                        }
                    }


                }

                // Add IFormFile to the multipart form data
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await file.CopyToAsync(stream);
                            var fileContent = new ByteArrayContent(stream.ToArray());
                            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                            multipartFormDataContent.Add(fileContent, "Images", file.FileName);
                        }
                    }
                }

                // Post the content
                var response = await client.PutAsync(url, multipartFormDataContent);
                var body = await response.Content.ReadAsStringAsync();

                // Log the response content for debugging
                Console.WriteLine($"Response Status Code: {response.StatusCode}");
                Console.WriteLine($"Response Body: {body}");

                if (!response.IsSuccessStatusCode)
                {
                    var objectResult = JsonConvert.DeserializeObject<ApiErrorResult<TResponse>>(body);
                    return objectResult;
                }

                return JsonConvert.DeserializeObject<ApiSuccessResult<TResponse>>(body);
            }
        }

        protected async Task<ApiResult<TResponse>> GetAsync<TResponse>(string url)
        {
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromMinutes(5);

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
            string json = null;
            if (obj != null)
            {
                json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Include, // Ensure null values are included in the serialized JSON
                    DateFormatHandling = DateFormatHandling.IsoDateFormat, // Ensure DateTime is serialized in ISO 8601 format
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc // Handle DateTime in UTC format if necessary
                });
            }

            var httpContent = obj != null ? new StringContent(json, Encoding.UTF8, "application/json") : null;
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromMinutes(5);

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
            client.Timeout = TimeSpan.FromMinutes(5);
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
                    else if (propertyValue is DateTime dateTime)
                    {
                        // Convert DateTime to string in a specific format (e.g., ISO 8601)
                        string stringContent = dateTime.ToString("o"); // "o" stands for the round-trip format, which is ISO 8601
                        multipartFormDataContent.Add(new StringContent(stringContent, Encoding.UTF8, "application/json"), property.Name);
                    }
                    else if (propertyValue is Guid || propertyValue is Guid?)
                    {
                        // Convert Guid to string
                        string stringContent = propertyValue.ToString();
                        multipartFormDataContent.Add(new StringContent(stringContent, Encoding.UTF8, "application/json"), property.Name);
                    }
                    else if (propertyValue is IEnumerable<Guid> guidList)
                    {
                        // Add each Guid in the list separately
                        foreach (var guid in guidList)
                        {
                            multipartFormDataContent.Add(new StringContent(guid.ToString(), Encoding.UTF8, "application/json"), $"{property.Name}[]");
                        }
                    }
                    else if (propertyValue is string strValue)
                    {
                        // Handle string values directly without JSON serialization
                        multipartFormDataContent.Add(new StringContent(strValue, Encoding.UTF8, "text/plain"), property.Name);
                    }
                    else
                    {
                        // Convert other property values to JSON and then to plain text
                        string stringContent = JsonConvert.SerializeObject(propertyValue);
                        multipartFormDataContent.Add(new StringContent(stringContent, Encoding.UTF8, "application/json"), property.Name);
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
            if (_httpClientFactory == null)
            {
                Console.WriteLine("HttpClientFactory is null.");
                return new ApiErrorResult<TResponse>("HttpClientFactory is null.");
            }

            var client = _httpClientFactory.CreateClient();

            if (client == null)
            {
                Console.WriteLine("HttpClient creation failed.");
                return new ApiErrorResult<TResponse>("HttpClient creation failed.");
            }

            client.Timeout = TimeSpan.FromMinutes(5);


            client.BaseAddress = new Uri(_configuration[DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.BaseAddress]);


            var response = await client.DeleteAsync(url);

            if (response == null)
            {
                Console.WriteLine("Response from DELETE request is null.");
                return new ApiErrorResult<TResponse>("Response from DELETE request is null.");
            }

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
            var json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat, // Ensure DateTime is serialized in ISO 8601 format
                DateTimeZoneHandling = DateTimeZoneHandling.Utc // Handle DateTime in UTC format if necessary
            });

            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromMinutes(5);

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
