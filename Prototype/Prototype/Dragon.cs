﻿using System;
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
    class Dragon : GameObject
    {
        Model model;
        float angle;
        int PosX;
        int PosZ;
        bool turn;
        public BoundingSphere Boundingsphere
        {
            get
            {
                var sphere = model.Meshes[0].BoundingSphere;
                sphere.Center += Position;
                return sphere;
            }
        }

        public void Initialize(ContentManager contentManager, int x, int z, bool circle)
        {
            model = contentManager.Load<Model>("Dragon 2.5_fbx");
            PosX = x;
            PosZ = z;
            turn = circle;
        }

        public void Update(GameTime gameTime)
        {
            angle += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        Matrix GetWorldMatrix()
        {
            Position = new Vector3(PosX, 0, PosZ);

            Matrix translationMatrix = Matrix.CreateTranslation(PosX, 0, PosZ);
            
            Matrix rotationMatrix = Matrix.CreateRotationY(angle);
            Matrix combined = translationMatrix * rotationMatrix;

            Position = Vector3.Transform(Position, rotationMatrix);
            if (turn)
                return combined;
            else return translationMatrix;
        }

        public void Dispose()
        {
            model = null;
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
