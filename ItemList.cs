using System.Collections;
using System.Collections.Generic;

namespace DragonsDogmaFileCopierBot
{
    public class ItemList : IEnumerable<Item>
    {
        List<Item> Items = new List<Item>();
        public string ArrayType { get; set; }

        public ItemList(string arrayType)
        {
            ArrayType = arrayType;
        }

        public ItemList(string arrayType, List<Item> items)
        {
            ArrayType = arrayType;
            Items = items;
        }

        public Item this[int index]
        {
            get { return Items[index]; }
            set { Items.Insert(index, value); }
        }

        public IEnumerator<Item> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Item item)
        {
            Items.Add(item);
        }
    }
}
