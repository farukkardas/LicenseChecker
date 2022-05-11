using System;
using System.Globalization;
using System.Management;
using System.Threading;
using System.Threading.Tasks;
using Pastel;

namespace LicenseChecker
{
    internal static class Program
    {
        private static readonly AuthService AuthService = new AuthService();
      

        static async Task Main(string[] args)
        {
            Console.Title = "License Checker";
          
            
          await WelcomeAndCheckLicense();
        }



        private static async Task WelcomeAndCheckLicense()
        {
            Console.WriteLine("Welcome to License System Test - Discord: Wiwy#9904".Pastel("#FF0000"));
           
            //Get GPU ID
            using (var searcher = new ManagementObjectSearcher("select * from Win32_VideoController"))
            {
                foreach (var o in searcher.Get())
                {
                    var obj = (ManagementObject)o;
                    Console.Write("\n");
                    Console.Write("[GPU]".Pastel("#00ff00"));
                    Console.Write(" " + obj["Name"], ($"{obj["Name"]}", ConsoleColor.Green));
                }
            }


            //Get CPU ID
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor"))
            {
                foreach (var o in searcher.Get())
                {
                    var moProcessor = (ManagementObject)o;
                    Console.Write("\n");
                    Console.Write("[CPU]".Pastel("#00ff00"));

                    Console.Write(" " + moProcessor["Name"]);
                    Console.Write("\n");
                    Console.Write("[Time] ".Pastel("#00ff00"));
                    Console.Write(DateTime.Now.ToString(CultureInfo.CurrentCulture));
                }
            }

            Console.Write("\n");
            Console.Write("[HWID] ".Pastel("#00ff00"));
            Console.Write(GetHwid());

            await CheckLicense();
        }

        private static async Task CheckLicense()
        {
            
            int appId = 1039;
            string secretKey = "U596TmFIhCdSrmyyZSwFxqwmAunPXcDtQsdPeP0K8Z78nCk31F";
            
            Console.Write("\n");
            Console.Write("Enter License: ".Pastel("#00ff00"));
            var keyInfo = Console.ReadLine();


            try
            {
                var result = await AuthService.CheckKeyIsValid(keyInfo,appId,secretKey);

                if (String.IsNullOrWhiteSpace(keyInfo))
                {
                    Console.WriteLine("[ERROR] ".Pastel("#ff0000"));
                    Console.WriteLine("License cannot be empty!");
                    await Task.Delay(1500);
                    Environment.Exit(0);
                    return;
                }

                switch (result.Success)
                {
                    case true:
                        Console.Write("\n");
                        Console.Write("[SUCCESS] ".Pastel("#00ff00"));
                        Console.Write(result.Message);
                        Console.Write("\n\n Do whatever you want..");
                        Console.ReadLine();
                        break;
                    case false:
                        Console.Write("\n");
                        Console.Write("[ERROR] ".Pastel("#ff0000"));
                        Console.Write(result.Message);
                        await Task.Delay(2000);
                        Environment.Exit(0);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.Write("\n");
                Console.Write("[CONERR] ".Pastel("#ff0000"));
                Console.Write($"Connection server error!");
                await Task.Delay(3000);
                Environment.Exit(0);
            }
        }


        public static string GetHwid()
        {
            var cpuId = "";
            var motherboardId = "";
            using (var searcher = new ManagementObjectSearcher("select * from Win32_BaseBoard"))
            {
                foreach (var o in searcher.Get())
                {
                    var obj = (ManagementObject)o;
                    motherboardId = (string)obj["SerialNumber"];
                }
            }

            using (var searcher = new ManagementObjectSearcher("select * from Win32_processor"))
            {
                foreach (var o in searcher.Get())
                {
                    var obj = (ManagementObject)o;
                    motherboardId = (string)obj["ProcessorID"];
                }
            }


            return cpuId + motherboardId;
        }

      


    }
}