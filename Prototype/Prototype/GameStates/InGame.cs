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
    abstract class GameObject
    {
        public Vector3 Position { get; protected set; }
    }

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
        List<Dragon> dragons;
        Horse horse;
        Vector3 moveVector;
        bool pressed;
        float aspectRatio;

        int _score = 0;

        Plane plane;
        Texture2D planeTexture;
        SpriteBatch sBatch;
        BasicEffect effect;
        SpriteFont font;

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
            sBatch = new SpriteBatch(gDevice);

            moveVector = new Vector3(0, 0, 0);
            
            effect = new BasicEffect(gDevice);
            camera.Initialize();

            dragons = new List<Dragon>();

            Dragon addedDragon = new Dragon();
            Dragon dragonTwo = new Dragon();
            Dragon dragonTree = new Dragon();

            addedDragon.Initialize(Content, 100, 100, true);
            dragonTwo.Initialize(Content, 200, 100, false);
            dragonTree.Initialize(Content, -50, -300, false);

            dragons.Add(addedDragon);
            dragons.Add(dragonTwo);
            dragons.Add(dragonTree);


            horse = new Horse();
            horse.Initialize(Content);

            camera.SetFocus(horse);

            aspectRatio = gDevice.DisplayMode.AspectRatio;
        }

        public void LoadContent()
        {
            planeTexture = new Texture2D(gDevice, (int)plane.Width, (int)plane.Heigth);
            planeTexture.SetData<Color>(plane.TextureData);
            font = Content.Load<SpriteFont>("Score");
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
            float farClipPlane = 1000;

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
            moveVector = Vector3.Zero;

            Vector2 front = new Vector2(camera.Front.X, camera.Front.Z);
            front.Normalize();

            if (state.IsKeyDown(Keys.W))
            {
                moveVector.Z += front.Y;
                moveVector.X += front.X;
            }
            if (state.IsKeyDown(Keys.S))
            {
                moveVector.Z -= front.Y;
                moveVector.X -= front.X;
            }

            //if(state.IsKeyDown(Keys.A))
            //{
            //    moveVector.X += 1;
            //}
            //if (state.IsKeyDown(Keys.D))
            //    moveVector.X -= 1;

            if (state.IsKeyDown(Keys.F)&& !pressed)
            {
                pressed = true;
                camera.ToggleFocus();
            }

            if (pressed && !state.IsKeyDown(Keys.F))
                pressed = false;

            camera.Move(moveVector);
            horse.Move(moveVector);
            horse.Rotate(camera.Front);
        }

        public EGameState Update(KeyboardState kState, KeyboardState previousState, GameTime gameTime, ref int Score)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kState.IsKeyDown(Keys.Escape))
            {
                return EGameState.Credits;
            }

            UpdateKeyboard(kState);

            camera.Update(kState, previousState);

            foreach (Dragon d in dragons)
                d.Update(gameTime);

            previousState = kState;

            //System.Diagnostics.Debug.WriteLine("Clear");

            for(int i = 0; i< dragons.Count; ++i)
                if (dragons[i].Boundingsphere.Intersects(horse.Boundingsphere))
                {
                    Score+= 100;
                    dragons.RemoveAt(i--);
                }

            _score = Score;
            return EGameState.InGame;
        }

        public void Draw()
        {
            foreach (Dragon d in dragons)
                d.Draw(camera.CamaraPosition, aspectRatio, camera);

            horse.Draw(camera.CamaraPosition, aspectRatio, camera);
            DrawGround();

            sBatch.Begin();
            sBatch.DrawString(font, "Score: " + _score, new Vector2(0, 0), Color.Black);
            sBatch.End();

            gDevice.DepthStencilState = DepthStencilState.Default;
        }

        public void Dispose()
        {
            graphics = null;
            gDevice = null;
            Content = null;
            camera.Dispose();
            dragons = null;
            horse = null;
        }
    }
}
