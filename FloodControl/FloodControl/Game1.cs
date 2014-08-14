using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace FloodControl
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D playingPieces;
        Texture2D background;
        Texture2D titleScreen;

        GameBoard gameBoard;

        Vector2 gameBoardDisplayOrigin = new Vector2(70, 89);//”Œœ∑œ‘ æ«¯”Ú
        int playerScore = 0;
        enum GameState { TitleScreen, Playing };
        GameState gameState = GameState.TitleScreen;
        Rectangle EmptyPiece = new Rectangle(1, 247, 40, 40);
        const float MinTimeSinceLastInput = 0.25f;
        float timeSinceLastInput = 0.0f;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();
            gameBoard = new GameBoard();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            playingPieces = Content.Load<Texture2D>(@"Textures\Tile_Sheet");
            background = Content.Load<Texture2D>(@"Textures\Background");
            titleScreen = Content.Load<Texture2D>(@"Textures\TitleScreen");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            switch (gameState)
            {
                case GameState.TitleScreen:
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        gameBoard.ClearBoard();
                        gameBoard.GenerateNewPieces(false);
                        playerScore = 0;
                        gameState = GameState.Playing;
                    }
                    break;

                case GameState.Playing:
                    {
                        timeSinceLastInput += (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (timeSinceLastInput >= MinTimeSinceLastInput)
                        {
                            HandleMouseInput(Mouse.GetState());
                        }
                        gameBoard.ResetWater();
                        for (int y = 0; y < GameBoard.GameBoardHeight; y++)
                        {
                            CheckScoringChain(gameBoard.GetWaterChain(y));
                        }
                        gameBoard.GenerateNewPieces(true);
                        break;
                    }
            }
            


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            if (gameState == GameState.TitleScreen)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(titleScreen, 
                    new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height),
                    Color.White);
                spriteBatch.End();
            }

            if (gameState == GameState.Playing)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(background, new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height),
                    Color.White);
                for (int x = 0; x < GameBoard.GameBoardWidth; x++)
                    for (int y = 0; y < GameBoard.GameBoardHeight; y++)
                    {
                        int pixelX = (int)gameBoardDisplayOrigin.X +
                        (x * GamePieces.PieceWidth);
                        int pixelY = (int)gameBoardDisplayOrigin.Y +
                        (y * GamePieces.PieceHeight);
                        spriteBatch.Draw(
                        playingPieces,
                        new Rectangle(
                        pixelX,
                        pixelY,
                        GamePieces.PieceWidth,
                        GamePieces.PieceHeight),
                        EmptyPiece,
                        Color.White);
                        spriteBatch.Draw(
                        playingPieces, new Rectangle(
                        pixelX,
                        pixelY,
                        GamePieces.PieceWidth,
                        GamePieces.PieceHeight),
                        gameBoard.GetSourceRect(x, y),
                        Color.White);
                    }
                this.Window.Title = playerScore.ToString();
                spriteBatch.End();
            }


            base.Draw(gameTime);
        }

        private int DetermineScore(int SquareCount)
        {
            return (int)((Math.Pow((SquareCount / 5) , 2) + SquareCount) *10);
        }

        private void CheckScoringChain(List<Vector2> WaterChain)
        {
            if (WaterChain.Count > 0)
            {
                Vector2 LastPipe = WaterChain[WaterChain.Count - 1];
                if (LastPipe.X == GameBoard.GameBoardWidth - 1)
                {
                    if (gameBoard.HasConnector((int)LastPipe.Y, (int)LastPipe.Y, "Right"))
                    {
                        playerScore += DetermineScore(WaterChain.Count);
                        foreach (Vector2 ScoringSquare in WaterChain)
                        {
                            gameBoard.SetSquare( (int)ScoringSquare.X, (int)ScoringSquare.Y,"Empty");
                        }
                    }
                }
            }
        }

        private void HandleMouseInput(MouseState mouseState)
        {
            int x = ((mouseState.X - (int)gameBoardDisplayOrigin.X / GamePieces.PieceWidth));
            int y = ((mouseState.Y - (int)gameBoardDisplayOrigin.Y / GamePieces.PieceHeight));
            if ((x >= 0) && (x < GameBoard.GameBoardWidth) && (y >= 0) && (y < GameBoard.GameBoardHeight))
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    gameBoard.RotatePiece(x, y, false);
                    timeSinceLastInput = 0.0f;
                }
                if (mouseState.RightButton == ButtonState.Pressed)
                {
                    gameBoard.RotatePiece(x, y, true);
                    timeSinceLastInput = 0.0f;
                }
            }
        }
    }
}
