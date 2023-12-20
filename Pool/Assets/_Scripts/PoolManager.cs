using System;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;
using UnityEngine.SceneManagement;

using Random = UnityEngine.Random;


public class PoolManager : MonoBehaviour
{
	[SerializeField] private GameObject ballWhite;
	[SerializeField] private float testBreakForce;
	[SerializeField] private Vector3 testBreakDir;

	[ContextMenu("Test Break")]
	public void TestBreak() {
		ballWhite.GetComponent<Rigidbody>().AddForce(testBreakDir * (testBreakForce * Random.Range(0.9f, 1.1f)), ForceMode.Impulse);
	}

	[ContextMenu("Reload Scene")]
	public void ReloadScene() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.R)) {
			ReloadScene();
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			TestBreak();
		}
	}

	private void OnDrawGizmos() {
		Gizmos.DrawRay(ballWhite.transform.position, testBreakDir * (testBreakForce/10));
	}
}
