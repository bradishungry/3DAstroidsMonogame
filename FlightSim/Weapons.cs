using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using BEPUphysics;
using BEPUphysics.Entities.Prefabs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysics.BroadPhaseEntries;

namespace FlightSim
{

	struct Torpedo
	{
		public const float speed = 2.0f;
		public Model model;
		public BEPUutilities.Vector3 position;
	}

	public class Weapons
	{
		private int torpedos_on_screen;
		private bool can_torpedo_fire;
		private Model torpedo_model;
		private List<Torpedo> torpedos = new List<Torpedo>();
		private List<Box> colliders = new List<Box>();
		Space space;


		public void Initialize(ContentManager contentManager)
		{
			torpedo_model = contentManager.Load<Model>("spaceshipnew2017");
			can_torpedo_fire = true;
		}

		public void setSpace(Space space){ this.space = space; }

		public void update(Vector3 position, Vector3 forward)
		{
			KeyboardState state = Keyboard.GetState();
			if (state.IsKeyDown(Keys.E) && can_torpedo_fire)
			{
				Torpedo torpedo = new Torpedo();
				torpedo.position = new BEPUutilities.Vector3(position.X, position.Y - 30f, position.Z);
				torpedo.model = torpedo_model;
				torpedos.Add(torpedo);

				Box collider = new Box(torpedo.position, 1, 1, 1, 1);
				collider.Tag = torpedo;
				collider.LinearVelocity = new BEPUutilities.Vector3(forward.X, forward.Y, forward.Z) * 4000f;
				colliders.Add(collider);
				space.Add(collider);
				//can_torpedo_fire = false;
			}

			for (int i = 0; i < torpedos.Count; i += 1)
			{
				if (!(position.Equals(torpedos[i].position))){
					colliders[i].CollisionInformation.Events.InitialCollisionDetected += HandleCollision;
				}
			}
		}

		void HandleCollision(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
		{
			var otherEntityInformation = other as EntityCollidable;
			if (otherEntityInformation != null)
			{
					space.Remove(sender.Entity);
					if (torpedos.Contains((Torpedo)sender.Entity.Tag))
					{
						colliders.Remove((Box)sender.Entity);
						torpedos.Remove((Torpedo)sender.Entity.Tag);
					}
			}
		}

		private Matrix getWorldMatrix(Box box)
		{
			return Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(new Vector3(box.Position.X, box.Position.Y, box.Position.Z));
		}

		public void draw(Camera camera)
		{
			for (int i = 0; i < colliders.Count; i++)
			{
				Matrix w_matrix = getWorldMatrix(colliders[i]);
				Matrix[] transforms = new Matrix[torpedos[i].model.Bones.Count];

				torpedos[i].model.CopyAbsoluteBoneTransformsTo(transforms);
				foreach (ModelMesh mesh in torpedos[i].model.Meshes)
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
	}
}
