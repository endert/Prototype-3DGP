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
    class Plane
    {
        public VertexPositionTexture[] floorVerts;

        public uint Width;
        public uint Heigth;

        public Color[] TextureData;

        public Plane(uint size)
        {
            floorVerts = new VertexPositionTexture[6];

            floorVerts[0].Position = new Vector3(size, 0, -size);
            floorVerts[1].Position = new Vector3(-size, 0, size);
            floorVerts[2].Position = new Vector3(-size, 0, -size);

            floorVerts[3].Position = floorVerts[0].Position;
            floorVerts[4].Position = new Vector3(size, 0, size);
            floorVerts[5].Position = floorVerts[1].Position;

            Width = size;
            Heigth = size;

            TextureData = new Color[Width * Heigth];

            float r;
            float g;
            float b;

            for(int i = 0; i<Width*Heigth; ++i)
            {
                

                //TextureData[i] = new Color(r, g, b, 1);
            }
        }
    }

    class InGame : IGameState
    {
        GraphicsDeviceManager graphics;
        GraphicsDevice gDevice;
        ContentManager Content;
        float angle = 0;
        Camera camera;
        Dragon dragon;
        Model horse;
        Vector3 posModel;
        bool pressed;
        float aspectRatio;

        Plane plane;
        Texture2D planeTexture;

        BasicEffect effect;

        public InGame(GraphicsDeviceManager g, GraphicsDevice gD, ContentManager content)
        {
            graphics = g;
            gDevice = gD;
            Content = content;
            camera = new Camera(graphics);

            //LoadContent();
            //Initialize();

            plane = new Plane(500);
        }

        public void Initialize()
        {
            posModel = new Vector3(0, 0, 0);
            
            effect = new BasicEffect(gDevice);
            camera.Initialize();

            dragon = new Dragon();
            dragon.Initialize(Content);

            aspectRatio = gDevice.DisplayMode.AspectRatio;
        }

        public void LoadContent()
        {
            camera.ToggleFocus(posModel);
            horse = Content.Load<Model>("horse");
        
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

            float fieldOfView = MathHelper.ToRadians(90f);
            float nearClipPlane = 1;
            float farClipPlane = 200;

            effect.Projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane);


            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                gDevice.DrawUserPrimitives(PrimitiveType.TriangleList, plane.floorVerts, 0, 2);
            }
        }
        private void UpdateKeyboard(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.D))
                angle += 0.1f;
            if (state.IsKeyDown(Keys.A))
                angle -= 0.1f;
            if (state.IsKeyDown(Keys.W))
            {
                posModel.Z -= 1;
                camera.Move(new Vector3(0, 0, -1));
            }
            if (state.IsKeyDown(Keys.S))
            {
                posModel.Z += 1;
                camera.Move(new Vector3(0, 0, 1));
            }

            if (state.IsKeyDown(Keys.F)&& !pressed)
            {
                pressed = true;
                camera.ToggleFocus(posModel);
            }

            if (pressed && !state.IsKeyDown(Keys.F))
                pressed = false;

            camera.worldMatrix = Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(posModel);
        }

        public EGameState Update(KeyboardState kState, KeyboardState previousState, GameTime gameTime)
        {
            UpdateKeyboard(kState);

            camera.Update(kState, previousState);
            dragon.Update(gameTime);

            previousState = kState;

            return EGameState.InGame;
        }

        public void Draw()
        {
            dragon.Draw(camera.CamaraPosition, aspectRatio, camera);
            camera.Draw(horse);
            DrawGround();
        }

        public void Dispose()
        {
            graphics = null;
            gDevice = null;
            Content = null;
            camera.Dispose();
            dragon = null;
            horse = null;
        }
    }
}
