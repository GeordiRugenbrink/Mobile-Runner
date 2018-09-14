using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage : MonoBehaviour {

    [SerializeField]
    private int _damage = 1;
    [SerializeField]
    private bool _isShotByPlayer;

    /// <summary>
    /// When the projectile collides with either the player or the enemy 
    /// it deals the damage and sets itself inactive so it can be used again by the object pool.
    /// </summary>
    /// <param name="other">The other collider that the projectile collides with</param>
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<HostileEntity>()) {
            if (other.GetComponent<HostileEntity>().isShootable && _isShotByPlayer) {
                other.GetComponent<HostileEntity>().TakeDamage(_damage);
                gameObject.SetActive(false);
            } else if (!other.GetComponent<HostileEntity>().isShootable) {
                gameObject.SetActive(false);
            }
        } else if (other.GetComponent<PlayerEntity>() && !_isShotByPlayer) {
            other.GetComponent<PlayerEntity>().TakeDamage(_damage);
            gameObject.SetActive(false);
        }
    }
}
