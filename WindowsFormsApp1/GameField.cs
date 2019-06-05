using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class GameField
    {
        public int[,] Field = new int [10,10];
        private Form1 parentForm = null;
        public BoatPlacer boatPlacer = null;
        List<System.Windows.Forms.CheckBox> checkBoxes = new List<System.Windows.Forms.CheckBox>(100);


        //0 - water; 1- boat; 2 - locked; 3 - destroyed boat
        public GameField(int x, int y, Form1 form)
        {
            parentForm = form;
            boatPlacer = new BoatPlacer(0);

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Field[i, j] = 0;
                }
            }

            for(int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    checkBoxes.Add(new System.Windows.Forms.CheckBox());
                    checkBoxes[i * 10 + j].Tag = (i * 10 + j).ToString() + this.ToString();
                    checkBoxes[i * 10 + j].Name = (i * 10 + j).ToString();
                    checkBoxes[i * 10 + j].AutoSize = true;
                    checkBoxes[i * 10 + j].Location = new Point(x + i * 20, y + j * 20);
                    checkBoxes[i * 10 + j].Click += GameField_CheckedChanged;
                    parentForm.Controls.Add(checkBoxes[i * 10 + j]);
                }
            }

        }

        public void UpdateCheckBoxes()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (Field[i, j] == 0) {checkBoxes[i * 10 + j].Checked = false; checkBoxes[i * 10 + j].Enabled = true; }
                    if (Field[i, j] == 1) { checkBoxes[i * 10 + j].Checked = true; checkBoxes[i * 10 + j].Enabled = true; }
                    if (Field[i, j] == 2) { checkBoxes[i * 10 + j].Checked = false; checkBoxes[i * 10 + j].Enabled = false; }
                    if (Field[i, j] == 3) { checkBoxes[i * 10 + j].Checked = true; checkBoxes[i * 10 + j].Enabled = false; }
                }
            }
        }

        public void SetFieldReady(bool isActive)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if(Field[i, j] != 1) Field[i, j] = 0;
                    checkBoxes[i * 10 + j].Click -= GameField_CheckedChanged;
                    if(isActive) checkBoxes[i * 10 + j].Click += GameField_PlayCheck;
                }
            }
            UpdateCheckBoxes();
        }


        public void SetFieldActive()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    checkBoxes[i * 10 + j].Click += GameField_PlayCheck;
                    checkBoxes[i * 10 + j].Click -= GameField_SafeCheck;
                }
            }
        }


        public void SetFieldUnactive()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    checkBoxes[i * 10 + j].Click += GameField_SafeCheck;
                    checkBoxes[i * 10 + j].Click -= GameField_PlayCheck;
                }
            }
        }


        public bool IsGameEnd()
        {
            int counter = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if(Field[i, j] == 3) counter++;
                }
            }
            if (counter == 20) return true;
            else return false;
        }


        public int Strike(int x, int y)
        {
            if (Field[x, y] == 1)
            {
                Field[x, y] = 3;
                return 3;
            }
            if (Field[x, y] == 0)
            {
                Field[x, y] = 2;
                return 2;
            }
            UpdateCheckBoxes();
            return 0;
        }


        


        public int TileState(int x, int y)
        {
            return Field[x, y];
        }


        private void GameField_PlayCheck(object sender, EventArgs e)
        {
            if (parentForm.Turn)
            {
                System.Windows.Forms.CheckBox box = (System.Windows.Forms.CheckBox)sender;
                int.TryParse(box.Name, out int Num);
                parentForm.StrikeEnemy(Num);
            }
        }


        private void GameField_SafeCheck(object sender, EventArgs e)
        {
            System.Windows.Forms.CheckBox box = (System.Windows.Forms.CheckBox)sender;
            box.Checked = !box.Checked;
            sender = box;
        }


        private void GameField_CheckedChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.CheckBox box = (System.Windows.Forms.CheckBox)sender;
            int.TryParse(box.Name, out int Num);
            if (boatPlacer.Place(this, (int)(Num / 10), Num % 10))
            {
                parentForm.BoatPlacerControl(boatPlacer.GetLength());
                boatPlacer.SetLength(0);
            }
            UpdateCheckBoxes();
        }
    }
}
