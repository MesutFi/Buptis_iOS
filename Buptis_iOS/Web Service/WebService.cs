﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Linq;
using System.Net;
using System.Text;
using Buptis_iOS.Database;
using Foundation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UIKit;

namespace Buptis_iOS.Web_Service
{
    class WebService
    {
        string kokurl = "http://185.184.208.157:8080/api/";
        public string ServisIslem(string url, string istekler, bool isLogin = false, string Method = "POST",string ContentType = "application/json")
        {
            Atla:
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(kokurl + url);
                request.Method = Method;
                request.ContentType = ContentType; /*"application/x-www-form-urlencoded";*/
                request.Accept = "*/*";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/73.0.3683.103 Safari/537.36";
                if (!isLogin)
                {
                    request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + GetApiToken());
                }
                byte[] _byteVersion = Encoding.UTF8.GetBytes(string.Concat(istekler));
                request.ContentLength = _byteVersion.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(_byteVersion, 0, _byteVersion.Length);
                stream.Close();

                // JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));

                //string  aa =  Ayristir(jsonDoc);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string aas = reader.ReadToEnd();
                    try
                    {
                        var bob = Newtonsoft.Json.Linq.JObject.Parse(aas);
                        var json_string = bob.ToString();
                        return json_string;
                    }
                    catch
                    {
                        if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent)
                        {
                            return "";
                        }
                        else
                        {
                            return "Hata";
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                string aaa = ex.Message;
                if (url == "authenticate")
                {
                    return "Hata";
                }
                else
                {
                    if (ex.Response != null)
                    {
                        if (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Unauthorized)
                        {
                            SetApiToken();
                            goto Atla;
                        }
                        var Mes = ex.Message.ToString();
                        return "Hata";
                    }
                    else
                    {
                        return "Hata";
                    }
                    
                }

            }

        }
        public JsonValue OkuGetir(string url, bool DontUseHostURL = false)
        {
            Atla:
            try
            {
                HttpWebRequest request;
                if (DontUseHostURL)
                {
                    request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
                }
                else
                {
                    request = (HttpWebRequest)HttpWebRequest.Create(new Uri(kokurl + url));
                }

                request.ContentType = "application/json";
                request.Method = "GET";
                request.Accept = "*/*";
                request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + GetApiToken());

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                    using (Stream stream = response.GetResponseStream())
                    {
                        JsonValue jsonDoc = JsonObject.Load(stream);
                        // Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());

                        return jsonDoc;
                    }
                }
            }
            catch (WebException Ex)
            {
                string aa = Ex.Message.ToString();
                if (Ex.Response != null)
                {
                    if (((HttpWebResponse)Ex.Response).StatusCode == HttpStatusCode.Unauthorized)
                    {
                        SetApiToken();
                        goto Atla;
                    }
                }
                else
                {
                    return null;
                }
                return null;
            }
        }

        string GetApiToken()
        {
            if (!string.IsNullOrEmpty(APITOKEN.TOKEN))
            {
                return APITOKEN.TOKEN;
            }
            else
            {
                var a = DataBase.MEMBER_DATA_GETIR();
                if (a.Count > 0)
                {
                    APITOKEN.TOKEN = a[0].API_TOKEN;
                    return a[0].API_TOKEN;
                }
                else
                {
                    return "";
                }
            }
        }
        void SetApiToken()
        {
            var MemberInfo = DataBase.MEMBER_DATA_GETIR()[0];
            LoginRoot loginRoot = new LoginRoot()
            {
                password = MemberInfo.password,
                rememberMe = true,
                username = MemberInfo.email
            };
            string jsonString = JsonConvert.SerializeObject(loginRoot);
            WebService webService = new WebService();
            var Donus = webService.ServisIslem("authenticate", jsonString);
            if (Donus != "Hata")
            {
                JObject obj = JObject.Parse(Donus);
                string Token = (string)obj["id_token"];
                if (Token != null && Token != "")
                {
                    APITOKEN.TOKEN = Token;
                    MemberInfo.API_TOKEN = Token;
                    APITOKEN.TOKEN = Token;
                }
            }
        }
    }

    public static class APITOKEN
    {
        public static string TOKEN { get; set; }
    }
    public static class CDN
    {
        public static string CDN_Path { get; set; } = "http://590333323.origin.radorecdn.net/";
    }
}