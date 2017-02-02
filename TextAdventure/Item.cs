using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    class Item
    {
        public string name = " ";
        public string info = " ";
        private List<Item> playerInventory;



        public Item()
        {
            playerInventory = new List<Item>();
        }


        public List<Item> getPlayerInventory()
        {
            return new List<Item>(playerInventory);
        }


        public void AddItems(Item item)
        {
            playerInventory.Add(item);
        }

        public void RemoveItems(Item item)
        {
            playerInventory.Remove(item);

        }

        public String GetInfo(String item)
        {

            foreach (Item items in playerInventory)
            {
                if (items.name == item)
                {
                    string info = items.info;
                    return info;
                }
            }

            return "You dont have this item";
                }
    }
}

     











