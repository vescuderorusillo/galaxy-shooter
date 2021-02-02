using UnityEngine;

public class Asteroid : MonoBehaviour
{
    //[SerializeField]
    //private float _speed = 2f;

    [SerializeField]
    private float _rotateSpeed = 20f;

    [SerializeField] 
    private GameObject _explosionPrefab;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
        //transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    
}
