using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance { get; private set; }

    [Header("Lista de prefabs de PowerUps/PowerDowns")]
    public GameObject[] powerUpPrefabs; // arrastras aquí tus 5 prefabs

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SpawnRandomPowerUp(Vector3 position)
    {
        if (powerUpPrefabs == null || powerUpPrefabs.Length == 0)
        {
            Debug.LogWarning("No hay powerUps asignados en PowerUpManager.");
            return;
        }

        int index = Random.Range(0, powerUpPrefabs.Length);
        Instantiate(powerUpPrefabs[index], position, Quaternion.identity);
    }
}
