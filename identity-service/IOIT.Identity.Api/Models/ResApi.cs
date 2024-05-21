using Newtonsoft.Json;

namespace IOIT.Identity.Api.Models
{
    public class ResApi
    {
        [JsonProperty("status")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }
        [JsonProperty("code")]
        public string Message { get; set; }

        public ResApi()
        {
            Code = 0;
            Message = string.Empty;
            Data = null;
        }

        public ResApi Success(int code, object data, string msg)
        {
            Code = code;
            Data = data;
            Message = msg;

            return this;
        }
        public ResApi Error(string msg)
        {
            Code = -500;
            Message = msg;
            Data = null;

            return this;
        }

        public ResApi Error(int code, string msg)
        {
            Code = code;
            Message = msg;
            Data = null;

            return this;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
