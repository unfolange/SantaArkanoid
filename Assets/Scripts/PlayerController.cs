using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class PlayerController : MonoBehaviour
{
    public float horizontalInput;
    public float speed = 10.0f;
    public GameObject ball;
    private int points = 0;
    private MoveForward ballScript;
    public int lives = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ballScript = ball.GetComponent<MoveForward>();
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
        transform.Translate(Vector3.back * horizontalInput * Time.deltaTime * speed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ballScript.Launch(); // hay llamo a launch para que lance la pelota
        }
        if (!ballScript.IsLaunched)
        {
            ResetBallPosition();
        }
    }

    public int addPoints(int newPoints)
    {
        points = points + newPoints;
        Debug.Log("points: " + points.ToString());
        return points;
    }

    void ResetBallPosition()
    {
        Vector3 offset = new Vector3(0.4f, 0, 0);
        ball.transform.position = transform.position + offset;
    }
    public void LoseLifeAndResetBall()
    {
        lives--;
        Debug.Log("Vida perdida. Vidas restantes: " + lives);

        if (lives <= 0)
        {
            Debug.Log("GAME OVER");
            // Aquí puedes:
            // - Desactivar controles,
            // - Mostrar UI,
            // - Recargar escena:
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        // Reiniciar estado de la pelota
        ballScript.ResetBallState();

        // Reposicionar la pelota encima de la barra
        ResetBallPosition();
    }

}
