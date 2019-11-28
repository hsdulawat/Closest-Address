using ClosestAddress.Helper;
using ClosestAddress.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace ClosestAddress.BusinessLogic
{
    public class BAL : DisposeBaseClass
    {
        /// <summary>
        /// This method used to retrieve the address list from CSV file.
        /// </summary>
        /// <returns>Address List</returns>
        internal List<Address> GetAllAddresses()
        {
            List<Address> addressList = new List<Address>();
            string cacheKeyName = "CSVAddressList";
            string cacheDurantion = Constants.CacheTimeDuration;
            // Get from Cache
            try
            {
                if (CacheHelper.Exists(cacheKeyName))
                {
                    CacheHelper.Get(cacheKeyName, out addressList);
                    if (addressList == null)
                    {
                        CacheHelper.Clear(cacheKeyName);
                    }
                }
                if (!CacheHelper.Exists(cacheKeyName))
                {
                    string csvPath = string.Empty;
                    if (HttpContext.Current != null)
                    {
                        csvPath = HttpContext.Current.Server.MapPath(Constants.FileLocation);
                    }
                    else
                    {
                        csvPath = "D:\\Projects\\IsobarAssignment\\ClosestAddress\\ClosestAddress\\CSV\\Address_List_Australia.txt";
                    }
                    if (!string.IsNullOrWhiteSpace((csvPath)))
                    {
                        using (StreamReader reader = new StreamReader(csvPath))
                        {
                            while (!reader.EndOfStream)
                            {
                                string line = reader.ReadLine();
                                if (!string.IsNullOrWhiteSpace(line.Trim().Substring(line.LastIndexOf(' '))))
                                {
                                    addressList.Add(new Address
                                    {
                                        Name = line.Trim().Substring(0, line.LastIndexOf(' ')).Trim(),
                                        KM = Convert.ToInt32(line.Trim().Substring(line.LastIndexOf(' ')).Trim())
                                    });
                                }
                            }
                        }
                    }
                    if (addressList != null && addressList.Count > 0)
                    {
                        CacheHelper.Add(addressList, cacheKeyName, Convert.ToDouble(cacheDurantion));
                    }
                }
            }
            catch (Exception ex)
            {
                LogWrite.LogError(ex);
            }
            return addressList;
        }
        // Flag: Has Dispose already been called
        bool disposed = false;
        // Protected implementation of Dispose pattern.
        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (disposing)
            {
                // Free any other managed objects here.
            }
            // Free any unmanaged objects here.
            disposed = true;
            // Call the base class implementation.
            base.Dispose(disposing);
        }
        ~BAL()
        {
            Dispose(false);
        }
    }
}