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

        static int currentFloorAddress = 0x21ECD638;
        static int currentDungeonAddress = 0x20376638;
        static int currentFloor;

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
            prevFloor = -1;

            Console.WriteLine("Enemy randomizer on");
            MainMenuCheck();
            
            while (true)
            {
                if (Memory.ReadByte(currentFloorAddress) > 0)
                {
                    if (Memory.ReadByte(currentFloorAddress) != prevFloor) // New Floor
                    {
                        Console.WriteLine("New floor");
                        currentFloor = Memory.ReadByte(currentFloorAddress);
                        if (exitError ==  false)
                        {
                            RandomizeEnemies();
                            originalEnemies = false;
                            originalNames = false;
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
                    prevFloor = -1;
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
            if (randomEnemies)
            {
                Memory.WriteString(currentAddress, completeEnemy[0]); // Name, Model, AI
            }
            currentAddress += 0x00000044;
            if (randomSound)
            {
                Memory.WriteString(currentAddress, completeEnemy[1]); // Sound. It's funny to have incorrect noises
            }
            currentAddress += 0x00000004;
            if(randomEnemies)
            {
                Memory.WriteString(currentAddress, completeEnemy[2]); // Writing an unknown value...
            }
            currentAddress += 0x00000004;
            if (randomHP)
            {
                Memory.WriteString(currentAddress, completeEnemy[3]); // HP
            }
            currentAddress += 0x00000004;
            if (randomEnemies)
            {
                Memory.WriteString(currentAddress, completeEnemy[4]); // Enemy family
            }
            currentAddress += 0x00000002;
            if (randomABS)
            {
                Memory.WriteString(currentAddress, completeEnemy[5]); // ABS Dropped
            }
            currentAddress += 0x00000002;
            if (randomGilda)
            {
                Memory.WriteString(currentAddress, completeEnemy[6]); // Gilda Dropped
            }
            currentAddress += 0x00000002;
            if (randomEnemies)
            {
                Memory.WriteString(currentAddress, completeEnemy[7]); // Writing an unknown value...
            }
            currentAddress += 0x00000006;
            if (randomRage)
            {
                Memory.WriteString(currentAddress, completeEnemy[8]); // Rage Counter
            }
            currentAddress += 0x00000002;
            if (randomEnemies)
            {
                Memory.WriteString(currentAddress, completeEnemy[9]); // Writing an unknown value...
            }
            currentAddress += 0x00000004;
            if (randomDamage)
            {
                Memory.WriteString(currentAddress, completeEnemy[10]); // Attack
            }
            currentAddress += 0x00000002;
            if (randomDefense)
            {
                Memory.WriteString(currentAddress, completeEnemy[11]); // Defense
            }
            currentAddress += 0x00000002;
            Memory.WriteUShort(currentAddress, 0); // Not-Boss Flag
            currentAddress += 0x00000002;
            if (randomWeaknesses)
            {
                Memory.WriteString(currentAddress, completeEnemy[13]); // Weaknesses
            }
            currentAddress += 0x00000010;
            if (randomEffectiveness)
            {
                Memory.WriteString(currentAddress, completeEnemy[14]); // Weapon Effectiveness
            }
            currentAddress += 0x00000018;
            if (randomEnemies)
            {
                Memory.WriteString(currentAddress, completeEnemy[15]); // Writing an unknown value...
            }
            currentAddress += 0x0000000C;
            if (randomItems)
            {
                Memory.WriteString(currentAddress, completeEnemy[16]); // Items
            }
            currentAddress += 0x00000006;
            if (randomEnemies)
            {
                Memory.WriteString(currentAddress, completeEnemy[17]); // Writing an unknown value...
            }
            currentAddress += 0x0000000A;
            if (randomEnemies)
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
            while (Memory.ReadByte(0x2037869C) == 0) // 0 is open inventory/cutscene, 1 is moving
            {
                Thread.Sleep(1);
            }

            if (originalEnemies == false) // Only able to reset if the enemies have been changed
            {
                long prevTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                int veryHigh = 0x0BEBC200;
                Thread.Sleep(1000);
                while (Memory.ReadByte(0x20D06FE4) == 0 || Memory.ReadByte(0x20D06FE4) > veryHigh) // Wait until an enemy spawns / allocates first HP address
                {
                    Thread.Sleep(1);

                    // On boss floors no enemy will spawn, breaking monster transforms
                    // If boss floor, force wait for 5 seconds and break waiting while loop
                    if (IsBossFloor())
                    {
                        break;
                    }

                    long currentTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                    // If waiting longer than 5 seconds, break (In case of non-updating dungeon address)
                    if ((currentTime - prevTime) > 5000)
                    {
                        Console.WriteLine("Took longer than 5 secs, exiting");
                        break;
                    }
                }
                Thread.Sleep(3000);
                int currentAddress = 0x2033DA04; // Beginning of all enemy data, starts with "load position"
                if ( !(Memory.ReadByte(currentDungeonAddress) == 0 && Memory.ReadByte(currentFloorAddress) == 7) ) // Channel Reservoir Random Monster transform easter egg
                {
                    Console.WriteLine("Resetting models and AI for monster transforms");
                    for (int i = 0; i < 280; i++)
                    {
                        string originalModelAI = enemyData.Substring(36 + (i * 184), 40);
                        Memory.WriteString(currentAddress, originalModelAI);
                        currentAddress += 0x000000B8;
                    }
                }
            }
            originalEnemies = true;
        }

        private static void ResetNames(string enemyData)
        {
            int currentAddress = 0x2033D9E4;
            Console.WriteLine("Resetting data for monster transform names and bestiary fix");
            for (int i = 0; i < 280; i++)
            {
                string originalName = enemyData.Substring(4 + (i * 184), 32);
                Memory.WriteString(currentAddress, originalName);

                string originalFamily = enemyData.Substring(84 + (i * 184), 2);
                currentAddress += 0x00000050; 
                Memory.WriteString(currentAddress, originalFamily);

                currentAddress += 0x00000002;
                if (!randomABS)
                {
                    string originalABS = enemyData.Substring(86 + (i * 184), 2);
                    Memory.WriteString(currentAddress, originalABS);
                }

                currentAddress += 0x00000002;
                if (!randomGilda)
                {
                    string originalGilda = enemyData.Substring(88 + (i * 184), 2);
                    Memory.WriteString(currentAddress, originalGilda);
                }

                string originalWeakness = enemyData.Substring(108 + (i * 184), 16);
                currentAddress += 0x00000014; 
                Memory.WriteString(currentAddress, originalWeakness);

                string originalEffectiveness = enemyData.Substring(124 + (i * 184), 24);
                currentAddress += 0x00000010; 
                Memory.WriteString(currentAddress, originalEffectiveness);

                string originalItems = enemyData.Substring(160 + (i * 184), 6);
                currentAddress += 0x00000024;
                Memory.WriteString(currentAddress, originalItems);

                string originalHabitat = enemyData.Substring(176 + (i * 184), 2);
                currentAddress += 0x00000010;
                Memory.WriteString(currentAddress, originalHabitat);

                string originalBestiary = enemyData.Substring(178 + (i * 184), 2);
                currentAddress += 0x00000002;
                Memory.WriteString(currentAddress, originalBestiary);

                currentAddress += 0x0000000A; // Moving to next enemy's name
            }
        }

        // Very bad hardcoded method to determine if the floor is a boss floor and the first HP address will not
        // be loaded when it is done generating
        private static bool IsBossFloor()
        {
            int currentDungeon = Memory.ReadByte(currentDungeonAddress);
            int currentFloor = Memory.ReadByte(currentFloorAddress);
            // This design is flawed, if the player moves directly to a non boss floor from the main map, the current dungeon doesn't update.

            // Pump Room: 0, 4
            // Reservoir: 0, 7
            // Rainbow falls: 1, 15
            // Earth gem chamber: 1, 12
            // Barga's Valley: 2, 8
            // Yorda's valley: 2, 18
            // Lighthouse: 2, 23
            // Wind gem chamber: 2, 15
            // Ancient mural: 3, 9
            // Shigura village: 3, 20
            // Water gem chamber: 3, 14
            // mt gundor peak: 4, 16
            // mt gundor mouth: 4, 22
            // fire gem chamber: 4, 8
            // sirus fight?: 5, 0
            // moonflower sun chamber: 5, 27
            // miners breakroom: 6, 5
            // sus opening: 6, 27
            // dark genie: 6, 38 
            // dead end 1: 6, 10
            // dead end 2: 6, 16
            // dead end 3: 6, 21
            // dead end 4: 6, 26

            if ( (currentDungeon == 0 && currentFloor == 4) || (currentDungeon == 0 && currentFloor == 7) || (currentDungeon == 1 && currentFloor == 15) || (currentDungeon == 1 && currentFloor == 12)
                || (currentDungeon == 2 && currentFloor == 8) || (currentDungeon == 2 && currentFloor == 18) || (currentDungeon == 2 && currentFloor == 23) || (currentDungeon == 2 && currentFloor == 15) 
                || (currentDungeon == 3 && currentFloor == 9) || (currentDungeon == 3 && currentFloor == 20) || (currentDungeon == 3 && currentFloor == 14) || (currentDungeon == 4 && currentFloor == 16) 
                || (currentDungeon == 4 && currentFloor == 22) || (currentDungeon == 4 && currentFloor == 8) || (currentDungeon == 5 && currentFloor == 0) || (currentDungeon == 5 && currentFloor == 27) 
                || (currentDungeon == 6 && currentFloor == 5) || (currentDungeon == 6 && currentFloor == 27) || (currentDungeon == 6 && currentFloor == 38) || (currentDungeon == 6 && currentFloor == 10) 
                || (currentDungeon == 6 && currentFloor == 16) || (currentDungeon == 6 && currentFloor == 21) || (currentDungeon == 6 && currentFloor == 26) )
            {
                Console.WriteLine("Boss floor");
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void MainMenuCheck()
        {
            if (Memory.ReadByte(currentDungeonAddress) == 1 && Memory.ReadByte(currentFloorAddress) == 0)
            {
                Console.WriteLine("Mod started on the main menu.");
                while (Memory.ReadByte(currentDungeonAddress) == 1 && Memory.ReadByte(currentFloorAddress) == 0) // Main Menu
                {
                    Thread.Sleep(1);
                }
            }
        }
    }
}
