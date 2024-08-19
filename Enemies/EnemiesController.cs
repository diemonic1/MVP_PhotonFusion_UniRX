using Fusion;
using UnityEngine;

public class EnemiesController : NetworkBehaviour
{
    [SerializeField] private int _countOfEnemiesOnScene = 10;
    [SerializeField] private float _spawnRadius = 10f;
    [SerializeField] private NetworkPrefabRef[] _enemyPrefab;

    public override void Spawned()
    {
        if (Object.HasStateAuthority == false) return;

        for (int i = 0; i < _countOfEnemiesOnScene; i++)
            Spawn();
    }

    public void Despawn(NetworkObject obj)
    {
        if (HasStateAuthority == false) return;

        Runner.Despawn(obj);
        Spawn();
    }

    private void Spawn()
    {
        Runner.Spawn(_enemyPrefab[Random.Range(0, _enemyPrefab.Length)], GeneratePosition(), Quaternion.identity, PlayerRef.None);
    }

    private Vector3 GeneratePosition()
    {
        return new Vector3(Random.Range(-_spawnRadius, _spawnRadius), 1, Random.Range(-_spawnRadius, _spawnRadius));
    }
}
