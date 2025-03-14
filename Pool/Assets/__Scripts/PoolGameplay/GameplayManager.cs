using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;
    private void Awake() {
        instance = this;
    }

    [SerializeField] private int playersTurn = 1;
    [SerializeField] private int turnNumber = 0;

    [Header("Settings")]
    public Vector2 tableSize;

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

    public Vector2 GenerateTablePosition() {
        float x = Random.Range(transform.position.x - tableSize.x / 2, transform.position.x + tableSize.x / 2);
        float y = Random.Range(transform.position.y - tableSize.y / 2, transform.position.y + tableSize.y / 2);
        return new Vector2(x, y);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = new Color(0.75f, 0.3f, 0.5f, 0.5f);
        Vector3 size = new Vector3(tableSize.x, 3f, tableSize.y);
        Gizmos.DrawCube(transform.position, size);
    }
}
