using Fusion;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Enemy : NetworkBehaviour
{
    [Networked(OnChanged = nameof(NetworkHealthChanged))]
    [SerializeField] private int _health { get; set; }

    [SerializeField] private TextMeshProUGUI _healthText;

    public override void Spawned()
    {
        UpdateUI();
    }
    
    public bool TryToKill(int damage)
    {
        if (damage <= 0)
            return false;

        int healthBeforeDamage = _health;

        DealDamageRpc(damage);

        if (damage >= healthBeforeDamage)
            return true;

        return false;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void DealDamageRpc(int damage)
    {
        _health -= damage;

        if (_health <= 0)
            FindObjectOfType<EnemiesController>().Despawn(GetComponent<NetworkObject>());
    }

    private static void NetworkHealthChanged(Changed<Enemy> changed)
    {
        changed.Behaviour.UpdateUI();
    }

    private void UpdateUI()
    {
        _healthText.text = _health.ToString(); // !!!!!
    }
}
