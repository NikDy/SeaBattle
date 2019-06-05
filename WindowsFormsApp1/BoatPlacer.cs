using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class BoatPlacer
    {
        private int Length = 0;
        private bool Direction = true; // true - vertical ; false - horizontal

        public BoatPlacer(int length)
        {
            Length = length;
        }

        public int GetLength()
        {
            return Length;
        }

        public bool Place(GameField field, int x, int y)
        {
            if (Length > 0 && x + Length <= 10 && y + Length <= 10)
            {
                for (int i = 0; i < Length; i++)
                {
                    if (Direction) field.Field[x, y + i] = 1;
                    else field.Field[x + i, y] = 1;
                }
                LockBoatSides(field);
                return true;
            }
            else return false;
        }

        private void LockBoatSides(GameField field)
        {
            for(int i = 0; i < 10; i++)
            {
                for(int j = 0; j < 10; j++)
                {
                    if(field.Field[i,j] == 1)
                    {
                        if (field.Field[SafeDec(i), j] != 1) field.Field[SafeDec(i), j] = 2;
                        if (field.Field[SafeInc(i), j] != 1) field.Field[SafeInc(i), j] = 2;
                        if (field.Field[i, SafeDec(j)] != 1) field.Field[i, SafeDec(j)] = 2;
                        if (field.Field[i, SafeInc(j)] != 1) field.Field[i, SafeInc(j)] = 2;
                        if (field.Field[SafeDec(i), SafeDec(j)] != 1) field.Field[SafeDec(i), SafeDec(j)] = 2;
                        if (field.Field[SafeInc(i), SafeDec(j)] != 1) field.Field[SafeInc(i), SafeDec(j)] = 2;
                        if (field.Field[SafeDec(i), SafeInc(j)] != 1) field.Field[SafeDec(i), SafeInc(j)] = 2;
                        if (field.Field[SafeInc(i), SafeInc(j)] != 1) field.Field[SafeInc(i), SafeInc(j)] = 2;
                    }
                }
            }
        }

        private int SafeDec(int val)
        {
            int Res = val - 1;
            if (Res >= 0) return Res;
            else return val;
        }

        private int SafeInc(int val)
        {
            int Res = val + 1;
            if (Res < 10) return Res;
            else return val;
        }

        public void SetLength(int length)
        {
            Length = length;
        }

        public void Rotate()
        {
            Direction = !Direction;
        }
    }
}
