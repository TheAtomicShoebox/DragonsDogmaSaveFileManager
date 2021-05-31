using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DragonsDogmaFileCopierBot
{
    public class SaveItems
    {
        public SaveItems(string fileAsString)
        {
            var itemLists = ConvertItemArrays(fileAsString);
            EquipmentList = itemLists.FirstOrDefault(il => il.ArrayType == "Equipment");
            InventoryList = itemLists.FirstOrDefault(il => il.ArrayType == "Inventory");
            StorageList = itemLists.FirstOrDefault(il => il.ArrayType == "Storage");
        }

        public ItemList EquipmentList { get; set; }
        public ItemList InventoryList { get; set; }
        public ItemList StorageList { get; set; }

        public IEnumerable<ItemList> ConvertItemArrays (string fileAsString)
        {
            #region RegexInitialization (Consider Changing to assembled or compiled)
            Regex arrayRegex = new Regex(@"(^<array name=""(\w+)"" type=""class"" count=""(\d+)"">$.).*?(?=^</array>$.)", RegexOptions.Singleline | RegexOptions.Multiline);

            Regex headerRegex = new Regex(@"(?:<array name=""(\w+)"" type=""class"" count=""(\d+)"">)");

            Regex itemRegex = new Regex(
                @"(?<=^<class type=""sItemManager::cITEM_PARAM_DATA"">$.)(?:^<s16 name=""data\.(?:\w+)"" value=""(\d+)""/>$.)(?:^<s16 name=""data\.(?:\w+)"" value=""(\d+|-1)""/>$.)(?:^<u32 name=""data\.(?:\w+)"" value=""(\d+)""/>$.)(?:^<u16 name=""data\.(?:\w+)"" value=""(\d+)""/>$.)(?:^<u16 name=""data\.(?:\w+)"" value=""(\d+)""/>$.)(?:^<u16 name=""data\.(?:\w+)"" value=""(\d+)""/>$.)(?:^<u16 name=""data\.(?:\w+)"" value=""(\d+)""/>$.)(?:^<s8 name=""data\.(?:\w+)"" value=""(\d+)""/>$.)(?:^<s8 name=""data\.(?:\w+)"" value=""(\d+)""/>$.)(?:^<u32 name=""data\.(?:\w+)"" value=""(\d+)""/>$.)(?=^</class>$)",
                RegexOptions.Singleline | RegexOptions.Multiline);
            #endregion

            var arrayMatches = arrayRegex.Matches(fileAsString);

            foreach (Match arrayMatch in arrayMatches)
            {
                var array = arrayMatch.ToString();
                ItemList itemList;
                var match = headerRegex.Match(array);
                
                switch (match.Groups[1].Value)
                {
                    case "mEquipItem":
                        itemList = new ItemList("Equipment");
                        break;
                    case "mItem":
                        itemList = new ItemList("Inventory");
                        break;
                    case "mStorageItem":
                        itemList = new ItemList("Storage");
                        break;
                    default:
                        Console.WriteLine($"Detected unsupported array of type {match.Captures[0].Value}");
                        continue;
                }

                var itemMatches = itemRegex.Matches(array);

                Console.WriteLine($"{itemList.ArrayType} List Captures:");

                foreach (Match itemMatch in itemMatches)
                {
                    Console.WriteLine($"{itemList.ArrayType} List Captures:");
                    foreach(var group in itemMatch.Groups)
                    {
                        Console.WriteLine($"{group}");
                    }
                    var groups = itemMatch.Groups;
                    var item = new Item()
                    {
                        Num = int.Parse(groups[1].Value),
                        ItemNo = int.Parse(groups[2].Value),
                        Flag = int.Parse(groups[3].Value),
                        ChgNum = int.Parse(groups[4].Value),
                        Day1 = int.Parse(groups[5].Value),
                        Day2 = int.Parse(groups[6].Value),
                        Day3 = int.Parse(groups[7].Value),
                        MutationPool = int.Parse(groups[8].Value),
                        OwnerId = int.Parse(groups[9].Value),
                        Key = int.Parse(groups[10].Value)
                    };
                    itemList.Add(item);
                }
                yield return itemList;
            }
        }
    }
}
