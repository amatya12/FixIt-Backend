using FixIt_Data.Context;
using FixIt_Service.CustomExceptionHelper;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace FixIt_Service
{
    public class LoginServiceLibrary
    {
        private readonly DataContext context;

        public LoginServiceLibrary(DataContext context)
        {
            this.context = context;
        }

        public static JObject FulFillRequest(WebRequest requestObject)
        {
            HttpWebResponse responseObject;
            try
            {
                responseObject = (HttpWebResponse)requestObject.GetResponse();
            }
            catch
            {
                throw new EmailNotFoundException();
            }

            string rawJson = null;
            using (Stream stream = responseObject.GetResponseStream())
            {
                StreamReader sr = new StreamReader(stream);

                rawJson = sr.ReadToEnd();
                sr.Close();
            }

            return JObject.Parse(rawJson);
        }

        #region CallGoogleApi
        public static String GettingUserProfileDataFromGoogleApi(string accessToken)
        {
            string googleApiUrl = String.Format("https://www.googleapis.com/userinfo/v2/me");
            WebRequest requestObject = WebRequest.Create(googleApiUrl);

            requestObject.Method = "GET";
            String authorizationValue = "Bearer " + accessToken;
            requestObject.Headers.Add("Authorization", authorizationValue);

            requestObject.ContentType = "application/json";

            var jsonValue = FulFillRequest(requestObject);

            string email = jsonValue["email"].ToObject<string>();


            return email;
        }
        #endregion

    }
}

