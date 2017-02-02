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
        public string description = " ";
        private List<Item> playerInventory;

        public Item()
        {
            playerInventory = new List<Item>();
        }

        //Returns the items in player inventory when called
        public List<Item> getPlayerInventory()
        {
            return new List<Item>(playerInventory);
        }

        //Adds the item to player inventory
        public void AddItems(Item item)
        {
            playerInventory.Add(item);
        }

        //Removes the item to player inventory
        public void RemoveItems(Item item)
        {
            playerInventory.Remove(item);
        }

        //Sorts through player inventory looking for match. If found return the description and if not return that item isn't in their inventory
        public String GetDescription(String item)
        {
            foreach (Item items in playerInventory)
            {
                if (items.name == item)
                {
                    string description = items.description;
                    return description;
                }
            }

            return "You don't have this item in your inventory";
        }
    }
}
