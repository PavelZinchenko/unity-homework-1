using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private Transform[] _spawnPoints = {};
    [Range(1, 10)][SerializeField] private float _respawnTime = 5f;
    [Range(1, 100)][SerializeField] private int _maxNumberOfCoins = 10;

    private void OnValidate()
    {
        if (_maxNumberOfCoins > _spawnPoints.Length)
            _maxNumberOfCoins = _spawnPoints.Length;
    }

    private void Start()
    {
        _freeSpawnPoints.AddRange(Enumerable.Range(0, _spawnPoints.Length));

        for (int i = 0; i < _maxNumberOfCoins; ++i)
            SpawnCoin();
    }

    private void Update()
    {
        _cooldown -= Time.deltaTime;
        if (_cooldown > 0) return;

        SpawnCoin();
        _cooldown = _respawnTime;
    }

    private void SpawnCoin()
    {
        bool allCoinsUsed = _coins.Count >= _maxNumberOfCoins || _freeSpawnPoints.Count == 0;
        if (allCoinsUsed && !TryRemoveCollectedCoins()) return;

        var index = _random.Next(_freeSpawnPoints.Count);
        var id = _freeSpawnPoints[index];
        _freeSpawnPoints.RemoveAt(index);
        var spawnPoint = _spawnPoints[id];

        var coin = GameObject.Instantiate(_coinPrefab, spawnPoint.position, Quaternion.identity);
        _coins.Add(coin);
        _usedSpawnPoints.Add(coin, id);
    }

    private bool TryRemoveCollectedCoins()
    {
        bool found = false;
        foreach (var coin in _coins)
        {
            if (coin) continue;

            var id = _usedSpawnPoints[coin];
            _usedSpawnPoints.Remove(coin);
            _freeSpawnPoints.Add(id);
            found = true;
        }

        if (found)
            _coins.RemoveAll(item => !item);

        return found;
    }

    private float _cooldown;
    private readonly List<int> _freeSpawnPoints = new();
    private readonly List<GameObject> _coins = new();
    private readonly Dictionary<GameObject, int> _usedSpawnPoints = new();
    private readonly System.Random _random = new();
}
