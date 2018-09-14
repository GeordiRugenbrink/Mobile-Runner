using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButtonLevelSelect : MonoBehaviour {

    public void BackTomainMenu() {
        Utility.sceneLoader.LoadScene(0);
    }
}
