using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {

    [SerializeField]
    private Button[] _levelButtons;
    private int _levelreached;

    /// <summary>
    /// Initializes the level reached in the playerprefs and
    /// locks the buttons in the LevelSelect scene if the player hasn't reached that level yet.
    /// </summary>
    private void Start() {
        _levelreached = PlayerPrefs.GetInt("levelReached", 1);

        for (int i = 0; i < _levelButtons.Length; i++) {
            if (i + 1 > _levelreached) {
                _levelButtons[i].interactable = false;
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
