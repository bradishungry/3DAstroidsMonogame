using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using BEPUphysics;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysics.BroadPhaseEntries;

namespace FlightSim
{
	public class Ship
	{
		Model model;
		Space space;
		Vector3 position;
		Quaternion rotation;
		Quaternion orientation;
		Sphere collider;
		//Vector2 direction;
		float turn_angle = 0.0f;
		float pull_angle = 0.0f;
		float acceleration = 0.0f;
		int ship_count = 3;
		int invincible_counter = 0;
		private bool invincible = false;
		public bool game_over = false;

		public void Initialize(ContentManager contentManager)
		{
			model = contentManager.Load<Model>("spaceshipnew2017");
			position = new Vector3(0.0f, 0.0f, 0.0f);
			rotation = Quaternion.Identity;
			orientation = Quaternion.Identity;
			collider = new Sphere(new BEPUutilities.Vector3(0, 0, 0), 10f);
			collider.Tag = "ship";

			//direction = new Vector3((float)Math.Cos(pull_angle), (float)Math.Sin(pull_angle));
		}

		public void update()
		{
			if (invincible == true)
			{
				if (invincible_counter < 300)
				{
					invincible_counter += 1;
				}
				else
				{
					invincible = false;
					invincible_counter = 0;
				}
			}

			KeyboardState state = Keyboard.GetState();

			if (state.IsKeyDown(Keys.W))
			{
				
				if (acceleration < 1.0f)
				{
					acceleration += 0.009f;
				}

				position += getWorldMatrix().Forward * acceleration;

			}

			/*if (state.IsKeyDown(Keys.S))
			{
				position += getWorldMatrix().Backward * -acceleration;
			}*/

			if (state.IsKeyDown(Keys.A))
			{
				if (acceleration > 0.2f)
				{
					if (turn_angle > 0.0f) { turn_angle = 0.0f; }

					if (turn_angle > -0.03f)
					{
						turn_angle -= 0.0009f;
					}

					Quaternion additionalRot = Quaternion.CreateFromAxisAngle(new Vector3(0, 0, -1), turn_angle);

					rotation *= additionalRot;
				}

				//orientation.Z += 0.03f;
				//position_x += 0.1f;
			}

			if (state.IsKeyDown(Keys.D))
			{
				if (acceleration > 0.2f)
				{
					if (turn_angle < 0.0f) { turn_angle = 0.0f; }

					if (turn_angle < 0.03f)
					{
						turn_angle += 0.009f;
					}

					Quaternion additionalRot = Quaternion.CreateFromAxisAngle(new Vector3(0, 0, -1), turn_angle);
					rotation *= additionalRot;
					//position_x -= 0.1f;
				}
			}

			if (state.IsKeyDown(Keys.Down))
			{
				if (acceleration > 0.2f)
				{
					if (pull_angle < 0.0f) { turn_angle = 0.0f; }

					if (pull_angle < 0.06f)
					{
						pull_angle += 0.0006f;
					}
					Quaternion additionalRot = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), pull_angle);
					rotation *= additionalRot;
				}
			}

