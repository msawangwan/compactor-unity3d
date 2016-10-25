﻿using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class Game  {
    [System.Serializable]
    public class GameParameters {
        [SerializeField] private Score winningScore;
        [SerializeField] private Score requiredWins;

        public Score WinningScore { get { return winningScore; } }
        public Score RequiredWins { get { return requiredWins; } }
    }

    public enum State { 
        None, 
        Menu, 
        Serve, 
        Play 
    }

    public IEnumerable<Player> Players { get { return players; } }
    public Player PlayerOne            { get { return players [0]; } }
    public Player PlayerTwo            { get { return players [1]; } }
    public Player PlayerServing        { get { return playerServing; } }
    public Player PlayerRoundWinner    { get { return winnerRound.Current; } }
    public Player PlayerGameWinner     { get { return winnerGame.Current; } }

    private Player[] players       = null;
    private Player   playerServing = null;
    private Winner   winnerRound   = default(Winner);
    private Winner   winnerGame    = default(Winner);

    public static Game EnterState (GameState state) {
        return state.UpdateState();
    }

    public Game () {
        Player player1 = new Player (Player.PlayerID.P1);
        Player player2 = new Player (Player.PlayerID.P2);

        players = MapPlayer (player1, player2);
    }

    public Game (Player[] pAll, bool useOldScore = true) {
        Player player1 = Player.NewCopyFrom (pAll[0]);
        Player player2 = Player.NewCopyFrom (pAll[1]);

        players = MapPlayer (player1, player2);
    }

    public Game (Player p1, Player p2, bool useOldScore = true) {
        Player player1 = Player.NewCopyFrom (p1);
        Player player2 = Player.NewCopyFrom (p2);

        players = MapPlayer (player1, player2);
    }

    public Game (Player p1, Player p2, Winner winner, bool gameWon = false) {
        Player player1 = Player.NewCopyFrom (p1);
        Player player2 = Player.NewCopyFrom (p2);

        if (gameWon == false) {
            this.winnerRound = winner;
        } else {
            this.winnerGame = winner;
        }

        players = MapPlayer (player1, player2);
    }

    public static Game CopyOf (Game game) {
        return new Game (game.PlayerOne, game.PlayerTwo);
    }

    public static Player.PlayerID TryDetermineScoringPlayer (Func<int> onScore) {
        Player.PlayerID scorer = (Player.PlayerID) onScore.InvokeSafe ();

        if (scorer != 0) {
            return scorer;
        }

        return 0;
    }

    private Player[] MapPlayer (Player p1, Player p2) {
        p1.AssignPaddle ();
        p2.AssignPaddle ();

        return new Player[] { p1, p2 };
    }
}