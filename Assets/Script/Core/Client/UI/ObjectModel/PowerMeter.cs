using UnityEngine;
using UnityEngine.UI;

public class PowerMeter : MonoBehaviour {
    private Slider cachedSlider = null;
    private Slider slider {
        get {
            if (! cachedSlider) {
                cachedSlider = GetComponent<Slider>();
            }
            return cachedSlider;
        }
    }

    public float Level {
        get {
            return slider.value;
        }
    }

    public void Zero () {
        slider.value = 0.0f;
    }

    public void Accumulate () {
        slider.value += 1.0f * Time.deltaTime;
    }
}
