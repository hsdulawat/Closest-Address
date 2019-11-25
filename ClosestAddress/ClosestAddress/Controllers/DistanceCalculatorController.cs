using ClosestAddress.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Xml.Linq;

namespace ClosestAddress.Controllers
{
    public class DistanceCalculatorController : ApiController
    {
        private static string GoogleAPIKey = WebConfigurationManager.AppSettings["GoogleAPIKey"];

        [HttpGet]
        public List<Address> Get(string originAddress)
        {
            List<Address> addresses = new List<Address>();
            try
            {
                var addressList = GetAllAddresses();
                foreach (var destinationAddress in addressList)
                {
                    if (!string.IsNullOrEmpty(destinationAddress))
                    {
                        string km = GetLocation(originAddress, destinationAddress);
                        if (!string.IsNullOrWhiteSpace(km))
                        {
                            var list = km.Split(' ');
                            if (list.Length > 0 && !string.IsNullOrEmpty(list[0]))
                            {
                                var item = new Address
                                {
                                    KM = list[0],
                                    Name = destinationAddress
                                };
                                addresses.Add(item);
                            }
                        }
                    }
                }
                if (addresses.Count > 0)
                {
                    addresses = addresses.OrderBy(x => x.KM).Take(5).ToList();
                }
            }
            catch (Exception ex)
            {
                LogWrite.LogError(ex);
            }
            return addresses;
        }
        public IEnumerable<string> GetAllAddresses()
        {
            List<string> addressList = new List<string>();
            try
            {
                string csvPath = HttpContext.Current.Server.MapPath("~/CSV/Address_List_Australia.txt");
                if (!string.IsNullOrWhiteSpace((csvPath)))
                {
                    using (var reader = new StreamReader(csvPath))
                    {

                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            addressList.Add(line);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogWrite.LogError(ex);
            }
            return addressList;
        }
        public static string GetLocation(string originAddress, string destinationAddress)
        {
            string distanceInKM = string.Empty;
            try
            {
                var requestUri = string.Format("https://maps.googleapis.com/maps/api/distancematrix/xml?origins={0}&destinations={1}&key={2}",
                Uri.EscapeDataString(originAddress), Uri.EscapeDataString(destinationAddress), GoogleAPIKey);
                var request = WebRequest.Create(requestUri);
                var response = request.GetResponse();
                if (response != null)
                {
                    var xdoc = XDocument.Load(response.GetResponseStream());
                    var result = xdoc.Element("DistanceMatrixResponse").Element("row");
                    if (result != null)
                    {
                        var locationElement = result.Element("element").Element("distance");
                        if (locationElement != null)
                        {
                            var distance = locationElement.Element("text").Value;
                            if (!string.IsNullOrEmpty(distance))
                            {
                                distanceInKM = distance;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogWrite.LogError(ex);
            }
            return distanceInKM;
        }
        
    }
}