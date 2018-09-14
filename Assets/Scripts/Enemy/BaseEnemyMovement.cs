using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyMovement : MonoBehaviour {

    [SerializeField]
    private float _speed = 5f;
    private int _currentLane = 0;
    private float _progress = 0f;
    [Space]
    [Header("Lane switch variables")]
    [SerializeField]
    private float _horizontalTransitionTime = 0.75f;
    [Tooltip("The height at which the enemy switches lane")]
    [SerializeField]
    private float switchLaneHeight;
    [SerializeField]
    private int targetLane;
    [SerializeField]
    private bool switchesLane = false;
    private bool _movementFinished = true;
    private Vector3 _movementVector;

    private void Start() {
        _movementVector = new Vector3(0, -_speed, 0);
    }

    private void Update() {
        Movement();
        if (switchesLane) {
            StartCoroutine(SwitchLane(targetLane, switchLaneHeight));
        }
    }

    public virtual void Movement() {
        transform.position += _movementVector * Time.deltaTime;
    }

    public virtual IEnumerator SwitchLane(int targetLane, float startHeight) {
        _movementFinished = false;
        if (_currentLane == targetLane) { yield return null; }
        var startPos = new Vector2(transform.position.x, startHeight);
        var endPos = new Vector2(Utility.gameController.GetLaneX(targetLane), transform.position.y + (_movementVector.y / 2));
        if (transform.position.y == startHeight) {
            while (_progress < 1f) {
                _progress += Time.deltaTime * (1 / _horizontalTransitionTime);
                transform.position = Vector2.Lerp(startPos, endPos, _progress);
                yield return null;
            }
        }
        _movementFinished = true;
    }
}
