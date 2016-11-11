using UnityEngine;

namespace mUnityFramework.Game {
	public class GameProperty : MonoBehaviour {
		[SerializeField] private float levelTimeInterval;
		[SerializeField] private float levelScoreInterval;

		public float LevelTimeInterval {
			get {
				return levelTimeInterval;
			}
		}

		public float LevelScoreInterval {
			get {
				return levelScoreInterval;
			}
		}
	}
}