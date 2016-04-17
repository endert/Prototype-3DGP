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

        VertexPositionTexture[] floorVerts;
        BasicEffect effect;

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

            floorVerts = new VertexPositionTexture[6];
            floorVerts[0].Position = new Vector3(-20, 0, -20);
            floorVerts[1].Position = new Vector3(-20, 0, 20);
            floorVerts[2].Position = new Vector3(20, 0, -20);

            floorVerts[3].Position = floorVerts[1].Position;
            floorVerts[4].Position = new Vector3(20, 0, 20);
            floorVerts[5].Position = floorVerts[2].Position;

            effect = new BasicEffect(gDevice);
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

        public void DrawGround()
        {
            var cameraPosition = camera.CamaraPosition;
            var cameraLookAtVector = camera.CamaraLookAt;
            var cameraUpVector = Vector3.UnitY;

            effect.View = Matrix.CreateLookAt(cameraPosition, cameraLookAtVector, cameraUpVector);

            float aspectRatio = gDevice.DisplayMode.AspectRatio;
            float fieldOfView = MathHelper.ToRadians(90f);
            float nearClipPlane = 1;
            float farClipPlane = 200;

            effect.Projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane);

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                gDevice.DrawUserPrimitives(PrimitiveType.TriangleList, floorVerts, 0, 2);

            }
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
            DrawGround();
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
