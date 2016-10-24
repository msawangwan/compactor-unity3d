﻿using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// base class for a paddle gameObject
/// </summary>

[RequireComponent(typeof(EdgeCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PaddleServeController))]
[RequireComponent(typeof(PaddleStrikeController))]
public abstract class Paddle : MonoBehaviour {
    [System.Serializable]
    public class PaddleParamters {
        public Paddle.PaddleID PID                   = Paddle.PaddleID.None;
        [RangeAttribute(00.01f, 100.00f)]
        public float  HorizontalMoveForceMultiplier  = 3.0f;
        public float  FixedHorizontalPosition        = 0.0f; // y position
    }

    public enum PaddleID : int { 
        None = 0,
        Bottom, 
        Top 
    }

    public enum GameState : int { None, Menu, Enter, Idle, Serve, Play, Exit }

    private static List<Paddle>      instances        = new List<Paddle>();
    public static IEnumerable<Paddle> Instances { get { return instances; } }

    public Player.PlayerID AssignedPlayer { 
        get {
            return assignedPlayer;
        }

        set {
            assignedPlayer = value;
        } 
    }

    public PaddleServeController ServeController {
        get {
            if (serveController == null) {
                serveController = gameObject.GetComponentSafe<PaddleServeController> ();
            }
            return serveController;
        }
    }

    public PaddleStrikeController StrikeController {
        get {
            if (serveController == null) {
                strikeController = gameObject.GetComponentSafe<PaddleStrikeController> ();
            }
            return strikeController;
        }
    }

    public Rigidbody2D RB {
        get {
            return rb;
        }
    }

    public Vector3 ColliderNormal {
        get {
            return colliderNormal;
        }
    }

    public float ColliderLength {
        get {
            return colliderLength;
        }
    }

    public float ColliderMidpoint {
        get {
            return colliderMidpoint;
        }
    }

    public          PaddleParamters        Parameters       = null;

    protected const string                 kHAxis           = "Horizontal";

    protected       Player.PlayerID        assignedPlayer   = Player.PlayerID.None;
    protected       Ball                   ball             = null;

    private         PaddleServeController  serveController  = null;
    private         PaddleStrikeController strikeController = null;
    private         Rigidbody2D            rb               = null;
    private         int                    serveState       = 0;
    private         int                    playState        = 0;
    private         float                  colliderLength   = 1.0f;
    private         float                  colliderMidpoint = 0.0f;
    private         float                  xLeftWrapBound   = 0.0f;
    private         float                  xRightWrapBound  = 0.0f;
    private         bool                   isInServePhase   = false;
    private         bool                   isInPlayPhase    = false;
    private         bool                   isSetAsServing   = false;
    private         bool                   hasServed        = false;
    private         Vector3                colliderNormal   = Vector3.zero;
    private         Vector3                hMoveForce       = Vector3.zero;

    public bool IsInServePhase {
        get {
            return isInServePhase;
        }
        set {
            isInServePhase = value;
        }
    }

    public bool IsInPlayPhase {
        get {
            return isInPlayPhase;
        }
        set {
            isInPlayPhase = value;
        }
    }

    public bool HasServed {
        get {
            return hasServed;
        }
        set {
            hasServed = value;
        }
    }

    public bool IsSetAsServing {
        get {
            return isSetAsServing;
        }
        set {
            isSetAsServing = value;
        }
    }

    private float hForceMultiplier { 
        get { 
            return Parameters.HorizontalMoveForceMultiplier; 
        } 
    }

    protected abstract bool ServeBall ();
    protected abstract Vector3 CalculateHorizontalMoveForce (float paddleMoveSpeed);

    protected virtual void NewGameInitialisation () {
        transform.position = Parameters.FixedHorizontalPosition.AsVectorComponenty ();

        Vector2[] colliderEndPoints = GetComponent<EdgeCollider2D> ().points;
        colliderLength = CalculateLength (colliderEndPoints);
        colliderMidpoint = colliderLength / 2;

        float[] wallBounds = WallManager.CalculateVerticalLeftAndRightWrapBounds (colliderLength);
        xLeftWrapBound = wallBounds[0];
        xRightWrapBound = wallBounds[1];

        rb = GetComponent<Rigidbody2D> ();
    }

    private Vector3 WrapPositionIfOffScreen (float xComponentCurrentPosition) {
        float xInverted = xComponentCurrentPosition * -1.0f; // sign flip
        if ((xComponentCurrentPosition - colliderMidpoint) < xLeftWrapBound) {
            return new Vector3 (xInverted - colliderLength, Parameters.FixedHorizontalPosition, 0.0f);
        } else if ((xComponentCurrentPosition + colliderMidpoint) > xRightWrapBound) {
            return new Vector3 (xInverted + colliderLength, Parameters.FixedHorizontalPosition, 0.0f);
        } else {
            return transform.position;
        }
    }

    private Vector3 CalculateNormal (Player.PlayerID pid) {
        if (pid == Player.PlayerID.P1) {
            return transform.position.FindNormal2DLeftHand ();
        } else {
            return transform.position.FindNormal2DRightHand ();
        }
    }

    private float CalculateLength (Vector2[] paddleEndpoints) {
        return Mathf.Abs (paddleEndpoints [0].x - paddleEndpoints [1].x);
    }

    private void Awake () {
        instances.Add (this);
    }

    private void Start () {
        NewGameInitialisation ();
    }

    private void Update () {
        if (isInServePhase == true && isSetAsServing == true) {
            if (serveState == 0) {
                if (ball == null) {
                    ball = BallManager.StaticInstance.CurrentBall;
                }
                serveState = 1;
            }

            if (serveState == 1) {
                ServeController.SetForService(this, ball);
                serveState = 2;
            }

            if (serveState == 2) {
                hasServed = ServeBall ();
                if (hasServed == true) {
                    Vector3 servePower = ServeController.CalculateServiceForce (this);
                    ServeController.Serve (ball, servePower);
                    isSetAsServing = false;
                    isInServePhase = false;
                    isInPlayPhase = true;
                    serveState = 0;
                }
            }
        } else {
            colliderNormal = CalculateNormal (assignedPlayer);
            isInPlayPhase = true;
        }

        if (isInPlayPhase == true) {
            if (playState == 0)  {
                Debug.LogWarningFormat("[STATUS] Paddle: {0} in play phase update loop", AssignedPlayer);
            }
        }
    }

    private void FixedUpdate () {
        hMoveForce = CalculateHorizontalMoveForce (hForceMultiplier);
        rb.AddForce (hMoveForce);

        transform.position = WrapPositionIfOffScreen (transform.position.x);
    }
}