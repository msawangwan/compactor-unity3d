using UnityEngine;
using mUnityFramework.Game.Pong;

namespace mUnityFramework.Game.Compactor {
    public class Reflector : MonoBehaviour {
        private void OnTriggerEnter2D (Collider2D c) {
            if (c.GetComponent<Ball>()) {
                // GetComponent<Rigidbody2D>().AddForce(new Vector3(0, upForce, 0));
            }
        }
    }
}
