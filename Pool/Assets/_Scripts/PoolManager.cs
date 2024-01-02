using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;

using Unity.VisualScripting;

using UnityEditor;

using UnityEngine;
using UnityEngine.PlayerLoop;
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

	[Header("Collision Detection")]
	[SerializeField] private float ballRadius = 0.09f;



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

		if (!IsWhiteMoving()) {
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
					// Debug.Log("Shot Taken (Force: " + shotForce + ")");
				}
			}
		}
		
		
		
		//Other Shit
		if (!IsWhiteMoving()) {
			guideline.enabled = true;
			DrawGuideline();	
		} else {
			guideline.enabled = false;
		}
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

		// if (GetPredictedCollisionPoint().hitObject != ballWhite) {
		// 	guideline.positionCount = 3;
		// 	Vector3 hitPoint = GetPredictedCollisionPoint().hitPoint;
		// 	guideline.SetPosition(1, hitPoint);
		// 	guideline.SetPosition(2, hitPoint + (GetPredictedCollisionPoint().hitObject.transform.position - hitPoint).normalized * 2);
		// } else {
		// 	guideline.positionCount = 2;
		// }
	}

	private struct PredictionData
	{
		public Vector3 hitPoint;
		public GameObject hitObject;

		public PredictionData(Vector3 point, GameObject obj) {
			hitObject = obj;
			hitPoint = point;
		}
	}
	private PredictionData GetPredictedCollisionPoint() {
		RaycastHit hit;
		Vector3 mouse = Vector3.zero;
		if (Input.GetMouseButton(0)) {
			mouse = mouseStartPos;
		} else {
			mouse = MousePos();
		}
		
		mouse.y = ballWhite.transform.position.y;
		Vector3 castDir = (mouse - ballWhite.transform.position).normalized;
		if (Physics.SphereCast(ballWhite.transform.position, ballRadius, castDir, out hit, 100, 1 << 7)) {
			return new PredictionData(hit.point, hit.transform.gameObject);
		}
		return new PredictionData(Vector3.zero, ballWhite);
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

	private bool IsWhiteMoving() {
		return ballWhite.GetComponent<Rigidbody>().velocity.magnitude >= 0.1f;
	}

	private void OnDrawGizmos() {
		// Gizmos.DrawRay(ballWhite.transform.position, testBreakDir * (testBreakForce/10));
		//
		// Gizmos.color = Color.white;
		// Gizmos.DrawLine(ballWhite.transform.position, MousePos());

		// Gizmos.color = new Color(1, 0, 1, 0.33f);
		// Gizmos.DrawSphere(ballWhite.transform.position, collisionBoundsRadius);
		//
		// Gizmos.color = Color.yellow;
		// for (int i = 0; i < predictionRayCount; i++) {
		// 	float x = collisionBoundsRadius * Mathf.Cos((360 / predictionRayCount) * i) + ballWhite.transform.position.x;
		// 	float z = collisionBoundsRadius * Mathf.Sin((360 / predictionRayCount) * i) + ballWhite.transform.position.z;
		// 	Gizmos.DrawRay(new Vector3(x, ballWhite.transform.position.y, z), Vector3.forward);
		// }
	}
}
