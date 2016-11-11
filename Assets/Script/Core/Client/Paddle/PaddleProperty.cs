using UnityEngine;

namespace mUnityFramework.Game.Pong {
    public class PaddleProperty : MonoBehaviour {
        [SerializeField] private float horizontalPosition;

        [SerializeField] private float horizontalMoveForceMultiplier;
        [SerializeField] private float maximumAttainableMoveForce;

        [SerializeField] private float launchPowerScalar;
        [SerializeField] private float launchSteepness;
        [SerializeField] private float launchMaximumForce;
        [SerializeField] private float launchOffset;

        public float hPosition { 
            get {
                return Mathf.Abs (horizontalPosition) * -1.0f;
            }
        }

        public float hMoveForceMultiplier {
            get {
                return horizontalMoveForceMultiplier;
            }
        }

        public float MaxAttainableMoveForce {
            get {
                return maximumAttainableMoveForce;
            }
        }

        public float LaunchPowerScalar {
            get {
                return launchPowerScalar;
            }
        }

        public float LaunchSteepness {
            get {
                return launchSteepness;
            }
        }

        public float LaunchMaximumForce {
            get {
                return launchMaximumForce;
            }
        }

        public float LaunchOffset {
            get {
                return launchOffset;
            }
        }
    }
}