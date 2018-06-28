using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;

namespace DevBootCampBasicTemplate.Controllers
{
    public class HomeController : Controller
    {
        static HttpClientHandler handler = new HttpClientHandler();
        static HttpClient client = new HttpClient(handler,false);
        

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SlowResponse()
        {
            ViewBag.Message = "Your application description page.";
            //var product = "value";
            try
            {
                using (var client = new HttpClient(handler, false))
                {
                    var response = client.GetAsync("https://jebrook-functionapp2.azurewebsites.net/api/HttpTriggerCSharp3").Result;
                }
                System.Threading.Thread.Sleep(5000);
                ViewBag.Message = "Response from the remote endpoint successful";

                var connStr = $"Server=tcp:devbootcamp.database.windows.net,1433;Initial Catalog=devbootcamp;Persist Security Info=False;User ID=devbootcamp;Password=Pa$$w0rd!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";



                using (SqlConnection connection = new SqlConnection(connStr))
                using (SqlCommand command = new SqlCommand("", connection))
                {
                    connection.Open();
                    ///TEST opening the connection to the web app 
                    command.CommandText = @"SELECT TOP (1000) [ProductID] ,[Name],[ProductNumber]FROM [SalesLT].[Product]";
                    command.ExecuteNonQuery();
                    SqlDataReader reader = command.ExecuteReader();
                }

        }
            catch (Exception e)
            {
                ViewBag.Message = ("Exception : " + e.ToString());
            }

            //  }

            return View();
        }
        private static void ExecuteInForeground()
        {
            throw new Exception("this is second chance exception");
        }

        public ActionResult Exception()    ///crash the application
        {
            {
                var th = new System.Threading.Thread(ExecuteInForeground);
                th.Start();

            }

            return View();
        }

        public ActionResult TLSandSSL()     //TLS errors
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            using (var client = new HttpClient(handler, false))
            {
                try
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                    var response = client.GetAsync("https://www.wellsfargo.com").Result;
                    ViewBag.Message = "Response from the remote endpoint : " + response;
                    client.Dispose();
                }
                catch (Exception e)
                {
                    ViewBag.Message = ("Exception : " + e.ToString());
                    return View();
                }
            }
            return View();
        }

        public ActionResult Http500()
        {
            while (ConfigurationManager.AppSettings["disable"] == null)
            {
                var connStr = $"server";
                if (ConfigurationManager.ConnectionStrings["myConnString"] == null)
                {
                    connStr = $"Server=tcp:xxx.database.windows.net,1433;Initial Catalog=xxxx;Persist Security Info=False;User ID=xxx;Password=xxxx;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=5;";
                }
                else
                {
                    connStr = ConfigurationManager.ConnectionStrings["myConnString"].ConnectionString;
                }

                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    connection.Open();   ///TEST opening the connection to the web app 
                }
            }
            return View();
        }

    }

    internal class SaleModel
    {
        string productID;
        string Name;
        string ProductNumber;

        public SaleModel()
        {
            productID = "null" ;
            Name = "null";
            ProductNumber = "null";
        }
        public SaleModel(string pI, string nm, string PN)
        {
            productID = pI;
            Name = nm;
            ProductNumber = PN;
        }
        public void SetName(string pI, string nm, string PN)
        {
            productID = pI;
            Name = nm;
            ProductNumber = PN;
        }

    }
}