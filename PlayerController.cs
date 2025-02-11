using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public GameObject MobileUI;
    Animator animator;
    Rigidbody2D rigidb;

    float movOrizzontale;
    float movVerticale;

    public Joystick joystickInput;
    private float velocitaMovimento = 5f;
    private float forzaSalto = 700f;
    private float climbingSpeed = 3f;


    bool guardaDestra = true;
    public bool perTerra = false;
    bool corre = false;
    bool isLadder = false;
    bool isClimbing = false;
    private string tipoInput;

    public bool Corre { get { return corre; } set { corre = value; } }

    void Start()
    {
        if (Application.isEditor)
        {
            tipoInput = "Mobile";
            //MobileUI.SetActive(false);
        }
        else if (Application.isMobilePlatform)
        {
            tipoInput = "Mobile";
        }
        else if (PCUser())
        {
            tipoInput = "PC";
            MobileUI.SetActive(false);
        }
    }


    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidb = GetComponent<Rigidbody2D>();
    }

    //Chiamato ad ogni frame
    void Update()
    {
        if (tipoInput == "PC")
        {
            GestoreInputTastiera();
        }
        else if (tipoInput == "Mobile")
        {
            GestoreInputMobile();
        }
    }

    private void GestoreInputTastiera()
    {
        //Ottengo direzione del movimento
        movOrizzontale = Input.GetAxisRaw("Horizontal");
        movVerticale = Input.GetAxisRaw("Vertical");

        //Check corsa
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            corre = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            corre = false;
        }

        //Check se il player si trova su una scala
        if (isLadder && Mathf.Abs(movVerticale) > 0f)
        {
            isClimbing = true;
            //Se player salta sulla scala e comincia a scalare
            //blocco animazione del salto.
            if (perTerra == false)
            {
                //Blend tree jumping animation off
                animator.SetBool("isJumping", false);
            }
            perTerra = false;
        }
        else
        {
            isClimbing = false;
        }

        if (isClimbing)
        {
            Scala();
        }
        else
        {
            animator.SetBool("isClimbing", false);
            rigidb.gravityScale = 2.5f;
        }

        //Check se il player ha premuto spazio
        if (Input.GetButtonDown("Jump"))
            Salta();

        //Do valore al blend tree che gestisce animazione del salto e della caduta
        // yVel neg = Caduta, yVel pos = Salita
        animator.SetFloat("velocitaVerticale", rigidb.linearVelocityY);

    }


    private void GestoreInputMobile()
    {
        //Ottengo direzione del movimento
        movOrizzontale = joystickInput.Horizontal;
        movVerticale = joystickInput.Vertical;

        //Check se il player si trova su una scala
        if (isLadder && Mathf.Abs(movVerticale) > 0f)
        {
            isClimbing = true;
            //Se player salta sulla scala e comincia a scalare
            //blocco animazione del salto.
            if (perTerra == false)
            {
                //Blend tree jumping animation off
                animator.SetBool("isJumping", false);
            }
            perTerra = false;
        }
        else
        {
            isClimbing = false;
        }


        //Check se il player sta salendo con la scala
        if (isClimbing)
        {
            Scala();
        }
        else
        {
            animator.SetBool("isClimbing", false);
            rigidb.gravityScale = 2.5f;
        }
        //Do valore al blend tree che gestisce animazione del salto e della caduta
        // yVel neg = Caduta, yVel pos = Salita
        animator.SetFloat("velocitaVerticale", rigidb.linearVelocityY);

    }
    void FixedUpdate()
    {
        Movimento(movOrizzontale);
    }

    void Scala()
    {
        //Climbing animation
        animator.SetBool("isClimbing", true);
        rigidb.gravityScale = 0f;
        rigidb.linearVelocity = new Vector2(rigidb.linearVelocityX, movVerticale * climbingSpeed);
    }

    public void Salta()
    {
        if (perTerra)
        {
            //Blend tree jumping animation
            animator.SetBool("isJumping", true);
            rigidb.AddForce(new Vector2(0f, forzaSalto));
            SoundSystem.instance.Suona("salto");
        }
    }

    void Movimento(float direzione)
    {

        float movimentoAsseX = direzione * velocitaMovimento;
        if (corre)
        {
            movimentoAsseX *= 1.5f;
        }
        Vector2 finale = new Vector2(movimentoAsseX, rigidb.linearVelocityY);
        rigidb.linearVelocity = finale;

        Vector3 scalePersonaggio = transform.localScale;

        if (direzione < 0 && guardaDestra)
        {
            guardaDestra = false;
            scalePersonaggio.x = -scalePersonaggio.x;
        }
        else if (direzione > 0 && !guardaDestra)
        {
            guardaDestra = true;
            scalePersonaggio.x = -scalePersonaggio.x;
        }
        transform.localScale = scalePersonaggio;

        //Tramite la velocità di movimento modifico il parametro del blend tree
        //così da riprodurre l'animazione di movimento adeguata alla velocità
        animator.SetFloat("Speed", Mathf.Abs(rigidb.linearVelocityX));
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Se player collide col terreno allora è a terra
        if (collision.gameObject.CompareTag("Ground"))
        {
            perTerra = true;
        }

        animator.SetBool("isJumping", !perTerra);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Se non collide col terreno non è a terra
        if (collision.gameObject.CompareTag("Ground"))
        {
            perTerra = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Verifico che player collida con scala
        if (collision.gameObject.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Uscito dalla collisione non sono più vicino ad una scala
        if (collision.gameObject.CompareTag("Ladder"))
        {
            isLadder = false;
        }

    }

    //Morte del player quando interagisce con qualsiasi trappola. Ha 1 solo hp
    public void Morte()
    {
        animator.Play("PlayerDeath");
        StartCoroutine(AspettaAnimazioneAndRestart());
    }

    private IEnumerator AspettaAnimazioneAndRestart()
    {
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerDeath") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        FindFirstObjectByType<DeathAndRespawn>().Restart();
    }

    //Per capire se il giocatore è da pc
    bool PCUser()
    {
        return (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.LinuxPlayer);
    }
}
