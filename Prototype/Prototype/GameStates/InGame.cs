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

            floorVerts[0].TextureCoordinate = new Vector2(0, 0);
            floorVerts[1].TextureCoordinate = new Vector2(0, 1);
            floorVerts[2].TextureCoordinate = new Vector2(1, 0);

            floorVerts[3].TextureCoordinate = floorVerts[1].TextureCoordinate;
            floorVerts[4].TextureCoordinate = new Vector2(1, 1);
            floorVerts[5].TextureCoordinate = floorVerts[2].TextureCoordinate;

            TextureData = new Color[Width * Heigth];

            float r;
            float g;
            float b;

            for(int i = 0; i<Width*Heigth; ++i)
            {
                r = 0;
                g = 0;
                b = 0;

                if (i <= (Width * Heigth)/4)
                    r = 1;
                if (i > (Width * Heigth) / 4)
                    g = 1;
                if (i > (Width * Heigth) / 2)
                {
                    g = 0;
                    b = 1;
                }
                if (i > 3 * (Width * Heigth) / 4)
                {
                    r = 1;
                }

                TextureData[i] = new Color(r, g, b, 1);
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
        Model dragon;
        Model horse;
        Vector3 posModel;
        bool pressed;

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
        }

        public void LoadContent()
        {
            dragon = Content.Load<Model>("Dragon 2.5_fbx");
            camera.ToggleFocus(posModel);
            horse = Content.Load<Model>("horse");

            planeTexture = new Texture2D(gDevice, (int)plane.Width, (int)plane.Heigth);
            planeTexture.SetData<Color>(plane.TextureData);
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

            effect.TextureEnabled = true;
            effect.Texture = planeTexture;

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

        public EGameState Update(KeyboardState kState, KeyboardState previousState)
        {
            UpdateKeyboard(kState);

            camera.Update(kState, previousState);

            previousState = kState;

            return EGameState.InGame;
        }

        public void Draw()
        { 
            camera.Draw(dragon);
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
