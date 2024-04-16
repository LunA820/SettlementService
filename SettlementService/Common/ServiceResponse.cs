namespace YourProject.Common
{
    public class ServiceResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}
