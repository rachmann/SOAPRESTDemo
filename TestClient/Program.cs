using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Net;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using System.Reflection;

namespace TestClient
{
    class Program
    {
        public static string echoGet;
        public static string echoPost;
        public static string urlOfService;
        public static string message;
        public static string serviceUrlSOAP;
        public static string serviceUrlREST;

        static void Main(string[] args)
        {
            try
            {

                urlOfService = args[0];
                message = args[1];
                string switchArg = args[2];
                serviceUrlSOAP = Path.Combine(urlOfService,"soap");
                serviceUrlREST = Path.Combine(urlOfService,"rest");

                string cfgFilePath = Path.Combine(Helper.GetCurrentExecutingFolder(), "TestClient.exe.config");
                Helper.TrySetSingleNode(cfgFilePath, "//configuration/system.serviceModel/client/endpoint", "address", serviceUrlSOAP);

                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("***************************");

                switch (switchArg)
                {
                        

                    case "1":
                        Console.WriteLine("About to call REST web service interface : TestRESTUsingWebChannelFactory()");
                        TestRESTUsingWebChannelFactory();
                        break;
                    case "2":
                        Console.WriteLine("About to call SOAP web service interface TestSOAPUsingChannelFactory()");
                        TestSOAPUsingChannelFactory();
                        break;
                    case "3":
                        Console.WriteLine("About to call SOAP web service interface TestSOAPUsingReference()");
                        TestSOAPUsingReference();
                        break;
                    case "4":
                        Console.WriteLine("About to call REST web service interface TestRESTByManualGET()");
                        TestRESTByManualGET();
                        break;
                    case "5":
                        Console.WriteLine("About to call REST web service interface TestRESTByManualPOST()");
                        TestRESTByManualPOST();
                        break;
                    default:
                        break;
                    
                }

                Console.WriteLine("Successfull!");
                Console.WriteLine("***************************");
                Console.WriteLine(Environment.NewLine);

                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine("There was an error" + ex.ToString());
                Console.ReadLine();
            }


        }

        public static void TestRESTUsingWebChannelFactory()
        {
            //1) consuming the REST based service
            WebChannelFactory<ISOAPRESTDemo> cf = new WebChannelFactory<ISOAPRESTDemo>(new Uri(serviceUrlREST));
            ISOAPRESTDemo client = cf.CreateChannel();
            PrintSuccessMessage(client.EchoWithGet(message), client.EchoWithPost(message));
        }

        public static void TestSOAPUsingChannelFactory()
        {
            //2) consuming SOAP with channel factory
            using (ChannelFactory<ISOAPRESTDemo> scf = new ChannelFactory<ISOAPRESTDemo>(new BasicHttpBinding(), serviceUrlSOAP))
            {
                ISOAPRESTDemo channel = scf.CreateChannel();
                PrintSuccessMessage(channel.EchoWithGet(message), channel.EchoWithPost(message));
            }
        }

        public static void TestSOAPUsingReference()
        {
            //3) consuming SOAP service with reference [the old way]
            SoapService.SOAPRESTDemoClient cs = new SoapService.SOAPRESTDemoClient("BasicHttpBinding_ISOAPRESTDemo");
            PrintSuccessMessage(cs.EchoWithGet(message), cs.EchoWithPost(message));
        }

        public static void TestRESTByManualGET()
        {
            //4) consuming the REST based service by constructing manual request
            WebRequest wrGETURL = WebRequest.Create(serviceUrlREST + "EchoWithGet/" + message);

            Stream objStream;
            objStream = wrGETURL.GetResponse().GetResponseStream();

            StreamReader objReader = new StreamReader(objStream);

            string sLine = "";
            int i = 0;

            Console.WriteLine("Successfull! - printing response");

            while (sLine != null)
            {
                i++;
                sLine = objReader.ReadLine();
                if (sLine != null)
                    Console.WriteLine("{0}:{1}", i, sLine);
            }
        }

        public static void TestRESTByManualPOST()
        {

            //construct the POST data
            string postData = "<EchoWithPost xmlns=\"http://tempuri.org/\"><message>" + message + "</message></EchoWithPost>";

            //create a byte array for the POST 'request' data to go into
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            //create a webrequest object (what .net uses to create a http method programatically)
            WebRequest request = WebRequest.Create(serviceUrlREST + "EchoWithPOST");

            //this type of request is POST
            request.Method = "POST";

            //the content type for the particular transaction
            request.ContentType = "application/xml;";

            //the lengh of the POST request
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();

            //this will write the request stream into a datastream object, this must be done before you can call request.Response();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse webResponse = request.GetResponse();

            // Output raw string result
            string rawStringResult = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();

            Console.WriteLine("Successfull! - printing response");
            Console.WriteLine(rawStringResult);

        }

        private static void PrintSuccessMessage(string echoGet, string echoPost)
        {
            Console.WriteLine(echoGet);
            Console.WriteLine(echoPost);
        }


    }
}
