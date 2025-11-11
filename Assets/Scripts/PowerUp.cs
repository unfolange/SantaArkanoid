using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour
{
    public enum PowerType { Expand, Shrink, SpeedUp, SlowDown, SlowBallDown, Die }
    public PowerType type;
    public float speed = 3f;
    public float duration = 5f;

    private Transform target;
    private Vector3 direction;

    void Start()
    {
        direction = Vector3.left;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        // si se fue muy lejos, destruir
        if (transform.position.x < -10f || transform.position.x > 10f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ApplyPowerUp(type, duration);
        }

        Destroy(gameObject);
    }
}
