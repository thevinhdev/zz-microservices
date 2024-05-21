using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using static IOIT.Shared.Commons.Constants.ApiConstants;

namespace IOIT.Shared.ViewModels
{
    public class DefaultResponse
    {
        [JsonProperty("meta")]
        public Meta Meta { get; set; }
        [JsonProperty("data")]
        public object Data { get; set; }
        [JsonProperty("metadata")]
        public object Metadata { get; set; }

        public DefaultResponse() {
            Meta = new Meta();
            Data = null;
            Metadata = null;
        }

        public DefaultResponse Success(object data)
        {
            this.Data = data;
            this.Metadata = null;
            this.Meta.Error_code = StatusCode.Success200;
            this.Meta.Error_message = MessageResource.ACCTION_SUCCESS;

            return this;
        }

        public DefaultResponse Success(object data, int code)
        {
            this.Data = data;
            this.Metadata = null;
            this.Meta.Error_code = code;
            this.Meta.Error_message = MessageResource.ACCTION_SUCCESS;

            return this;
        }

        public DefaultResponse ToPage(object data, int countResult)
        {
            this.Data = data;
            this.Metadata = countResult;
            this.Meta.Error_code = StatusCode.Success200;
            this.Meta.Error_message = MessageResource.ACCTION_SUCCESS;
            this.Metadata = countResult;

            return this;
        }

        public DefaultResponse Success(object data, string msg)
        {
            this.Data = data;
            this.Metadata = null;
            this.Meta.Error_code = StatusCode.Success200;
            this.Meta.Error_message = msg;

            return this;
        }

        public DefaultResponse Success(object data, string msg, int code)
        {
            this.Data = data;
            this.Metadata = null;
            this.Meta.Error_code = code;
            this.Meta.Error_message = msg;

            return this;
        }

        public DefaultResponse Success(object data, int metadata, string msg, int code)
        {
            this.Data = data;
            this.Metadata = metadata;
            this.Meta.Error_code = code;
            this.Meta.Error_message = msg;

            return this;
        }

        public DefaultResponse Success(object data, string msg, int code, int count)
        {
            this.Data = data;
            this.Metadata = null;
            this.Meta.Error_code = code;
            this.Meta.Error_message = msg;
            this.Metadata = count;

            return this;
        }

        public DefaultResponse Error(string msg, int code)
        {
            this.Data = null;
            this.Metadata = null;
            this.Meta.Error_code = code;
            this.Meta.Error_message = msg;

            return this;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class Output
    {
        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        [JsonProperty("metadata")]
        public int? Metadata { get; set; }
    }

    public class Output<T> : Output
    {
        [JsonProperty("data")]
        public T Data { get; set; }
    }

    public class Meta
    {
        [JsonProperty("error_code")]
        public int Error_code { get; set; }
        [JsonProperty("error_message")]
        public string Error_message { get; set; }

        public Meta()
        {
            Error_code = 200;
            Error_message = string.Empty;
        }

        public Meta(int errorCode, string errorMessage)
        {
            this.Error_code = errorCode;
            this.Error_message = errorMessage;
        }
    }

    public class Metadata
    {
        public int item_count { get; set; }
        public decimal total { get; set; }
        public Metadata()
        { }

        public Metadata(int item_count)
        {
            this.item_count = item_count;
        }
        public Metadata(decimal total)
        {
            this.total = total;
        }

        public Metadata(int item_count, decimal total)
        {
            this.item_count = item_count;
            this.total = total;
        }
    }
}
