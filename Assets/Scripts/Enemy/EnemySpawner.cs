using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [System.Serializable]
    public class Wave {
        [System.Serializable]
        public class Enemy {
            public GameObject enemy;
            public int lane;
            public float spawnHeight;
        }

        public Enemy[] enemies;
        public float timeBetweenNextWave;
    }

    public static int enemiesAlive = 0;
    public Wave[] waves;
    [SerializeField]
    private float _enemySpawnRate;

    [Tooltip("The initial countdown before the first wave spawns")]
    [SerializeField]
    private float _waveCountDown = 2f;
    [HideInInspector]
    public int waveIndex = 0;
    private int _currentEnemy = 0;
    private int _lane;
    private float _spawnHeight;
    [HideInInspector]
    public static List<GameObject> currentEnemies;

    [SerializeField]
    private ObjectPooling _projectileObjectPool;

    [SerializeField]
    private float _enemyFireRate = 1f;
    private float _lastEnemyShot;

    private void Awake() {
        _projectileObjectPool = GameObject.FindGameObjectWithTag("EnemyProjectilePool").GetComponent<ObjectPooling>();
    }

    private void Start() {
        _lastEnemyShot = Time.time + _waveCountDown;
        enemiesAlive = 0;
        currentEnemies = new List<GameObject>();
    }

    private void Update() {
        if (waveIndex == waves.Length && enemiesAlive == 0) {
            Utility.gameController.LevelCompleted();
            this.enabled = false;
        }
        if (_waveCountDown <= 0f && waveIndex < waves.Length) {
            StartCoroutine(SpawnWave());
            _waveCountDown = waves[waveIndex].timeBetweenNextWave;
            return;
        }
        if (Time.time > _lastEnemyShot + _enemyFireRate) {
            if (GetRandomEnemy() != null) {
                GameObject enemyToShoot = GetRandomEnemy();
                if (enemyToShoot != null) {
                    if (enemyToShoot.activeInHierarchy) {
                        enemyToShoot.GetComponent<EnemyShooting>().Shoot();
                    }
                    _lastEnemyShot = Time.time;
                }
            }
        }
        _waveCountDown -= Time.deltaTime;
        _waveCountDown = Mathf.Clamp(_waveCountDown, 0f, Mathf.Infinity);
    }

    private IEnumerator SpawnWave() {
        if (waveIndex < waves.Length) {
            Wave wave = waves[waveIndex];
            enemiesAlive += wave.enemies.Length;

            for (int i = 0; i < wave.enemies.Length; i++) {
                Spawnenemy(wave.enemies[i].enemy);
                yield return null;
            }
            waveIndex++;
        }
    }

    private void Spawnenemy(GameObject enemy) {
        Wave wave = waves[waveIndex];
        if (wave.enemies[_currentEnemy].enemy.GetComponent<HostileEntity>().canShoot) {
            wave.enemies[_currentEnemy].enemy.GetComponent<EnemyShooting>()._projectilePool = _projectileObjectPool;
        }

        _lane = wave.enemies[_currentEnemy].lane;
        _spawnHeight = wave.enemies[_currentEnemy].spawnHeight;
        GameObject obj = Instantiate(enemy, new Vector2(Utility.gameController.GetLaneX(_lane), _spawnHeight), Quaternion.identity);
        currentEnemies.Add(obj);
        _currentEnemy++;
        if (_currentEnemy > wave.enemies.Length - 1) {
            _currentEnemy = 0;
        }
    }

    private GameObject GetRandomEnemy() {
        var tempList = currentEnemies;
        for (int i = tempList.Count - 1; i >= 0; i--) {
            if (tempList[i].GetComponent<HostileEntity>()) {
                if (!tempList[i].GetComponent<HostileEntity>().canShoot) {
                    tempList.Remove(tempList[i]);
                }
            }
        }
        int randomNumber = Random.Range(0, tempList.Count - 1);
        if (tempList.Count > 0) {
            return tempList[randomNumber];
        } else {
            return null;
        }
    }
}
