using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBlackHole : Card
{
    [Header("Card Specific Variables")]
    [SerializeField] private GameObject blackHolePrefab;
    private GameObject blackHoleObject;
    [SerializeField] private LayerMask cardPlacementLayer;
    
    public override void DrawCard() {
        base.DrawCard();
    }
    public override void PlayCard() {
        if (blackHoleObject) {
            blackHoleObject.GetComponent<SpawnableBlackHole>().InitBlackHole();
        }
        base.PlayCard();
    }
    public override void UpdateCard() {
        if (!blackHoleObject) {
            blackHoleObject = Instantiate(blackHolePrefab);
        } else {
            // var targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray mouseRay = Camera.main.ScreenPointToRay (Input.mousePosition);
            if (Physics.Raycast (mouseRay, out RaycastHit hit, Mathf.Infinity, cardPlacementLayer)){
                blackHoleObject.transform.position = Vector3.Lerp(blackHoleObject.transform.position, hit.point, 5 * Time.deltaTime);
                // Debug.Log(hit.point);
            }
        }
    }
    public override void DiscardCard() {
        if (blackHoleObject) {
            Destroy(blackHoleObject);
        }
        base.DiscardCard();
    }
}
