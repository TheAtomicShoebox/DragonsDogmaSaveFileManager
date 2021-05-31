using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonsDogmaFileCopierBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var oldSaveItems = FileHandler.GetArraysByFilePath("D:\\DragonsDogmaFileCopierBot\\DragonsDogmaFileCopierBot\\Files\\DDDA-210528232046.sav.xml");
            var newSaveItems = FileHandler.GetArraysByFilePath("D:\\DragonsDogmaFileCopierBot\\DragonsDogmaFileCopierBot\\Files\\DDDA.sav.xml");
        }
    }
}
