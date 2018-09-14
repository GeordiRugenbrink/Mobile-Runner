using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float _yPosition = -4;
    private int _currentLane;

    private Vector2 _firstTouch;
    private Vector2 _lastTouch;
    [SerializeField]
    private float _minimalSwipeDistance;

    [SerializeField]
    private float _transitionTime = 0.5f;
    private float _progress = 0f;
    private bool _movementFinished = true;

    [SerializeField]
    private float _fireDelay = 0.3f;
    private float _lastShot = 0f;

    [SerializeField]
    private ObjectPooling projectileObjectPool;

    /// <summary>
    /// Configures the position of the player and the minimal swipe distance on the start of the program.
    /// </summary>
    private void Start() {
        transform.position = new Vector2(Utility.gameController.GetLaneX(Utility.gameController.AmountOfLanes / 2), _yPosition);
        _currentLane = Utility.gameController.AmountOfLanes / 2;
        _minimalSwipeDistance = Screen.width * _minimalSwipeDistance;
    }

    /// <summary>
    /// Switches between lanes and does so by lerping the position of tthe player.
    /// </summary>
    /// <returns>Nothing</returns>
    public IEnumerator HorizontalMovement() {
        _movementFinished = false;
        _progress = 0;
        var startPos = transform.position;
        var endPos = new Vector2(Utility.gameController.GetLaneX(_currentLane), _yPosition); ;
        while (_progress < 1) {
            _progress += (Time.deltaTime * (1 / _transitionTime));
            transform.position = Vector2.Lerp(startPos, endPos, _progress);
            yield return null;
        }
        _movementFinished = true;
    }
    /// <summary>
    /// It calls the method that checks for a swipe.
    /// </summary>
    private void Update() {
        //Remove in final build
        if (Input.GetKeyDown(KeyCode.LeftArrow) && _currentLane > 0) {
            if (_movementFinished) {
                _currentLane--;
                StartCoroutine(HorizontalMovement());
            }
        } else if (Input.GetKeyDown(KeyCode.RightArrow) && _currentLane < Utility.gameController.AmountOfLanes - 1) {
            if (_movementFinished) {
                _currentLane++;
                StartCoroutine(HorizontalMovement());
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && _movementFinished && Time.time > _lastShot + _fireDelay) {
            Shoot();
            _lastShot = Time.time;
        }
        CheckForSwipe();
    }
    /// <summary>
    /// Creates a reference to an object by retreiving one from the object pool.
    /// Defines it's spawnposition and sets the position and rotation for the player.
    /// At the end enable the object so it's seen in the world.
    /// </summary>
    private void Shoot() {
        GameObject obj = projectileObjectPool.GetPooledObject();
        Vector3 spawnPosition = transform.position + new Vector3(0, GetComponent<SpriteRenderer>().size.y / 2, 0);

        if (obj == null) {
            return;
        }

        obj.transform.position = spawnPosition;
        obj.transform.rotation = transform.rotation;
        obj.SetActive(true);
    }
    /// <summary>
    /// Checks if the input is a touch or a swipe.
    /// By checking if the first touch is in another position than the last touch.
    /// If the minimal swipe distance is not reached, it's registered as a touch.
    /// </summary>
    private void CheckForSwipe() {
        if (Input.touchCount == 1) {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) {
                _firstTouch = touch.position;
                _lastTouch = touch.position;
            } else if (touch.phase == TouchPhase.Moved) {
                _lastTouch = touch.position;
            } else if (touch.phase == TouchPhase.Ended) {
                _lastTouch = touch.position;
                //Check if the swipe distance is greater than the minimal swipe distance
                if (Mathf.Abs(_lastTouch.x - _firstTouch.x) > _minimalSwipeDistance ||
                    Mathf.Abs(_lastTouch.y - _firstTouch.y) > _minimalSwipeDistance) {
                    //It's a swipe
                    if (Mathf.Abs(_lastTouch.x - _firstTouch.x) > Mathf.Abs(_lastTouch.y - _firstTouch.y)) {
                        //Horizontal swipes
                        if (_lastTouch.x > _firstTouch.x && _currentLane < Utility.gameController.AmountOfLanes - 1) {
                            //Swipe to the right
                            //If the last movement has finished, it moves the player to the right
                            if (_movementFinished) {
                                _currentLane++;
                                StartCoroutine(HorizontalMovement());
                            }
                        } else if (_lastTouch.x < _firstTouch.x && _currentLane > 0) {
                            //Swipe to the left
                            if (_movementFinished) {
                                _currentLane--;
                                StartCoroutine(HorizontalMovement());
                            }
                        }
                    } else {
                        //Vertical swipes
                        if (_lastTouch.y > _firstTouch.y) {
                            //Swipe upwards
                        } else {
                            //Swipe downwards
                        }
                    }
                } else {
                    //It's a touch
                    if (Time.time > _lastShot + _fireDelay && _movementFinished) {
                        Shoot();
                        _lastShot = Time.time;
                    }
                }
            }
        }
    }
}
