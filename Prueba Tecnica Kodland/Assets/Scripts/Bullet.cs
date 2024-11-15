using System.Security.Cryptography;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed = 3;
    Vector3 direction;

    public void setDirection(Vector3 dir)
    {
        direction = dir;
    }

    void FixedUpdate()
    {
        transform.position += direction * speed * Time.deltaTime;
        speed += 1f;

        Collider[] targets = Physics.OverlapSphere(transform.position, 1);
        foreach (var item in targets)
        {
            if (item.tag == "Enemy")
            {
                Debug.Log("enemigo");//probamos si el codigo reacciona a las acciones con un debug
                Destroy(this.gameObject);//destruimos la bala al entrar en contacto con el objetivo
                Destroy(item.gameObject);//destruimos el enemigo 
                break;
            }
        }
    }
}
