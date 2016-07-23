using UnityEngine;
using System.Collections;

public class SaltarPared : MonoBehaviour {

    Saltar saltarCs;
    Movimiento movimientoCs;
    Rigidbody2D rb;
    Transform body;
    Vector2 pointVector;
    float fuerzaDeSalto = 6.5f;
    bool puedeSaltar = false;
    bool saltando = false;
    bool enLaPared = false;
    int layerPiso;
    float gravity;
	void Awake ()
    {
        if (GetComponent<Saltar>()!=null)
            saltarCs = GetComponent<Saltar>();
        if (GetComponent<Movimiento>() != null)
            movimientoCs = GetComponent<Movimiento>();
        rb = GetComponent<Rigidbody2D>();
        GameObject child = new GameObject();
        child.name = "Body";
        child.transform.SetParent(transform);
        child.transform.position = new Vector2(transform.position.x, transform.position.y);
        body = transform.FindChild("Body");
        pointVector = new Vector2(0.7f, 0.25f);
    }
	void Start()
    {
        layerPiso = 1 << 8;
        gravity = rb.gravityScale;
    }
	void Update ()
    {
        puedeSaltar = saltarCs.GetPuedeSaltar();
        enLaPared = Physics2D.OverlapArea((Vector2)transform.position- pointVector, (Vector2)transform.position+ pointVector, layerPiso);
        Collider2D col = Physics2D.OverlapArea((Vector2)transform.position - pointVector, (Vector2)transform.position + pointVector, layerPiso);
        if (!puedeSaltar && enLaPared)
        {
            saltando = Input.GetButtonDown("Jump");
            if (saltando)
            {
                movimientoCs.enabled = false;
                rb.gravityScale = 0;
                Vector2 direction;
                if (col.transform.position.x > transform.position.x)
                {
                    direction = Vector2.left;
                    SaltoSimple(direction);
                }
                else if (col.transform.position.x < transform.position.x)
                {
                    direction = Vector2.right;
                    SaltoSimple(direction);
                }
                
            }
           
        }
	}
    void SaltoSimple(Vector2 dir)
    {
        dir *= 3;
        Vector2 direction = Vector2.up + dir;
        print(direction);
        rb.AddRelativeForce(direction * fuerzaDeSalto, ForceMode2D.Impulse);
        movimientoCs.enabled = true;
        rb.gravityScale = gravity;
    }
}
