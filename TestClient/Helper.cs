using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Data.Sql;
using System.Data;
using System.Data.SqlClient;

namespace TestClient
{
    public static class Helper
    {
        public static void TrySetSingleNode(string xmlFilePath, string xpathArg, string attributeArg, string valueArg)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlFilePath);
                doc.SelectSingleNode(xpathArg).Attributes[attributeArg].Value = valueArg;
                doc.Save(xmlFilePath);
            }
            catch { }
        }

        public static string GetCurrentExecutingFolder()
        {
            Assembly a = Assembly.GetCallingAssembly();
            FileInfo thisAssembly = new FileInfo(a.Location);
            return thisAssembly.Directory.FullName;
        }
    }
}
