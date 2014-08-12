using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
//using System.Drawing;


namespace FloodControl
{
    class GamePieces
    {
        public static string[] PieceTypes =
        {
            "Left,Right",
            "Top,Bottom",
            "Left,Top",
            "Right,Bottom",
            "Bottom,Left",
            "Empty"
        };

        public const int PieceHeight = 40;
        public const int PieceWidth = 40;

        public const int MaxPlayablePieceIndex = 5;
        public const int EmptyPieceIndex = 6;

        private const int textureOffsetX = 1;
        private const int textureOffsetY = 1;
        private const int texturePaddingX = 1;
        private const int texturePaddingY = 1;

        private string pieceType = "";   //管道类型 5种之一
        private string pieceSuffix = ""; //是否注水

        public string PieceType
        {
            get { return pieceType; }
        }

        public string PieceSurfix 
        {
            get { return pieceSuffix; }
        }

        public GamePieces(string type,string suffix)//构造函数
        {
            pieceType = type;
            pieceSuffix = suffix;
        }
        public GamePieces(string type)//构造函数
        {
            pieceType = type;
            pieceSuffix = "";
        }

        public void SetPiece(string type, string suffix)//更新管道
        {
            pieceType = type;
            pieceSuffix = suffix;
        }

        public void SetPiece(string type)
        {
            SetPiece(type, "");
        }


        public void  AddSuffix(string suffix) //添加后缀，管道是否注满水
        {
            if (!pieceSuffix.Contains(suffix))
            {
                pieceSuffix += suffix;
            }
        }

        public void RemoveSuffix(string suffix)//移除后缀，不再注满水
        {
            pieceSuffix = pieceSuffix.Replace(suffix, "");
        }
        
        //旋转管道,更改管道的类型
        public void RotatePiece(bool ColocWise)
        {
            switch (pieceType)
            {
                case "Left,Right":
                    pieceType = "Top,Bottom";
                    break;         
                case "Top,Bottom":
                    pieceType = "Left,Right";
                    break;
                case "Left,Top":
                    pieceType = "Top,Right";
                    break;
                case "Top,Right":
                    pieceType = "Right,Bottom";
                    break;
                case "Right,Bottom":
                    pieceType = "Bottom,Left";
                    break;
                case "Bottom,Left":
                    pieceType = "Left,Top";
                    break;
                case "Empty":
                    break;
            }
        }

        public string[] GetOtherEnds(string startingEnd)
        {
            List<string> oppsites = new List<string>();
            foreach (string end in pieceType.Split(','))
            {
                if (end != startingEnd)
                    oppsites.Add(end);
            }
            return oppsites.ToArray();//变成数组返回
        }

        public bool HasConnector(string direction) //是否有某个方向的连接
        {

            return pieceType.Contains(direction);
        }

        public Rectangle GetSourceRect()//返回该sprit在图集当中的位置
        {
            int x = textureOffsetX;
            int y = textureOffsetY;

            if (pieceSuffix.Contains("W"))
                x += PieceWidth + texturePaddingX;

            y += (Array.IndexOf(PieceTypes, pieceType) *
                 (PieceHeight + texturePaddingY));


            return new Rectangle(x, y, PieceWidth, PieceHeight);
        }

    }

}
