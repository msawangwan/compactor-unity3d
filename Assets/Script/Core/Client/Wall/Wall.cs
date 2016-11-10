using UnityEngine;
using mUnityFramework.Physics.TwoDee;

namespace mUnityFramework.Game.Pong {
	[RequireComponent(typeof(EdgeCollider2D))]
	public class Wall : MonoBehaviour {
		public Surface2D.Axis Axis;
		public Surface2D.Front FrontFacing;

		private Surface2D cachedSurface = null;

		public Surface2D Surface {
			get {
				if (cachedSurface == null) {
					cachedSurface = Surface2D.New (
						Axis,
						FrontFacing,
						GetComponent<EdgeCollider2D>()
					);
				}
				return cachedSurface;
			}
		}

		private void Update () {
			Surface.DrawNormal();
		}

		protected virtual void OnTriggerEnter2D (Collider2D c) {
			Ball b = c.gameObject.GetComponent<Ball>();
			if (b) {
				Vector3 rf = b.Force.SetVelocityTo (
					b.Force.DeriveReflectionForce (
						b.Rb.velocity,
						Surface.Normal
					)
				);

				Debug.DrawRay (c.transform.position, rf, Color.green, 3.0f);
			}
		}
	}
}