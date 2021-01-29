using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;

    [SerializeField] // 0 = TripleShot, 1 = Speed, 2 = Shields
    private int powerupID;

    void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -5f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            var player = other.GetComponent<Player>();

            switch (powerupID)
            {
                case 0:
                    player?.TripleShotActive();
                    break;
                case 1:
                    player?.SpeedActive();
                    break;
                case 2:
                    player?.ShieldActive();
                    break;
            }

            Destroy(gameObject);
        }
    }
}
