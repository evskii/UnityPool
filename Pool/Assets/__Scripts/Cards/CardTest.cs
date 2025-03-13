using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTest : Card
{

    [Header("Card Specific Variables")]
    [SerializeField] private GameObject wallPrefab;
    private GameObject wallObject;
    [SerializeField] private LayerMask cardPlacementLayer;
    
    public override void DrawCard() {
        base.DrawCard();
        
        Debug.Log("Card has been drawn: " + cardName);
    }
    
    public override void PlayCard() {
        base.PlayCard();
    }

    public override void UpdateCard() {
        Debug.Log("Card has been updated: " + cardName);

        if (!wallObject) {
            wallObject = Instantiate(wallPrefab);
        } else {
            // var targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray mouseRay = Camera.main.ScreenPointToRay (Input.mousePosition);
            if (Physics.Raycast (mouseRay, out RaycastHit hit, Mathf.Infinity, cardPlacementLayer)){
                wallObject.transform.position = Vector3.Lerp(wallObject.transform.position, hit.point, 5 * Time.deltaTime);
                // Debug.Log(hit.point);
            }
        }
    }
    public override void DiscardCard() {
        Debug.Log("Card has been discarded: " + cardName);
        if (wallObject) {   
            Destroy(wallObject);
        }
        base.DiscardCard();
    }
}
