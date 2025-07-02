using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class FusionLauncher : MonoBehaviour, INetworkRunnerCallbacks
{
    public TMP_InputField roomCodeInput;
    public Button createButton;
    public Button joinButton;
    public NetworkRunner runnerPrefab;

    private void Start()
    {
        createButton.onClick.AddListener(() => StartHost(roomCodeInput.text));
        joinButton.onClick.AddListener(() => StartClient(roomCodeInput.text));
    }

    async void StartHost(string roomCode)
    {
        if (string.IsNullOrWhiteSpace(roomCode))
        {
            Debug.LogWarning("Room code is empty. Cannot create room.");
            return;
        }

        var runner = Instantiate(runnerPrefab);
        runner.AddCallbacks(this);
        DontDestroyOnLoad(runner.gameObject);

        await runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Host,
            SessionName = roomCode,
            Scene = SceneRef.FromIndex(1),
            SceneManager = runner.GetComponent<NetworkSceneManagerDefault>()
        });
    }

    async void StartClient(string roomCode)
    {
        if (string.IsNullOrWhiteSpace(roomCode))
        {
            Debug.LogWarning("Room code is empty. Cannot join room.");
            return;
        }

        var runner = Instantiate(runnerPrefab);
        runner.AddCallbacks(this);
        DontDestroyOnLoad(runner.gameObject);

        await runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Client,
            SessionName = roomCode,
            SceneManager = runner.GetComponent<NetworkSceneManagerDefault>()
        });
    }

    // ✅ FULL INetworkRunnerCallbacks (as of latest Fusion version)
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
}