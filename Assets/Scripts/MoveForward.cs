using UnityEngine;
using Unity.Mathematics;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

public class MoveForward : MonoBehaviour
{
    public float speed = 1.0f;
    public PlayerController player;
    public GameObject contenedorBloques;
    public AudioClip rebotaSound;
    private Vector3 direction;
    private bool fueLanzada = false;// es lanzada
    public bool FueLanzada => fueLanzada;
    private Dictionary<string, string> scenes;
    private AudioSource pelotaAudio;

    void Start()
    {
        direction = Vector3.zero;// la pelota se queda quieta por defecto
        scenes = new Dictionary<string, string>();
        scenes.Add("Nivel1", "Nivel2");
        scenes.Add("Nivel2", "Ganaste");
        pelotaAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fueLanzada)
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
                fueLanzada = false;
                player.DescontarVida();
            }


        }
    }
    public void Lanzar()
    {
        if (fueLanzada == false)
        {
            float randomZ = 0.3f;//UnityEngine.Random.Range(-0.7f, 0.7f);

            direction = new Vector3(1, 0, randomZ).normalized;
            fueLanzada = true;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        direction = Vector3.Reflect(direction, collision.contacts[0].normal).normalized;// la normal es el vector unitario perpendicular a la superficie
                                                                                        // Define la nueva dirección despues del golpe
        Debug.Log("direccion de colision: " + direction);
        float minGrado = 0.4f;
        if (Mathf.Abs(direction.x) < minGrado)
        {
            direction.x = Mathf.Sign(direction.x) * minGrado;
            direction = direction.normalized;
        }
        if (Mathf.Abs(direction.z) < minGrado)
        {
            direction.z = Mathf.Sign(direction.z) * minGrado;
            direction = direction.normalized;
        }
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("pared"))
        {
            pelotaAudio.PlayOneShot(rebotaSound);
        }
    }
    public void AdicionarPuntos(int puntos)
    {
        player.AdicionarPuntos(puntos);
    }
    public void ReiniciarEstadoPelota()
    {
        fueLanzada = false;
        direction = Vector3.zero;
    }
    public void VerificarBloquesExistentes()
    {
        int cantidadBloques = 0;
        Transform[] childArray = contenedorBloques.GetComponentsInChildren<Transform>();

        foreach (Transform child in childArray)
        {
            cantidadBloques = cantidadBloques + child.transform.childCount;
        }

        if (cantidadBloques == 7)
        {
            Scene scene = SceneManager.GetActiveScene();
            string nextScene = scenes[scene.name];
            SceneManager.LoadScene(nextScene);
        }

    }
}
