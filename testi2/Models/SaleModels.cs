using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testi2.Models
{
    public class SaleModels
    {
        public class JsonCLass
        {
            public string salePerson { get; set; }
            public string clientPerson { get; set; }

            [JsonProperty("sale")]
            public string UserName { get; set; }

            public detail Userinfo { get; set; }

            public detail[] detail { get; set; }
        }


        public class detail
        {
            public string productId { get; set; }
            public string productDescription { get; set; }
            public string productQuantity { get; set; }
            public string productSubtotal { get; set; }
        }
    }
}