			if (state.IsKeyDown(Keys.Up))
			{
				if (acceleration > 0.2f)
				{
					if (pull_angle > 0.0f) { turn_angle = 0.0f; }

					if (pull_angle > -0.06f)
					{
						pull_angle -= 0.0006f;
					}
					Quaternion additionalRot = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), pull_angle);
					rotation *= additionalRot;
				}
			}

			if (!state.IsKeyDown(Keys.W))
			{
				if (acceleration > 0.0f)
				{
					acceleration -= 0.004f;
					position += getWorldMatrix().Forward * acceleration;
				}
			}

			if (!state.IsKeyDown(Keys.A))
			{
				if (turn_angle < 0.0f)
				{
					turn_angle -= turn_angle * 0.05f;

					Quaternion additionalRot = Quaternion.CreateFromAxisAngle(new Vector3(0, 0, -1), turn_angle);
					rotation *= additionalRot;
				}
			}

			if (!state.IsKeyDown(Keys.D))
			{
				if (turn_angle > 0.0f)
				{
					turn_angle -= turn_angle * 0.05f;

					Quaternion additionalRot = Quaternion.CreateFromAxisAngle(new Vector3(0, 0, -1), turn_angle);
					rotation *= additionalRot;
				}
			}

			if (!state.IsKeyDown(Keys.Down) && pull_angle > 0.0f)
			{
					pull_angle -= pull_angle * 0.05f;
					Quaternion additionalRot = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), pull_angle);
				rotation *= additionalRot;
			}

			if (!state.IsKeyDown(Keys.Up))
			{
				if (pull_angle < 0.0f)
				{
					pull_angle -= pull_angle * 0.05f;
					Quaternion additionalRot = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), pull_angle);
					rotation *= additionalRot;
				}
			}

			if (acceleration < 0.2)
			{
				turn_angle = 0.0f;
				pull_angle = 0.0f;
			}

			if (collider.Position.X > (400f + collider.Radius) || collider.Position.X < -(394f - collider.Radius) ||
				    collider.Position.Y > (400f + collider.Radius) || collider.Position.Y < -(394f - collider.Radius) || 
				    collider.Position.Z > (400f + collider.Radius) || collider.Position.Z < -(394f - collider.Radius))
			{
				acceleration = 0;
				position -= getWorldMatrix().Forward * 50.0f;

			}


			collider.CollisionInformation.Events.InitialCollisionDetected += HandleShipCollision;
		}

		public Matrix getWorldMatrix()
		{
			//position = new Vector3(collider.Position.X, collider.Position.Y, collider.Position.Z); 
			Matrix translationMatrix = Matrix.CreateTranslation(position);



			//Matrix rotationMatrix = Matrix.CreateFromQuaternion(rotation);

			//orientation = orientation * rotation;




			//Matrix pull_matrix = Matrix.CreateRotationX(pull_angle);

			return Matrix.CreateFromQuaternion(rotation) * translationMatrix;
		}

		public Vector3 getLocation()
		{
			return position;
		}

		public void draw(Camera camera)
		{
						System.Diagnostics.Debug.Print(collider.Position + "");

			Matrix[] transforms = new Matrix[model.Bones.Count];
			model.CopyAbsoluteBoneTransformsTo(transforms);
			Matrix w_matrix = getWorldMatrix();
			collider.WorldTransform = new BEPUutilities.Matrix(w_matrix.M11, w_matrix.M12, w_matrix.M13, w_matrix.M14,
															   w_matrix.M21, w_matrix.M22, w_matrix.M23, w_matrix.M24,
															   w_matrix.M31, w_matrix.M32, w_matrix.M33, w_matrix.M34,
															   w_matrix.M41, w_matrix.M42, w_matrix.M43, w_matrix.M44);
			foreach (ModelMesh mesh in model.Meshes)
			{
				foreach (BasicEffect effect in mesh.Effects)
				{
					effect.EnableDefaultLighting();
					effect.PreferPerPixelLighting = true;

					effect.World = getWorldMatrix();
					effect.View = camera.ViewMatrix;
					effect.Projection = camera.ProjectionMatrix;
				}
				mesh.Draw();
			}
		}

		public void HandleShipCollision(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
		{
			var otherEntityInformation = other as EntityCollidable;
			if (otherEntityInformation != null)
			{
				if (invincible == false)
				{
					decrementShipCount();
					if (ship_count <= 0)
					{
						game_over = true;
					}

					invincible = true;

				}
			}
		}

		public bool getInvincibility() { return invincible; }
		public void setInvincibility(bool inv) { invincible = inv; }
		public void decrementShipCount() { ship_count--; }
		public int getShipCount() { return ship_count; }


		public void addToSpace(Space space)
		{
			space.Add(collider);
			this.space = space;
		}
	}
}
