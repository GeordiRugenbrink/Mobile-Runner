using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour {

    [HideInInspector]
    public static Object level;

    private void Awake() {
        LoadLevel(Utility.currentLevel);
    }

    public void LoadLevel(int levelToLoad) {
        if (Utility.currentLevel != 0) {
            level = Resources.Load("Stages/Stage" + levelToLoad);
        }

        if (level != null) {
            Instantiate(level);
        } else {
            Debug.Log("Level is not found!");
        }
    }
}
