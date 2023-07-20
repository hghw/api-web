namespace api_web.Controllers.Helpers
{
    public class RestError
    {
        public string status { get; set; }

        public RestError(string status)
        {
            this.status = status;
        }
    }
}
