using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlightSim
{
	public class Camera
	{

		GraphicsDevice graphicsDevice;
		public Vector3 camera_position = new Vector3(0, 0.2f, 0);
		public Quaternion camera_rotation = Quaternion.Identity;
		Vector3 look_at_vector = new Vector3(0, 0, 1);
		Vector3 camera_up_vector = Vector3.UnitY;

		public Matrix ViewMatrix
		{
			get
			{
				return Matrix.CreateLookAt(
					camera_position, look_at_vector, camera_up_vector);
			}
		}

		public Matrix ProjectionMatrix
		{
			get
			{
				float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
				float nearClipPlane = 1;
				float farClipPlane = 1600;
				float aspectRatio = graphicsDevice.Viewport.Width / (float)graphicsDevice.Viewport.Height;

				return Matrix.CreatePerspectiveFieldOfView(
					fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
			}
		}

		public Camera(GraphicsDevice graphicsDevice)
		{
			this.graphicsDevice = graphicsDevice;
		}

		public void Update(GameTime gameTime, Vector3 look_at, Matrix ship_position)
		{
			look_at_vector = look_at;

			camera_position = new Vector3(0.0f, 0.2f, 45.0f);

			camera_position = Vector3.Transform(camera_position, Matrix.CreateFromQuaternion(ship_position.Rotation));

			camera_position += ship_position.Translation;

			camera_up_vector = Vector3.UnitY;

			camera_up_vector = Vector3.Transform(camera_up_vector, Matrix.CreateFromQuaternion(ship_position.Rotation));
		}
	}
}
