using Fusion;
using TMPro;
using UniRx;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public delegate void AwakeHandler(float playerSpeed, int probabilityLVLUPOfSpeed, float stepLVLUPOfSpeed);
    public delegate void InputHandler(float xInput, float zInput, float deltaTime);

    public event AwakeHandler OnAwaked;
    public event InputHandler OnPlayerMoved;

    [Header("Parameters for initialization")]
    [SerializeField] private int _probabilityLVLUPOfSpeed = 60;
    [SerializeField] private float _stepLVLUPOfSpeed = 0.5f;

    [SerializeField] private ReactiveProperty<float> _playerSpeed = new ReactiveProperty<float>();

    private CharacterController _controller;

    public void Init()
    {
        _controller = GetComponent<CharacterController>();

        UniRxSubscribe();

        OnAwaked?.Invoke(_playerSpeed.Value, _probabilityLVLUPOfSpeed, _stepLVLUPOfSpeed);
    }

    public void ChangeParametrs(float playerSpeed, int damagePerSecond, int radius)
    {
        _playerSpeed.Value = playerSpeed;
    }

    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority == false) return;

        OnPlayerMoved?.Invoke(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Runner.DeltaTime);
    }

    public void MovePlayer(Position move, Position rotation)
    {
        _controller.Move(new Vector3(move.x, move.y, move.z));

        if (rotation.x != 0 || rotation.z != 0)
            gameObject.transform.forward = new Vector3(rotation.x, rotation.y, rotation.z);
    }

    private void UniRxSubscribe()
    {
        TextMeshProUGUI playerSpeedText = GameObject.FindGameObjectWithTag("PlayerSpeedText").GetComponent<TextMeshProUGUI>();
        _playerSpeed.SubscribeWithState(playerSpeedText, (x, t) => t.text = "Speed: " + x.ToString() + "; lvlup chance: " + _probabilityLVLUPOfSpeed.ToString() + "%");
    }
}
