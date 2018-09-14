using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour {

    [SerializeField]
    private GameObject _projectilePrefab;
    public ObjectPooling _projectilePool;

    /// <summary>
    /// Gets a projectile out of a pool of projectiles and adds the spawnposition to it
    /// and then activates the projectile in the scene.
    /// </summary>
    public void Shoot() {
        GameObject obj = _projectilePool.GetPooledObject();
        Vector3 spawnPosition = transform.position + new Vector3(0, GetComponent<SpriteRenderer>().size.y / 2, 0);

        if (obj == null) {
            return;
        }

        obj.transform.position = spawnPosition;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true);
    }
}
