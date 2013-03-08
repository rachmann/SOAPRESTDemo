using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;


// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISOAPRestDemo" in both code and config file together.
[ServiceContract]
public interface ISOAPRESTDemo
{

    [OperationContract]
    [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "EchoWithGet/{message}", BodyStyle = WebMessageBodyStyle.Bare)]
    string EchoWithGet(string message);

    [OperationContract]
    [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "EchoWithPost", ResponseFormat = WebMessageFormat.Xml)]
    string EchoWithPost(string message);


}

