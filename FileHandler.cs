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
            var fileAsString = File.ReadAllText(filepath);
            var itemLists = GetItemLists(fileAsLines, fileAsString);
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

            var equipmentMatch = Regex.Match(fileAsString, @"^<array name=""mEquipItem"".*</array>$", RegexOptions.Singleline | RegexOptions.Multiline);
            if (equipmentMatch.Success)
            {
                var equipmentArray = Regex.Split(equipmentMatch.Value, @"^<class type=""sItemManager::cITEM_PARAM_DATA"">$");
                //Is this bad?
                equipmentArray.RemoveAt(0);
                var headerRegex = new Regex(@"(?:^<array name=""(\w+)"" type=""class"" count=""(\d+)""$.)");
                if (int.TryParse(headerRegex.Match(equipmentMatch.Value).Captures[0].Value, out var count))
                {
                    equipmentList = ConvertItemArray(equipmentArray, equipmentList);
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

            var inventoryMatch = Regex.Match(fileAsString, @"^<array name=""mItem"".*</array>$", RegexOptions.Singleline | RegexOptions.Multiline);
            if (inventoryMatch.Success)
            {
                var inventoryArray = Regex.Split(inventoryMatch.Value, @"^<class type=""sItemManager::cITEM_PARAM_DATA>$");
                inventoryArray.RemoveAt(0);
                var headerRegex
            }
        }

        public static IEnumerable<ItemList> ConvertItemArrays(string fileAsString, ItemList itemList)
        {
            Regex arrayRegex = new Regex(@"^<array name=""(\w+)"".*</array>$", RegexOptions.Singleline | RegexOptions.Multiline);
            var arrays = arrayRegex.Matches(fileAsString);

            
            var itemRegex = new Regex(@"(?:^<class type=""sItemManager::cITEM_PARAM_DATA"">$.)
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
                                        (?:^</class>$)",
                                        RegexOptions.Singleline | RegexOptions.Multiline);

            foreach(var itemString in itemArray)
            {
                var itemMatch = itemRegex.Match(itemString);
                Console.WriteLine($"{itemList.ArrayType} List Captures:\n{itemMatch.Captures}");
                var captures = itemMatch.Captures;
                var item = new Item()
                {
                    Num = int.Parse(captures[1].Value),
                    ItemNo = int.Parse(captures[3].Value),
                    Flag = int.Parse(captures[5].Value),
                    ChgNum = int.Parse(captures[7].Value),
                    Day1 = int.Parse(captures[9].Value),
                    Day2 = int.Parse(captures[11].Value),
                    Day3 = int.Parse(captures[13].Value),
                    MutationPool = int.Parse(captures[15].Value),
                    OwnerId = int.Parse(captures[17].Value),
                    Key = int.Parse(captures[19].Value)
                };
                itemList.Add(item);
            }
            return itemList;
        }
    }
}
