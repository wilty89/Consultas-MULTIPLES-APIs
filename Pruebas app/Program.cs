using Newtonsoft.Json;
using RestSharp;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace Consulta_De_Dispocitivo
{
    public class Consulta
    {
        public string esn { get; set; }
        public string imei { get; set; }
        public string configGroup { get; set; }
        public string configStatus { get; set; }
        public string lastIdReportTime { get; set; }
        public bool unitStatusBit4 { get; set; }
        public IdReport idReport { get; set; }
        public int scriptVersion { get; set; }
        public int configVersion { get; set; }
        public string iccid { get; set; }
        public string customerName { get; set; }
    }
    public class IdReport
    {
        public bool gpsantennaStatusOK { get; set; }
        public bool gpsreceiverTestOK { get; set; }
        public bool gpstrackingOK { get; set; }
    }
    public class ConsultaT
    {
        public string company_name { get; set; }
        public string imei { get; set; }
        public string current_configuration { get; set; }
        public string seen_at { get; set; }
        public string model { get; set; }
        public string country { get; set; }
        public string last_sync { get; set; }
        public string status_name { get; set; }
        public dynamic company { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Consulta de Dispositivos CALAMP o TELTONIKA";
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();
            ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
            Console.ForegroundColor = ConsoleColor.DarkBlue;

            Console.WriteLine("Digite el CP a consultar\r");
            
            Console.WriteLine("Ejemplos:1333121788 ,357544377320037, 868003033473369, 4634510168 \r");
            Console.WriteLine("\n");
            Console.SetWindowSize(70, 30);
            //Console.ReadKey(true);
            int y = 10;
            int z = 15;
            while (true)
            {
                try
                {
                string cp = "";
                cp = Console.ReadLine();
                    
                Console.WriteLine("\n");

                if (cp.Length == y && cp.StartsWith("133") || cp.StartsWith("463") || cp.StartsWith("486") || cp.StartsWith("487") || cp.StartsWith("213") || cp.StartsWith("1831") || cp.StartsWith("1632"))
                {
                    string apikey = "9G5XWBo_E-KGNkHgBV9V6PDj3KYDjNMXdLTS784JtX6FJR_3sojcBBl4eJFNlrzS";
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://puls.calamp.com/service/device/" + cp + "?apikey=" + apikey);
                    request.Proxy = null;
                    request.Credentials = CredentialCache.DefaultCredentials;
                    request.ContentLength = 0;

                    request.ContentType = "application/json";
                    request.MediaType = "application/json";
                    byte[] authBytes = Encoding.UTF8.GetBytes("hdo_helpdesk:Hunter2019@".ToCharArray());

                    request.Headers.Add(HttpRequestHeader.Authorization, String.Format("Basic {0}", Convert.ToBase64String(authBytes)));
                    string ua = "User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; rv:8.0) Gecko/20100101 Firefox/8.0";

                    request.UserAgent = ua;
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    string responseFromServer = reader.ReadToEnd();
                    ///Console.WriteLine(responseFromServer);
                    Consulta consulta;
                    consulta = JsonConvert.DeserializeObject<Consulta>(responseFromServer);

                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("DATOS PULS O FOTA");
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        //Console.WriteLine("\n");
                        string conf = $"{consulta.scriptVersion}.{consulta.configVersion}".ToString();

                        Console.WriteLine($"Empresa:   " + consulta.customerName);
                        Console.WriteLine($"Numero de CP:   " + consulta.esn);
                        Console.WriteLine($"Numero de IMEI: " + consulta.imei);
                        Console.WriteLine($"Tarjeta SIM: " + consulta.iccid);
                        Console.WriteLine("Configuracion:   " + consulta.scriptVersion +"."+ consulta.configVersion + " " + consulta.configGroup);
                        Console.WriteLine("Estado de configuracion: " + consulta.configStatus);
                        Console.WriteLine("Ultima Vez que reporto:  " + consulta.lastIdReportTime);
                        string desi;
                        desi = (conf == "9.21") || (conf == "9.17") || (conf == "9.26") || (conf == "9.78") || (conf == "9.69") || (conf == "9.7") || (conf == "9.121") || (conf == "9.122") || (conf == "32.73") || (conf == "32.75") || (conf == "9.13") ? "HunterTrack_TOBO" 
                            : (conf == "9.1") || (conf == "9.22") || (conf == "9.96") || (conf == "9.97") || (conf == "9.98") || (conf == "9.99") || (conf == "30.0") || (conf == "32.90") || (conf == "32.91") || (conf == "32.254") || (conf == "32.77") || (conf == "32.210") ? "Global_AVL" 
                            : (conf == "9.30") || (conf == "9.33") || (conf == "32.72") || (conf == "32.74") || (conf == "32.200") || (conf == "32.98") || (conf == "9.120") || (conf == "9.119") || (conf == "9.118") || (conf == "9.95") || (conf == "32.206") ? "GURTAM_WIALON" : "N/A";
                        
                        
                        Console.WriteLine("Plataforma: " + desi);
                        

                        Console.WriteLine("--------------------Conexion----------------");
                        Console.WriteLine("Problema con Antena GPS: " + consulta.idReport.gpsantennaStatusOK);
                        Console.WriteLine("GpsreceiverTestOK:   " + consulta.idReport.gpsreceiverTestOK);
                        //Console.WriteLine("Problema con Señal GPS:  " + consulta.idReport.gpstrackingOK);
                        
                        if (consulta.unitStatusBit4)
                    {
                        Console.WriteLine("Senal GPS: Con Problema (No GPS)");
                    }
                    else
                    {
                        Console.WriteLine("Estado de Senal: Correcta");
                    }
                    //Console.WriteLine("\n");
                    
                }
                else if (cp.Length == z && cp.StartsWith("3540") || cp.StartsWith("3526") || cp.StartsWith("3575") || cp.StartsWith("3596"))
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.teltonika.lt/devices/" + cp);
                    request.Proxy = null;
                    request.Credentials = CredentialCache.DefaultCredentials;
                    request.ContentLength = 0;
                    request.ContentType = "application/json";
                    request.MediaType = "application/json";
                    request.Headers["Authorization"] = "Bearer 620|fJg81cvyuQUvsPp5txRd7Od2Zqluq13v114yPjNx";
                    string ua = "User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; rv:8.0) Gecko/20100101 Firefox/8.0";
                    request.UserAgent = ua;
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    string responseFromServer = reader.ReadToEnd();
                    //Console.WriteLine(responseFromServer);
                    ConsultaT consulta;
                    consulta = JsonConvert.DeserializeObject<ConsultaT>(responseFromServer);

                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("DATOS PULS O FOTA");
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.WriteLine("\n");

                    Console.WriteLine("Compania :" + consulta.company_name);
                    Console.WriteLine("Dispositivo :" + consulta.imei);
                    Console.WriteLine("Configuracion :" + consulta.current_configuration);
                    Console.WriteLine("Ultimo Reporte :" + consulta.seen_at);
                    Console.WriteLine("Modelo :" + consulta.model);
                    Console.WriteLine("Ultimo estado visto :" + consulta.status_name);
                    Console.WriteLine("Ultima Actulizacion :" + consulta.last_sync);
                        
                        //Console.WriteLine("\n");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\n");
                    Console.WriteLine("Dispositivo No encontrado [PULS, FOTA]");
                    
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("\n");
                    Console.Write("Coloque otro Dispositivo a consultar: ");
                    //Console.ReadKey(true);
                    Console.WriteLine("\n");
                }
                if ((cp.Length == z && cp.StartsWith("3540")) || (cp.Length == z && cp.StartsWith("3526"))||(cp.StartsWith("463") && cp.Length == y) || (cp.Length == y && cp.StartsWith("1831")) || (cp.StartsWith("133") && cp.Length == y)   || (cp.StartsWith("8621") && cp.Length == z) || (cp.StartsWith("8649") && cp.Length == z) || (cp.StartsWith("35951") && cp.Length == z) || (cp.StartsWith("86800") && cp.Length == z) || (cp.StartsWith("8638") && cp.Length == z) || (cp.StartsWith("8687") && cp.Length == z) || (cp.StartsWith("8628") && cp.Length == z) || (cp.StartsWith("3592") && cp.Length == z) || (cp.StartsWith("357") && cp.Length == z) || (cp.StartsWith("8625") && cp.Length == z))
                    {
                        var client = new RestClient("https://huntertrack.com.do:443/huntertrack/portal/ws_tobo/ws_up_cemex.php?wsdl");
                        client.Timeout = -1;
                        var request1 = new RestRequest(Method.POST);
                        request1.AddHeader("Content-Type", "text/xml;charset=\"iso-8859-1\"");

                        var body = @"<soapenv:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:uri=""uri:ToboWsCemex"">
" + "\n" +
                        @"   <soapenv:Header/>
" + "\n" +
                        @"   <soapenv:Body>
" + "\n" +
                        @"      <uri:getUpWsCemex soapenv:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
" + "\n" +
                        @"         <Datos xsi:type=""ws:ToboWsCemex"" xmlns:ws=""urn:/huntertrack/portal/ws_tobo/ws_up_cemex.php"">
" + "\n" +
                        @"            <!--You may enter the following 3 items in any order-->
" + "\n" +
                        @"            <user xsi:type=""xsd:string"">MIGUELPRUEBA</user>
" + "\n" +
                        @"            <passwd xsi:type=""xsd:string"">MIG5432</passwd>
" + "\n" +
                        @$"            <placa xsi:type=""xsd:string"">{cp}</placa >
" + "\n" +
                        @"         </Datos>
" + "\n" +
                        @"      </uri:getUpWsCemex>
" + "\n" +
                        @"   </soapenv:Body>
" + "\n" +
                        @"</soapenv:Envelope>";
                        request1.AddParameter("text/xml", body, ParameterType.RequestBody);
                        IRestResponse response1 = client.Execute(request1);
                        XmlDocument doc1 = new XmlDocument();
                        doc1.LoadXml(Convert.ToString(response1.Content));
                        XmlNode root = doc1.DocumentElement.FirstChild;
                        var viene = root.InnerText.Remove(0, 2);

                        if (viene != "No se encontraron registros.")
                        {
                            dynamic json = JsonConvert.DeserializeObject(viene);
                            foreach (var e in json)
                            {
                                Console.WriteLine("\n");
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                Console.WriteLine("DATOS BANCO DE PRUEBA");
                                Console.ForegroundColor = ConsoleColor.DarkBlue;
                                Console.WriteLine("\n");                            
                                Console.WriteLine("Fecha_evento:" + e.fecha_evento);
                                Console.WriteLine("Nombre Objeto:" + e.placa);
                                Console.WriteLine("Latitud:" + e.latitud);
                                Console.WriteLine("Longitud:" + e.longitud);
                                Console.WriteLine("Velocidad:" + e.velocidad);
                                Console.WriteLine("\n");
                                Console.Write("Coloque otro No. Dispositivo: ... ");
                                Console.WriteLine("\n");
                            }
                        }
                        else if (viene == "No se encontraron registros.")
                        {
                            Console.WriteLine("\n");
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("Dispositivo no encontrado en Banco de Prueba.");
                            Console.WriteLine("\n");
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.Write("Coloque otro No. Dispositivo: ... ");
                            Console.WriteLine("\n");
                        }
                        /*else if (viene == "ERROR 1")
                        {
                            Console.WriteLine("\n");
                            Console.WriteLine("Posible API fuera de servicio.");
                            Console.Write("Coloque otro No. Dispositivo: ... ");
                            Console.WriteLine("\n");
                        }*/
                        else
                        {
                            Console.WriteLine("\n");
                            Console.WriteLine("Ha ocurrido un Problema.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Ha ocurrido un Problema. Intente nuevamente");
                    //Console.WriteLine("{0} Exception caught.", ex);
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("\n");
                    Console.Write("Coloque otro Dispositivo a consultar: ");
                    Console.WriteLine("\n");
                }                
            }
        }
    }
}
