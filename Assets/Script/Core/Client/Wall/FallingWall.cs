using UnityEngine;

namespace mUnityFramework.Game.Pong {
    [RequireComponent(typeof(Rigidbody2D))]
    public class FallingWall : Wall {
        [SerializeField] private float increment;
        [SerializeField] private float tickInterval;

        private float tickTime = 0.0f;

        private GameController gc = null;

        private Rigidbody2D cachedRb = null;
        private Rigidbody2D rb {
            get {
                if (! cachedRb) {
                    cachedRb = GetComponent<Rigidbody2D>();
                }
                return cachedRb;
            }
        }

        private float nextTick {
            get {
                return Time.time + tickInterval;
            }
        }

        private bool isTimeForUpdate {
            get {
                return Time.time > tickTime;
            }
        }

        private void Start () {
            tickTime = 0;
        }

        private void FixedUpdate () {
            if (! gc) {
                gc = GameController.Instance;
            }

            switch (gc.ControllerState) {
                case GameController.State.Enter:
                    return;
                case GameController.State.Execute:
                    if (isTimeForUpdate) {
                        tickTime = nextTick;
                        Vector3 newPosition = transform.position - new Vector3 (0, increment, 0);
                        rb.MovePosition (newPosition);
                    }
                    return;
                default:
                    return;
            }
        }

        protected override void OnTriggerEnter2D (Collider2D c) {
            base.OnTriggerEnter2D(c);
            Vector3 newPosition = transform.position + new Vector3 (0, increment, 0);
            rb.MovePosition (newPosition);
        }
    }
}
