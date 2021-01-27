using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;

    [SerializeField]
    private GameObject _laserPrefab;
    
    private float _laserOffset = 1.05f;
    
    [SerializeField]
    private float _fireRate = 0.5f;
    
    private float _nextFire = -1f;

    [SerializeField]
    private int _lives = 3;

    [SerializeField] 
    private SpawnManager _spawnManager;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
        {
            FireLaser();
        }
    }

    public void Damage()
    {
        _lives--;

        if (_lives < 1)
        {
            _spawnManager?.OnPlayerDeath();
            Destroy(gameObject);
        }
    }

    private void CalculateMovement()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");
        var direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        var yPos = Mathf.Clamp(transform.position.y, -3.8f, 0);

        float xPos;
        if (transform.position.x >= 11.3f)
        {
            xPos = -11.3f;
        }
        else if (transform.position.x <= -11.3f)
        {
            xPos = 11.3f;
        }
        else
        {
            xPos = transform.position.x;
        }

        transform.position = new Vector3(xPos, yPos, 0);
    }

    private void FireLaser()
    {
        _nextFire = Time.time + _fireRate;

        var laserPosition = transform.position + new Vector3(0, _laserOffset, 0);
        Instantiate(_laserPrefab, laserPosition, Quaternion.identity);
    }
}
