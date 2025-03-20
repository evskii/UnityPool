using System;
using System.Collections;
using System.Collections.Generic;

using Fusion;

using UnityEngine;

public class GameplayManager : NetworkBehaviour
{
    public static GameplayManager instance;
    private ChangeDetector changeDetector;
    
    private int turnNumber = 0;

    [SerializeField] private GameObject shotUi;

    public override void Spawned() {
        instance = this;
        
        changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);

        turnNumber = 0;
    }
    
    [Networked] public int playersTurn { get; set; }
    
    public void NextTurn(PoolPlayer calledBy) {
        if (playersTurn == calledBy.playerNumber) {
            playersTurn = playersTurn == 1 ? 2 : 1;
            turnNumber++;
        }
    }

    public bool MyTurn(PoolPlayer player) {
        return player.playerNumber == playersTurn;
    }

    public int GetTurnNumber() {
        return turnNumber;
    }

    public Vector3 GenerateTablePosition() {
        return Vector3.zero;
    }
}
