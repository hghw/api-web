namespace api_web.Controllers.Helpers
{
    public class RestData
    {
        public string status { get; set; }
        public dynamic? data { get; set; }

        public RestData(string status, dynamic? data)
        {
            this.status = status;
            this.data = data;
        }
        public RestData(string status)
        {
            this.status = status;
        }
    }
}
