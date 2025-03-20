using System;
using System.Collections;
using System.Collections.Generic;

using Fusion;
using Fusion.Addons.Physics;
using Fusion.Sockets;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
	private NetworkRunner runner;

	[SerializeField] private NetworkPrefabRef playerPrefab;
	private Dictionary<PlayerRef, NetworkObject> spawnCharacters = new Dictionary<PlayerRef, NetworkObject>();

	private bool mouseButton0;

	private void Update() {
		mouseButton0 = mouseButton0 | Input.GetMouseButton(0);
	}

	async void StartGame(GameMode mode) {
		//Create our runner
		runner = gameObject.AddComponent<NetworkRunner>();
		runner.ProvideInput = true;

		var runnerSimulatePhysics3D = gameObject.AddComponent<RunnerSimulatePhysics3D>();
		runnerSimulatePhysics3D.ClientPhysicsSimulation = ClientPhysicsSimulation.SimulateAlways;
		
		//Create scene info for our networked scene
		var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
		var sceneInfo = new NetworkSceneInfo();
		if (scene.IsValid) {
			sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
		}
		
		//Start a session
		await runner.StartGame(new StartGameArgs()
		{
			GameMode = mode,
			SessionName = "Test Room",
			Scene = scene,
			SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
		});
	}

	// 0 = Host | 1 = Client
	public void StartGameHost(int modeIndex) {
		if (runner == null) {
			var x = modeIndex == 0 ? GameMode.Host : GameMode.Client;
			StartGame(x);
		}
	}
	

	public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
	public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
	public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) {
		if (runner.IsServer) {
			// Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 1, 0);
			NetworkObject networkPlayerObject = runner.Spawn(playerPrefab, Vector3.zero, Quaternion.identity, player);
			spawnCharacters.Add(player, networkPlayerObject);
			networkPlayerObject.GetComponent<PoolPlayer>().InitPoolPlayer(spawnCharacters.Count);
		}
	}
	public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) {
		if (spawnCharacters.TryGetValue(player, out NetworkObject networkObject)) {
			runner.Despawn(networkObject);
			spawnCharacters.Remove(player);
		}
	}
	public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
	public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
	public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
	public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
	public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
	public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
	public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
	public void OnInput(NetworkRunner runner, NetworkInput input) {
		var data = new NetworkInputData();
		
		

		data.direction = FindObjectOfType<PhysicsBall>().ShotDirection();
		
		data.buttons.Set(NetworkInputData.MOUSEBUTTON0, mouseButton0);
		mouseButton0 = false;

		input.Set(data);
	}
	public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
	public void OnConnectedToServer(NetworkRunner runner) { }
	public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
	public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) {	}
	public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
	public void OnSceneLoadDone(NetworkRunner runner) { }
	public void OnSceneLoadStart(NetworkRunner runner) { }
}
