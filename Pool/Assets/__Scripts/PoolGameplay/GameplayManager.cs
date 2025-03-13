using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;
    private void Awake() {
        instance = this;
    }

    [SerializeField] private int playersTurn = 1;
    [SerializeField] private int turnNumber = 0;

    private void Start() {
        turnNumber = 0;
        playersTurn = 1;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            NextTurn();
        }
    }

    public bool MyTurn(int playerNumber) {
        return playerNumber == playersTurn;
    }

    public void NextTurn() {
        playersTurn = playersTurn == 1 ? 2 : 1;
        turnNumber++;
    }

    public int GetTurnNumber() {
        return turnNumber;
    }
}
