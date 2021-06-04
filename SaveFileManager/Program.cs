using System;
using SaveFileManager.Base;

namespace SaveFileManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var oldSaveItems = FileHandler.GetSaveItemsByFilePath("D:\\DragonsDogmaFileCopierBot\\DragonsDogmaFileCopierBot\\Files\\DDDA-210528232046.sav.xml");
            var newSaveItems = FileHandler.GetSaveItemsByFilePath("D:\\DragonsDogmaFileCopierBot\\DragonsDogmaFileCopierBot\\Files\\DDDA.sav.xml");

            PrintSaveItems(oldSaveItems);
            PrintSaveItems(newSaveItems);

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void PrintSaveItems(SaveItems saveItems)
        {
            Console.WriteLine("Equipment Lists:");
            foreach(var list in saveItems.EquipmentLists)
            {
                Console.WriteLine("Equipment List:");
                foreach (var item in list)
                {
                    PrintItem(item);
                }
            }
            Console.WriteLine("Inventory List:");
            foreach (var list in saveItems.InventoryLists)
            {
                Console.WriteLine("Inventory List:");
                foreach (var item in list)
                {
                    PrintItem(item);
                }
            }
            Console.WriteLine("Storage List: ");
            foreach (var list in saveItems.StorageLists)
            {
                Console.WriteLine("Storage List:");
                foreach (var item in list)
                {
                    PrintItem(item);
                }
            }
        }

        private static void PrintItem(Item item)
        {
            Console.WriteLine($"Quantity: {item.Num}");
            Console.WriteLine($"Item Number: {item.ItemNo}");
            Console.WriteLine($"ChgNum: {item.ChgNum}");
            Console.WriteLine($"Primary Stat value: {item.Day1}");
            Console.WriteLine($"Secondary Stat value: {item.Day2}");
            Console.WriteLine($"Tertiary value: {item.Day3}");
            Console.WriteLine($"Mutation Pool: {item.MutationPool}");
            Console.WriteLine($"Owner Id: {item.OwnerId}");
            Console.WriteLine($"Key: {item.Key}");
        }
    }
}
