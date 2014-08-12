using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;

namespace FloodControl
{
    class GamePiece
    {
        public static string[]PieceTpyes =
        {
            "Left,Right",
            "Top,Botton",
            "Left,Top",
            "Right,Bottom",
            "Bottom,Left",
            "Empty"
        };
        
        public const int PieceHeight = 40; //一节水管的图形的高度
        public const int PieceWight =40; //宽度
        
        public const int MaxPlayablePieceIndex = 5;
        public const int EmptyPieceIndex = 6;


    }
}
