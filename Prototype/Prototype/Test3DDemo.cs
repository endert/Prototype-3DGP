using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Prototype.GameStates;

namespace Prototype
{
    public class Test3DDemo : Game
    {
        GraphicsDeviceManager graphics;
        KeyboardState kState;
        KeyboardState previousState;
     
        EGameState currentGameState;
        EGameState previousGameState = EGameState.None;
        IGameState gameState;

        int Score;


        public Test3DDemo()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = false;
            currentGameState = EGameState.MainMenu;      
        }

        protected override void Initialize()
        {
            base.Initialize();
            IsMouseVisible = false;

            previousState = Keyboard.GetState();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (previousGameState == EGameState.None)
                HandleGameState();

            kState = Keyboard.GetState();

            currentGameState = gameState.Update(kState, previousState, gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kState.IsKeyDown(Keys.Escape))
            {
                currentGameState = EGameState.None;
            }

            if (currentGameState != previousGameState)
            {
                HandleGameState();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            gameState.Draw();

            base.Draw(gameTime);
        }
        
        void HandleGameState()
        {
            switch (currentGameState)
            {
                case EGameState.MainMenu:
                    if(gameState != null)
                    gameState.UnLoadContent();
                    gameState = new MainMenu(graphics, GraphicsDevice, Content);
                    gameState.LoadContent();
                    gameState.Initialize();
                    break;

                case EGameState.InGame:
                    gameState.UnLoadContent();
                    gameState = new InGame(graphics, GraphicsDevice, Content);
                    gameState.LoadContent();
                    gameState.Initialize();
                    break;

                case EGameState.None:
                    Exit();
                    break;

                default:
                    break;
            }

            previousGameState = currentGameState;
        }

    }
}