using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostileEntity : Entity {

    [SerializeField]
    private float despawnHeight;

    public bool isShootable;
    public bool canShoot;

    [SerializeField]
    private int playerDamageOnCollision = 1;

    public override void Update() {
        base.Update();
        if (despawnHeight > 0) {
            if (transform.position.y > despawnHeight) {
                Death();
            }
        } else {
            if (transform.position.y < despawnHeight) {
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

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.GetComponent<PlayerEntity>()) {
            other.gameObject.GetComponent<PlayerEntity>().TakeDamage(playerDamageOnCollision);
            Death();
        }
    }
}
