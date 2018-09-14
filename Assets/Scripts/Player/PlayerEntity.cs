using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : Entity {


    [SerializeField]
    private GameObject _gameOverScreen;

    [SerializeField]
    private GameObject[] _lifeIcons = new GameObject[3];

    public override void Start() {
        base.Start();
        if (_gameOverScreen != null) {
            _gameOverScreen.SetActive(false);
        }
        for (int i = 0; i < _lifeIcons.Length; i++) {
            _lifeIcons[i].SetActive(true);
        }
    }

    public override void Update() {
        base.Update();
    }

    /// <summary>
    /// When the player takes damage it subtracts the damage from it's current health
    /// and deactivates an icon in the top left corner.
    /// </summary>
    /// <param name="amount"></param>
    public override void TakeDamage(int amount) {
        base.TakeDamage(amount);
        for (int i = _lifeIcons.Length - 1; i >= 0; i--) {
            if (_lifeIcons[i].activeInHierarchy) {
                _lifeIcons[i].SetActive(false);
                break;
            }
        }
    }

    public override void Death() {
        base.Death();
        //TODO: add player death animation
        Time.timeScale = 0;
        _gameOverScreen.SetActive(true);
    }
}
