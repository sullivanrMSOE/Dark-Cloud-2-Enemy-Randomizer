# Dark Cloud 2 Enemy Randomizer

## How To Use:
Open Dark Cloud 2 (USA) with PCSX2. Run the Enemy Randomizer executable, select your desired settings, and press begin at any point outside of a dungeon floor to begin randomizing enemies.

## Warning:
Turbo mode and especially save states can break the mod. Use them at your own risk.
There may be some undetected bugs. Save often!

## Settings:
There are 22 different checkboxes that allow you to customize how and what is randomized. The default settings when the mod is opened are the recommended settings if you want to start a fair playthrough with different enemies instead of the usual ones. 

"Different" settings means that the selected attribute will not be the same as the original enemy's, and will be the randomized enemy that replaces the original enemy's usual attribute.

If "Random" is selected as well, then that selected attribute will be completely random and belong to a completely random enemy.

## Information:
This mod writes over enemy data in memory to randomize enemies. Monica's monster transformation relies on the same data in memory that the dungeons do, so the mod reverts the data back after a dungeon floor is generated in order to not break her transformations. It is recommend to wait at least a few seconds (recommend roughly 10) before attempting to transform into a monster when first entering a floor, especially on boss floors, or the game may softlock when transforming or attempting to attack.

Gemrons and Elementals rely on their position in memory in order to shoot their projectiles. In order to not break this attack, only gemrons are randomized with other gemrons, and the same applies to elemental spirits. Enemies that spawn new enemies, such as Tores and Pirate Chariots (sometimes), also suffer from a similar fate. However, since their primary attack is more threatening and they each have significantly less variety, I decided not to randomize them within their own enemy type.

The bestiary is intended to be the same as vanilla and does not reflect randomization.

In order to upgrade a weapon that requires certain enemies to be defeated, you must defeat the enemy that replaces the one that is usually required, i.e. clearing floor 1 of Mt. Gundor will mark Rifle Wolf as being defeated.

Based on testing, enemy-based scoops will work regardless, as long as the relevant enemy spawns. Note: It seems later-game scoops such as "Faintin' Bone Lord" can only be gotten once the correct chapter is reached.

## Bug Reports:
There are several bugs that arose naturally from switching data around in memory and the systems in place to also ensure monster transformations work. If you find any gamebreaking bugs with this mod, create a GitHub issue or message me on Discord at Ridepod#0685.

## Credit:
WorldOfWind for helping me utilize Cheat Engine to find memory addresses and the Chest Randomizer code that this was built off of.
