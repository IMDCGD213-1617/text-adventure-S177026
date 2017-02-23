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
            //Initialises players inventory list and items
            inventory = new List<Item>();
            playerInv = new Item();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("For all commands type 'help'!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\nWelcome, prepare yourself for a journey.\n");

            // builds the locations and adds all the items, and adds the desctiption to the item
            Location l1 = new Location("Cave entrance", "You stand at the entrance of a cave. The cave is dark.\ntheir is a flashing light in the distance.");
            Item brick = new Item();
            brick.name = "brick";
            brick.description = "This brick looks like it could be used to break something.";
            l1.addItem(brick);

            Location l2 = new Location("End of cave", "You have reached the end of the cave in fornt of you is a small candle. You can\nsee a ladder leading up.");
            Item Rope = new Item();
            Rope.name = "rope";
            Rope.description = "This might be useful to get down a long drop.";
            l2.addItem(Rope);


            Location l3 = new Location("small shed", "This is a cluttered shed, could be something here.");
            Item key = new Item();
            key.name = "crowbar";
            key.description = "A crowbar. It could brake a lock.";
            l3.addItem(key);

            l4 = new Location("Top of ladder", "You reach the top of the ladder. There is a\nwindow that leads outside, it could be smashed.");

            l5 = new Location("The Forest", "You are outside in the forest. There is something moving ahead.");
            Item stick = new Item();
            stick.name = "stick";
            stick.description = "You could craft this with something.";
            l5.addItem(stick);
            Item flint = new Item();
            flint.name = "flint";
            flint.description = "You could craft something with this.";
            l5.addItem(flint);

            l6 = new Location("Wolf den", "You walk deeper into the forest until a wolf attacks from behind.");

            l7 = new Location("Small shack", "You find a locked shack with wolf fur on the bottom, it could be in there!");

            l8 = new Location("Inside shack", "There is blood all over the shack. There is a trap\ndoor in the middle of the room and a door at the back.");

            l9 = new Location("Inside trap door", "You see a long drop below.");

            Location l10 = new Location("Second shack room", "You see the wolf infront of you. There\nis blood all over the room and the wolf is badly hurt.");

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
            //Gets the current location details and writes them
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n" + currentLocation.getTitle() + "\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(currentLocation.getDescription());

            //Checks what items are in the current location, and writes them
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

            //Gives the player options of input when looking at inventory
            if (input.Length > 0)
            {
                if (input[0] == "inventory" || input[0] == "inv" || input[0] == "i")
                {
                    showInventory();
                    return;
                }
            }

            #region Take 
            //Allows player to take items from a level and put it in their inventory
            if (input.Length > 1)
            {
                if (input[0] == "take" || input[0] == "t")
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
            //Allows players to examine an item thats in their inventory and writes the description written above 
            if (input.Length > 1)
            {
                if (input[0] == "look" || input[0] == "examine" || input[0] == "e")
                {

                    string desc = playerInv.GetDescription(input[1]);
                    Console.Clear();
                    showLocation();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n" + desc + "\n");
                    return;
                }
            }
            #endregion

            #region craft
            //Craft command, most input 2 items that can carft together
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
                        //Checks to make sure players are crafting the correct item, says can craft if wrong
                        if (item1.name == "flint" && item2.name == "stick" || item1.name == "stick" && item2.name == "flint")
                        {
                            playerInv.RemoveItems(item1);
                            playerInv.RemoveItems(item2);

                            Item spear = new Item();
                            spear.name = "spear";
                            spear.description = "This could be used to kill something.";

                            playerInv.AddItems(spear);

                            Console.Clear();
                            showLocation();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nYou combine the flint and stick and make a spear!\n");
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

                    //Use, checks if an iteam can be used in a certain location, and removes it from inventory after use
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
                        if (item1.name == "brick" && currentLocation.ToString() == "Top of ladder")
                        {

                            l6.addExit(new Exit(Exit.Directions.South, l5));
                            l4.addExit(new Exit(Exit.Directions.West, l5));
                            playerInv.RemoveItems(item1);

                            Console.Clear();
                            showLocation();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("You smash the window with the brick.");

                            return;
                        }
                        else if (item1.name == "crowbar" && currentLocation.ToString() == "Small shack")
                        {

                            l7.addExit(new Exit(Exit.Directions.East, l8));

                            playerInv.RemoveItems(item1);

                            Console.Clear();
                            showLocation();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("You try to brake the lock.");
                            Console.WriteLine("The crowbar brakes the lock, the door is now open.");
                            return;
                        }
                        else if (item1.name == "rope" && currentLocation.ToString() == "Inside trap door")
                        {
                            l9.addExit(new Exit(Exit.Directions.Down, l11));

                            playerInv.RemoveItems(item1);

                            l9.setDescription("The rope leads down into the darkness.");

                            Console.Clear();
                            showLocation();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Theres nothing down here, go back up.");
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
            //checks if player is in the correct location to attack, make sure the player has the correct items in inventory. Also doesn't remove weapon from inventory
            if (input.Length > 0)
            {
                if (input[0] == "attack" || input[0] == "a")
                {

                    Console.Clear();
                    showLocation();
                    Console.ForegroundColor = ConsoleColor.Red;

                    foreach (Item item in playerInv.getPlayerInventory())
                    {
                        if (currentLocation.ToString() == "Wolf den")
                        {
                            if (item.name == "spear")
                            {
                                l6.addExit(new Exit(Exit.Directions.North, l7));
                                Console.Clear();
                                showLocation();
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("You attack the wolf with the spear. It's wounded but manages to escape");
                                l6.setDescription("You walk deeper into the forest. There is a blood trail left from the wolf.");
                                return;
                            }
                        }
                        else if (currentLocation.ToString() == "Second shack room")
                        {
                            if (item.name == "spear")
                            {
                                Console.Clear();
                                showLocation();
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("You attack the wolf again killing it!!");
                                Console.WriteLine("\nCongratulations you killed the wolf and completed the game!");
                                Console.WriteLine("Push any key to end the game.....");
                                Console.ReadLine();
                                isRunning = false;
                                return;
                            }
                        }
                    }

                    if (currentLocation.ToString() == "Second shack room" || currentLocation.ToString() == "Wolf den")
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
            //Show possible command you can use
            if (input.Length > 0)
            {
                if (input[0] == "help" || input [0] == "h")
                {
                    Console.Clear();
                    Console.WriteLine(@"HELP!
_________

Attack: a or attack
Take: t ""itemName"" or take ""itemName""
Examine: e ""itemName"" or examine ""itemName""
Help: h or help
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
            //shows the items in the players inventory, says its empty if you have nothing
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