﻿using System.Net.Http;
using System.Threading.Tasks;

namespace ToDoMvc.Models.Helpers
{
    public interface IApiHelper
    {
        HttpClient ApiClient { get; }

        Task<AuthenticatedUser> Authenticate(string username, string password);
    }
}