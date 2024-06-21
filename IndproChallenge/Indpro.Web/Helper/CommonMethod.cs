﻿using Indpro.Web.Models;
using Newtonsoft.Json;
using System.Net;

namespace Indpro.Web.Helper;
public class CommonMethod
{
    /// <summary>
    /// Description: To handle API response 
    /// </summary>
    /// <param name="responseMessage"></param>    
    public static async Task<APIResponseResult<T>> HandleApiResponseAsync<T>(
        HttpResponseMessage responseMessage)
    {
        var result = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
        var response = JsonConvert.DeserializeObject<APIResponseResult<T>>(result);
        return response;
    }
}
