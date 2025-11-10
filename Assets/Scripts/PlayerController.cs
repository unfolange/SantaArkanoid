using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float horizontalInput;
    public float speed = 10.0f;
    public GameObject ball;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float positionLimit = 2.5f;

        if (math.abs(transform.position.z) > positionLimit)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, positionLimit * math.sign(transform.position.z));
        }
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.forward * horizontalInput * Time.deltaTime * speed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ball.GetComponent<MoveForward>().Launch(); // hay llamo a launch para que lance la pelota
        }
    }
}
