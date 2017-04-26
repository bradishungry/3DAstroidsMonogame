using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using BEPUphysics;
using BEPUphysics.EntityStateManagement;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.NarrowPhaseSystems.Pairs;

namespace FlightSim
	{
		public class Meteors
		{
			int meteor_count = 0;
			//float[] radii;
			Space space;
			//Model[] meteors;
			Model small_meteor_1;
			Model small_meteor_2;
			Model medium_meteor_1;
			Model medium_meteor_2;
			Model large_meteor_1;
			Model large_meteor_2;
			//Vector3[] directions;

			List<Vector3> directions = new List<Vector3>();
			List<float> radii = new List<float>();
			List<Model> meteors = new List<Model>();
			//List<BoundingSphere> targetList = new List<BoundingSphere>();
			List<Sphere> colliders = new List<Sphere>();

			public void Initialize(ContentManager contentManager)
			{
				small_meteor_1 = contentManager.Load<Model>("meteors/scmeteor");
				small_meteor_2 = contentManager.Load<Model>("meteors/smeteor");
				medium_meteor_1 = contentManager.Load<Model>("meteors/meteor");
				medium_meteor_2 = contentManager.Load<Model>("meteors/meteor2");
				large_meteor_1 = contentManager.Load<Model>("meteors/blmeteor");
				large_meteor_2 = contentManager.Load<Model>("meteors/bmeteor");

				//directions = new Vector3[meteor_count];

	            AddTargets();
			}

			public void AddTargets()
			{
				Random astroid_generator = new Random();
					int i = 0;

				while (colliders.Count < meteor_count)
				{
						int rng = astroid_generator.Next(6);
					
						if (rng == 0)
						{
							meteors.Add(small_meteor_1);
							radii.Add(0.5f);
						}
						if (rng == 1)
						{
					meteors.Add(small_meteor_2);
							radii.Add(0.5f);
						}
						if (rng == 2)
						{
					meteors.Add(medium_meteor_1);
							radii.Add(1.25f);
						}
						if (rng == 3)
						{
					meteors.Add(medium_meteor_2);
							radii.Add(1.25f);
						}
						if (rng == 4)
						{
					meteors.Add(large_meteor_1);
							radii.Add(2f);
						}
						if (rng == 5)
						{
							meteors.Add(large_meteor_2);
							radii.Add(2f);
						}

					float dx = ((float) astroid_generator.Next(100) / (float)astroid_generator.Next(50)) * 
						((astroid_generator.Next(2) * 2) - 1);
					float dy = ((float)astroid_generator.Next(100) / (float)astroid_generator.Next(50)) *
						((astroid_generator.Next(2) * 2) - 1);
					float dz = ((float)astroid_generator.Next(100) / (float)astroid_generator.Next(50)) *
						((astroid_generator.Next(2) * 2) - 1);
					directions.Add(new Vector3(dx, dy, dz));


					int x = astroid_generator.Next(100) * ((astroid_generator.Next(2) * 2) - 1);
					int z = astroid_generator.Next(100) * ((astroid_generator.Next(2) * 2) - 1);
					int y = astroid_generator.Next(100) * ((astroid_generator.Next(2) * 2) - 1);

					BoundingSphere newTarget = new BoundingSphere(new Vector3(x, y, z), 0.04f);
					Sphere collider = new Sphere(new BEPUutilities.Vector3(x, y, z), 2f, 1f);
					collider.Tag = i;
					directions[i] += newTarget.Center;
					collider.LinearVelocity = new BEPUutilities.Vector3(directions[i].X, directions[i].Y, directions[i].Z);
					i += 1;

					colliders.Add(collider);
					//targetList.Add(newTarget);
				}
	 		}

		public void addToSpace(Space space)
		{
			for (int i = 0; i < colliders.Count; i += 1){
				space.Add(colliders[i]);
			}
            this.space = space;
		}

		public void update()
		{
			for (int i = 0; i < colliders.Count; i += 1)
			{
				colliders[i].CollisionInformation.Events.InitialCollisionDetected += HandleCollision;
				if (colliders[i].Position.X > (400f + colliders[i].Radius) || colliders[i].Position.X < -(394f - colliders[i].Radius) ||
				    colliders[i].Position.Y > (400f + colliders[i].Radius) || colliders[i].Position.Y < -(394f - colliders[i].Radius) || 
				    colliders[i].Position.Z > (400f + colliders[i].Radius) || colliders[i].Position.Z < -(394f - colliders[i].Radius))
				{
					colliders[i].LinearVelocity = -colliders[i].LinearVelocity;
				}
			}
		}

		private Matrix getWorldMatrix(Sphere sphere)
		{
			return Matrix.CreateScale(0.05f) * Matrix.CreateTranslation(new Vector3(sphere.Position.X, sphere.Position.Y, sphere.Position.Z)); 
		}

		public void draw(Camera camera)
		{
			for (int i = 0; i < colliders.Count; i++)
			{
				Matrix w_matrix = getWorldMatrix(colliders[i]);
					Matrix[] transforms = new Matrix[meteors[i].Bones.Count];

					meteors[i].CopyAbsoluteBoneTransformsTo(transforms);
					foreach (ModelMesh mesh in meteors[i].Meshes)
					{
						foreach (BasicEffect effect in mesh.Effects)
						{
							effect.EnableDefaultLighting();
							effect.PreferPerPixelLighting = true;



							effect.World = transforms[mesh.ParentBone.Index] * w_matrix;
							effect.View = camera.ViewMatrix;
							effect.Projection = camera.ProjectionMatrix;
						}
						mesh.Draw();
					}
				}
	 	}

		void HandleCollision(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
		{
			var otherEntityInformation = other as EntityCollidable;
			if (otherEntityInformation != null)
			{
				//if (meteors.Contains((Model)sender.Entity.Tag))
				//{
				//space.Remove(sender.Entity);
				//System.Diagnostics.Debug.Print(sender.Entity.Tag + "");
				//System.Diagnostics.Debug.Print(meteors.Count + "");
				//int index = meteors.RemoveAt((Model)sender.Entity.Tag);

				//What the fuck????
				colliders.Remove((Sphere)sender.Entity);


				//meteor_count--;
				//meteors.Remove(;
				//radii.RemoveAt((int)sender.Entity.Tag);
				//directions.RemoveAt((int)sender.Entity.Tag);
				//colliders.Remove((Sphere)sender.Entity);

				//}
			}
		}


		public void setMeteors(int meteor_count) { this.meteor_count = meteor_count; }
		}
	}
