using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDestroyScript : MonoBehaviour {

    [SerializeField]
    private float _timeBeforeDestruction = 2f;

    void OnEnable() {
        StartCoroutine(DestroyProjectiles());
    }

    /// <summary>
    /// After a certain delay it deactivates the projectile so it can be added to the pool again.
    /// </summary>
    /// <returns></returns>
    private IEnumerator DestroyProjectiles() {
        yield return new WaitForSeconds(_timeBeforeDestruction);
        this.gameObject.SetActive(false);
    }
}
