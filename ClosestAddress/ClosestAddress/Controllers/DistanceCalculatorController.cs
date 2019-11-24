using ClosestAddress.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Http;
using System.Xml.Linq;
namespace ClosestAddress.Controllers
{
    public class DistanceCalculatorController : ApiController
    {
        private const string Key = "AIzaSyBHLvlufM3Oy0neJhLs0dE6DSPUEHq6GLU";
        public IEnumerable<string> GetAllAddresses()
        {
            using (var reader = new StreamReader(@"D:\Projects\IsobarAssignment\ClosestAddress\ClosestAddress\CSV\address list australia.txt"))
            {
                List<string> addressList = new List<string>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    addressList.Add(line);
                }
                return addressList;
            }
        }
        public static string GetLocation(string originAddress, string destinationAddress)
        {
            string distanceInKM = string.Empty;
            var requestUri = string.Format("https://maps.googleapis.com/maps/api/distancematrix/xml?origins={0}&destinations={1}&key={2}",
                Uri.EscapeDataString(originAddress), Uri.EscapeDataString(destinationAddress), Key);
            var request = WebRequest.Create(requestUri);
            var response = request.GetResponse();
            if (response != null)
            {
                var xdoc = XDocument.Load(response.GetResponseStream());
                var result = xdoc.Element("DistanceMatrixResponse").Element("row");
                if (result != null)
                {
                    var locationElement = result.Element("distance");
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
            return distanceInKM;
        }
        //[HttpGet]
        //public List<Address> Get(string originAddress)
        //{
        //    List<Address> addresses = new List<Address>();
        //    var addressList = GetAllAddresses();
        //    foreach (var destinationAddress in addressList)
        //    {
        //        if (!string.IsNullOrEmpty(destinationAddress))
        //        {
        //            string km = GetLocation(originAddress, destinationAddress);
        //            if (string.IsNullOrEmpty(km))
        //            {
        //                var list = km.Split(' ');
        //                if (list.Length > 0 && !string.IsNullOrEmpty(list[0]))
        //                {
        //                    var item = new Address();
        //                    item.KM = list[0];
        //                    item.Name = destinationAddress;
        //                    addresses.Add(item);
        //                }
        //            }
        //        }
        //    }
        //    if (addresses.Count > 0)
        //    {
        //        addresses = addresses.OrderBy(x => x.KM).Take(5).ToList();
        //    }
        //    return addresses;
        //}
        [HttpGet]
        public List<Address> Get(string originAddress)
        {
            List<Address> addresses = new List<Address>();
            var addressList = GetAllAddresses();
            foreach (var destinationAddress in addressList)
            {
                var item = new Address();
                item.Name = destinationAddress;
                addresses.Add(item);
            }
            return addresses;
        }
    }
}