using UnityEngine;
using System.Collections;

public class Saltar : MonoBehaviour
{
    Rigidbody2D rb;
    Transform pies;
    Vector2 dobleSalto;

    Transform body;
    Vector2 pointVector;

    float fuerzaDeSalto = 8f;// 1 cuadro es 6.5f
    bool puedeSaltar = true;        
    bool puedeSaltarDoble = true;   //para doble salto
    bool saltando = false;          
    public bool tieneDobleSalto = true;    //habilita el doble salto
    public bool tieneWallJump = true;      //habilita el wall jump
    bool enLaPared = false;         
    int layerSuelo;                 
    int layerPisable;               
    int layers;
    void Awake()
    {
        GameObject child = new GameObject();
        child.name = "Pies";
        child.transform.SetParent(transform);
        child.transform.position = new Vector2(transform.position.x, transform.position.y - GetComponent<Renderer>().bounds.size.y / 2);
        pies = transform.FindChild("Pies");
        rb = GetComponent<Rigidbody2D>();
        GameObject childB = new GameObject();
        childB.name = "Body";
        childB.transform.SetParent(transform);
        childB.transform.position = new Vector2(transform.position.x, transform.position.y);
        body = transform.FindChild("Body");
        pointVector = new Vector2(0.55f, 0.25f);
        rb.gravityScale = 2;
    }
    void Start()
    {
        layerSuelo = 1 << 8;
        layerPisable = 1 << 12;
        layers = layerSuelo | layerPisable;
    }
    void Update()
    {
        puedeSaltar = Physics2D.OverlapCircle(pies.position, 0.2f, layers);
        enLaPared = Physics2D.OverlapArea((Vector2)transform.position - pointVector, (Vector2)transform.position + pointVector, layerSuelo);
        Collider2D col = Physics2D.OverlapArea((Vector2)transform.position - pointVector, (Vector2)transform.position + pointVector, layerSuelo);
        saltando = Input.GetButtonDown("Jump");
        if (puedeSaltar && tieneDobleSalto)
            puedeSaltarDoble = true;
        if (saltando && puedeSaltar)
            SaltoSimple();
        if (saltando && !puedeSaltar && puedeSaltarDoble && tieneDobleSalto)
            SaltoDoble();
        if (enLaPared && tieneWallJump)
        {
            gameObject.SendMessage("CambiarVelocidad", 1);
            puedeSaltar = false;
            if (tieneDobleSalto)
                puedeSaltarDoble = false;
            if (saltando)
            {
                rb.velocity = Vector2.zero;
                SaltoEnLaPared(col);
            }
        }
        else
        {
            gameObject.SendMessage("CambiarVelocidad", 4);//numero # hardcodeado de script movimiento del personaje
        }
    }
    void SaltoSimple()
    {
        rb.AddRelativeForce(Vector2.up * fuerzaDeSalto, ForceMode2D.Impulse);
    }
    void SaltoDoble()
    {
        puedeSaltarDoble = false;
        dobleSalto = new Vector2(rb.velocity.x,0);
        rb.velocity = dobleSalto;
        rb.AddRelativeForce(Vector2.up * fuerzaDeSalto, ForceMode2D.Impulse);
    }
    void SaltoEnLaPared(Collider2D col)
    {
        if (col.transform.position.x > transform.position.x)
        {
            rb.AddForce((Vector2.left * 10) + (Vector2.up * fuerzaDeSalto*1.2f), ForceMode2D.Impulse);
        }
        else if (col.transform.position.x < transform.position.x)
        {
            rb.AddForce((Vector2.right * 10) + (Vector2.up * fuerzaDeSalto*1.2f), ForceMode2D.Impulse);
        }
    }
    public bool GetPuedeSaltar() { return puedeSaltar; }
    public void SetPuedeSaltar(bool v) { puedeSaltar = v; }
}
