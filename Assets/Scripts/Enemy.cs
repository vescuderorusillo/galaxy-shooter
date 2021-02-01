using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;

    private Player _player;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        CalculateMovement();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            other.GetComponent<Player>()?.Damage();
            Destroy(gameObject);
        }
        else if (other.tag.Equals("Laser"))
        {
            Destroy(other.gameObject);
            _player?.AddScore(Random.Range(5, 12));
            Destroy(gameObject);
        }
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -5f)
        {
            transform.position = new Vector3(Random.Range(-8f, 8f), 7f, 0);
        }
    }

}
