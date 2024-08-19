using System.Collections.Generic;
using System;

public class PlayerModel
{
    public delegate void MoveHandler(Position move, Position rotation);
    public delegate void ParametrsHandler(float playerSpeed, int damagePerSecond, int radius);
    public delegate void EnemyCounterHandler(int count);

    public event MoveHandler OnMoveCalculated;
    public event ParametrsHandler OnParametrsChanged;
    public event EnemyCounterHandler OnEnemyCounted;

    private float _playerSpeed;
    private int _damagePerSecond;
    private int _radius;

    private int _countOfEnemiesToATK;

    private int _counter;

    private int _probabilityLVLUPOfSpeed;
    private int _probabilityLVLUPOfDps;
    private int _probabilityLVLUPOfRadius;

    private float _stepLVLUPOfSpeed;
    private int _stepLVLUPOfDps;
    private int _stepLVLUPOfRadius;

    private float _gravityValue = -9.81f;
    private List<Tuple<Position, Enemy>> _enemiesNearby = new List<Tuple<Position, Enemy>>();

    public void InitAtackParams(int damagePerSecond, int radius, int countOfEnemiesToATK, int probabilityLVLUPOfDps, int probabilityLVLUPOfRadius, int stepLVLUPOfDps, int stepLVLUPOfRadius)
    {
        _damagePerSecond = damagePerSecond;
        _radius = radius;
        _countOfEnemiesToATK = countOfEnemiesToATK;
        _probabilityLVLUPOfDps = probabilityLVLUPOfDps;
        _probabilityLVLUPOfRadius = probabilityLVLUPOfRadius;
        _stepLVLUPOfDps = stepLVLUPOfDps;
        _stepLVLUPOfRadius = stepLVLUPOfRadius;
    }

    public void InitMoveParams(float playerSpeed, int probabilityLVLUPOfSpeed, float stepLVLUPOfSpeed)
    {
        _playerSpeed = playerSpeed;
        _probabilityLVLUPOfSpeed = probabilityLVLUPOfSpeed;
        _stepLVLUPOfSpeed = stepLVLUPOfSpeed;
    }

    public void LevelUp()
    {
        if (_probabilityLVLUPOfSpeed + _probabilityLVLUPOfDps + _probabilityLVLUPOfRadius != 100)
            throw new ArgumentException("The sum of the probability of level up should be equal to 100! Now it's " + _probabilityLVLUPOfSpeed + _probabilityLVLUPOfDps + _probabilityLVLUPOfRadius);

        if (_stepLVLUPOfSpeed < 0 || _stepLVLUPOfDps < 0 || _stepLVLUPOfRadius < 0)
            throw new ArgumentException("Steps of Level Ups Must be positive!");

        Random random = new Random();
        int randonSolution = random.Next(101);

        if (0 <= randonSolution && randonSolution <= _probabilityLVLUPOfSpeed)
            _playerSpeed += _stepLVLUPOfSpeed;
        else if (_probabilityLVLUPOfSpeed + 1 <= randonSolution && randonSolution <= _probabilityLVLUPOfSpeed + _probabilityLVLUPOfDps)
            _damagePerSecond += _stepLVLUPOfDps;
        else
            _radius += _stepLVLUPOfRadius;

        OnParametrsChanged?.Invoke(_playerSpeed, _damagePerSecond, _radius);
    }

    public void AttackEnemies(Position playerPosition)
    {
        _enemiesNearby.RemoveAll(item => item.Item2 == null);
        
        List<Tuple<Position, Enemy>> secure—opyEnemiesNearby = new List<Tuple<Position, Enemy>>(_enemiesNearby);

        PrepareDistanceInfo(secure—opyEnemiesNearby, playerPosition);
    }

    public void AddEnemyNearby(Position enemyPosition, Enemy enemy)
    {
        _enemiesNearby.Add(Tuple.Create(enemyPosition, enemy));
    }

    public void RemoveEnemyNearby(Enemy enemy)
    {
        _enemiesNearby.RemoveAll(item => item.Item2 == enemy);
    }

    public void —alculatePlayerMove(float xInput, float zInput, float deltaTime)
    {
        Position move = new Position(xInput * deltaTime * _playerSpeed, _gravityValue * deltaTime, zInput * deltaTime * _playerSpeed);

        Position rotation = new Position(xInput * deltaTime * _playerSpeed, 0, zInput * deltaTime * _playerSpeed);

        OnMoveCalculated?.Invoke(move, rotation);
    }

    private void PrepareDistanceInfo(List<Tuple<Position, Enemy>> secure—opyEnemiesNearby, Position playerPosition)
    {
        List<Tuple<float, Enemy>> distancesAndEnemies = new List<Tuple<float, Enemy>>();

        foreach (var enemy in secure—opyEnemiesNearby)
        {
            distancesAndEnemies.Add(Tuple.Create(GetDistance(enemy.Item1, playerPosition), enemy.Item2));
        }

        distancesAndEnemies.Sort((a, b) => a.Item1.CompareTo(b.Item1));

        AttackNearbyTargets(distancesAndEnemies);
    }

    private void AttackNearbyTargets(List<Tuple<float, Enemy>> distancesAndEnemies)
    {
        int i = 0;

        foreach (var item in distancesAndEnemies)
        {
            bool isKill = item.Item2.gameObject.GetComponent<Enemy>().TryToKill(_damagePerSecond);

            if (isKill)
            {
                _counter++;
                OnEnemyCounted?.Invoke(_counter);
            }

            i++;
            if (i >= _countOfEnemiesToATK)
                break;
        }
    }

    private float GetDistance(Position fPosition, Position sPosition)
    {
        float num = fPosition.x - sPosition.x;
        float num2 = fPosition.y - sPosition.y;
        float num3 = fPosition.z - sPosition.z;
        return (float)Math.Sqrt(num * num + num2 * num2 + num3 * num3);
    }
}
