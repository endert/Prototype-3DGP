using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Prototype
{
    class Dragon
    {
        Model model;
        float angle;

        public void Initialize(ContentManager contentManager)
        {
            model = contentManager.Load<Model>("Dragon 2.5_fbx");
        }

        public void Update(GameTime gameTime)
        {
            angle += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        Matrix GetWorldMatrix()
        {
            const float circleRadius = 80;
            const float heightOffGround = 3;

            Matrix translationMatrix = Matrix.CreateTranslation(circleRadius, 0, heightOffGround);

            Matrix rotationMatrix = Matrix.CreateRotationY(angle);
            Matrix combined = translationMatrix * rotationMatrix;

            return combined;
        }

        public void Draw(Vector3 cameraPosition, float aspectRatio, Camera camera)
        {
            foreach (var mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;

                    effect.World = GetWorldMatrix();
                    var cameraLookAtVector = camera.CamaraLookAt;
                    var cameraUpVector = Vector3.UnitY;

                    effect.View = Matrix.CreateLookAt(cameraPosition, cameraLookAtVector, cameraUpVector);

                    float fieldOfView = MathHelper.ToRadians(90f);
                    float nearClipPlane = 1;
                    float farClipPlane = 200;

                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
                }
                mesh.Draw();
            }
        }
    }
}
