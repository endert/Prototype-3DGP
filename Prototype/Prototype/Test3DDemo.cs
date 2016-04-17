using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Prototype;

namespace Prototype
{

    public class Test3DDemo : Game
    {
        GraphicsDeviceManager graphics;
        Camera camera;
        SpriteBatch spriteBatch;
        Vector2 position;
        Vector3 posModel;
        Texture2D texture;
        KeyboardState previousState;
        bool keyboardControl = true;
        float angle;

        ////BasicEffect for rendering
        //BasicEffect basicEffect;

        ////Geometric info
        //VertexPositionColor[] triangleVertices;
        //VertexBuffer vertexBuffer;

        Model model;

        public Test3DDemo()
        {
            graphics = new GraphicsDeviceManager(this);
            camera = new Camera(graphics);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
            IsMouseVisible = true;

            position = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);
            posModel = new Vector3(0, 0, 0);
            angle = 0;

            previousState = Keyboard.GetState();

            camera.Initialize();
            
            /*
             *BasicEffect
             *basicEffect = new BasicEffect(GraphicsDevice);
             *basicEffect.Alpha = 1f;
             *
             *Want to see the colors of the vertices, this needs to be on
             *basicEffect.VertexColorEnabled = true;
             *
             *Lighting requires normal information which VertexPositionColor does not have
             *If you want to use lighting and VPC you need to create a custom def
             *basicEffect.LightingEnabled = false;
             *      
             *Geometry  - a simple triangle about the origin
             *triangleVertices = new VertexPositionColor[3];
             *triangleVertices[0] = new VertexPositionColor(new Vector3(0, 20, 0), Color.Red);
             *triangleVertices[1] = new VertexPositionColor(new Vector3(-20, -20, 0), Color.Green);
             *triangleVertices[2] = new VertexPositionColor(new Vector3(20, -20, 0), Color.Blue);
             *
             *Vert buffer
             *vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
             *vertexBuffer.SetData<VertexPositionColor>(triangleVertices);
             */
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = this.Content.Load<Texture2D>("BB-8 Thumb-Up");
            model = Content.Load<Model>("Dragon 2.5_fbx");
        }

        protected override void UnloadContent()
        {
        }

        private void UpdateKeyboard(KeyboardState state)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (var key in state.GetPressedKeys())
                sb.Append("Key: ").Append(key).Append(" pressed ");

            if (sb.Length > 0)
                System.Diagnostics.Debug.WriteLine(sb.ToString());
            else
                System.Diagnostics.Debug.WriteLine("No Keys pressed");

            //if (state.IsKeyDown(Keys.D))
            //    position.X += 10;
            //if (state.IsKeyDown(Keys.A))
            //    position.X -= 10;
            //if (state.IsKeyDown(Keys.W))
            //    position.Y -= 10;
            //if (state.IsKeyDown(Keys.S))
            //    position.Y += 10;

            if (state.IsKeyDown(Keys.D))
                angle += 0.1f;
            if (state.IsKeyDown(Keys.A))
                angle -= 0.1f;
            if (state.IsKeyDown(Keys.W))
                posModel.Z -= 1;
            if (state.IsKeyDown(Keys.S))
                posModel.Z += 1;

            camera.worldMatrix = Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(posModel);
        }

        private void UpdateMouse()
        {
            MouseState mState = Mouse.GetState();

            position.X = mState.X;
            position.Y = mState.Y;

            if (mState.MiddleButton == ButtonState.Pressed)
                Mouse.SetPosition(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState kState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kState.IsKeyDown(Keys.Escape))
                Exit();

            if (kState.IsKeyDown(Keys.Enter) & !previousState.IsKeyDown(Keys.Enter))
                keyboardControl = !keyboardControl;

            if (keyboardControl) UpdateKeyboard(kState);
            else UpdateMouse();

            camera.Update(kState, previousState);
           
            base.Update(gameTime);

            previousState = kState;
        }

        protected override void Draw(GameTime gameTime)
        {
            //basicEffect.Projection = projectionMatrix;
            //basicEffect.View = viewMatrix;
            //basicEffect.World = worldMatrix;

            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(texture, position, origin: new Vector2(texture.Width / 2, texture.Height / 2));
            spriteBatch.End();

            camera.Draw(model);

            //GraphicsDevice.SetVertexBuffer(vertexBuffer);

            ////Turn off culling so we see both sides of our rendered triangle
            //RasterizerState rasterizerState = new RasterizerState();
            //rasterizerState.CullMode = CullMode.None;
            //GraphicsDevice.RasterizerState = rasterizerState;

            //foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            //{
            //    pass.Apply();
            //    GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 3);
            //}

            base.Draw(gameTime);
        }
    }
}