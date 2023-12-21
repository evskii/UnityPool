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
			TakeShot(shotDir);
			shotLinedUp = false;
			Debug.Log(shotForce);
		}
		
		//Other Shit
		DrawGuideline();
	}

	private void DrawGuideline() {
		guideline.enabled = true;
		Vector3 ballWhiteGuidePos = new Vector3(ballWhite.transform.position.x, 0.25f, ballWhite.transform.position.z);
		guideline.SetPosition(0, ballWhiteGuidePos);

		if (shotLinedUp) {
			guideline.SetPosition(1, ballWhiteGuidePos + shotDir.normalized * Vector3.Distance(ballWhite.transform.position, mouseStartPos));
		} else {
			Vector3 mouseGuidePos = new Vector3(MousePos().x, 0.25f, MousePos().z);
			guideline.SetPosition(1, mouseGuidePos);
		}
		
	}
	
	private Vector3 MousePos() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100)) {
			return hit.point;
		}
		return Vector3.zero;
	}

	private void TakeShot(Vector3 dir) {
		ballWhite.GetComponent<Rigidbody>().AddForce(dir * shotForce, ForceMode.Impulse);
	}

	private void OnDrawGizmos() {
		// Gizmos.DrawRay(ballWhite.transform.position, testBreakDir * (testBreakForce/10));
		
		Gizmos.color = Color.white;
		Gizmos.DrawLine(ballWhite.transform.position, MousePos());

	}
}
