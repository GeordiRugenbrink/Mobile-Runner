using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour {

    [SerializeField]
    private GameObject _objectToPool;
    [SerializeField]
    private int _pooledAmount = 10;
    [SerializeField]
    private bool _willGrow;

    private List<GameObject> _pooledObjects;

    private void Start() {
        _pooledObjects = new List<GameObject>();
        CreateInitialPool();
    }

    /// <summary>
    /// Creates the initial amount of objects that need to be created.
    /// </summary>
    private void CreateInitialPool() {
        for (int i = 0; i < _pooledAmount; i++) {
            GameObject obj = (GameObject)Instantiate(_objectToPool);
            obj.SetActive(false);
            _pooledObjects.Add(obj);
        }
    }

    /// <summary>
    /// Gets an inactive object out of the Objectpool.
    /// If there aren't any and it is set to grow it creates a new Object and adds it to the pool.
    /// </summary>
    /// <returns></returns>
    public GameObject GetPooledObject() {
        for (int i = 0; i < _pooledObjects.Count; i++) {
            if (!_pooledObjects[i].activeInHierarchy) {
                return _pooledObjects[i];
            }
        }

        if (_willGrow) {
            GameObject obj = (GameObject)Instantiate(_objectToPool);
            _pooledObjects.Add(obj);
            return obj;
        }

        return null;
    }
}
