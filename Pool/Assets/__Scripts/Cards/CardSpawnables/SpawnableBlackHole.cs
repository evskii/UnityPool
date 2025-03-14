using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

using UnityEngine;

public class SpawnableBlackHole : MonoBehaviour
{
    private bool isActive = false;
    
    public void InitBlackHole() {
        isActive = true;
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Ball") && isActive) {
            var tablePos = GameplayManager.instance.GenerateTablePosition();
            Vector3 moveToPos = new Vector3(tablePos.x, other.transform.position.y, tablePos.y);
            other.transform.position = moveToPos;
        }
    }
}
