using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Prototype
{
    class Camera : IDisposable
    {
        GraphicsDeviceManager graphics;
        public Vector3 CamaraLookAt { get; private set; }
        public Vector3 CamaraPosition { get; private set; }
        Matrix projectionMatrix;
        Matrix viewMatrix;
        public Matrix worldMatrix;
        bool orbit = false;
        MouseState mState;
        MouseState pmState;
        public bool Focused { get; private set; }
        int lastValue = 0;

        /// <summary>
        /// if already focsed the Vector has no effect
        /// <para>if it is not focuse, this is the position where the focus is set</para>
        /// </summary>
        /// <param name="FocusPosition"></param>
        public void ToggleFocus(Vector3 FocusPosition)
        {
            Focused = !Focused;
            CamaraLookAt = FocusPosition;
        }


        public void Dispose()
        {
            graphics = null;
        }

        public Camera(GraphicsDeviceManager g)
        {
            graphics = g;
            Mouse.SetPosition(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
            Focused = false;
        }

        public void Move(Vector3 moveVector)
        {
            CamaraLookAt += moveVector;
            CamaraPosition += moveVector;
        }

        public void Initialize()
        {
            CamaraLookAt = new Vector3(0f, 0f, 0f);
            CamaraPosition = new Vector3(0f, 0f, -100f);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90f), graphics.GraphicsDevice.DisplayMode.AspectRatio, 1f, 1000f);
            viewMatrix = Matrix.CreateLookAt(CamaraPosition, CamaraLookAt, new Vector3(0f, 1f, 0f)); // Y up
            worldMatrix = Matrix.CreateWorld(CamaraLookAt, Vector3.Forward, Vector3.Up);

            graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }

        private void ControlMouse()
        {
            mState = Mouse.GetState();

            if(mState.ScrollWheelValue != lastValue)
            {
                if ((CamaraLookAt - CamaraPosition).Length() > 100)
                {
                    CamaraPosition = CamaraLookAt + (100 / (CamaraLookAt - CamaraPosition).Length()) * (CamaraPosition - CamaraLookAt);
                }
                else if((CamaraLookAt - CamaraPosition).Length() < 50)
                {
                    CamaraPosition = CamaraLookAt + (50 / (CamaraLookAt - CamaraPosition).Length()) * (CamaraPosition - CamaraLookAt);
                }
                else
                {
                    CamaraPosition += ((mState.ScrollWheelValue - lastValue) / 100) * new Vector3(0, 0, 1);

                    System.Diagnostics.Debug.WriteLine("MouseWheelValue: " + mState.ScrollWheelValue);
                }
                lastValue = mState.ScrollWheelValue;
            }

            if (!Focused)
            {
                CamaraLookAt -= CamaraPosition;

                Matrix rotateY = Matrix.CreateFromAxisAngle(new Vector3(0, 1, 0), MathHelper.ToRadians((pmState.X - mState.X) / 2));

                CamaraLookAt = Vector3.Transform(CamaraLookAt, rotateY);
                CamaraLookAt += CamaraPosition;
            }
            else
            {
                CamaraPosition -= CamaraLookAt;

                Matrix rotateY = Matrix.CreateFromAxisAngle(new Vector3(0, 1, 0), MathHelper.ToRadians((pmState.X - mState.X) / 2));

                CamaraPosition = Vector3.Transform(CamaraPosition, rotateY);
                CamaraPosition += CamaraLookAt;
            }
            
            System.Diagnostics.Debug.WriteLine("MousePos.X: " + mState.X);
            System.Diagnostics.Debug.WriteLine("MousePos.Y: " + mState.Y);
            System.Diagnostics.Debug.WriteLine("MouseWheel: " + mState.ScrollWheelValue);

            //Mouse.SetPosition(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
            pmState = Mouse.GetState();
        }

        public void Update(KeyboardState state, KeyboardState pState)
        {
            
            ControlMouse();

            if (orbit)
            {
                Matrix rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(1f));
                CamaraPosition = Vector3.Transform(CamaraPosition, rotationMatrix);
            }
            viewMatrix = Matrix.CreateLookAt(CamaraPosition, CamaraLookAt, Vector3.Up);
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
