using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading;
using System.Threading.Tasks;
using ControlID.iDAccess;
using ControlID;

namespace UnitTestAcesso
{
    public partial class BaseTest
    {
        public static string URL = "http://192.168.2.121/";
        public static string Login = "admin";
        public static string Password = "admin";
    }
}