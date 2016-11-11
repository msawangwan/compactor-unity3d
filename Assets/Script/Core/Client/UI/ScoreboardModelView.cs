using UnityEngine;
using UnityEngine.UI;
using mUnityFramework.UI.Animation;
using System.Collections.Generic;

public class ScoreboardModelView : MonoBehaviour {
    public Text txtScore = null;
    public TextBulge bulgeAnimation = null;

    private static List<ScoreboardModelView> instances = new List<ScoreboardModelView>();

    public static ScoreboardModelView Instance {
        get {
            return instances [0];
        }
    }

    public void WriteScore (int v) {
        txtScore.text = v.ToString();
        // bulgeAnimation.BulgeAnimation();
    }

    public void Reset () {
        txtScore.text = 0.ToString();
    }

    private void Awake () {
        if (instances.Count > 0) {
            instances[0] = this;
        } else {
            instances.Add (this);
        }
    }
}
