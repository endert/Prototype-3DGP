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


        public Test3DDemo()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = false;
            currentGameState = EGameState.MainMenu;
            gameState = new MainMenu(graphics, graphics.GraphicsDevice, Content);
            //LoadContent();
            //Initialize();         
        }

        protected override void Initialize()
        {
            base.Initialize();
            IsMouseVisible = false;

            previousState = Keyboard.GetState();

            gameState.Initialize();

            
            //inGame = new InGame(graphics, graphics.GraphicsDevice, Content);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            gameState.LoadContent();
        }

        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            kState = Keyboard.GetState();
            currentGameState = gameState.Update(kState, previousState);

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