using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ToDoMvc
{
    public static class GlobalVariables
    {
        public static HttpClient WebApiClient = new HttpClient();

        static GlobalVariables()
        {
          //  WebApiClient.BaseAddress = new Uri("https://localhost:5001/api/");
            WebApiClient.BaseAddress = new Uri("https://localhost:44367/api/");
        
          WebApiClient.DefaultRequestHeaders.Clear();
          WebApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
