using System;
using BEPUphysics;
using BEPUphysics.Entities.Prefabs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlightSim
{
	public class Skybox
	{
		
		Model model;
		//private float size = 800f;
		Sphere collider;




		public void Initialize(ContentManager contentManager)
		{
			model = contentManager.Load<Model>("skybox");
			collider = new Sphere(new BEPUutilities.Vector3(0, 0, 0), 400f);
		}

		public void addToSpace(Space space)
		{
		}

		public void update()
		{
		}

		public void checkCollision(Sphere sphere)
		{
		}

		public void draw(Camera camera)
		{
			Matrix World = Matrix.CreateScale(collider.Radius);
			Matrix[] transforms = new Matrix[model.Bones.Count];
			model.CopyAbsoluteBoneTransformsTo(transforms);
				 
			foreach (ModelMesh mesh in model.Meshes)
			{
				foreach (BasicEffect effect in mesh.Effects)
				{
					effect.EnableDefaultLighting();

					effect.World = World;
					effect.View = camera.ViewMatrix;
					effect.Projection = camera.ProjectionMatrix;
				}

				mesh.Draw();
			}
		}
	}
}
