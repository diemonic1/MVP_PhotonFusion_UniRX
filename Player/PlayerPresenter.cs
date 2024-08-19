public class PlayerPresenter
{
    private readonly PlayerMovement playerMovement;
    private readonly PlayerAtack playerAtack;
    private readonly PlayerUI playerView;
    private readonly PlayerModel playerModel;

    public PlayerPresenter(PlayerUI playerView, PlayerMovement playerMovement, PlayerAtack playerAtack, PlayerModel playerModel)
    {
        this.playerView = playerView;
        this.playerMovement = playerMovement;
        this.playerAtack = playerAtack;
        this.playerModel = playerModel;
    }

    public void Enable()
    {
        playerView.OnLevelUp += LevelUp;
        playerAtack.OnAwaked += InitAtackParams;
        playerAtack.OnTryToAtack += TryToAtack;
        playerAtack.OnEnemyAppeared += AddEnemyNearby;
        playerAtack.OnEnemyDisappeared += RemoveEnemyNearby;
        playerMovement.OnAwaked += InitMoveParams;
        playerMovement.OnPlayerMoved += ÑalculatePlayerMove;

        playerModel.OnMoveCalculated += MovePlayer;
        playerModel.OnParametrsChanged += ChangeParametrs;
        playerModel.OnEnemyCounted += UpdateCounterUI;
    }

    public void Disable()
    {
        playerView.OnLevelUp -= LevelUp;
        playerAtack.OnAwaked -= InitAtackParams;
        playerAtack.OnTryToAtack -= TryToAtack;
        playerAtack.OnEnemyAppeared -= AddEnemyNearby;
        playerAtack.OnEnemyDisappeared -= RemoveEnemyNearby;
        playerMovement.OnAwaked -= InitMoveParams;
        playerMovement.OnPlayerMoved -= ÑalculatePlayerMove;

        playerModel.OnMoveCalculated -= MovePlayer;
        playerModel.OnParametrsChanged -= ChangeParametrs;
        playerModel.OnEnemyCounted -= UpdateCounterUI;
    }

    private void UpdateCounterUI(int count)
    {
        playerView.UpdateCounterUI(count);
    }

    private void LevelUp()
    {
        playerModel.LevelUp();
    }

    private void ChangeParametrs(float playerSpeed, int damagePerSecond, int radius)
    {
        playerAtack.ChangeParametrs(playerSpeed, damagePerSecond, radius);
        playerMovement.ChangeParametrs(playerSpeed, damagePerSecond, radius);
    }

    private void InitAtackParams(int damagePerSecond, int radius, int countOfEnemiesToATK, int probabilityLVLUPOfDps, int probabilityLVLUPOfRadius, int stepLVLUPOfDps, int stepLVLUPOfRadius)
    {
        playerModel.InitAtackParams(damagePerSecond, radius, countOfEnemiesToATK, probabilityLVLUPOfDps, probabilityLVLUPOfRadius, stepLVLUPOfDps, stepLVLUPOfRadius);
    }

    private void InitMoveParams(float playerSpeed, int probabilityLVLUPOfSpeed, float stepLVLUPOfSpeed)
    {
        playerModel.InitMoveParams(playerSpeed, probabilityLVLUPOfSpeed, stepLVLUPOfSpeed);
    }

    private void TryToAtack(Position playerPosition)
    {
        playerModel.AttackEnemies(playerPosition);
    }

    private void AddEnemyNearby(Position enemyPosition, Enemy enemy)
    {
        playerModel.AddEnemyNearby(enemyPosition, enemy);
    }

    private void RemoveEnemyNearby(Position enemyPosition, Enemy enemy)
    {
        playerModel.RemoveEnemyNearby(enemy);
    }

    private void ÑalculatePlayerMove(float xInput, float zInput, float deltaTime)
    {
        playerModel.ÑalculatePlayerMove(xInput, zInput, deltaTime);
    }

    private void MovePlayer(Position move, Position rotation)
    {
        playerMovement.MovePlayer(move, rotation);
    }
}
