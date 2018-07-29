using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Brobet.Services
{
    public class PushNotificationService
    {
        public static void SendNotification(string heading, string content, int? userId = null)
        {
            var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;

            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";

            request.Headers.Add("authorization", "Basic MDBhNDgzMDAtYTkwOS00YjJjLWI1YmQtMWQ4YTVhZTliYWVh");

            var serializer = new JavaScriptSerializer();
            var allObj = new
            {
                app_id = "5980c12d-fa68-4fc9-9c60-5b8f29ef2f1a",
                headings = new { en = heading },
                contents = new { en = content },
                included_segments = new string[] { "All" }

                //filters = new object[] { new { field = "tag", key = "level", value = "10" }, new { @operator = "OR" }, new { field = "amount_spent", relation = ">", value = "0" } }
            };

            var param = serializer.Serialize(allObj);
            if(userId.HasValue)
            {
                var obj = new
                {
                    app_id = "5980c12d-fa68-4fc9-9c60-5b8f29ef2f1a",
                    headings = new { en = heading },
                    contents = new { en = content },
                    filters = new object[] { new { field = "tag", key = "userId", value = userId.Value } }
                };
                param = serializer.Serialize(obj);
            }
            byte[] byteArray = Encoding.UTF8.GetBytes(param);

            string responseContent = null;

            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
            }

            System.Diagnostics.Debug.WriteLine(responseContent);
        }
    }
}