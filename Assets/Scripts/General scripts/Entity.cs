using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    [SerializeField]
    private int _maxHealth = 3;
    [SerializeField]
    private int _currentHealth;

    public virtual void Start() {
        _currentHealth = _maxHealth;
    }

    public virtual void Update() {
        if (_currentHealth <= 0) {
            Death();
        }
    }

    public virtual void TakeDamage(int amount) {
        _currentHealth -= amount;
    }

    public virtual void Death() {
        Destroy(gameObject);
    }
}
