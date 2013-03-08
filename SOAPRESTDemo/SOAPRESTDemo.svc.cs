using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

public class SOAPRESTDemo : ISOAPRESTDemo
{

    public string EchoWithGet(string s)
    {
        return "ECHO with GET : You said " + s;

        
    }

    public string EchoWithPost(string s)
    {
        return "ECHO with POST : You said " + s;
    }


}

