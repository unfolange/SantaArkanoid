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
    private int puntos = 0;
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
        ReiniciarPosicionPelota();
        ActualizarPuntaje();

    }

    // Update is called once per frame
    void Update()
    {
        float limiteMovimientoIzq = -1.5f;
        float limiteMovimientoDer = 2.5f;

        if (transform.position.z < limiteMovimientoIzq)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, limiteMovimientoIzq);
        }
        if (transform.position.z > limiteMovimientoDer)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, limiteMovimientoDer);
        }
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.back * horizontalInput * Time.deltaTime * speed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ballScript.Lanzar(); // hay llamo a launch para que lance la pelota
        }
        if (!ballScript.FueLanzada)
        {
            ReiniciarPosicionPelota();
        }
    }

    public int AdicionarPuntos(int nuevosPuntos)
    {
        puntos += nuevosPuntos;
        ActualizarPuntaje();
        Debug.Log("puntos: " + puntos.ToString());
        return puntos;
    }

    void ReiniciarPosicionPelota()
    {
        Vector3 offset = new Vector3(0.4f, 0, 0);
        ball.transform.position = transform.position + offset;
    }
    public void DescontarVida()
    {
        lives--;
        RestarCorazones(lives);
        ActualizarVidas();
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
        ballScript.ReiniciarEstadoPelota();

        // Reposicionar la pelota encima de la barra
        ReiniciarPosicionPelota();
    }

    public void AplicarPowerUp(PowerUp.PowerType type, float duration)
    {
        StopAllCoroutines(); // simple: un efecto a la vez
        StartCoroutine(PowerUpRoutine(type, duration));
    }

    private IEnumerator PowerUpRoutine(PowerUp.PowerType type, float duration)
    {
        // aplicar efecto
        switch (type)
        {
            case PowerUp.PowerType.Expandir:
                transform.localScale = originalScale + new Vector3(0, 0, 0.7f);
                break;

            case PowerUp.PowerType.Achicar:
                transform.localScale = originalScale + new Vector3(0, 0, -0.8f);
                break;

            case PowerUp.PowerType.AumentarVelocidad:
                speed = originalSpeed + 5f;
                break;

            case PowerUp.PowerType.DisminuirVelocidad:
                speed = Mathf.Max(2f, originalSpeed - 8f);
                break;

            case PowerUp.PowerType.RalentizarPelota:
                ballScript.speed = Mathf.Max(2f, originalBallSpeed - 4f);
                break;

            case PowerUp.PowerType.Muerte:
                DescontarVida();
                break;
        }

        yield return new WaitForSeconds(duration);

        // revertir efecto
        transform.localScale = originalScale;
        speed = originalSpeed;
        ballScript.speed = originalBallSpeed;
    }

    void ActualizarPuntaje()
    {
        if (scoreText != null)
            scoreText.text = "Puntaje: " + puntos;
    }

    void ActualizarVidas()
    {
        if (livesText != null)
            livesText.text = "Vidas: " + lives;
    }
    void RestarCorazones(int indice)
    {
        Image imagenCorazon = ListaCorazones[indice].GetComponent<Image>();
        imagenCorazon.sprite = corazonesDesactivado;
    }


}
