using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;

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

    private GameManager _gameManager;

    [SerializeField]
    private bool _isPlayer1 = false;

    private KeyCode fireKeyCode;

    private Animator _animator;

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _animator = GetComponent<Animator>();

        if (!_gameManager.IsMultiPlayerMode())
        {
            transform.position = new Vector3(0, 0, 0);
        }

        if (IsPlayer1())
        {
            fireKeyCode = KeyCode.Space;
        }
        else
        {
            fireKeyCode = KeyCode.Return;
        }

        _audioSource.clip = _laserSoundClip;
    }

    void Update()
    {
        if (IsPlayer1())
        {
            CalculateMovementPlayerOne();
            AnimatePlayerOne();
        }
        else
        {
            CalculateMovementPlayerTwo();
            AnimatePlayerTwo();
        }

        if (Input.GetKeyDown(fireKeyCode) && Time.time > _nextFire)
        {
            FireLaser();
        }
    }

    public bool IsPlayer1()
    {
        return _isPlayer1;
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
            _uiManager.CheckForBestScore(_score);
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

    private void CalculateMovementPlayerOne()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");
        var direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        ApplyScreenLimitToPlayerPosition();
    }

    private void AnimatePlayerOne()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            _animator.SetBool("Turn_Left", false);
            _animator.SetBool("Turn_Right", false);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            _animator.SetBool("Turn_Left", false);
            _animator.SetBool("Turn_Right", false);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            _animator.SetBool("Turn_Left", true);
            _animator.SetBool("Turn_Right", false);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _animator.SetBool("Turn_Left", false);
            _animator.SetBool("Turn_Right", true);
        }
    }

    private void AnimatePlayerTwo()
    {
        if (Input.GetKeyUp(KeyCode.Keypad4))
        {
            _animator.SetBool("Turn_Left", false);
            _animator.SetBool("Turn_Right", false);
        }
        if (Input.GetKeyUp(KeyCode.Keypad6))
        {
            _animator.SetBool("Turn_Left", false);
            _animator.SetBool("Turn_Right", false);
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            _animator.SetBool("Turn_Left", true);
            _animator.SetBool("Turn_Right", false);
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            _animator.SetBool("Turn_Left", false);
            _animator.SetBool("Turn_Right", true);
        }
    }

    private void CalculateMovementPlayerTwo()
    {
        if (Input.GetKey(KeyCode.Keypad8))
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Keypad5))
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Keypad4))
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Keypad6))
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }

        ApplyScreenLimitToPlayerPosition();
    }

    private void ApplyScreenLimitToPlayerPosition()
    {
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
