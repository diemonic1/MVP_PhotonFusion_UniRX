using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class PlayerAtack : MonoBehaviour
{
    public delegate void AwakeHandler(int damagePerSecond, int radius, int countOfEnemiesToATK, int probabilityLVLUPOfDps, int probabilityLVLUPOfRadius, int stepLVLUPOfDps, int stepLVLUPOfRadius);
    public delegate void AttackHandler(Position playerPosition);
    public delegate void EnemyNearbyHandler(Position enemyPosition, Enemy enemy);

    public event AwakeHandler OnAwaked;
    public event AttackHandler OnTryToAtack;
    public event EnemyNearbyHandler OnEnemyAppeared;
    public event EnemyNearbyHandler OnEnemyDisappeared;

    [Header("Parameters for initialization")]
    [SerializeField] private int _probabilityLVLUPOfDps = 30;
    [SerializeField] private int _probabilityLVLUPOfRadius = 10;

    [SerializeField] private int _stepLVLUPOfDps = 10;
    [SerializeField] private int _stepLVLUPOfRadius = 1;

    [SerializeField] private ReactiveProperty<int> _damagePerSecond = new ReactiveProperty<int>();
    [SerializeField] private ReactiveProperty<int> _radius = new ReactiveProperty<int>();

    [SerializeField] private int _countOfEnemiesToATK = 3;
    [SerializeField] private int _atkDelay = 1;

    [SerializeField] private GameObject _atackRadiusObj;

    private SphereCollider sphereCollider;

    private CompositeDisposable _disposable = new CompositeDisposable();

    public void Init()
    {
        sphereCollider = GetComponent<SphereCollider>();
        _atackRadiusObj.SetActive(true);
        ChangeAtackRadius();

        InvokeRepeating("AttackEnemiesTick", _atkDelay, _atkDelay);

        UniRxSubscribe();

        OnAwaked?.Invoke(_damagePerSecond.Value, _radius.Value, _countOfEnemiesToATK, _probabilityLVLUPOfDps, _probabilityLVLUPOfRadius, _stepLVLUPOfDps, _stepLVLUPOfRadius);
    }

    public void ChangeParametrs(float playerSpeed, int damagePerSecond, int radius)
    {
        _damagePerSecond.Value = damagePerSecond;
        _radius.Value = radius;

        ChangeAtackRadius();
    }

    private void UniRxSubscribe()
    {
        TextMeshProUGUI damagePerSecondText = GameObject.FindGameObjectWithTag("DamagePerSecondText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI radiusText = GameObject.FindGameObjectWithTag("RadiusText").GetComponent<TextMeshProUGUI>();

        _damagePerSecond.SubscribeWithState(damagePerSecondText, (x, t) => t.text = "DPS: " + x.ToString() + "; lvlup chance: " + _probabilityLVLUPOfDps.ToString() + "%");
        _radius.SubscribeWithState(radiusText, (x, t) => t.text = "Radius: " + x.ToString() + "; lvlup chance: " + _probabilityLVLUPOfRadius.ToString() + "%");

        GetComponent<Collider>().OnTriggerEnterAsObservable()
            .Where(t => t.gameObject.GetComponent<Enemy>())
            .Subscribe(other =>
            {
                Position position = new Position(other.transform.position.x, other.transform.position.y, other.transform.position.z);
                OnEnemyAppeared?.Invoke(position, other.gameObject.GetComponent<Enemy>());
            }).AddTo(_disposable);

        GetComponent<Collider>().OnTriggerExitAsObservable()
            .Where(t => t.gameObject.GetComponent<Enemy>())
            .Subscribe(other =>
            {
                Position position = new Position(other.transform.position.x, other.transform.position.y, other.transform.position.z);
                OnEnemyDisappeared?.Invoke(position, other.gameObject.GetComponent<Enemy>());
            }).AddTo(_disposable);
    }

    private void ChangeAtackRadius()
    {
        sphereCollider.radius = _radius.Value;
        _atackRadiusObj.transform.localScale = new Vector3(_radius.Value * 2, 0.01f, _radius.Value * 2);
    }

    private void AttackEnemiesTick()
    {
        Position position = new Position(transform.position.x, transform.position.y, transform.position.z);
        OnTryToAtack?.Invoke(position);
    }

    private void OnDisable()
    {
        _disposable.Clear();
    }
}
