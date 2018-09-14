using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour {

    [SerializeField]
    private int _amountOfLanes;
    public int AmountOfLanes {
        get { return _amountOfLanes; }
    }
    private float _screenWidthWorld;
    private float _laneWidth;

    private float[] _horizontalPositions;

    [SerializeField]
    private GameObject _victoryScreen;

    private void Awake() {
        Utility.gameController = this;
        InitLanes();
        _victoryScreen.SetActive(false);
    }

    /// <summary>
    /// Initiates the array for the horizontal positions of the lanes.
    /// Then calculates the screen width in Units.
    /// Proceeds to calculate the width of the lane by multiplying the screen width by 2
    /// to get the real width of the screen and dividing it by the amount of lanes.
    /// After this it calculates the horizontalpositions and adds them to the array
    /// that's been initiated earlier in this method.
    /// </summary>
    public void InitLanes() {
        _horizontalPositions = new float[_amountOfLanes];
        _screenWidthWorld = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z)).x;
        _laneWidth = (_screenWidthWorld * 2) / _amountOfLanes;
        for (int i = 0; i < _amountOfLanes; i++) {
            _horizontalPositions[i] = i * _laneWidth - _screenWidthWorld + (_laneWidth / 2);
        }
    }

    /// <summary>
    /// Gets the x position out of the array with x positions.
    /// </summary>
    /// <param name="amountOfLanes">the index of the lane used to get the x position</param>
    /// <returns></returns>
    public float GetLaneX(int amountOfLanes) {
        amountOfLanes = Mathf.Clamp(amountOfLanes, 0, _amountOfLanes - 1);
        return _horizontalPositions[amountOfLanes];
    }

    /// <summary>
    /// Restarts current Level by reloading it.
    /// </summary>
    public void RestartLevel() {
        Time.timeScale = 1;
        Utility.sceneLoader.LoadScene(2);
    }

    public void GoToMainMenu() {
        Time.timeScale = 1;
        Utility.currentLevel = 0;
        Utility.sceneLoader.LoadScene(0);
    }

    public void GoToLevelSelect() {
        Time.timeScale = 1;
        Utility.sceneLoader.LoadScene(1);
    }

    /// <summary>
    /// Unlocks the next level and shows the victoryScreen
    /// </summary>
    public void LevelCompleted() {
        Time.timeScale = 0;
        //Code to unlock next Level
        if (PlayerPrefs.GetInt("levelReached") <= Utility.currentLevel) {
            PlayerPrefs.SetInt("levelReached", Utility.currentLevel + 1);
        }
        //Show Victory screen with buttons to go back to the main menu or play again
        _victoryScreen.SetActive(true);
    }
}