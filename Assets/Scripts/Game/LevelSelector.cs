using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {

    [SerializeField]
    private Button[] levelButtons;
    private int levelreached;

    private void Start() {
        levelreached = PlayerPrefs.GetInt("levelReached", 1);

        for (int i = 0; i < levelButtons.Length; i++) {
            if (i + 1 > levelreached) {
                levelButtons[i].interactable = false;
            }
        }
    }

    /// <summary>
    /// Sets the level to load to the utility class so it can be read in every scene
    /// and then proceeds to load the scene in which the level is generated.
    /// </summary>
    /// <param name="levelIndex">The level to generate</param>
    public void SetLevelToLoad(int levelIndex) {
        Utility.currentLevel = levelIndex;
        //Loads the scene where the level is generated in, which is indexed by 2.
        Utility.sceneLoader.LoadScene(2);
    }
}
