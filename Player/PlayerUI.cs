using Fusion;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]

public class PlayerUI : NetworkBehaviour
{
    public delegate void LevelUpHandler();

    public event LevelUpHandler OnLevelUp;

    private ReactiveProperty<float> _enemyCounter = new ReactiveProperty<float>();

    private CompositeDisposable _disposable = new CompositeDisposable();

    public void Init()
    {
        UniRxSubscribe();
    }

    public void UpdateCounterUI(int count)
    {
        _enemyCounter.Value = count;
    }

    private void UniRxSubscribe()
    {
        GameObject.FindGameObjectWithTag("LevelUpButton").GetComponent<Button>()
            .OnClickAsObservable()
            .Subscribe(_ => { OnLevelUp?.Invoke(); }).AddTo(_disposable);

        TextMeshProUGUI enemyCounterText = GameObject.FindGameObjectWithTag("EnemyCounter").GetComponent<TextMeshProUGUI>();
        _enemyCounter.SubscribeWithState(enemyCounterText, (x, t) => t.text = "Kills: " + x.ToString());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void OnDisable()
    {
        _disposable.Clear();
    }
}
