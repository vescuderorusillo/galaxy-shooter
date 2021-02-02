﻿using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 20f;

    [SerializeField] 
    private GameObject _explosionPrefab;

    private SpawnManager _spawnManager;

    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Laser"))
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            
            Destroy(other.gameObject);

            _spawnManager.StartSpawning();
            Destroy(gameObject, 0.2f);
            
        }
    }
}
