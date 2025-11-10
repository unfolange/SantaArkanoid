using UnityEngine;
using Unity.Mathematics;


public class MoveForward : MonoBehaviour
{

    public float speed = 1.0f;
    private Vector3 direction;
    private bool isLaunched = false;// es lanzada


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

            if (math.abs(transform.position.y) > positionLimit)
            {
                transform.position = new Vector3(transform.position.x, positionLimit * math.sign(transform.position.y), transform.position.z);
            }


        }
    }
    public void Launch()
    {
        direction = Vector3.right;
        isLaunched = true;
    }
    void OnCollisionEnter(Collision collision)
    {
        direction = Vector3.Reflect(direction, collision.contacts[0].normal);// la normal es el vector unitario perpendicular a la superficie
                                                                             // Define la nueva dirección despues del golpe
        if (collision.gameObject.CompareTag("Block"))
        {
            Destroy(collision.gameObject);
        }

    }
}
