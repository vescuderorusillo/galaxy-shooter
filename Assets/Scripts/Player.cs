using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    private float _laserOffset = 1.05f;
    
    [SerializeField]
    private float _fireRate = 0.5f;
    
    private float _nextFire = -1f;

    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;

    private bool _isTripleShotActive;
    private bool _isShieldActive;

    [SerializeField]
    private GameObject _shield;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    [SerializeField]
    private GameObject _leftEngine, _rightEngine;

    [SerializeField]
    private AudioClip _laserSoundClip;

    private AudioSource _audioSource;


    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        _audioSource.clip = _laserSoundClip;
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
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shield.SetActive(false);
            return;
        }

        _lives--;
        _uiManager.UpdateLives(_lives);

        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        }else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }else if (_lives < 1)
        {
            _spawnManager?.OnPlayerDeath();
            Destroy(gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(PowerDownRoutine("TripleShot"));
    }

    public void SpeedActive()
    {
        _speed = 10f;
        StartCoroutine(PowerDownRoutine("Speed"));
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shield.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    private IEnumerator PowerDownRoutine(string powerup)
    {
       yield return new WaitForSeconds(5.0f);
        if (powerup == "TripleShot")
        {
            _isTripleShotActive = false;
        }
        else if (powerup == "Speed")
        {
            _speed = 5f;
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

        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            var laserPosition = transform.position + new Vector3(0, _laserOffset, 0);
            Instantiate(_laserPrefab, laserPosition, Quaternion.identity);
        }

        _audioSource.Play();
    }
}
