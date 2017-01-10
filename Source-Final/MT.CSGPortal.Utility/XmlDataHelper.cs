using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace MT.CSGPortal.Utility
{
    public static class XmlDataHelper
    {
        private static string dataSourcePath = string.Empty;
        private static readonly Dictionary<string, XmlSerializer> dataDict = new Dictionary<string, XmlSerializer>();

        /// <summary>
        /// Method for loading static data
        /// </summary>
        private static void GetXmlDataSourcePath(string locale)
        {
            string baseDir = System.AppDomain.CurrentDomain.BaseDirectory;
            dataSourcePath = string.Format(CultureInfo.InvariantCulture,@"{0}\{1}", baseDir, @"Content\XMLDataSource");
        }

        public static string GetXmlFilePath(string locale, string dataSourceFileName)
        {
            string xmlFileName = string.Empty;
            if (string.IsNullOrEmpty(locale) == false && string.IsNullOrEmpty(dataSourceFileName) == false)
            {
                //Even for different locales, this has to get the right path
                GetXmlDataSourcePath(locale);
                xmlFileName = dataSourcePath + dataSourceFileName;
            }
            return xmlFileName;
        }

        /// <summary>
        /// Gets the data from content xml and returns relevant Content Page type
        /// </summary>
        /// <typeparam name="T">Type of Page (as defined in generated cs files)</typeparam>
        /// <param name="locale">locale</param>
        /// <param name="fileName">XML File name</param>
        /// <returns></returns>
        public static T GetSerializedDataFromFile<T>(string locale, string fileName)
        {
            T returnData = default(T);
            string filename = GetXmlFilePath(locale, fileName);
            if (System.IO.File.Exists(filename))
            {
                using (StreamReader fileData = new StreamReader(filename))
                {
                    XmlSerializer xmlSerializer = CreateXmlSerializer(returnData);
                    if (xmlSerializer != null)
                    {
                        returnData = (T)xmlSerializer.Deserialize(fileData);
                    }
                }
            }

            return returnData;
        }


        public static XmlSerializer CreateXmlSerializer<T>(T returnData)
        {
            Type type = typeof(T);
            XmlSerializer xmlSerializer = null;
            var key = string.Format(
                      CultureInfo.InvariantCulture, "{0}", type);

            if (!dataDict.ContainsKey(key))
            {
                xmlSerializer = new XmlSerializer(typeof(T));
                if (xmlSerializer != null)
                    dataDict.Add(key, xmlSerializer);
            }
            else
                xmlSerializer = dataDict[key];
            return xmlSerializer;
        }

        /// <summary>
        /// Gets the data from content xml and returns relevant Content Page type
        /// </summary>
        /// <typeparam name="T">Type of Page (as defined in generated cs files)</typeparam>
        /// <param name="locale">locale</param>
        /// <param name="fileName">XML File name</param>
        /// <returns></returns>
        public static T GetSerializedDataFromXmlDocument<T>(IXPathNavigable xmlContent)
        {
            T returnData = default(T);
            XmlDocument xmlDocument = xmlContent as XmlDocument;

            if (xmlDocument != null)
            {
                using (XmlNodeReader xmlReader = new XmlNodeReader(xmlDocument))
                {
                    XmlSerializer xmlSerializer = CreateXmlSerializer<T>(returnData);
                    if (xmlSerializer != null)
                    {
                        returnData = (T)xmlSerializer.Deserialize(xmlReader);
                    }
                }
            }

            return returnData;
        }

    }
}
