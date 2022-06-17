# Dark Cloud 2 Enemy Randomizer

This mod randomizes enemies in Dark Cloud 2 (USA), emulated through PCSX2. Created by Ridepod.

## How To Use

Open Dark Cloud 2 (USA) with PCSX2. Run the executable and press begin at any point outside of a dungeon floor to begin randomizing enemies.

## Information

This mod writes over enemy data in memory to randomize enemies. Monica's monster transformation relies on the same data in memory that the dungeons do, so the mod reverts the data back 5 seconds after a dungeon floor is generated in order to not break her transformations. It is recommend to wait at least a few seconds before attempting to transform into a monster when first entering a floor, or the game may freeze and softlock.
Additionally, the Bestiary is random and inaccurate when this mod is used. 

Gemrons and Elementals rely on their position in memory in order to shoot their projectiles. In order to not break this attack, only gemrons are randomized with another gemron, and the same applies to elemental spirits. Enemies that spawn new enemies, such as Tores and Pirate Chariots, also suffer from a similar fate. However, since their primary attack is more threatening, I decided not to randomize them within their own enemy type.

Using save states may break the mod, and are not recommended. If the mod is closed at any point after beginning, it is recommended to reset the game before resuming the mod.

Enemies maintain their resistance to being stunned, so enemies that attempt to replace enemies such as King Mimics may pose a substantial threat. 

## Settings

The executable provides several different options to not randomize, in order to keep the game balanced. By default, HP, ABS, Gilda, Attack, and Defense are not randomized. 
The options to keep the same include:
- Enemy
- Sounds
- HP
- ABS
- Gilda
- Rage Counter
- Attack
- Defense
- Weaknesses
- Weapon Effectiveness
- Items

## Credit

WorldOfWind for helping me utilize Cheat Engine to find memory addresses and the Chest Randomizer code that this was built off of.

## Bug Reports

If you find any bugs with this mod, create a GitHub issue or message me at Ridepod#0685.



# Have Fun!
