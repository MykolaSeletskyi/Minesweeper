using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
namespace Minesweeper
{
    partial class Miner
    {
        class Menu:GroupBox
    {
        Label exit = new Label();
        Label label1 = new Label();
        Label label2 = new Label();
        NumericUpDown bombsUpDown = new NumericUpDown();
       NumericUpDown countElementsUpDown = new NumericUpDown();
         Button btnGameAndStop = new Button();
            public Menu() : base()
        {
                this.Font = new System.Drawing.Font("Segoe UI",1, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);

                this.Size = new Size(240, 64);
                this.Location = new Point(0, 0);

                this.exit.Font = new System.Drawing.Font("Segoe UI", 15, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                this.exit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
                this.exit.AutoSize = true;
                this.exit.Text = "X";
                this.Controls.Add(this.exit);
                this.exit.Show();
                this.exit.Click += exitClick;
                this.exit.Location = new System.Drawing.Point(this.Size.Width - this.exit.Size.Width-5, 5);

                this.label1.Font= this.label2.Font  = new System.Drawing.Font("Segoe UI", 7, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                this.label1.AutoSize = this.label2.AutoSize =  true;
                this.label1.Location = new System.Drawing.Point(4, 13);
                this.label2.Location = new System.Drawing.Point(44,13);
                this.label1.Text = "Bombs";
                this.label2.Text = "Elements";
                this.Controls.Add(this.label1);
                this.Controls.Add(this.label2);
                this.label1.Show();
                this.label2.Show();

                this.bombsUpDown.AutoSize = true;
                this.bombsUpDown.Font = new System.Drawing.Font("Segoe UI", 7, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                this.bombsUpDown.Location = new System.Drawing.Point(5,30);
                this.bombsUpDown.Minimum = 1;
                this.bombsUpDown.Maximum = Miner.quantityElement * Miner.quantityElement - 1;
                this.bombsUpDown.Name = "Bombs";
                this.bombsUpDown.Size = new System.Drawing.Size(6,20);
                this.bombsUpDown.Value = (decimal)8;
                this.Controls.Add(this.bombsUpDown);
                this.bombsUpDown.Show();
                this.bombsUpDown.ValueChanged += editBombs;

                this.countElementsUpDown.AutoSize = true;
                this.countElementsUpDown.Font = new System.Drawing.Font("Segoe UI", 7, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                this.countElementsUpDown.Location = new System.Drawing.Point(44,30);
                this.countElementsUpDown.Minimum = 12;
                this.countElementsUpDown.Maximum = 30;
                this.countElementsUpDown.Name = "Elements";
                this.countElementsUpDown.Size = new System.Drawing.Size(31,20);
                this.countElementsUpDown.Value = (decimal)15;
                this.Controls.Add(this.countElementsUpDown);
                this.countElementsUpDown.ValueChanged += editcountElements;


                this.btnGameAndStop.Font = new System.Drawing.Font("Segoe UI", 15, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                this.btnGameAndStop.Size = new System.Drawing.Size(80,40);
                this.btnGameAndStop.Text = "Game";
                this.btnGameAndStop.Show();
                this.btnGameAndStop.Location = new System.Drawing.Point(90,10);
                this.Controls.Add(this.btnGameAndStop);
                this.btnGameAndStop.Click += gameAndStopClick;
            }
            public void gameAndStopClick(object sender, EventArgs e)
            {
                if (btnGameAndStop.Text == "Game")
                {
                    bombsUpDown.Enabled = false;
                    countElementsUpDown.Enabled = false;
                    Miner.GetInstance().start();
                    btnGameAndStop.Text = "Stop";
                }
                else
                {
                        btnGameAndStop.Text = "Game";
                        bombsUpDown.Enabled = true;
                        countElementsUpDown.Enabled = true;
                    if (sender!=null)
                    {
                        Miner.GetInstance().clearElements();                   
                    }
                }
            }
            private void editcountElements(object sender, EventArgs e)
            {
                Miner.quantityElement=(UInt16)(countElementsUpDown.Value);
            }
            private void editBombs(object sender, EventArgs e)
            {
                Miner.quantityBombs = (UInt16)bombsUpDown.Value;
            }
            private void exitClick(object sender, EventArgs e)
            {
                Miner.GetInstance().Close();
            }
        }
}
}