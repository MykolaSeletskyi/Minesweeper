using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Minesweeper
{

    partial class Miner
    {
        public class EndGame
        {
            static readonly Image imageBomb = Image.FromFile(@"..\..\..\png\bomb.png");
            static readonly Image gameOver = Image.FromFile(@"..\..\..\png\gameOver.png");
            static Timer timerBomb = new Timer();
            static Timer timerImage = new Timer();
            static UInt16 counter = 0;
            static UInt16 indexOpenBomb;
            static EndGame()
            {
                timerBomb.Interval = 200;
                timerBomb.Tick += showBombs;
                timerImage.Interval = 500;
                timerImage.Tick += showImage;
            }
            static public void GameOver(Element openBomb)
            {
                Miner.GetInstance().menu.gameAndStopClick(null, null);
                openBomb.Enabled = false;
                openBomb.FlatStyle = FlatStyle.Flat;
                openBomb.BackgroundImage = imageBomb;
                openBomb.BackColor = Color.FromArgb(255, 0, 0);
                openBomb.BackgroundImageLayout = ImageLayout.Zoom;
                indexOpenBomb = (UInt16)Miner.GetInstance().elementsBomb.IndexOf(openBomb);
                timerBomb.Start();
                timerImage.Start();
            }
            static void showBombs(object obj, EventArgs e)
            {
                List<Element> bombs = Miner.GetInstance().elementsBomb;
                if (counter >= Miner.quantityBombs)
                {
                    counter = 0;
                    timerBomb.Stop();
                    return;
                }
                bombs[counter].Enabled = false;
                bombs[counter].FlatStyle = FlatStyle.Flat;
                bombs[counter].BackgroundImage = imageBomb;
                bombs[counter].BackgroundImageLayout = ImageLayout.Zoom;
                ++counter;
                if (indexOpenBomb == counter)
                {
                    ++counter;
                }
            }
            static void showImage(object obj, EventArgs e)
            {
                timerImage.Stop();
                Form tmp = new Form();
                tmp.Deactivate += Tmp_Deactivate;
                tmp.Show();
                tmp.Size = new Size(Miner.GetInstance().Size.Width, Miner.GetInstance().Size.Height - Miner.GetInstance().menu.Height);
                tmp.Location = new Point(Miner.GetInstance().Location.X, Miner.GetInstance().Location.Y + Miner.GetInstance().menu.Height);
                tmp.TransparencyKey = Color.White;
                tmp.Opacity = 100;
                tmp.BackColor = Color.White;
                tmp.FormBorderStyle = FormBorderStyle.None;
                tmp.WindowState = Miner.GetInstance().WindowState;
                tmp.BackgroundImage = gameOver;
                tmp.BackgroundImageLayout = ImageLayout.Zoom;
            }

            private static void Tmp_Deactivate(object sender, EventArgs e)
            {
                               ((Form)sender).Close();

            }
        }

    }

}

