using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectileMovement : MonoBehaviour {

    [SerializeField]
    private bool _isHostile;
    [SerializeField]
    private float _speed = 5f;

    private void Update() {
        Movement();
    }

    /// <summary>
    /// Moves the projectile up or down depending on if the projectile is hostile or not. 
    /// </summary>
    public virtual void Movement() {
        if (!_isHostile) {
            transform.Translate(new Vector3(0, _speed * Time.deltaTime, 0));
        } else {
            transform.Translate(new Vector3(0, -_speed * Time.deltaTime, 0));
        }
    }
}
