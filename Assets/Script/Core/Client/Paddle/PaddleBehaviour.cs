using UnityEngine;
using mUnityFramework.Physics.TwoDee;

namespace mUnityFramework.Game.Pong {
	[RequireComponent(typeof(Paddle))]
	public abstract class PaddleBehaviour : MonoBehaviour {
		private Paddle cachedPaddle = null;

		protected System.Action<string> info = msg => Debug.LogFormat (
			"[info][paddleBehaviour][{0}]", msg
		);

		protected Paddle paddle {
			get {
				if (cachedPaddle == null) {
					cachedPaddle = GetComponent<Paddle>();
				}
				return cachedPaddle;
			}
		}

		protected PaddleProperty property {
			get {
				return paddle.Property;
			}
		}

		protected Rigidbody2D rb {
			get {
				return paddle.Rb;
			}
		}

		protected EdgeCollider2D ec {
			get {
				return paddle.Ec;
			}
		}

		protected Surface2D surface {
			get {
				return paddle.Surface;
			}
		}

		protected TrailRenderer tr {
			get {
				return paddle.Tr;
			}
		}
	}
}