using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyController _enemyPrefab;
    [SerializeField] private Transform[] _spawnLocations = {};
    [Range(1, 10)][SerializeField] private float _respawnTime = 2f;
    [Range(1, 100)][SerializeField] private int _maxNumberOfEnemies = 100;
    [SerializeField] private Transform _target;

    private int _nextSpawnIndex;
    private readonly List<GameObject> _enemies = new();

    private void Start()
    {
        if (_spawnLocations.Length > 0)
            StartCoroutine(StartSpawningEnemies(_respawnTime));
    }

    private IEnumerator StartSpawningEnemies(float respawnTime)
    {
        var delay = new WaitForSeconds(respawnTime);

        while (true)
        {
            if (_enemies.Count < _maxNumberOfEnemies || _enemies.RemoveAll(item => !item) > 0)
            {
                var location = _spawnLocations[_nextSpawnIndex];
                _nextSpawnIndex = ++_nextSpawnIndex % _spawnLocations.Length;
                var enemy = Instantiate(_enemyPrefab, location.position, Quaternion.identity);
                enemy.SetTarget(_target);
                _enemies.Add(enemy.gameObject);
            }

            yield return delay;
        }
    }
}
