namespace cs_quick_ship_authentication_server.Validation.Response
{
    public abstract class BaseValidationResponse
    {
        public bool Succeeded { get; set; }
        public string Error { get; set; } = string.Empty;
        public string ErrorDescription { get; set; }
        public bool HasError => !string.IsNullOrEmpty(Error);
    }
}
