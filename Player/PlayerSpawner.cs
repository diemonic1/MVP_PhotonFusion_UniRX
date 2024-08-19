using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] private GameObject _playerUI;
    [SerializeField] private GameObject _playerPrefab;

    [SerializeField] private PlayerBootstrap playerBootstrap;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            NetworkObject playerObj = Runner.Spawn(_playerPrefab, new Vector3(0, 1, 0), Quaternion.identity, player);

            Instantiate(_playerUI, Vector3.zero, Quaternion.identity);

            playerBootstrap.Init(
                playerObj.GetComponent<PlayerUI>(),
                playerObj.GetComponent<PlayerMovement>(),
                playerObj.GetComponent<PlayerAtack>()
                );
        }
    }
}
