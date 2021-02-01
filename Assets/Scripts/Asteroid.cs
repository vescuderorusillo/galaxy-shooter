using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    //[SerializeField]
    //private float _speed = 2f;

    [SerializeField]
    private float _rotateSpeed = 20f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
        //transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }
}
