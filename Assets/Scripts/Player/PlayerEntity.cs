using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : Entity {


    [SerializeField]
    private GameObject gameOverScreen;

    [SerializeField]
    private GameObject[] lifeIcons = new GameObject[3];

    public override void Start() {
        base.Start();
        if (gameOverScreen != null) {
            gameOverScreen.SetActive(false);
        }
        for (int i = 0; i < lifeIcons.Length; i++) {
            lifeIcons[i].SetActive(true);
        }
    }

    public override void Update() {
        base.Update();
    }

    public override void TakeDamage(int amount) {
        base.TakeDamage(amount);
        for (int i = lifeIcons.Length - 1; i >= 0; i--) {
            if (lifeIcons[i].activeInHierarchy) {
                lifeIcons[i].SetActive(false);
                break;
            }
        }
    }

    public override void Death() {
        base.Death();
        //TODO: add player death animation
        Time.timeScale = 0;
        gameOverScreen.SetActive(true);
    }
}
