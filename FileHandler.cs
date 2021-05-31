using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using DragonsDogmaFileCopierBot.Extensions;

namespace DragonsDogmaFileCopierBot
{
    public static class FileHandler
    {
        public static SaveItems GetArraysByFilePath(string filepath)
        {
            var fileAsLines = File.ReadAllLines(filepath);
            var itemLists = GetItemLists(fileAsLines);
            var results = new SaveItems()
            {
                EquipmentList = itemLists.FirstOrDefault(il => il.ArrayType == "Equipment"),
                InventoryList = itemLists.FirstOrDefault(il => il.ArrayType == "Inventory"),
                StorageList = itemLists.FirstOrDefault(il => il.ArrayType == "Storage")
            };

            return results;
        }

        public static IEnumerable<ItemList> GetItemLists(string[] fileAsLines, string fileAsString)
        {
            ItemList equipmentList = new ItemList("Equipment");
            ItemList inventoryList;
            ItemList storageList;

            var equipmentMatch = Regex.Match(fileAsString, "^<array name=\"mEquipItem\".*</array>$", RegexOptions.Singleline | RegexOptions.Multiline);
            if (equipmentMatch.Success)
            {
                var equipmentArray = Regex.Split(equipmentMatch.Value, "^<class type=\"sItemManager::cITEM_PARAM_DATA\">$");
                equipmentArray.RemoveAt(0);
                if (int.TryParse(Regex.Match(equipmentMatch.Value, "count=(.+)").Groups[0].Value, out var count))
                {
                    equipmentList = ConvertItemArray(equipmentArray, count, equipmentList);
                }
                else
                {
                    Console.WriteLine("Unable to parse the count");
                }
            }
            else
            {
                Console.WriteLine("Unable to find equipment array");
            }

            var beginEquip = Array.FindIndex(fileAsLines, l => l.StartsWith("<array name=\"mEquipItem\" type=\"class\" count=\"", StringComparison.OrdinalIgnoreCase));
            var beginInventory = Array.FindIndex(fileAsLines, l => l.StartsWith("<array name=\"mItem\" type=\"class\" count=\"", StringComparison.OrdinalIgnoreCase));
            var beginStorage = Array.FindIndex(fileAsLines, l => l.StartsWith("<array name=\"mStorageItem\" type=\"class\" count=\"", StringComparison.OrdinalIgnoreCase));

            var equipStartLine = fileAsLines[beginEquip];
            var inventoryStartLine = fileAsLines[beginInventory];
            var storageStartLine = fileAsLines[beginStorage];
        }

        public static ItemList ConvertItemArray(string[] itemArray, int count, ItemList emptyList)
        {
            var itemRegex = new Regex(@"(?:^<array name=""(\w+)"" type=""class"" count=""(\d+)""$.)
                                        (?:^<class type=""sItemManager::cITEM_PARAM_DATA"">$.)
                                        (?:^<s16 name=""data\.(\w+)"" value=""(\d+)""/>$.)
                                        (?:^<s16 name=""data\.(\w+)"" value=""(\d+)""/>$.)
                                        (?:^<u32 name=""data\.(\w+)"" value=""(\d+)""/>$.)
                                        (?:^<u16 name=""data\.(\w+)"" value=""(\d+)""/>$.)
                                        (?:^<u16 name=""data\.(\w+)"" value=""(\d+)""/>$.)
                                        (?:^<u16 name=""data\.(\w+)"" value=""(\d+)""/>$.)
                                        (?:^<u16 name=""data\.(\w+)"" value=""(\d+)""/>$.)
                                        (?:^<s8 name=""data\.(\w+)"" value=""(\d+)""/>$.)
                                        (?:^<s8 name=""data\.(\w+)"" value=""(\d+)""/>$.)
                                        (?:^<u32 name=""data\.(\w+)"" value=""(\d+)""/>$.)
                                        (^</class>$)",
                                        RegexOptions.Singleline | RegexOptions.Multiline);
            foreach(var itemString in itemArray)
            {
                var itemMatch = itemRegex.Matches(itemString);

            }
        }
    }
}
