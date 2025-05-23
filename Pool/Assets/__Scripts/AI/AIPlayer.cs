using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

using Unity.VisualScripting;

using UnityEditor.PackageManager;
using UnityEditor.ShaderKeywordFilter;

using UnityEngine;
using UnityEngine.UIElements;

using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class AIPlayer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float whiteballRadius;
    [SerializeField] private float guidelineLength = 2;
    [SerializeField] private float shotPower = 2.5f;
    
    [Space]
    [SerializeField] private GameObject whiteBall;
    [SerializeField] private GameObject targetBall;

    [SerializeField] private Transform[] pocketIdentifiers;

    [SerializeField] private Vector3 aimDirection;
    private Vector3 ballHitPoint;
    private Vector3 whiteBallPositionAtContact;

    private void Update() {
        Vector3 mousePos = MousePos();
        mousePos.y = whiteBall.transform.position.y;
        aimDirection = (mousePos - whiteBall.transform.position).normalized;

        //Spherecast to get where the white ball will collide
        RaycastHit hit;
        if (Physics.SphereCast(whiteBall.transform.position, whiteballRadius, aimDirection, out hit, 10)) {
            ballHitPoint = hit.point;

            whiteBallPositionAtContact = whiteBall.transform.position + (aimDirection.normalized * hit.distance);
        }

        if (!IsWhiteBallMoving()) {
            SetGuideline();   
        }
        
        if (Input.GetKeyDown(KeyCode.Space)) {
            TakeShot();
        }
    }

    private bool IsWhiteBallMoving() {
        return whiteBall.GetComponent<Rigidbody>().velocity.magnitude >= 0.1f;
    }

    public void TakeShot() {
        whiteBall.GetComponent<Rigidbody>().AddForce(aimDirection * shotPower, ForceMode.Impulse);
    }

    private void SetGuideline() {
        Vector3[] guidelinePoints = new Vector3[3];
        guidelinePoints[0] = whiteBall.transform.position;
        guidelinePoints[1] = whiteBallPositionAtContact;

        Vector3 direction = (targetBall.transform.position - whiteBallPositionAtContact).normalized;
        guidelinePoints[2] = ballHitPoint + (direction * guidelineLength);
        
        Guideline.instance.SetGuidelinePoints(guidelinePoints, whiteBallPositionAtContact);

    }
    
    private Vector3 MousePos() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, 1<<6)) {
            return hit.point;
        }
        return Vector3.zero;
    }

    private Transform FindClosestPocket(Vector3 hitPoint) {
        Transform closest = pocketIdentifiers[0];
        float closestDistance = Vector3.Distance(pocketIdentifiers[0].position, hitPoint);
        for (int i = 0; i < pocketIdentifiers.Length; i++) {
            float distance = Vector3.Distance(pocketIdentifiers[i].position, hitPoint);
            if (distance < closestDistance) {
                closestDistance = distance;
                closest = pocketIdentifiers[i];
            }
        }
        return closest;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(whiteBall.transform.position, whiteballRadius);
        
        Gizmos.color = Color.red;
        Gizmos.DrawRay(whiteBall.transform.position, aimDirection * 10);
        
        Gizmos.color = Color.blue;
        // Gizmos.DrawSphere(ballHitPoint, 0.05f);

        Gizmos.color = new Color(1, 1, 1, 0.5f);
        Gizmos.DrawSphere(whiteBallPositionAtContact, whiteballRadius);

    }
}
