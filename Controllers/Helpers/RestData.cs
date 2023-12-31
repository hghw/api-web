﻿namespace api_web.Controllers.Helpers
{
    public class RestData
    {
        public string status { get; set; }
        public dynamic? data { get; set; }
        public RestData()
        {
            this.status = "OK";
        }
        public RestData(string status)
        {
            this.status = status;
        }
        public RestData(string status, dynamic? data)
        {
            this.status = status;
            this.data = data;
        }

    }
}
