using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform rifleStart;
    [SerializeField] private Text HpText;

    [SerializeField] private GameObject GameOver;
    [SerializeField] private GameObject Victory;

    public float health = 0;

    //character controller
    [SerializeField] private CharacterController characterCtrl;
    [SerializeField] public float speed;
    [SerializeField] private float gravity = 9.81f;
    private Vector3 moveDirection;

    void Start()
    {
        //Destroy(this);//esto evita que el player se pueda mover, ya que elimina sus scripts
        ChangeHealth(0);

        //obtenemos el componente que vamos a usar
        characterCtrl = GetComponent<CharacterController>();
    }

    //por cuestiones de orden, decidi ubicar el update seguido del start
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject buf = Instantiate(bullet);
            buf.transform.position = rifleStart.position;
            buf.GetComponent<Bullet>().setDirection(transform.forward);
            buf.transform.rotation = transform.rotation;
            
        }

        if (Input.GetMouseButtonDown(1))
        {
            Collider[] tar = Physics.OverlapSphere(transform.position, 2);
            foreach (var item in tar)
            {
                if (item.tag == "Enemy")
                {
                    Destroy(item.gameObject);
                }
            }
        }

        Collider[] targets = Physics.OverlapSphere(transform.position, 3);
        foreach (var item in targets)
        {
            if (item.tag == "Health")
            {
                Debug.Log("salud");
                ChangeHealth(50);
                Destroy(item.gameObject);
            }
            if (item.tag == "Finish")
            {
                Win();
            }
            if (item.tag == "Enemy")
            {
                Lost();
            }
        }

        //characterCtrl
        /*
        aqui usamos el vector 3, ya que nos vamos a mover en los ejes X y Z, el eje Y no lo usaremos, debido a que no saltaremos para este ejercicio.
        aplicamos la gravedad con la variable gravity, para que el jugador caiga
        */
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));//Definimos el vector 3
        move = transform.TransformDirection(move) * speed;//relacionamos el movimiento con la orientacion del player, para que vaya hacia adelante

        //verificamos si el player esta en el suelo
        if (!characterCtrl.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;//le asigna la gravedad al player si no esta en el suelo
        }

        else
        {
            moveDirection.y = 0;//si ya esta en el suelo la despreciamos
        }

        characterCtrl.Move((move + moveDirection) * Time.deltaTime);//combinamos el movimientos en el eje X con la gravedad en el eje Y
    }

    public void ChangeHealth(int hp)
    {
        health += hp;
        if (health > 100)
        {
            health = 100;
        }
        else if (health <= 0)
        {
            Lost();
        }
        HpText.text = health.ToString();
    }

    public void Win()
    {
        Victory.SetActive(true);
        Destroy(GetComponent<PlayerLook>());
        Cursor.lockState = CursorLockMode.None;
    }

    public void Lost()
    {
        GameOver.SetActive(true);
        Destroy(GetComponent<PlayerLook>());
        Cursor.lockState = CursorLockMode.None;
    }

}
