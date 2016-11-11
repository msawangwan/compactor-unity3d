using UnityEngine;
using UnityEngine.UI;

namespace mUnityFramework.UI.ObjectModel {
	public class LevelTimer : MonoBehaviour {
		private Text cachedTimerTextField;
		private float startTimeOffset;

		private Text timerTextField {
			get {
				if (! cachedTimerTextField) {
					cachedTimerTextField = GetComponent<Text> ();
				}
				return cachedTimerTextField;
			}
		}

		public void SetStartOffset () {
			startTimeOffset = Time.timeSinceLevelLoad;
		}

		public void PrintTime() {
			timerTextField.text = (Time.timeSinceLevelLoad - startTimeOffset).ToString ("0:00");
		}

		public void Reset () {
			startTimeOffset = 0.0f;
			timerTextField.text = (0.0f).ToString("0:00");
		}
	}
}