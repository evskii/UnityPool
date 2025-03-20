using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{
    public static GameplayUIController instance;

    private void Awake() {
        instance = this;
    }

    [SerializeField] private Image shotPowerImage;

    public void SetShotPowerFill(float amount) {
        shotPowerImage.fillAmount = amount;
    }
}
