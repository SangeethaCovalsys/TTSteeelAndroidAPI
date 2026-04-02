using TTSteelAndroidAPI.Data;
//using TTSteelAndroidAPI.Interface;

using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TTSteelAndroidAPI.Model.Login;
using TTSteelAndroidAPI.Interface;
namespace TTSteelWebAPI.Service
{
    public class SapService:ISapService

    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private string _sessionId;
       // private readonly AppDbContext _appDbContext;
        //private readonly ICurrentUserInterface _currentUserService;
        public SapService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
            //_appDbContext = appDbContext;
          
        }

        public async Task<SapLoginResult> LoginUserAsync(loginModel payload)
        {
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("Login", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorText = await response.Content.ReadAsStringAsync();
                    return new SapLoginResult
                    {
                        Success = false,
                        StatusCode = (int)response.StatusCode,
                        Message = $"Login failed: {response.ReasonPhrase}",
                        Error = errorText
                    };
                }

                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<JsonElement>(json);

                var sessionId = data.GetProperty("SessionId").GetString();
                _sessionId = sessionId;

                _httpClient.DefaultRequestHeaders.Add("Cookie", $"B1SESSION={_sessionId}");

                // Fetch user details from OUSR
                var (userCode, sapUserName) = await GetUserDetailsAsync(payload.UserName);

                return new SapLoginResult
                {
                    Success = true,
                    SessionId = sessionId,
                    Version = data.TryGetProperty("Version", out var version) ? version.GetString() : null,
                    SessionTimeout = data.TryGetProperty("SessionTimeout", out var timeout) ? timeout.GetInt32() : (int?)null,
                    UserCode = userCode,
                    SapUserName = sapUserName
                };
            }
            catch (Exception ex)
            {
                return new SapLoginResult
                {
                    Success = false,
                    Message = "Exception occurred while logging in.",
                    Error = ex.Message
                };
            }
        }


        // 🔹 Authenticate with SAP Service Layer
        public async Task<bool> LoginAsync()
        {
            var payload = new
            {
                CompanyDB = _config["SapSettings:CompanyDB"],
                UserName = _config["SapSettings:UserName"],
                Password = _config["SapSettings:Password"]
            };
            //var userContext = _currentUserService.GetUser();

            //var payload = new
            //{
            //    CompanyDB = userContext.Database,
            //    UserName = userContext.Username,
            //    Password = userContext.Password
            //};

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Login", content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<JsonElement>(json);
                _sessionId = data.GetProperty("SessionId").GetString();

                _httpClient.DefaultRequestHeaders.Add("Cookie", $"B1SESSION={_sessionId}");
                return true;
            }

            return false;
        }

        private async Task EnsureLoginAsync()
        {
            if (string.IsNullOrEmpty(_sessionId))
            {
                var success = await LoginAsync();
                if (!success)
                    throw new Exception("Failed to login to SAP Service Layer.");
            }
        }
        private async Task<(string? UserCode, string? SapUserName)> GetUserDetailsAsync(string username)
        {
            try
            {
                var encodedUsername = Uri.EscapeDataString(username);
                var url = $"Users?$filter=UserName eq '{encodedUsername}'";

                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Failed to fetch user details: {error}");
                    return (null, null);
                }

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.TryGetProperty("value", out var value) && value.GetArrayLength() > 0)
                {
                    var firstUser = value[0];
                    string? userCode = null;
                    if (firstUser.TryGetProperty("UserCode", out var codeElement))
                    {
                        // Try to get as string; if it's numeric, it will convert
                        userCode = codeElement.ToString();
                    }

                    string? sapUserName = null;
                    if (firstUser.TryGetProperty("UserName", out var nameElement))
                    {
                        sapUserName = nameElement.GetString();
                    }

                    return (userCode, sapUserName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user details: {ex.Message}");
            }

            return (null, null);
        }

        // 🔹 Generic GET Method
        public async Task<string> GetAsync(string endpoint)
        {
            await EnsureLoginAsync();

            var response = await _httpClient.GetAsync(endpoint);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"SAP GET Error ({response.StatusCode}): {result}");

            return result;
        }

        // 🔹 Generic POST Method
        public async Task<string?> PostAsync(string endpoint, object payload)
        {
            await EnsureLoginAsync();

            var jsonString = JsonSerializer.Serialize(payload);
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(endpoint, content);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var doc = JsonDocument.Parse(result);
                var errorMessage = doc.RootElement
                       .GetProperty("error")
                       .GetProperty("message")
                       .GetProperty("value")
                       .GetString();
                throw new Exception($"SAP POST Error ({response.StatusCode}): {errorMessage}");
            }

            return result;
        }

        public async Task<string?> PostCancelAsync(string endpoint, string key)
        {
            await EnsureLoginAsync();
            var content = new StringContent(JsonSerializer.Serialize("[]"), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{endpoint}({key})/Cancel", null);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var doc = JsonDocument.Parse(result);
                var errorMessage = doc.RootElement
                       .GetProperty("error")
                       .GetProperty("message")
                       .GetProperty("value")
                       .GetString();
                throw new Exception($"SAP POST Error ({response.StatusCode}): {errorMessage}");
            }

            return result;
        }

        public async Task<string?> PostCloseAsync(string endpoint, string key)
        {
            await EnsureLoginAsync();
            var content = new StringContent(JsonSerializer.Serialize("[]"), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{endpoint}({key})/Close", null);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var doc = JsonDocument.Parse(result);
                var errorMessage = doc.RootElement
                       .GetProperty("error")
                       .GetProperty("message")
                       .GetProperty("value")
                       .GetString();
                throw new Exception($"SAP POST Error ({response.StatusCode}): {errorMessage}");
            }

            return result;
        }

        public async Task<string> PatchAsync(string endpoint, string key, object payload)
        {
            await EnsureLoginAsync();
            var jsonString = JsonSerializer.Serialize(payload);
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync($"{endpoint}({key})", content);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var doc = JsonDocument.Parse(result);
                var errorMessage = doc.RootElement
                       .GetProperty("error")
                       .GetProperty("message")
                       .GetProperty("value")
                       .GetString();
                throw new Exception($"SAP PATCH Error ({response.StatusCode}): {errorMessage}");
            }

            return result;
        }


        public async Task<string> Patch_DocumentAsync(string endpoint, string key, object payload)
        {
            await EnsureLoginAsync();

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync($"{endpoint}({key})", content);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var doc = JsonDocument.Parse(result);
                var errorMessage = doc.RootElement
                       .GetProperty("error")
                       .GetProperty("message")
                       .GetProperty("value")
                       .GetString();
                throw new Exception($"SAP PATCH Error ({response.StatusCode}): {errorMessage}");
            }

            return result;
        }
        public async Task DeleteAsync(string endpoint, string key)
        {
            await EnsureLoginAsync();

            // Example endpoint:
            // CVS_MMR1('CODE',1)
            var response = await _httpClient.DeleteAsync($"{endpoint}({key})");
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var doc = JsonDocument.Parse(result);
                    var errorMessage = doc.RootElement
                        .GetProperty("error")
                        .GetProperty("message")
                        .GetProperty("value")
                        .GetString();

                    throw new Exception($"SAP DELETE Error ({response.StatusCode}): {errorMessage}");
                }
                catch
                {
                    throw new Exception($"SAP DELETE Error ({response.StatusCode}): {result}");
                }
            }
        }

        // 🔹 Common Filter Method (for OData queries)
        public async Task<string> GetFilteredAsync(string endpoint, string filterQuery)
        {
            // Example: endpoint = "BusinessPartners", filterQuery = "$filter=CardType eq 'C'"
            var fullUrl = $"{endpoint}?{filterQuery}";
            return await GetAsync(fullUrl);
        }

        public async Task<string> GetSingleAsync(string endpoint, string key)
        {
            await EnsureLoginAsync();

            // SAP SL entity by key → CVS_OQCGRN(12)  OR CVS_OQCGRN('ABC')
            string url = $"{endpoint}({key})";

            var response = await _httpClient.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"SAP GET Error ({response.StatusCode}): {result}");

            return result;
        }
    }
}