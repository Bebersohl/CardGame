using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardGame
{
    public class Square
    {
        Rectangle _squareDim;
        int _row;
        int _column;
        int _cardKind;
        Boolean _open;
        int squareCardNumber;

        public Square(Rectangle r, int x, int y, int kind, Boolean b)
        {
            _squareDim = r;
            _row = x;
            _column = y;
            _cardKind = kind;
            _open = b;
            squareCardNumber = 0;
        }

        public Rectangle squareDim
        {
            get { return _squareDim; }
            
        }
        public int SquareCardNumber
        {
            get { return squareCardNumber; }
            set { squareCardNumber = value; }
        }
        public int row
        {
            get { return _row; }
            set { _row = value; }
        }

        public int column
        {
            get { return _column; }
            set { _column = value; }
        }

        public int cardKind
        {
            get { return _cardKind; }
            set { _cardKind = value; }
        }

        public Boolean open
        {
            get { return _open; }
            set { _open = value; }
        }


    }
}
