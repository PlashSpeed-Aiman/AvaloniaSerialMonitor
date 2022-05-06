using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia1.Models
{
    internal class LoggerAsync
    {
        private static void WriteToFile(string x)
        {

        }
        public static async void SaveToTextFileAsync(string x)
        {
            await File.WriteAllTextAsync($"SaveData.txt",x);
        }
    }
}
