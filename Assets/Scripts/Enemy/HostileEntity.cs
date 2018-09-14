using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileEntity : Entity {

    [SerializeField]
    private float _despawnHeight;

    public bool isShootable;
    public bool canShoot;

    [SerializeField]
    private int _playerDamageOnCollision = 1;

    /// <summary>
    /// Checks if the enemy has reached their despawning point and despawns them.
    /// </summary>
    public override void Update() {
        base.Update();
        if (_despawnHeight > 0) {
            if (transform.position.y > _despawnHeight) {
                Death();
            }
        } else {
            if (transform.position.y < _despawnHeight) {
                Death();
            }
        }
    }

    public override void TakeDamage(int amount) {
        base.TakeDamage(amount);
    }

    public override void Death() {
        base.Death();
        EnemySpawner.enemiesAlive--;
        EnemySpawner.currentEnemies.Remove(gameObject);
    }

    /// <summary>
    /// If the enemy collides with the player it kills the enemy
    /// and deals damage to the player.
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.GetComponent<PlayerEntity>()) {
            other.gameObject.GetComponent<PlayerEntity>().TakeDamage(_playerDamageOnCollision);
            Death();
        }
    }
}
