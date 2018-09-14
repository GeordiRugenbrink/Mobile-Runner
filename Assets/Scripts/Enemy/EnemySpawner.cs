using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    /// <summary>
    /// The Wave class used to make waves of enemies.
    /// </summary>
    [System.Serializable]
    public class Wave {
        public Enemy[] enemies;
        public float timeBetweenNextWave;
    }
    /// <summary>
    /// The enemy spawned by the Wave class.
    /// </summary>
    [System.Serializable]
    public class Enemy {
        public GameObject enemy;
        public int lane;
        public float spawnHeight;
    }

    public static int enemiesAlive = 0;
    public Wave[] waves;

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

    /// <summary>
    /// Checks if the last wave is over and if there aren't any enemies alive anymore
    /// and then calls the method to complete the level.
    /// 
    /// Checks if the waveCountDown has reached 0 and if it isn't the last wave
    /// and then spawns the next wave.
    /// 
    /// Checks if an enemy is supposed to shoot and then grabs a random enemy to shoot.
    /// </summary>
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

    /// <summary>
    /// Makes a local wave object and gives it the values of the wave tto spawn.
    /// Then it loops through all the enemies of that wave and spawns them.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnWave() {
        Wave wave = waves[waveIndex];
        enemiesAlive += wave.enemies.Length;

        for (int i = 0; i < wave.enemies.Length; i++) {
            Spawnenemy(wave.enemies[i].enemy);
            yield return null;
        }
        waveIndex++;
    }

    /// <summary>
    /// Spawns the enemies and puts them in the positions defined by the wave.
    /// </summary>
    /// <param name="enemy">The enemy to spawn</param>
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
    /// <summary>
    /// Gets a random enemy that is able to shoot.
    /// </summary>
    /// <returns></returns>
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
