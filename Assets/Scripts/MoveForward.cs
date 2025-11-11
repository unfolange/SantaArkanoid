using UnityEngine;
using Unity.Mathematics;



public class MoveForward : MonoBehaviour
{
    public float speed = 1.0f;
    public PlayerController player;
    
    private Vector3 direction;
    private bool isLaunched = false;// es lanzada
    public bool IsLaunched => isLaunched;

    void Start()
    {
        direction = Vector3.zero;// la pelota se queda quieta por defecto
    }

    // Update is called once per frame
    void Update()
    {
        if (isLaunched)
        {
            transform.Translate(direction * Time.deltaTime * speed);
            float positionLimit = 0.1f;

            if (math.abs(transform.position.y) != 0.1f)
            {
                //mantiene la posicion en y que es el limite
                transform.position = new Vector3(transform.position.x, positionLimit * math.sign(transform.position.y), transform.position.z);
            }
            float deathLineX = -5.5f; // mueve esto según tu cámara / layout
            if (transform.position.x < deathLineX)
            {
                isLaunched = false;
                player.LoseLifeAndResetBall();
            }


        }
    }
    public void Launch()
    {
        if (isLaunched == false)
        {
            float randomZ = 0.3f;//UnityEngine.Random.Range(-0.7f, 0.7f);

            direction = new Vector3(1, 0, randomZ).normalized;
            isLaunched = true;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        direction = Vector3.Reflect(direction, collision.contacts[0].normal).normalized;// la normal es el vector unitario perpendicular a la superficie
                                                                                        // Define la nueva dirección despues del golpe
        float minZ = 0.3f;
        if (Mathf.Abs(direction.z) < minZ)
        {
            direction.z = Mathf.Sign(direction.z) * minZ;
            direction = direction.normalized;
        }
    }
    public void addPoints(int points)
    {
        player.addPoints(points);
    }
    public void ResetBallState()
    {
        isLaunched = false;
        direction = Vector3.zero;
    }
}
