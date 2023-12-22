using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;

using UnityEditor;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Random = UnityEngine.Random;


public class PoolManager : MonoBehaviour
{
	[SerializeField] private GameObject ballWhite;
	[SerializeField] private float testBreakForce;
	[SerializeField] private Vector3 testBreakDir;
	private LineRenderer guideline;

	[SerializeField] private float powerMax;
	[SerializeField] private float powerMin;
	[SerializeField] private float maxMouseDistance; 
	private Vector3 mouseStartPos;
	private float shotForce;
	private Vector3 shotDir;
	[SerializeField] private Image shotPowerImage;
	private bool shotLinedUp = false;

	[SerializeField] private LayerMask ballLayer;
	[SerializeField] private LayerMask tableLayer;
	
	
	private void Start() {
		guideline = GetComponent<LineRenderer>();
	}

	[ContextMenu("Test Break")]
	public void TestBreak() {
		ballWhite.GetComponent<Rigidbody>().AddForce(testBreakDir * (testBreakForce * Random.Range(0.9f, 1.1f)), ForceMode.Impulse);
	}

	[ContextMenu("Reload Scene")]
	public void ReloadScene() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	private void Update() {
		//Keyboard Controls
		if (Input.GetKeyDown(KeyCode.R)) {
			ReloadScene();
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			TestBreak();
		}

		
		if (Input.GetMouseButtonDown(0)) {
			mouseStartPos = MousePos();
			shotDir = (MousePos() - ballWhite.transform.position).normalized;
			shotLinedUp = true;
		}

		if (Input.GetMouseButton(0)) {
			shotForce = EvMath.Map(Mathf.Abs(Vector3.Distance(mouseStartPos, MousePos())), 0, maxMouseDistance, powerMin, powerMax);
			shotForce = Mathf.Clamp(shotForce, powerMin, powerMax);
			shotPowerImage.fillAmount = EvMath.Map(shotForce, powerMin, powerMax, 0, 1);
		}

		if (Input.GetMouseButtonUp(0)) {
			if (shotForce <= 0.25f) {
				shotLinedUp = false;
				Debug.Log("Shot Cancelled");
			} else {
				TakeShot(shotDir);
				shotLinedUp = false;
				Debug.Log("Shot Taken (Force: " + shotForce + ")");
			}
			
		}
		
		//Other Shit
		DrawGuideline();
	}

	private void DrawGuideline() {
		guideline.enabled = true;
		Vector3 ballWhiteGuidePos = ballWhite.transform.position;
		guideline.SetPosition(0, ballWhiteGuidePos);

		if (shotLinedUp) {
			Vector3 mousePosMod = new Vector3(mouseStartPos.x, ballWhiteGuidePos.y, mouseStartPos.z);
			Vector3 dir = shotDir;
			dir.y = 0;
			guideline.SetPosition(1, ballWhiteGuidePos + dir.normalized * Vector3.Distance(ballWhite.transform.position, mousePosMod));
		} else {
			Vector3 mouseGuidePos = new Vector3(MousePos().x, ballWhite.transform.position.y, MousePos().z);
			guideline.SetPosition(1, mouseGuidePos);
		}

		// RaycastHit hit;
		// Vector3 fixedMousPos = new Vector3(MousePos().x, ballWhite.transform.position.y, MousePos().z);
		// Vector3 rayDir = shotLinedUp ? new Vector3(shotDir.x, 0, shotDir.z) : (fixedMousPos - ballWhite.transform.position).normalized;
		// Debug.DrawRay(ballWhite.transform.position, (fixedMousPos - ballWhite.transform.position).normalized * 2, Color.red);
		// if (Physics.Raycast(ballWhite.transform.position, rayDir, out hit, Mathf.Infinity, ballLayer)) {
		// 	Debug.Log("HIT BALL: " + hit.transform.name);
		// 	Vector3 contactDir = (hit.transform.position - hit.point).normalized;
		// 	guideline.positionCount = 3;
		// 	guideline.SetPosition(2, hit.point + contactDir * 5);
		// }
	}
	
	private Vector3 MousePos() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100, 1<<6)) {
			return hit.point;
		}
		return Vector3.zero;
	}

	private void TakeShot(Vector3 dir) {
		ballWhite.GetComponent<Rigidbody>().AddForce(dir * (shotForce * Random.Range(0.90f,1.1f)), ForceMode.Impulse);
	}

	private void OnDrawGizmos() {
		// Gizmos.DrawRay(ballWhite.transform.position, testBreakDir * (testBreakForce/10));
		//
		// Gizmos.color = Color.white;
		// Gizmos.DrawLine(ballWhite.transform.position, MousePos());

	}
}
