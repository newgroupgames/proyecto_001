using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Saltar))]
public class Movimiento : MonoBehaviour {

    float velX = 0f;
    float limiteVelMax = 4f;
    float limiteVelMin;
    Rigidbody2D rb;
    Vector2 limiteDeVelocidad;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        rb.freezeRotation = true;
        limiteVelMin = limiteVelMax * -1;
    }
	void Update () {
        velX = Input.GetAxis("Horizontal")*limiteVelMax;
        LimitarVelocidad();
        VoltearPersonaje();
    }
    void FixedUpdate()
    {
        rb.AddRelativeForce(new Vector2(velX, 0), ForceMode2D.Impulse);
    }
    void LimitarVelocidad()
    {
        if (rb.velocity.x > limiteVelMax)
        {
            limiteDeVelocidad = rb.velocity;
            limiteDeVelocidad.x = limiteVelMax;
            rb.velocity = limiteDeVelocidad;
        }
        else if (rb.velocity.x < limiteVelMin)
        {
            limiteDeVelocidad = rb.velocity;
            limiteDeVelocidad.x = limiteVelMin;
            rb.velocity = limiteDeVelocidad;
        }
    }
    void VoltearPersonaje()
    {
        if (velX>0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (velX<0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
    public void CambiarVelocidad(float f)
    {
        limiteVelMax = f;
        limiteVelMin = f * -1;
    }
   
}
