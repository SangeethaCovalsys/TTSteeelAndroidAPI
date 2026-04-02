namespace TTSteelAndroidAPI.Model.Login
{
    public class loginModel
    {
        public string CompanyDB { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }

    }
    public class SapLoginResult
    {
        public bool Success { get; set; }
        public string? SessionId { get; set; }
        public string? Version { get; set; }
        public int? SessionTimeout { get; set; }
        public int? StatusCode { get; set; }
        public string? Message { get; set; }
        public string? Error { get; set; }
        public string? UserCode { get; set; }
        public string? SapUserName { get; set; }
    }
}
