using ClosestAddress.BusinessLogic;
using ClosestAddress.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Xml.Linq;

namespace ClosestAddress.Controllers
{
    /// <summary>
    /// Web API for Geolocation address
    /// </summary>
    public class DistanceCalculatorController : ApiController
    {
        /// <summary>
        /// This WEB API will get the list of closest address with distance in kilometer.
        /// </summary>
        /// <param name="originAddress"></param>
        /// <returns></returns>
        [HttpGet]
        public List<Address> Get(string originAddress)
        {
            List<Address> addressList = new List<Address>();
            try
            {
                using (var balObj = new BAL())
                {
                    //function call for get all the address from either in cache or CSV file. 
                    addressList = balObj.GetAllAddresses();
                    if (addressList.Count > 0)
                    {
                        addressList = addressList.OrderBy(x => x.KM).Take(Convert.ToInt32(Constants.NumberOfAddress != null ? Constants.NumberOfAddress : "4")).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                LogWrite.LogError(ex);
            }
            return addressList;
        }
        /// <summary>
        /// This WEB API will get the list of closest address with distance in kilometer, defined in a config file.
        /// </summary>
        /// <param name="originAddress"></param>
        /// <returns></returns>
        //[HttpGet]
        //public List<Address> Get(string originAddress)
        //{
        //    List<Address> addresses = new List<Address>();
        //    List<Address> addressList = new List<Address>();
        //    try
        //    {
        //        using (var balObj = new BAL())
        //        {
        //            //function call for get all the address from either in cache or CSV file. 
        //            addressList = balObj.GetAllAddresses();
        //        }
        //        foreach (var destinationAddress in addressList)
        //        {
        //            if (!string.IsNullOrEmpty(destinationAddress.Name))
        //            {
        //                //function call for getting the distance in kilometer between two address
        //                string km = GetLocationDistance(originAddress, destinationAddress.Name);
        //                if (!string.IsNullOrWhiteSpace(km))
        //                {
        //                    if (km.Split(' ').Length > 0 && !string.IsNullOrEmpty(km.Split(' ')[0]))
        //                    {
        //                        var item = new Address
        //                        {
        //                            KM = Convert.ToInt32(km.Split(' ')[0]),
        //                            Name = destinationAddress.Name
        //                        };
        //                        addresses.Add(item);
        //                    }
        //                }
        //            }
        //        }
        //        if (addresses.Count > 0)
        //        {
        //            addresses = addresses.OrderBy(x => x.KM).Take(Convert.ToInt32(Constants.NumberOfAddress)).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogWrite.LogError(ex);
        //    }
        //    return addresses;
        //}

        /// <summary>
        /// This method will find out the distance in kilometer between input and destination address via google API.
        /// </summary>
        /// <param name="Origin Address"></param>
        /// <param name="Destination Address"></param>
        /// <returns>Distance in kilometer</returns>
        private string GetLocationDistance(string originAddress, string destinationAddress)
        {
            string distanceInKM = string.Empty;
            try
            {
                var requestUri = string.Format(Constants.GoogleAPIUrl + "?origins={0}&destinations={1}&key={2}",
                Uri.EscapeDataString(originAddress), Uri.EscapeDataString(destinationAddress), Constants.GoogleAPIKey);
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