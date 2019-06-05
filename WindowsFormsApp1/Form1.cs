using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        GameField PlayerField = null; 
        GameField EnemyField = null;
        public Network networker = null;
        public bool EnemyIsReady = false;
        public bool Turn = false;

        private int b4 = 1;
        private int b3 = 2;  //fastfix for buttons locker
        private int b2 = 3;  //need to rewrite
        private int b1 = 4; 
                            

        public Form1()
        {
            networker = new Network(this);
            PlayerField = new GameField(50, 50, this);
            EnemyField = new GameField(470, 50, this);
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PlayerField.boatPlacer.SetLength(4);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PlayerField.boatPlacer.SetLength(3);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PlayerField.boatPlacer.SetLength(2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PlayerField.boatPlacer.SetLength(1);
        }


        public void BoatPlacerControl(int length)
        {
            if (length == 4) b4--;
            if (length == 3) b3--;
            if (length == 2) b2--;
            if (length == 1) b1--;
            if (b4 == 0) this.button1.Enabled = false;
            if (b3 == 0) this.button2.Enabled = false;
            if (b2 == 0) this.button3.Enabled = false;
            if (b1 == 0) this.button4.Enabled = false;
        }


        private void button6_Click(object sender, EventArgs e)
        {
            PlayerField.boatPlacer.Rotate();
            if (button6.Text == "horizontal") button6.Text = "vertical";
            else button6.Text = "horizontal";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (b4 + b3 + b2 + b1 <= 0)
            {
                this.button5.Enabled = false;
                networker.TransmitMessage("Start");
                if (!EnemyIsReady) Turn = true;
                SetLabel("Wait for opponent");
                while (!EnemyIsReady);
                if (Turn) SetLabel("Your turn");
                else SetLabel("Enemy turn");
                PlayerField.SetFieldReady(false);
                EnemyField.SetFieldReady(true);
            }
            else
            {
                this.label1.Text = "Place all your ships";
            }
        }


        private int lastEnemyPos = 0;
        public void StrikeEnemy(int pos)
        {
            lastEnemyPos = pos;
            networker.TransmitMessage((pos+1).ToString());
        }


        private void NextTurn()
        {
            Turn = !Turn;
            if (Turn)
            {
                SetLabel("Your turn");
            }
            else
            {
                SetLabel("Enemy turn");
            }
        }


        public void ConfirmEnemyStrike(int val)
        {
            EnemyField.Field[(int)(lastEnemyPos / 10), lastEnemyPos % 10] = -val;
            EnemyField.UpdateCheckBoxes();
            if (EnemyField.IsGameEnd())
            {
                Enabled = false;
                SetLabel("You win!");
            }
            NextTurn();
        }


        public void StrikePlayer(int pos)
        {
            PlayerField.Strike((int)(pos / 10), pos % 10);
            networker.TransmitMessage((-PlayerField.TileState((int)(pos / 10), pos % 10)).ToString());
            PlayerField.UpdateCheckBoxes();
            if (PlayerField.IsGameEnd())
            {
                Enabled = false;
                SetLabel("You lose");
            }
            NextTurn();
        }


        


        private void Form1_Load(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(this);
            SetLabel("Wait for connect");
            this.Enabled = false;
            form2.Show();
        }


        public void SetLabel(string text)
        {
            label1.Text = text;
            label1.Refresh();
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            networker.StopReciveThread();
        }
    }
}
