namespace DockerizeTaskManagementApi.AppCode.Infrastructure
{
    public class JsonDataResponse<T>
    {
        public bool Error { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
