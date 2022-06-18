using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace DarkCloud2_EnemyRandomizer
{
    public class Randomizer
    {      
        static int prevFloor;
        static bool exitError;
        public static int itemAmount;

        static bool randomEnemies;
        static bool randomSound;
        static bool randomHP;
        static bool randomDefense;
        static bool randomABS;
        static bool randomGilda;
        static bool randomRage;
        static bool randomWeaknesses;
        static bool randomEffectiveness;
        static bool randomItems;
        static bool randomDamage;

        static bool originalEnemies = true;
        static bool originalNames = true;

        static int currentFloorUSA = 0x21ECD638;
        static int currentFloorAddress;
        static int currentFloor;

        static int currentDungeonUSA = 0x20376638;
        static int currentDungeon;

        static int unableToMoveAddress = 0x2037869C;

        public static int gameVersion = 0;

        static ArrayList enemies = new ArrayList();
        static ArrayList gemrons = new ArrayList();
        static ArrayList elementals = new ArrayList();

        static string enemyData;
        static string randomEnemyData;
        public static void EnemyRandomizer(bool randomEnemiesChoice, bool randomSoundChoice, bool randomHPChoice, bool randomDefenseChoice, bool randomDamageChoice,
        bool randomABSChoice, bool randomGildaChoice, bool randomRageChoice, bool randomWeaknessesChoice, bool randomEffectivenessChoice, bool randomItemsChoice)
        {
            randomEnemies = randomEnemiesChoice;
            randomSound = randomSoundChoice;
            randomHP = randomHPChoice;
            randomDefense = randomDefenseChoice;
            randomABS = randomABSChoice;
            randomGilda = randomGildaChoice;
            randomRage = randomRageChoice;
            randomWeaknesses = randomWeaknessesChoice;
            randomEffectiveness = randomEffectivenessChoice;
            randomItems = randomItemsChoice;
            randomDamage = randomDamageChoice;

            enemyData = ObtainAllData();
            StoreEnemyData();

            currentFloorAddress = currentFloorUSA;
            currentDungeon = currentDungeonUSA;

            Console.WriteLine("Enemy randomizer on");
            while (true)
            {
                if (Memory.ReadByte(currentFloorAddress) > 0)
                {
                    //Console.WriteLine("old floor");
                    if (Memory.ReadByte(currentFloorAddress) != prevFloor) // New Floor
                    {
                        Console.WriteLine("New floor");
                        currentFloor = Memory.ReadByte(currentFloorAddress);
                        if (exitError ==  false)
                        {
                            RandomizeEnemies();
                            originalEnemies = false;
                            originalNames = false;
                            Thread.Sleep(5000);
                            ResetEnemies(enemyData);
                        }

                        if (currentFloor == 0)
                        {
                            Console.WriteLine("Error, went out of dungeon");
                            exitError = true;
                        }
                        else
                        {
                            exitError = false;
                        }

                        if (exitError == false)
                        {
                            prevFloor = currentFloor;
                        }
                    }
                    if (Memory.ReadByte(unableToMoveAddress) == 0) // Open Inventory
                    {
                        if(originalNames == false)
                        {
                            Console.WriteLine("Inventory Opened");
                            randomEnemyData = ObtainAllData();
                            // While inventory is open, reset names for monster transformations
                            ResetNames(enemyData);
                            originalNames = true;
                        }
                        
                    }  
                    else
                    {
                        if(originalNames == true)
                        {
                            if (randomEnemyData != null)
                            {
                                Console.WriteLine("Inventory Closed");
                                // When inventory is closed, change names back to randomized ones for Mimic spawns
                                ResetNames(randomEnemyData);
                                originalNames = false;
                            }
                        }
                    }
                }
                else
                {
                    prevFloor = 200;
                }

                if (gameVersion == 1)
                {
                    if (Memory.ReadInt(0x203694D0) != 1701667175)
                    {
                        Thread.Sleep(1000);
                        if (Memory.ReadInt(0x203694D0) != 1701667175)
                        {
                            Program.ExitProgram();
                        }
                    }
                }
                else if (gameVersion == 2)
                {

                    if (Memory.ReadInt(0x20364BD0) != 1701667175)
                    {
                        Thread.Sleep(1000);
                        if (Memory.ReadInt(0x20364BD0) != 1701667175)
                        {
                            Program.ExitProgram();
                        }
                    }
                }

                Thread.Sleep(1);
            }
        }

        // Saves all original enemy information
        private static void StoreEnemyData()
        {
            Console.WriteLine("Storing enemy data");
            enemies.Clear();
            int currentAddress = 0x2033D9E0;
            currentAddress += 0x00000004;
            for (int i = 0; i < 280; i++)
            {
                string enemy = Memory.ReadString(currentAddress, 68); // Basic Required Enemy Info
                currentAddress += 0x00000044;
                string sound = Memory.ReadString(currentAddress, 4); 
                currentAddress += 0x00000004;
                string unknownFirst = Memory.ReadString(currentAddress, 4);
                currentAddress += 0x00000004;
                string HP = Memory.ReadString(currentAddress, 4);
                currentAddress += 0x00000004;
                string family = Memory.ReadString(currentAddress, 2);
                currentAddress += 0x00000002;
                string ABS = Memory.ReadString(currentAddress, 2);
                currentAddress += 0x00000002;
                string gilda = Memory.ReadString(currentAddress, 2);
                currentAddress += 0x00000002;
                string unknownSecond = Memory.ReadString(currentAddress, 6);
                currentAddress += 0x00000006;
                string rage = Memory.ReadString(currentAddress, 2);
                currentAddress += 0x00000002;
                string unknownThird = Memory.ReadString(currentAddress, 4);
                currentAddress += 0x00000004;
                string damage = Memory.ReadString(currentAddress, 2);
                currentAddress += 0x00000002;
                string defense = Memory.ReadString(currentAddress, 2);
                currentAddress += 0x00000002;
                string bossFlag = Memory.ReadString(currentAddress, 2);
                currentAddress += 0x00000002;
                string weaknesses = Memory.ReadString(currentAddress, 16);
                currentAddress += 0x00000010;
                string effectiveness = Memory.ReadString(currentAddress, 24);
                currentAddress += 0x00000018;
                string unknownFourth = Memory.ReadString(currentAddress, 12);
                currentAddress += 0x0000000C;
                string items = Memory.ReadString(currentAddress, 6);
                currentAddress += 0x00000006;
                string unknownFifth = Memory.ReadString(currentAddress, 10);
                currentAddress += 0x0000000A;
                string habitat = Memory.ReadString(currentAddress, 2);
                currentAddress += 0x00000002;
                string bestiarySpot = Memory.ReadString(currentAddress, 2);
                currentAddress += 0x00000002;
                string sharedHP = Memory.ReadString(currentAddress, 2);
                currentAddress += 0x00000002;
                string unknownSixth = Memory.ReadString(currentAddress, 2);
                currentAddress += 0x00000002;
                string[] completeEnemy = { enemy, sound, unknownFirst, HP, family, ABS, gilda, unknownSecond, rage, 
                    unknownThird, damage, defense, bossFlag, weaknesses, effectiveness, unknownFourth, items, unknownFifth, habitat, bestiarySpot, sharedHP, unknownSixth};
                //Console.WriteLine(String.Join(" ", completeEnemy));
                currentAddress += 0x00000004;
                if(enemy.Contains("eJema"))
                {
                    gemrons.Add(completeEnemy);
                } 
                else if (enemy.Contains("element"))
                {
                    elementals.Add(completeEnemy);
                }
                else if (!enemy.Contains("Linda") && !enemy.Contains("Memo Eater"))
                {
                    enemies.Add(completeEnemy);
                }
            }
        }

        // Randomizes enemies. Depending on the choices selected, only certain parts in memory are randomized.
        // After testing each few bytes of enemy's data, I was able to find out most of their purposes, however many still remain unknown.
        // At least 2 bytes of one of the unknowns is responsible for the enemies resistance to being stunned while hit.
        // Another at least 2 bytes seem to be the enemy's chance to guard an attack.
        // Currently, the unknown bytes are always overwritten with the random enemy's.
        private static void RandomizeEnemies()
        {
            Console.WriteLine("Randomizing enemies");
            int currentAddress = 0x2033D9E0;
            currentAddress += 0x00000004;
            ArrayList enemiesRandomized = new ArrayList();
            enemiesRandomized = Shuffle(enemies);
            for ( int i = 0; i < enemiesRandomized.Count; i++ )
            {
                string[] completeEnemy = (string[])enemiesRandomized[i];
                //Console.WriteLine(String.Join(" ", completeEnemy));
                if(currentAddress == 0x20342A64) // Gemrons start
                {
                    currentAddress += 0x00000E60; // Bypassing gemrons
                    currentAddress = WriteRandomEnemyData(completeEnemy, currentAddress);
                }
                else if(currentAddress == 0x20341084) // Elements start
                {
                    currentAddress += 0x00000E60; // Bypassing elementals
                    currentAddress = WriteRandomEnemyData(completeEnemy, currentAddress);
                } 
                else if(currentAddress == 0x20340AC4 || currentAddress == 0x2033FC64) // Linda or Memo Eater
                {
                    currentAddress += 0x000000B8; // Bypassing Linda and Memo Eater
                    currentAddress = WriteRandomEnemyData(completeEnemy, currentAddress);
                }
                else
                {
                    currentAddress = WriteRandomEnemyData(completeEnemy, currentAddress);
                }
            }
            RandomizeGemrons();
            RandomizeElementals();
        }

        private static void RandomizeGemrons()
        {
            int currentAddress = 0x20342A60;
            currentAddress += 0x00000004;
            ArrayList gemronsRandomized = new ArrayList();
            gemronsRandomized = Shuffle(gemrons);
            for (int i = 0; i < gemronsRandomized.Count; i++)
            {
                string[] completeEnemy = (string[])gemronsRandomized[i];
                currentAddress = WriteRandomEnemyData(completeEnemy, currentAddress);
            }
        }
        
        private static void RandomizeElementals()
        {
            int currentAddress = 0x20341080;
            currentAddress += 0x00000004;
            ArrayList elementalsRandomized = new ArrayList();
            elementalsRandomized = Shuffle(elementals);
            for (int i = 0; i < elementalsRandomized.Count; i++)
            {
                string[] completeEnemy = (string[])elementalsRandomized[i];
                currentAddress = WriteRandomEnemyData(completeEnemy, currentAddress);
            }
        }

        private static int WriteRandomEnemyData(string[] completeEnemy, int currentAddress)
        {
            if (!randomEnemies)
            {
                Memory.WriteString(currentAddress, completeEnemy[0]); // Name, Model, AI
            }
            currentAddress += 0x00000044;
            if (!randomSound)
            {
                Memory.WriteString(currentAddress, completeEnemy[1]); // Sound. It's funny to have incorrect noises
            }
            currentAddress += 0x00000004;
            if(!randomEnemies)
            {
                Memory.WriteString(currentAddress, completeEnemy[2]); // Writing an unknown value...
            }
            currentAddress += 0x00000004;
            if (!randomHP)
            {
                Memory.WriteString(currentAddress, completeEnemy[3]); // HP
            }
            currentAddress += 0x00000004;
            if (!randomEnemies)
            {
                Memory.WriteString(currentAddress, completeEnemy[4]); // Enemy family
            }
            currentAddress += 0x00000002;
            if (!randomABS)
            {
                Memory.WriteString(currentAddress, completeEnemy[5]); // ABS Dropped
            }
            currentAddress += 0x00000002;
            if (!randomGilda)
            {
                Memory.WriteString(currentAddress, completeEnemy[6]); // Gilda Dropped
            }
            currentAddress += 0x00000002;
            if (!randomEnemies)
            {
                Memory.WriteString(currentAddress, completeEnemy[7]); // Writing an unknown value...
            }
            currentAddress += 0x00000006;
            if (!randomRage)
            {
                Memory.WriteString(currentAddress, completeEnemy[8]); // Rage Counter
            }
            currentAddress += 0x00000002;
            if (!randomEnemies)
            {
                Memory.WriteString(currentAddress, completeEnemy[9]); // Writing an unknown value...
            }
            currentAddress += 0x00000004;
            if (!randomDamage)
            {
                Memory.WriteString(currentAddress, completeEnemy[10]); // Attack
            }
            currentAddress += 0x00000002;
            if (!randomDefense)
            {
                Memory.WriteString(currentAddress, completeEnemy[11]); // Defense
            }
            currentAddress += 0x00000002;
            Memory.WriteUShort(currentAddress, 0); // Not-Boss Flag
            currentAddress += 0x00000002;
            if (!randomWeaknesses)
            {
                Memory.WriteString(currentAddress, completeEnemy[13]); // Weaknesses
            }
            currentAddress += 0x00000010;
            if (!randomEffectiveness)
            {
                Memory.WriteString(currentAddress, completeEnemy[14]); // Weapon Effectiveness
            }
            currentAddress += 0x00000018;
            if (!randomEnemies)
            {
                Memory.WriteString(currentAddress, completeEnemy[15]); // Writing an unknown value...
            }
            currentAddress += 0x0000000C;
            if (!randomItems)
            {
                Memory.WriteString(currentAddress, completeEnemy[16]); // Items
            }
            currentAddress += 0x00000006;
            if (!randomEnemies)
            {
                Memory.WriteString(currentAddress, completeEnemy[17]); // Writing an unknown value...
            }
            currentAddress += 0x0000000A;
            if (!randomEnemies)
            {
                Memory.WriteString(currentAddress, completeEnemy[18]); // Habitat
                currentAddress += 0x00000002;
                Memory.WriteString(currentAddress, completeEnemy[19]); // Bestiary Spot
                currentAddress += 0x00000002;
                Memory.WriteString(currentAddress, completeEnemy[20]); // sharedHP
                currentAddress += 0x00000002;
            }
            else
            {
                currentAddress += 0x00000006;
            }
            Memory.WriteString(currentAddress, completeEnemy[21]);
            currentAddress += 0x00000002;
            currentAddress += 0x00000004;
            return currentAddress;
        }

        // Shuffles a given array list
        private static ArrayList Shuffle(ArrayList enemies)
        {
            Random RandomGen = new Random();
            ArrayList ScrambledList = new ArrayList();
            ArrayList tempList = (ArrayList)enemies.Clone();
            Int32 Index;

            // randomly remove items from the first list and
            // put them in the second list
            while (tempList.Count > 0)
            {
                Index = RandomGen.Next(tempList.Count);
                ScrambledList.Add(tempList[Index]);
                tempList.RemoveAt(Index);
            }
            return ScrambledList;
        }

        // Writes every single byte of data
        private static void WriteAllData(string data)
        {
            int currentAddress = 0x2033D9E0;
            Memory.WriteString(currentAddress, data);
        }

        // Stores every single byte of enemy data
        private static string ObtainAllData()
        {
            int currentAddress = 0x2033D9E0;
            string allEnemyData = Memory.ReadString(currentAddress, 51520);
            return allEnemyData;
        }

        private static void ResetEnemies(string enemyData)
        {
            while (Memory.ReadByte(0x2037869C) == 0)
            {
                Thread.Sleep(1);
            }

            if (originalEnemies == false) // Only able to reset if the enemies have been changed
            {
                Thread.Sleep(5000);
                Console.WriteLine("Resetting enemies except names");
                int currentAddress = 0x2033D9E0; // Beginning of all enemy data, starts with "load position"
                for (int i = 0; i < 280; i++)
                {
                    string name = Memory.ReadString(currentAddress + 0x00000004, 32);

                    Memory.WriteString(currentAddress, enemyData.Substring(0 + (i * 184), 184));
                    currentAddress += 0x00000004;
                    Memory.WriteString(currentAddress, name);
                    currentAddress += 0x000000B4;
                }
            }
            originalEnemies = true;
        }

        private static void ResetNames(string enemyData)
        {
            int currentAddress = 0x2033D9E4;
            Console.WriteLine("Resetting names");
            for (int i = 0; i < 280; i++)
            {
                string originalName = enemyData.Substring(4+(i*184), 32);
                //Console.WriteLine("Original name: " + originalName);
                Memory.WriteString(currentAddress, originalName);
                currentAddress += 0x000000B8;
            }
        }
    }
}
