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
        Matrix projectionMatrix { get; set; }
        Matrix viewMatrix { get; set; }
        public Matrix worldMatrix { get; set; }
        bool orbit = false;

        public Camera(GraphicsDeviceManager g)
        {
            graphics = g;
        }

        public void Initialize()
        {
            camTarget = new Vector3(0f, 0f, 0f);
            camPosition = new Vector3(0f, 0f, -100f);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90f), graphics.GraphicsDevice.DisplayMode.AspectRatio, 1f, 1000f);
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, new Vector3(0f, 1f, 0f)); // Y up
            worldMatrix = Matrix.CreateWorld(camTarget, Vector3.Forward, Vector3.Up);
        }

        public void Update(KeyboardState state, KeyboardState pState)
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
