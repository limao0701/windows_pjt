using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace FloodControl
{
    class GameBoard
    {
        Random rand = new Random();
        public const int GameBoardWidth = 8;
        public const int GameBoardHeight = 10;
        private GamePieces[,] boardSquares = new GamePieces[GameBoardWidth, GameBoardHeight];
        private List<Vector2> WaterTracker = new List<Vector2>();
        public GameBoard()
        {
            ClearBoard();
        }
        public void ClearBoard() //初始化每个sprit
        {
            for (int i = 0; i < GameBoardHeight; i++)
            {
                for (int j = 0; j < GameBoardWidth; j++)
                    boardSquares[i, j] = new GamePieces("Empty");
            }
        }
        public void  RotatePiece(int x,int y,bool clockWise) //旋转某个sprit
        {
            boardSquares[x,y].RotatePiece(clockWise);
        }

        public Rectangle GetSourceRect(int x, int y) //获取sprit的位置
        {
            return boardSquares[x,y].GetSourceRect();
        }

        public string GetSquare(int x, int y) //类型
        {
            return boardSquares[x, y].PieceType;
        }

        public void SetSquare(int x, int y, string pieceName)
        {
            boardSquares[x, y].SetPiece(pieceName);
        }

        public bool HasConnector(int x, int y, string direction)
        {
            return boardSquares[x, y].HasConnector(direction);
        }

        public void RandomPiece(int x, int y)//随机产生一个管道
        {
            boardSquares[x, y].SetPiece(GamePieces.PieceTypes[rand.Next(0,
            GamePieces.MaxPlayablePieceIndex + 1)]);
        }

        public void FillFromAbove( int x,int y) //将自己上方的非空的sprit移动到下面
        {
            int rowLookup = y - 1; //上方有多少
            while (rowLookup >= 0)
            {
                if (GetSquare(x, rowLookup) != "Empty")
                {
                    SetSquare(x,y,GetSquare(x, rowLookup));
                    SetSquare(x, rowLookup, "Empty");
                    rowLookup = -1; 
                }
                rowLookup--;
            }

        }

        public void GenerateNewPieces(bool dropSquares)  //将所有消掉的管道用上方的非空管道补上
        {

            if (dropSquares)
            {
                for (int x = 0; x < GameBoard.GameBoardWidth; x++)
                {
                    for (int y = GameBoard.GameBoardHeight - 1; y >= 0; y--)
                    {
                        if (GetSquare(x, y) == "Empty")
                        {
                            FillFromAbove(x, y);
                        }
                    }
                }
            }

            for (int y = 0; y < GameBoard.GameBoardHeight; y++)//将空管道填充为新的管道
            {
                for (int x = 0; x < GameBoard.GameBoardWidth; x++)
                {
                    if (GetSquare(x, y) == "Empty")
                    {
                        RandomPiece(x, y);
                    }
                }
            }
        }


        public void ResetWater() //清除注水标志
        {
            for (int y = 0; y < GameBoardHeight; y++)
            {
                for (int x = 0; x < GameBoardWidth; x++)
                    boardSquares[x, y].RemoveSuffix("W");
            }
        }

        public void FillPiece(int x, int y) //添加注水标志
        {
            boardSquares[x, y].AddSuffix("w");
        }


    }
}
