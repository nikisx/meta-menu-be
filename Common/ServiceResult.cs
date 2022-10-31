namespace meta_menu_be.Common
{
    public class ServiceResult<T>
    {
        public ServiceResult()
        {

        }
        public ServiceResult(T data)
        {
            this.Data = data;
            this.Success = true;
        }
        public ServiceResult(string errors)
        {
            this.Message = errors;
            this.Success = false;
        }
        public string Status { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
