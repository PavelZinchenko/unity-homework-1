using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform[] _spawnLocations = {};
    [Range(1, 10)][SerializeField] private float _respawnTime = 2f;
    [Range(1, 100)][SerializeField] private int _maxNumberOfEnemies = 100;

    private void Update()
    {
        if (_spawnLocations.Length == 0) return;

        _cooldown -= Time.deltaTime;
        if (_cooldown > 0) return;
        _cooldown = _respawnTime;

        if (_enemies.Count >= _maxNumberOfEnemies && _enemies.RemoveAll(item => !item) == 0)
            return;

        var location = _spawnLocations[_nextSpawnIndex];
        _nextSpawnIndex = ++_nextSpawnIndex % _spawnLocations.Length;
        _enemies.Add(GameObject.Instantiate(_enemyPrefab, location.position, Quaternion.identity));
    }

    private int _nextSpawnIndex;
    private float _cooldown;
    private List<GameObject> _enemies = new();
}
    