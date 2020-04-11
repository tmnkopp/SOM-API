using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
namespace SOMAPI 
{
    public class AppSettings
    {
        public  string BasePath   { get; set; }
        public  string SourceDir { get; set; }
        public  string DestDir { get; set; } 
    }
    public static class Placeholder
    {
        public static string Basepath = "[basepath]";
        public static string Index = "[index]";
        public static string EXT = "[ext]";
    }
}
