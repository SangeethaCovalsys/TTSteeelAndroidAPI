using TTSteelAndroidAPI.Model.Login;

namespace TTSteelAndroidAPI.Interface
{
    public interface ISapService
    {
        Task<SapLoginResult> LoginUserAsync(loginModel payload);
        Task<bool> LoginAsync();  // optional, if you want to keep it
        Task<string> GetAsync(string endpoint);
        Task<string?> PostAsync(string endpoint, object payload);
        Task<string?> PostCancelAsync(string endpoint, string key);
        Task<string?> PostCloseAsync(string endpoint, string key);
        Task<string> PatchAsync(string endpoint, string key, object payload);
        Task<string> Patch_DocumentAsync(string endpoint, string key, object payload);
        Task DeleteAsync(string endpoint, string key);
        Task<string> GetFilteredAsync(string endpoint, string filterQuery);
        Task<string> GetSingleAsync(string endpoint, string key);
       
    }
}
