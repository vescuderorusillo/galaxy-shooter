using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;

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
