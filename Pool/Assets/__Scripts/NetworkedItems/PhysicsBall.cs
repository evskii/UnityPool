using System.Collections;
using System.Collections.Generic;

using Fusion;

using UnityEngine;

public class PhysicsBall : NetworkBehaviour
{
	public LayerMask mousePositionLayer;

	public LineRenderer lineRenderer;
	
	public Vector3 ShotDirection() {
		return (MousePos() - transform.position).normalized;
	}
	
	public Vector3 MousePos() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100, mousePositionLayer)) {
			return hit.point;
		}
		return Vector3.zero;
	}
}
