/*using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace OpenWindow
{
    internal class Program
    {
        public static Form window;
        public static string spritesPath;
        public static string cardLibraryPath;
        public static object _lock = new object();

        public static void Main(string[] args)
        {
            Console.WriteLine("Deck Name:");
            string deckName = Console.ReadLine();
            spritesPath = "Assets/Deck/"+deckName+"/Sprite";
            cardLibraryPath = "Assets/Deck/CardLibrary/Sprite";
            window = new Form();
            window.Height = 400;
            window.Width = 1500;
            DisplayImages();
            window.ShowDialog();
        }

        public static void DisplayImage(string cardName,int LeftOffSet,int id)
        {
            string cardSpriteLocation = spritesPath+"/" + cardName + ".jpeg";
            string cardLibraryLocation = cardLibraryPath +"/" + cardName + ".jpeg";
            
            Console.WriteLine("Try Display :" + cardName);
            Image sprite = null;
            if (File.Exists(cardSpriteLocation))
            {
                sprite = Image.FromFile(cardSpriteLocation);
            }else if (File.Exists(cardLibraryLocation))
            {
                sprite = Image.FromFile(cardLibraryLocation);
            }
            else
            {
                Console.WriteLine(cardSpriteLocation + " not found");
                Console.WriteLine(cardLibraryLocation + " not found");
                return;
            }


            PictureBox img = new PictureBox();
            img.Image = sprite;
            img.Height = 222;
            img.Width = 156;
            img.SizeMode = PictureBoxSizeMode.StretchImage;
            img.Left = LeftOffSet;
            window.Controls.Add(img);

            string[] buttons = {"Land", "Graveyard", "Creature", "Exil", "Enchantement", "Deck"};
            bool left = false;
            bool nextLine = false;
            int topOff = 222;
            for (int i = 0; i < buttons.Length; i++)
            {
                int leftOff = LeftOffSet;
                if (left)
                {
                    leftOff += 156 / 2;
                    left = false;
                    nextLine = true;
                }
                else
                {
                    left = true;
                }
                
                CreateButton(buttons[i],topOff,leftOff,id);

                if (nextLine)
                {
                    topOff += 20;
                    nextLine = false;
                }
            }
        }
        
        private static void CreateButton(string buttonName,int Top,int Left,int id)
        { 
            Button button1 = new Button();
            button1.Text = buttonName;
            button1.Width = 156 / 2;
            button1.Top = Top;
            button1.Left = Left;
            button1.Click += ((sender, args) =>
            {
                WriteAction("Hand "+ id + " " +buttonName);
                WindowClearAndRefresh(sender, args);
            });
            window.Controls.Add(button1);
        }

        public static void DisplayImages()
        {
            Button button1 = new Button();
            button1.Text = "Reset";
            button1.Top = 0;
            button1.Click += ((sender, args) =>
            {
                WindowClearAndRefresh(sender, args);
            });
            window.Controls.Add(button1);
            
            lock (_lock)
            {
                Console.WriteLine("Coucou");
                string[] socket = File.ReadAllLines("Assets/Socket/SocketToHand.txt");
                string[] cardsName = socket[0].Split(' ');
                for (int i = 0; i < cardsName.Length; i++)
                {
                    DisplayImage(cardsName[i],i*156,i-1);
                }
            }
        }

        public static void WriteAction(string action)
        {
            lock (_lock)
            {
                File.WriteAllText("Assets/Socket/SocketToUnity.txt",action);
            }
        }
        
        public static void WindowClearAndRefresh(object sender, EventArgs args)
        {
            Thread.Sleep(200);
            window.Controls.Clear();
            DisplayImages();
            window.Refresh();
        }
    }
}*/