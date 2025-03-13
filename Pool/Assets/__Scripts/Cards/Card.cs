using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Card : MonoBehaviour
{
    [Header("Details")]
    public string cardName;
    public Sprite cardSprite;
    public GameObject cardVisuals;
    public int turnsToLast;
    private int turnPlaced;

    [Header("References")]
    [SerializeField] private TMP_Text textCardName;
    [SerializeField] private Image imageCardImage;

    [Header("Card Info")]
    [SerializeField] private bool inHand = false;
    [SerializeField] private bool isHolding = false;
    [SerializeField] private bool inPlayArea = false;
    [SerializeField] private bool isPlayed = false;
    private Vector3 startPosition;

    private void Start() {
        DrawCard();
    }

    private void Update() {
        if (inHand) {
            if (isHolding) {
                var targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetPosition.y = startPosition.y;
                transform.position = Vector3.Lerp(transform.position, targetPosition, 5 * Time.deltaTime);
            }
        }

        if (inPlayArea && !isPlayed) {
            UpdateCard();
            
            cardVisuals.SetActive(false);
        } else {
            cardVisuals.SetActive(true);
        }

        if (isPlayed) {
            if (GameplayManager.instance.GetTurnNumber() > turnsToLast + turnPlaced) {
                DiscardCard();
            }
        }
    }

    private void OnMouseDown() {
        if (inHand) {
            isHolding = true;
        }
    }

    private void OnMouseUp() {
        isHolding = false;
        transform.position = startPosition;

        if (inPlayArea) {
            PlayCard();
        }
    }

    public void InitCardVisuals() {
        textCardName.text = cardName;
        imageCardImage.sprite = cardSprite;

        // DrawCard();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("PlayArea")) {
            inPlayArea = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("PlayArea")) {
            inPlayArea = false;
        }
    }

    //To be handled in a child class
    public virtual void DrawCard() {
        inHand = true;
        startPosition = transform.position;
        InitCardVisuals();
    }
    public virtual void PlayCard() {
        turnPlaced = GameplayManager.instance.GetTurnNumber();
        cardVisuals.SetActive(false);
        isPlayed = true;
        // Destroy(gameObject);
    }
    public abstract void UpdateCard();
    public virtual void DiscardCard() {
        Destroy(gameObject);
    }
}
