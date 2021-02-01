using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _ememyPrefab;

    [SerializeField]
    private GameObject _ememyContainer;

    [SerializeField]
    private GameObject[] powerups;

    private bool _stopSpawning;

    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        while (!_stopSpawning)
        {
            var posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            var newEnemy = Instantiate(_ememyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _ememyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    private IEnumerator SpawnPowerupRoutine()
    {
        while (!_stopSpawning)
        {
            var posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            var randomPowerUp = Random.Range(0, powerups.Length);
            Instantiate(powerups[randomPowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
