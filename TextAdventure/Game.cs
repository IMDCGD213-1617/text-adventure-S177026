using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    class Game
    {
        Location currentLocation;
        Item playerInv;

        public bool isRunning = true;

        private List<Item> inventory;

        Location l4;
        Location l5;
        Location l6;
        Location l7;
        Location l8;
        Location l9;
        Location l11;

        public Game()
        {
            //Initialises players inventory and items
            inventory = new List<Item>();
            playerInv = new Item();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("For all commands type 'help'!");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\nWelcome adventurer, prepare yourself for a fantastical journey into the unknown.\n");

            // build the "map" and adds all items
            Location l1 = new Location("Entrance to hall", "You stand at the entrance of a long hallway. The hallways gets darker\nand darker, and you cannot see what lies beyond. To the east\nis an old oaken door, unlocked and beckoning.");
            Item rock = new Item();
            rock.name = "rock";
            rock.info = "This rock looks like it could be used to break something.";
            l1.addItem(rock);

            Location l2 = new Location("End of hall", "You have reached the end of a long dark hallway. You can\nsee a ladder leading to the loft.");
            Item Rope = new Item();
            Rope.name = "rope";
            Rope.info = "This might be useful to get down a long drop.";
            l2.addItem(Rope);


            Location l3 = new Location("Small study", "This is a small and cluttered study, containing a desk covered with\npapers. Though they no doubt are of some importance,\nyou cannot read their writing.");
            Item key = new Item();
            key.name = "key";
            key.info = "A shiny key. Looks like it could open a door somewhere.";
            l3.addItem(key);

            l4 = new Location("Top of stairs", "You reach the top of the stairs. All the doors are locked however there is a \nsmall window at the end of the hallway that looks like it can be smashed.");

            l5 = new Location("The Forest", "You are outside in the forest. There is something moving ahead.");
            Item stick = new Item();
            stick.name = "stick";
            stick.info = "Looks like this could be sharpened into a weapon.";
            l5.addItem(stick);
            Item flint = new Item();
            flint.name = "flint";
            flint.info = "Looks like this could be used to sharpen a weapon!";
            l5.addItem(flint);

            l6 = new Location("Monster", "You walk deeper into the forest until a monster jumps out infront of you.");

            l7 = new Location("Outside shead", "You find a locked shead with a blood trail leading to it. Maybe the monster ran in there!");

            l8 = new Location("Inside shead", "There is blood all over the shead. Theres as trap door in the middle of the room and a door at the back.");

            l9 = new Location("Inside trap door", "You see a long drop below.");

            Location l10 = new Location("Second shead room", "You see the monster standing infront of you. There is blood all over the room and the monster is badly hurt.");

            l11 = new Location("Trap door death", "You climb down the rope. You can't see anything down here.");


            //Add exits for lcoations
            l1.addExit(new Exit(Exit.Directions.North, l2));
            l1.addExit(new Exit(Exit.Directions.East, l3));

            l2.addExit(new Exit(Exit.Directions.South, l1));
            l2.addExit(new Exit(Exit.Directions.Up, l4));

            l3.addExit(new Exit(Exit.Directions.West, l1));

            l4.addExit(new Exit(Exit.Directions.Down, l2));

            l5.addExit(new Exit(Exit.Directions.North, l6));

            l7.addExit(new Exit(Exit.Directions.South, l6));

            l8.addExit(new Exit(Exit.Directions.Down, l9));
            l8.addExit(new Exit(Exit.Directions.North, l10));

            l9.addExit(new Exit(Exit.Directions.Up, l8));


            l10.addExit(new Exit(Exit.Directions.South, l8));

            l11.addExit(new Exit(Exit.Directions.Up, l9));

            currentLocation = l1;
            showLocation();
        }

        public void showLocation()
        {
            //Gets lcoation details and writes them
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n" + currentLocation.getTitle() + "\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(currentLocation.getDescription());

            //Checks if current location has any items in the scene. if it does then write them
            if (currentLocation.getInventory().Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nThe room contains the following:\n");

                for (int i = 0; i < currentLocation.getInventory().Count; i++)
                {
                    Console.WriteLine(currentLocation.getInventory()[i].name);
                }
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nAvailable Exits: \n");
            //Write all available exits
            foreach (Exit exit in currentLocation.getExits())
            {
                Console.WriteLine(exit.getDirection());
            }

            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.White;

        }



        public void doAction(string command)
        {

            //Splits command into single word strings
            String[] input = command.Split(default(String[]), StringSplitOptions.RemoveEmptyEntries);

            //Checks every possible exit against where the player wants to move to (If either typed full direction or just a shortcut). If the player can move that way then set the new location.
            foreach (Exit exit in currentLocation.getExits())
            {
                if (input.Length > 0)
                {
                    if (input[0] == exit.ToString().ToLower() || input[0] == exit.ToString().ToLower().Remove(1, exit.ToString().Length - 1))
                    {
                        currentLocation = exit.getLeadsTo();
                        Console.Clear();
                        showLocation();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nYou moved " + exit + "\n");

                        return;
                    }
                }
            }

            //If player has typed inventory, inv or i then show the players inventory
            if (input.Length > 0)
            {
                if (input[0] == "inventory" || input[0] == "inv" || input[0] == "i")
                {
                    showInventory();
                    return;
                }
            }

            #region add 
            //If player says add then remove the add from the input and see what item they wanted to add. If the current location has that item in it then remove it from the scene and add t to players inventory
            if (input.Length > 1)
            {
                if (input[0] == "add")
                {
                    for (int i = 0; i < currentLocation.getInventory().Count; i++)
                    {
                        if (currentLocation.getInventory()[i].name == input[1])
                        {
                            Item result = currentLocation.takeItem(input[1]);
                            playerInv.AddItems(result);
                            Console.Clear();
                            showLocation();
                            Console.ForegroundColor = ConsoleColor.Red;
                            if (result != null)
                                Console.WriteLine("\nYou have picked up a " + input[1] + "!\n");
                            else
                                Console.WriteLine("Invalid item!");

                            return;
                        }
                    }
                    Console.Clear();
                    showLocation();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You can't do that!");
                    return;
                }
            }
            #endregion

            #region look 
            //If player examines then remove examine and see what they want to examine. This calls the getDescription fucntion and returns the items description if it is in the players inventory
            if (input.Length > 1)
            {
                if (input[0] == "look" || input[0] == "examine")
                {

                    string desc = playerInv.GetInfo(input[1]);
                    Console.Clear();
                    showLocation();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n" + desc + "\n");
                    return;
                }
            }
            #endregion

            #region craft
            //Checks to see if use is in command
            if (input.Length > 2)
            {
                if (input[0] == "craft")
                {
                    Item item1 = null;
                    Item item2 = null;

                    //Checks all items in player inv against the 2 items mentioned in use command. If they match then set the item to mentioned item
                    foreach (Item item in playerInv.getPlayerInventory())
                    {
                        if (input[1] == item.name)
                        {
                            item1 = item;
                        }

                        if (input[2] == item.name)
                        {
                            item2 = item;
                        }
                    }

                    if (item1 != null && item2 != null)
                    {
                        //Checks names and runs what happens when items are used together. If nothing can happen tell user that nothing happens
                        if (item1.name == "flint" && item2.name == "stick" || item1.name == "stick" && item2.name == "flint")
                        {
                            playerInv.RemoveItems(item1);
                            playerInv.RemoveItems(item2);

                            Item spear = new Item();
                            spear.name = "spear";
                            spear.info = "This could be used to kill something.";

                            playerInv.AddItems(spear);

                            Console.Clear();
                            showLocation();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nYou combine the rock and stick and make a spear!\n");
                            return;
                        }
                        else
                        {
                            Console.Clear();
                            showLocation();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Nothing happens");
                            return;
                        }

                        Console.Clear();
                        showLocation();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You can't craft those items!");
                        return;
                    }
                }
            }
            #endregion

            #region use
            if (input.Length > 1)
            {
                if (input[0] == "use")
                {
                    Item item1 = null;
                    Console.ForegroundColor = ConsoleColor.Red;

                    //Checks all items in player inv against the 2 items mentioned in use command. If they match then set the item to mentioned item
                    foreach (Item item in playerInv.getPlayerInventory())
                    {
                        if (input[1] == item.name)
                        {
                            item1 = item;
                            break;
                        }
                    }
                    if (item1 != null)
                    {
                        if (item1.name == "rock" && currentLocation.ToString() == "Top of stairs")
                        {

                            l6.addExit(new Exit(Exit.Directions.South, l5));
                            l4.addExit(new Exit(Exit.Directions.West, l5));
                            playerInv.RemoveItems(item1);

                            Console.Clear();
                            showLocation();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("You smash the window with the rock.");

                            return;
                        }
                        else if (item1.name == "key" && currentLocation.ToString() == "Outside shead")
                        {

                            l7.addExit(new Exit(Exit.Directions.East, l8));

                            playerInv.RemoveItems(item1);

                            Console.Clear();
                            showLocation();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("You try the key in the lock.");
                            Console.WriteLine("The key works and the door is unlocked.");
                            return;
                        }
                        else if (item1.name == "rope" && currentLocation.ToString() == "Inside trap door")
                        {
                            l9.addExit(new Exit(Exit.Directions.Down, l11));

                            playerInv.RemoveItems(item1);

                            l9.setDescription("All you can see is the rope. The rope leads down into the darkness.");

                            Console.Clear();
                            showLocation();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("You use the rope to give you a path down.");
                            return;

                        }
                    }
                    Console.Clear();
                    showLocation();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You can't do that!");

                    return;

                }
            }
            #endregion

            #region attack
            //If attack is typed then checked player inventory and players location. Depending on where the player is and whats in inventory then continue with action
            if (input.Length > 0)
            {
                if (input[0] == "attack")
                {

                    Console.Clear();
                    showLocation();
                    Console.ForegroundColor = ConsoleColor.Red;

                    foreach (Item item in playerInv.getPlayerInventory())
                    {
                        if (currentLocation.ToString() == "Monster")
                        {
                            if (item.name == "spear")
                            {
                                l6.addExit(new Exit(Exit.Directions.North, l7));
                                Console.Clear();
                                showLocation();
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("You attack the monster with the spear. It's wounded but manages to escape");
                                l6.setDescription("You walk deeper into the forest. There is a blood trail left from the monster.");
                                return;
                            }
                        }
                        else if (currentLocation.ToString() == "Second shead room")
                        {
                            if (item.name == "spear")
                            {
                                Console.Clear();
                                showLocation();
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("You attack the monster again killing it!!");
                                Console.WriteLine("\nCongratulations you killed the monster and completed the game!");
                                Console.WriteLine("Push any key to end the game.....");
                                Console.ReadLine();
                                isRunning = false;
                                return;
                            }
                        }
                    }

                    if (currentLocation.ToString() == "Second shead room" || currentLocation.ToString() == "Monster")
                    {
                        Console.Clear();
                        showLocation();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You have nothing to attack with!");
                        return;
                    }

                    Console.WriteLine("There is nothing to attack here!");
                    return;

                }
            }
            #endregion

            #region help
            //If help is typed then show all possible commands
            if (input.Length > 0)
            {
                if (input[0] == "help")
                {
                    Console.Clear();
                    Console.WriteLine(@"HELP!
_________

Attack: attack
Add: add ""itemName""
Examine: examine ""itemName""
Help: Help
Move: North,East, South, West or n, e, s, w
Inventory: i, inv or Inventory
Quit: q or Quit 
Use: Use ""itemName""
Craft: Craft ""itemName"" ""itemName""


Push any key to return!");
                    Console.ReadLine();
                    Console.Clear();
                    showLocation();
                    return;
                }
            }
            #endregion

            Console.Clear();
            showLocation();
            Console.WriteLine("\nInvalid command, are you confused?\n");

            Console.ForegroundColor = ConsoleColor.White;

        }

        private void showInventory()
        {

            Console.Clear();
            showLocation();

            Console.ForegroundColor = ConsoleColor.Magenta;
            //If player has items in inventory then show them
            if (playerInv.getPlayerInventory().Count > 0)
            {
                Console.WriteLine("\nA quick look in your bag reveals the following:\n");

                foreach (Item item in playerInv.getPlayerInventory())
                {
                    Console.WriteLine(item.name);
                }
            }
            else
            {
                Console.WriteLine("Your bag is empty.");
            }

            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;

        }

        public void Update()
        {
            string currentCommand = Console.ReadLine().ToLower();

            // instantly check for a quit
            if (currentCommand == "quit" || currentCommand == "q")
            {
                isRunning = false;
                return;
            }

            // otherwise, process commands.
            if (currentCommand != "")
                doAction(currentCommand);

            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}