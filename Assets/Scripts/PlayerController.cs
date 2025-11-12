using Unity.Mathematics;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem.Controls;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float horizontalInput;
    public float speed = 10.0f;
    public GameObject ball;
    private int points = 0;
    private MoveForward ballScript;
    public int lives = 3;

    private Vector3 originalScale;
    private float originalSpeed;
    private float originalBallSpeed;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    [SerializeField] private List<GameObject> ListaCorazones;
    [SerializeField] private Sprite corazonesDesactivado;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        ballScript = ball.GetComponent<MoveForward>();
        originalBallSpeed = ballScript.speed;
        originalScale = transform.localScale;
        originalSpeed = speed;
        ResetBallPosition();

        UpdateScore();

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
        points += newPoints;
        UpdateScore();
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
        RestarCorazones(lives);
        UpdateLives();
        Debug.Log("Vida perdida. Vidas restantes: " + lives);

        if (lives <= 0)
        {
            Debug.Log("GAME OVER");
            SceneManager.LoadScene("GameOver");

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

    public void ApplyPowerUp(PowerUp.PowerType type, float duration)
    {
        StopAllCoroutines(); // simple: un efecto a la vez
        StartCoroutine(PowerUpRoutine(type, duration));
    }

    private IEnumerator PowerUpRoutine(PowerUp.PowerType type, float duration)
    {
        // aplicar efecto
        switch (type)
        {
            case PowerUp.PowerType.Expand:
                transform.localScale = originalScale + new Vector3(0, 0, 0.7f);
                break;

            case PowerUp.PowerType.Shrink:
                transform.localScale = originalScale + new Vector3(0, 0, -0.8f);
                break;

            case PowerUp.PowerType.SpeedUp:
                speed = originalSpeed + 5f;
                break;

            case PowerUp.PowerType.SlowDown:
                speed = Mathf.Max(2f, originalSpeed - 8f);
                break;

            case PowerUp.PowerType.SlowBallDown:
                ballScript.speed = Mathf.Max(2f, originalBallSpeed - 4f);
                break;

            case PowerUp.PowerType.Die:
                LoseLifeAndResetBall();
                break;
        }

        yield return new WaitForSeconds(duration);

        // revertir efecto
        transform.localScale = originalScale;
        speed = originalSpeed;
        ballScript.speed = originalBallSpeed;
    }

    void UpdateScore()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + points;
    }

    void UpdateLives()
    {
        if (livesText != null)
            livesText.text = "Lives: " + lives;
    }
    void RestarCorazones(int indice)
    {
        Image imagenCorazon = ListaCorazones[indice].GetComponent<Image>();
        imagenCorazon.sprite = corazonesDesactivado;
    }


}
