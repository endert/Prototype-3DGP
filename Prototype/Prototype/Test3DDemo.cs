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
        InGame inGame;
        MainMenu mainMenu;
     
        EGameState currentGameState;
        EGameState previousGameState = EGameState.None;
        IGameState gameState;


        public Test3DDemo()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = false;
            currentGameState = EGameState.MainMenu;

            //LoadContent();
            //Initialize();         
        }

        protected override void Initialize()
        {
            base.Initialize();
            IsMouseVisible = true;

            previousState = Keyboard.GetState();

            

            mainMenu = new MainMenu(graphics, graphics.GraphicsDevice, Content);
            //inGame = new InGame(graphics, graphics.GraphicsDevice, Content);
        }

        protected override void LoadContent()
        {
            
        }

        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            kState = Keyboard.GetState();
            gameState.Update(kState, previousState);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            gameState.Draw();

            base.Draw(gameTime);
        }
    }
}