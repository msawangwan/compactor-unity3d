﻿using UnityEngine;

[RequireComponent (typeof (EdgeCollider2D))]
public class WallScoreZone : MonoBehaviour {
    public Player.PlayerID AttackingPlayer = Player.PlayerID.None;

    void OnTriggerEnter2D (Collider2D c) {
        Ball ball = c.GetComponent<Ball> ();
        if (ball != null) {
            Ball.ResetAndPositionAt (
                BallManager.StaticInstance.transform.parent.transform, 
                ball, 
                Vector3.zero
            );

            mStateFramework.Core.StateWaitUntilPointScore.onScore = () => {
                return AttackingPlayer;
            };
        }
    }
}
