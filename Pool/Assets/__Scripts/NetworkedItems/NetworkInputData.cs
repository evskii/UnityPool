using System.Collections;
using System.Collections.Generic;

using Fusion;

using UnityEngine;

public struct NetworkInputData : INetworkInput
{
	public const byte MOUSEBUTTON0 = 1;
	
	public NetworkButtons buttons;
	public Vector3 direction;
	public int shotPowerControl;
}
