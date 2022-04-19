using System;
using System.Reflection;

namespace Raditz
{
    internal class Program
    {
        static void Reflect(string FilePath)
        {
            Assembly dotnetProgram = Assembly.LoadFrom(FilePath);
            Object[] parameters = new string[] { null };
            dotnetProgram.EntryPoint.Invoke(null, parameters);
        }

        static void Main(string[] args)
        {
            Reflect(@"C:\Users\jongtek\Documents\Reflection\HelloReflectionWorld\HelloReflectionWorld\bin\x64\Release\HelloReflectionWorld.exe");
        }
    }
}
