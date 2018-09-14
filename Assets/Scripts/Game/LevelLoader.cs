using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour {

    [HideInInspector]
    public static Object level;

    private void Awake() {
        LoadLevel(Utility.currentLevel);
    }

    /// <summary>
    /// Loads a level from the resource folder depending on the level it needs to load.
    /// </summary>
    /// <param name="levelToLoad">The level it needs to load</param>
    public void LoadLevel(int levelToLoad) {
        if (levelToLoad != 0) {
            level = Resources.Load("Stages/Stage" + levelToLoad);
        }

        if (level != null) {
            Instantiate(level);
        } else {
            Debug.Log("Level is not found!");
        }
    }
}
