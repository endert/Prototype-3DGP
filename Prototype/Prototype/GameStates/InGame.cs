using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Prototype.GameStates
{
    class InGame : IGameState
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GraphicsDevice gDevice;
        ContentManager Content;
        float angle = 0;
        Camera camera;
        Model model;
        Vector3 posModel;
        bool pressed;

        public InGame(GraphicsDeviceManager g, GraphicsDevice gD, ContentManager content)
        {
            graphics = g;
            gDevice = gD;
            Content = content;
            camera = new Camera(graphics);

            LoadContent();
            Initialize();
        }

        public void Initialize()
        {
            posModel = new Vector3(0, 0, 0);
            camera.Initialize();
        }

        public void LoadContent()
        {
            model = Content.Load<Model>("Dragon 2.5_fbx");
        }

        public void UnLoadContent()
        {
            Content.Unload();
            Dispose();
        }

        private void UpdateKeyboard(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.D))
                angle += 0.1f;
            if (state.IsKeyDown(Keys.A))
                angle -= 0.1f;
            if (state.IsKeyDown(Keys.W))
                posModel.Z -= 1;
            if (state.IsKeyDown(Keys.S))
                posModel.Z += 1;

            if (state.IsKeyDown(Keys.F)&& !pressed)
            {
                pressed = true;
                camera.ToggleFocus(posModel);
            }

            if (pressed && !state.IsKeyDown(Keys.F))
                pressed = false;

            camera.worldMatrix = Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(posModel);
        }

        public EGameState Update(KeyboardState kState, KeyboardState previousState)
        {
            UpdateKeyboard(kState);

            camera.Update(kState, previousState);

            previousState = kState;

            return EGameState.InGame;
        }

        public void Draw()
        {
            camera.Draw(model);
        }

        public void Dispose()
        {
            graphics = null;
            spriteBatch = null;
            gDevice = null;
            Content = null;
            camera.Dispose();
            model = null;
        }
    }
}
