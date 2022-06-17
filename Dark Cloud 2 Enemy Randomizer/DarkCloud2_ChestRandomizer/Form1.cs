using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace DarkCloud2_EnemyRandomizer
{
    public partial class Form1 : Form
    {
        static bool randomEnemiesChoice = false;
        static bool randomSoundChoice = false;
        static bool randomHPChoice = true;
        static bool randomDefenseChoice = true;
        static bool randomABSChoice = true;
        static bool randomGildaChoice = true;
        static bool randomRageChoice = false;
        static bool randomWeaknessesChoice = false;
        static bool randomEffectivenessChoice = false;
        static bool randomItemsChoice = false;
        static bool randomAttackChoice = true;
        static bool skipGemronsAndElementalsChoice = false;

        public bool gameCheck = false;
        public static bool versionCheck;
        public Form1()
        {
            InitializeComponent();
            if (Memory.PID != 0)
            {
                label2.Text = "This mod randomizes enemies in Dark Cloud 2. You must be using PCSX2 and have Dark Cloud 2 (USA).";

                if (Memory.ReadInt(0x203694D0) == 1701667175)
                {
                    gameCheck = false;
                    Randomizer.gameVersion = 1;
                    button1.Enabled = false;
                    label3.Text = "Detected Dark Chronicle (PAL version)! However, this mod only works with the USA version. Sorry.";
                }
                else if (Memory.ReadInt(0x20364BD0) == 1701667175)
                {
                    gameCheck = true;
                    Randomizer.gameVersion = 2;
                    label3.Text = "Detected Dark Cloud 2 (USA version)! Press begin to start randomizing enemies.";
                }
                else
                {
                    button1.Enabled = false;
                    label3.Text = "Cannot find active Dark Cloud 2 game, please launch Dark Cloud 2 and restart this mod.";
                }
            }
            else
            {
                label3.Enabled = false;
                button1.Enabled = false;               
            }
        }

        public static Thread enemyThread = new Thread(() => Randomizer.EnemyRandomizer(Form1.randomEnemiesChoice, Form1.randomSoundChoice, Form1.randomHPChoice, 
            Form1.randomDefenseChoice, Form1.randomAttackChoice, Form1.randomABSChoice, Form1.randomGildaChoice, Form1.randomRageChoice, Form1.randomWeaknessesChoice,
            Form1.randomEffectivenessChoice, Form1.randomItemsChoice));

        private void button1_Click(object sender, EventArgs e)
        {
            // if every choice is true, reject begin
            if(randomEnemiesChoice && randomSoundChoice && randomHPChoice && randomDefenseChoice && randomAttackChoice && randomABSChoice 
                && randomGildaChoice && randomRageChoice && randomWeaknessesChoice && randomEffectivenessChoice && randomItemsChoice)
            {
                MessageBox.Show("If you're going to disable all randomization, why are you running this mod?", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                label3.Text = "Randomizing enemies! Do not use save states, as they can break the mod. If or when you exit the game, this program closes automatically.";
                label3.Font = new Font(label3.Font, FontStyle.Bold);
                button1.Enabled = false;
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
                checkBox3.Enabled = false;
                checkBox4.Enabled = false;
                checkBox5.Enabled = false;
                checkBox6.Enabled = false;
                checkBox7.Enabled = false;
                checkBox8.Enabled = false;
                checkBox9.Enabled = false;
                checkBox10.Enabled = false;
                checkBox11.Enabled = false;
                if (!enemyThread.IsAlive) //If we are not already running
                    enemyThread.Start(); //Start thread
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            randomEnemiesChoice = !randomEnemiesChoice;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            randomSoundChoice = !randomSoundChoice;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            randomHPChoice = !randomHPChoice;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            randomABSChoice = !randomABSChoice;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            randomGildaChoice = !randomGildaChoice;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            randomRageChoice = !randomRageChoice;
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            randomAttackChoice = !randomAttackChoice;
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            randomDefenseChoice = !randomDefenseChoice;
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            randomWeaknessesChoice = !randomWeaknessesChoice;
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            randomEffectivenessChoice = !randomEffectivenessChoice;
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            randomItemsChoice = !randomItemsChoice;
        }
    }
}
