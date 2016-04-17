using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Prototype
{
    class Camera
    {
        GraphicsDeviceManager graphics;
        Vector3 camTarget;
        Vector3 camPosition;
        Matrix projectionMatrix;
        Matrix viewMatrix;
        public Matrix worldMatrix;
        bool orbit = false;
        MouseState mState;
        MouseState pmState;

        public Camera(GraphicsDeviceManager g)
        {
            graphics = g;
            Mouse.SetPosition(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
        }

        public void Move(Vector3 moveVector)
        {
            camTarget += moveVector;
            camPosition += moveVector;
        }

        public void Initialize()
        {
            camTarget = new Vector3(0f, 0f, 0f);
            camPosition = new Vector3(0f, 0f, -100f);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90f), graphics.GraphicsDevice.DisplayMode.AspectRatio, 1f, 1000f);
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, new Vector3(0f, 1f, 0f)); // Y up
            worldMatrix = Matrix.CreateWorld(camTarget, Vector3.Forward, Vector3.Up);

            graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }

        private void ControlKeyboard(KeyboardState state, KeyboardState pState)
        {
            if (state.IsKeyDown(Keys.Left))
            {
                camPosition.X -= 1f;
                camTarget.X -= 1f;
            }
            if (state.IsKeyDown(Keys.Right))
            {
                camPosition.X += 1f;
                camTarget.X += 1f;
            }
            if (state.IsKeyDown(Keys.Up))
            {
                camPosition.Y -= 1f;
                camTarget.Y -= 1f;
            }
            if (state.IsKeyDown(Keys.Down))
            {
                camPosition.Y += 1f;
                camTarget.Y += 1f;
            }
            if (state.IsKeyDown(Keys.OemPlus))
            {
                camPosition.Z += 1f;
            }
            if (state.IsKeyDown(Keys.OemMinus))
            {
                camPosition.Z -= 1f;
            }
            if (state.IsKeyDown(Keys.Space) & !pState.IsKeyDown(Keys.Space))
            {
                orbit = !orbit;
            }
        }

        private void ControlMouse()
        {
            mState = Mouse.GetState();

            camTarget.X -= camPosition.X;
            camTarget.Y -= camPosition.Y;
            camTarget.Z -= camPosition.Z;

            Matrix rotateY = Matrix.CreateFromAxisAngle(new Vector3(0, 1, 0), MathHelper.ToRadians((pmState.X - mState.X)/2));

            camTarget = Vector3.Transform(camTarget, rotateY);
            camTarget += camPosition;
            
            System.Diagnostics.Debug.WriteLine("MousePos.X: " + mState.X);
            System.Diagnostics.Debug.WriteLine("MousePos.Y: " + mState.Y);

            Mouse.SetPosition(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
            pmState = Mouse.GetState();
        }

        public void Update(KeyboardState state, KeyboardState pState)
        {
            //ControlKeyboard(state, pState);
            ControlMouse();

            if (orbit)
            {
                Matrix rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(1f));
                camPosition = Vector3.Transform(camPosition, rotationMatrix);
            }
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up);
        }

        public void Draw(Model m)
        {
            foreach (ModelMesh mesh in m.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.AmbientLightColor = new Vector3(1f, 0, 0);
                    effect.View = viewMatrix;
                    effect.World = worldMatrix;
                    effect.Projection = projectionMatrix;
                }
                mesh.Draw();
            }
        }
    }
}
