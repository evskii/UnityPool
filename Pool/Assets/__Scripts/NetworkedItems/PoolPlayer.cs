using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Addons.Physics;

using UnityEngine;

public class PoolPlayer : NetworkBehaviour
{
	private ChangeDetector changeDetector;
    
	[Networked] public int playerNumber { get; set;}
	private PhysicsBall ball;


	[Header("Shot Settings")]
	[SerializeField] private float minShotPower;
	[SerializeField] private float maxShotPower;
	private float shotPower;
    
    

	public override void Spawned() {
		changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);

		ball = FindObjectOfType<PhysicsBall>();
	}
    
	public void InitPoolPlayer(int playerNumber) {
		this.playerNumber = playerNumber;
	}
    
	public override void FixedUpdateNetwork() {
		if (HasInputAuthority) {
			bool myTurn = GameplayManager.instance.MyTurn(this);
			ball.lineRenderer.enabled = myTurn && HasInputAuthority;
			if (myTurn && HasInputAuthority) {
				ball.lineRenderer.SetPosition(0, ball.transform.position);
				ball.lineRenderer.SetPosition(1, ball.MousePos());
			}

			if (Input.mouseScrollDelta.y > 0) {
				shotPower += 0.1f;
				shotPower = Mathf.Clamp(shotPower, minShotPower, maxShotPower);
				GameplayUIController.instance.SetShotPowerFill(EvMath.Map(shotPower, minShotPower, maxShotPower, 0, 1));
			}
		} 
        
        
		if (GetInput(out NetworkInputData data)) {
			if (HasStateAuthority && GameplayManager.instance.MyTurn(this)) {
				if (data.buttons.IsSet(NetworkInputData.MOUSEBUTTON0)) {
					Debug.Log("Force added by " + playerNumber);
					ball.GetComponent<NetworkRigidbody3D>().Rigidbody.AddForce(data.direction * shotPower, ForceMode.Impulse);
					GameplayManager.instance.NextTurn(this);
				}   
			}
		}
	}
    
}
