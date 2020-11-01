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
    class Element : Button
    {
        readonly static Color enabledColor = Color.FromArgb(199, 199, 199);
        readonly static Color disabledColor = Color.FromArgb(153, 153, 153);
        public const byte TypeNull = 0;
        public const byte TypeBomb = 9;
        public Element(UInt16 x = 0, UInt16 y = 0) : base()
        {
            this.Show();
            this.Type = TypeNull;
            this.Font = new Font("Arial", SizeElements / 2);
            this.Size = new Size(SizeElements, SizeElements);
            this.Location = new Point(x, y);
            Miner.GetInstance().Controls.Add(this);
            this.TextAlign = ContentAlignment.TopCenter;
            this.MouseUp += Miner.GetInstance().ClickElemet;
            this.Enabled = false;
            this.TabStop = false;
            this.Text = "";
            this.ForeColor = Color.FromArgb(0, 0, 0);
        }
        static byte sizeElements = 30;
        static public byte SizeElements
        {
            get => sizeElements;
            set => sizeElements = value;
        }
        public byte Type { get; set; }
        bool marker = false;
        public bool Marker
        {
            get => marker;
            set => marker = value;
        }
        bool open = false;
        public bool Open
        {
            get => open;
            set
            {
                if (value == true && Marker == false)
                {
                    open = true;
                    this.Enabled = false;
                    if (value == true && Marker == false)
                    {
                        if (this.Type == TypeBomb)
                        {
                            Miner.EndGame.GameOver(this);
                        }
                       else  if (this.Type != TypeNull)
                        {
                            this.Text = Type.ToString();
                        }
                    }
                }
                if (value == false)
                {
                    open = false;
                }
            }
        }
        public new bool Enabled
        {
            get => base.Enabled;
            set
            {
                this.FlatStyle = value ? FlatStyle.Standard : FlatStyle.Flat;
                this.BackColor = value ? enabledColor : disabledColor;
                base.Enabled = value;
            }
        }
    }
    partial class Miner : Form
    {
        static public UInt16 quantityBombs = 8;
        static public UInt16 quantityOpen = 0;
        static public UInt16 quantityElement = 15;
        Element[,] elements;
        List<Element> elementsBomb;
        Menu menu = new Menu();
        EndGame endGame;
        private System.ComponentModel.IContainer components = null;
        private Miner()
        {
            //  this.DoubleBuffered = false;
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Text = "Miner";
            endGame = new EndGame();
            this.Controls.Add(menu);
            menu.Show();
            this.Size = new Size(Element.SizeElements * quantityElement, Element.SizeElements * (int)quantityElement + menu.Height);
            this.Load += new System.EventHandler(this.Miner_Load);
        }
        private void Miner_Load(object sender, EventArgs e)
        {
            addElements();
        }
        private static Miner instance;
        public static Miner GetInstance()
        {
            if (instance == null)
            {
                instance = new Miner();
            }
            return instance;
        }
        void start()
        {
            if (quantityOpen!=0)
            {
                clearElements();
            }
            if (elements.Length != quantityElement * quantityElement)
            {
                this.Controls.Clear();
                this.Controls.Add(menu);
                addElements();
            }
            addBombs(quantityBombs);
            placementOfValues();
            for (int i = 0; i < quantityElement; i++)
            {
                for (int j = 0; j < quantityElement; j++)
                {
                    elements[i, j].Open = false;
                    elements[i, j].Enabled = true;
                    elements[i, j].FlatStyle = FlatStyle.Standard;
                }
            }
        }
        void clearElements()
        {
            quantityOpen = 0;
            for (int i = 0; i <  elements.GetLength(0); i++)
            {
                for (int j = 0; j < elements.GetLength(1); j++)
                {
                    elements[i, j].FlatStyle = FlatStyle.Flat;
                    elements[i, j].Text = "";
                    elements[i, j].Type = 0;
                    elements[i, j].Enabled = false;
                }
            }
            for (int i = 0; i < elementsBomb.Count; i++)
            {
                elementsBomb[i].BackgroundImage = null;
            }
        }
        void addElements()
        {
            this.Size = new Size(Element.SizeElements * quantityElement, Element.SizeElements * (int)quantityElement + menu.Height);
            // добавленя елементів
            elements = new Element[quantityElement, quantityElement];
            for (int i = 0; i < quantityElement; i++)
            {
                for (int j = 0; j < quantityElement; j++)
                {
                    elements[i, j] = new Element(Convert.ToUInt16(Element.SizeElements * i), Convert.ToUInt16(Element.SizeElements * j + menu.Height));
                }
            }
        }
        void addBombs(int Bombs)
        {
            //добавленя бомб
            elementsBomb = new List<Element>(quantityBombs);
            Random rnd = new Random();
            int x, y;
            for (uint i = 0; i < quantityBombs; i++)
            {
                x = rnd.Next(0, quantityElement);
                y = rnd.Next(0, quantityElement);
                if (elements[x, y].Type == Element.TypeBomb)//якщо зрандомило одну ту саму позицію
                {
                    i--;
                    continue;
                }
                elementsBomb.Add(elements[x, y]);
                elements[x, y].Type = Element.TypeBomb;
            }
        }
        void checPlacementOfValues(int x, int y, ref Int16 counter)
        {
            if (x < 0 || x > quantityElement - 1 || y < 0 || y > quantityElement - 1)
                return;
            if (elements[x, y].Type == Element.TypeBomb)
            {
                counter++;
            }
        }
        void placementOfValues()
        {
            //розміщення значень (placement of values)
            Int16 counter = 0;
            for (int i = 0; i < quantityElement; i++)
            {
                for (int j = 0; j < quantityElement; j++)
                {
                    if (elements[i, j].Type != Element.TypeBomb)
                    {
                        checPlacementOfValues(i - 1, j - 1, ref counter);
                        checPlacementOfValues(i, j - 1, ref counter);
                        checPlacementOfValues(i + 1, j - 1, ref counter);
                        checPlacementOfValues(i + 1, j, ref counter);
                        checPlacementOfValues(i + 1, j + 1, ref counter);
                        checPlacementOfValues(i, j + 1, ref counter);
                        checPlacementOfValues(i - 1, j + 1, ref counter);
                        checPlacementOfValues(i - 1, j, ref counter);
                        if (counter == 0)
                        {
                            elements[i, j].Type = Element.TypeNull;
                        }
                        else
                        {
                            elements[i, j].Type = (byte)counter;
                            counter = 0;
                        }
                    }
                }
            }
        }
        public void ClickElemet(object obj, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!((Element)obj).Marker)
                {
                    ClickOpenElement(obj, e);
                }
                return;
            }
            addMarker(obj, e);
        }
        void addMarker(object obj, MouseEventArgs e)
        {
            Element tmp = (Element)obj;
            if (tmp.Marker)
            {
                tmp.Text = "";
                tmp.ForeColor = default(Color);
                tmp.Marker = false;
            }
            else
            {
                tmp.Text = "!";
                tmp.ForeColor = Color.Red;
                tmp.Marker = true;
            }
        }
        void ClickOpenElementHelper(int x, int y)
        {
            if (x >= 0 && x < quantityElement && y >= 0 && y < quantityElement && !elements[x, y].Open)
            {
                ClickOpenElement(elements[x, y], null);
            }
        }
        void ClickOpenElement(object obj, MouseEventArgs e)
        {

            Element tmp = (Element)obj;
            int x = tmp.Location.X / Element.SizeElements;
            int y = (tmp.Location.Y - menu.Height) / Element.SizeElements;
            if (tmp.Open)
            {
                return;
            }
            if (tmp.Type == Element.TypeNull)
            {
                elements[x, y].Open = true;
                ClickOpenElementHelper(x - 1, y - 1);
                ClickOpenElementHelper(x, y - 1);
                ClickOpenElementHelper(x + 1, y - 1);
                ClickOpenElementHelper(x + 1, y);
                ClickOpenElementHelper(x + 1, y + 1);
                ClickOpenElementHelper(x, y + 1);
                ClickOpenElementHelper(x - 1, y + 1);
                ClickOpenElementHelper(x - 1, y);
            }
            tmp.Open = true;
            quantityOpen++;
            if (elements.Length == quantityOpen + quantityBombs)
            {
                //finish
                this.Close();
            }
        }
    }

}
