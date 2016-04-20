using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Prototype.GameStates;

namespace Prototype
{
    class Horse : GameObject
    {
        Model model;
        float angle;
        Vector2 LookInDirection;

        public BoundingSphere Boundingsphere
        {
            get
            {
                var sphere = model.Meshes[0].BoundingSphere;
                sphere.Center += Position;
                return sphere;
            }
        }

        public void Move(Vector3 move)
        {
            Position += move;
        }

        public void Rotate(float angle)
        {
            this.angle = angle;
        }

        public void Rotate(Vector3 LookInDirection)
        {
            this.LookInDirection.Normalize();
            Vector2 lid = new Vector2(LookInDirection.X, LookInDirection.Z);
            lid.Normalize();

            //if(Vector2.Dot(this.LookInDirection, lid) > 0.00125f)
            angle += (float)Math.Acos(Vector2.Dot(this.LookInDirection, lid));

            this.LookInDirection = lid;
        }

        public void Initialize(ContentManager contentManager)
        {
            model = contentManager.Load<Model>("horse");
            Position = new Vector3();
            angle = 0;
            LookInDirection = new Vector2(0, 1);
        }

        public void Update(GameTime gameTime)
        {
            //angle += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        Matrix GetWorldMatrix()
        {
            //const float circleRadius = 80;
            //const float heightOffGround = 3;

            Matrix translationMatrix = Matrix.CreateTranslation(Position);
            //Matrix invTrans = Matrix.CreateTranslation(-Position);

            //Matrix rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(angle));
            //Matrix combined = invTrans * rotationMatrix *  translationMatrix;

            //return combined;
            return translationMatrix;
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
                    float farClipPlane = 1000;

                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
                }
                mesh.Draw();
            }
        }
    }
}
