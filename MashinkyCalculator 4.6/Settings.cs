using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MashinkyCalculator
{
    public static class Settings
    {
        //public readonly static string Path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MashCalculator");
        public readonly static string Path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MashCalculator";
        public static string GameFolderPath { get; private set; }
        public static string Language { get; private set; }
        public static string ResultPriority { get; private set; }
        public static int SpeedTolerance { get; private set; }
        public static bool Miles { get; private set; }
        public static bool Music { get; private set; }

        public static void SetGameFolder(string folder)
        {
            GameFolderPath = folder;
        }

        public static void SetLanguage(string lang)
        {
            Language = lang;
        }

        public static void SetResultPriority(string priority)
        {
            ResultPriority = priority;
        }

        public static void SetSpeedTolerance (int speed)
        {
            SpeedTolerance = speed;
            
        }

        public static void SetMiles(bool miles)
        {
            Miles = miles;
        }

        public static void SetMusic(bool music)
        {
            Music = music;
        }
          
    }
}
