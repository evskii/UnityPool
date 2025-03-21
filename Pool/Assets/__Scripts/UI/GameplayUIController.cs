using System;
using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{
    public static GameplayUIController instance;

    private void Awake() {
        instance = this;
    }

    [SerializeField] private GameObject shotPowerUI;
    [SerializeField] private Image shotPowerImage;
    
    
    public void SetShotPowerFill(float amount) {
        shotPowerImage.fillAmount = amount;
    }

    public void ToggleShotPowerUI(bool enable) {
        shotPowerUI.SetActive(enable);
    }
}
