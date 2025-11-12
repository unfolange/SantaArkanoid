using UnityEngine;
using System.Collections.Generic;

public class Block : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int life = 1;
    public bool isGift = false;
    public Material[] changingMaterials;
    private int materialIndex = 0;

    private Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();

        if (changingMaterials != null && changingMaterials.Length != 0)
        {
            SetMaterialByIndex(materialIndex);
        }
    }

    private void SetMaterialByIndex(int index)
    {
        if (changingMaterials == null || index < 0 || index >= changingMaterials.Length)
        {
            Debug.LogWarning($"Índice {index} fuera de rango en Block '{name}'.");
            return;
        }

        var currentMaterial = changingMaterials[index];

        if (renderer != null && currentMaterial != null)
        {
            renderer.material = currentMaterial;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ball"))
        {
            life--;

            int collisionPoints;
            if (life <= 0)
            {
                if (isGift && PowerUpManager.Instance != null)
                {
                    PowerUpManager.Instance.SpawnRandomPowerUp(transform.position);
                }
                collision.gameObject.GetComponent<MoveForward>().verifyBlockCount();
                Destroy(gameObject);
                collisionPoints = 100;
            }
            else
            {
                if (changingMaterials != null && changingMaterials.Length != 0)
                {
                    materialIndex++;
                    SetMaterialByIndex(materialIndex);
                }
                collisionPoints = 50;
            }
            collision.gameObject.GetComponent<MoveForward>().addPoints(collisionPoints);
        }
    }
}
