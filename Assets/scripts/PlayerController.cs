using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    private Checkpoint cp;

    [SerializeField] private AudioSource soundPoint;
    [SerializeField] private AudioSource soundFootstep;
    [SerializeField] private AudioSource soundHurt;
    [SerializeField] private AudioSource soundJump;
    [SerializeField] private AudioSource soundHPotion;
    [SerializeField] private AudioSource soundPowerUp;
    [SerializeField] private AudioSource soundCheckpoint;
    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask paperplane;
    [SerializeField] private Text tPoints;
    [SerializeField] private Text tHP;
    [SerializeField] private Text tMultiplier;

    private enum State { idle, running, jumping, falling, hurt, death };
    private State state = State.idle;

    private float runningSpeed = 7f;
    private float jumpForce = 18f;
    private float hurtForce = 10f;
    private int ects;
    private int pointValue = 1;
    private int pointMultiplier = 1;
    private int health;
    private float powerUpDuration = 8f;
    private bool immortal = false; //niesmiertelnosc - domyslnie wylaczona

    private Vector2 spawnPoint = new Vector2(-13.12f, 0.320726f);


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        cp = GetComponent<Checkpoint>();

        cp.LoadGame();
        health = PlayerPrefs.GetInt("health", 5);
        ects = PlayerPrefs.GetInt("ects", 0);
        UpdateStatusBar();
    }

    private void Update()
    {
        //blokowanie ruchu gdy gracz ponosi obrazenia
        if (state != State.hurt)
        {
            Movement();
        }
        AnimationState();
        anim.SetInteger("state", (int)state);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //kolizja z obiektem z tagiem "collectable"
        if (other.tag == "collectable")
        {
            Destroy(other.gameObject); //usuniecie obiektu
            ects = ects + pointValue * pointMultiplier; // dodanie punktu
            soundPoint.Play(); //odtworzenie dzwieku
            UpdateStatusBar(); //aktualizacja UI
        }

        //kolizja z obiektem z tagiem "healthpotion"
        if (other.gameObject.tag == "healthpotion")
        {
            //healthpotion mozna zebrac tylko wtedy, gdy nie mamy max health
            if (health < 5)
            {
                health = 5; //odnowienie zycia
                UpdateStatusBar(); //aktualizacja UI
                soundHPotion.Play(); //odtworzenie dzwieku
                Destroy(other.gameObject); //usuniecie obiektu
            }
        }

        //kolizja z obiektem z tagiem "checkpoint"
        if (other.gameObject.tag == "checkpoint")
        {
            soundCheckpoint.Play();
            other.GetComponent<Animator>().SetTrigger("saved");
            cp.SaveGame();
        }

        //kolizja z obiektem z tagiem "powerup"
        if (other.gameObject.tag == "multiplier")
        {
            Destroy(other.gameObject); //usuniecie obiektu
            soundPowerUp.Play();
            StartCoroutine(IMultiplier());
        }

        //kolizja z obiektem z tagiem "speedup"
        if (other.gameObject.tag == "speedup")
        {
            Destroy(other.gameObject); //usuniecie obiektu
            soundPowerUp.Play();
            StartCoroutine(ISpeedUp());
        }

        //kolizja z obiektem z tagiem "immortality"
        if (other.gameObject.tag == "immortality")
        {
            Destroy(other.gameObject); //usuniecie obiektu
            soundPowerUp.Play();
            StartCoroutine(IImmortal());
        }

        //kolizja z obiektem z tagiem "superjump"
        if (other.gameObject.tag == "superjump")
        {
            Destroy(other.gameObject); //usuniecie obiektu
            soundPowerUp.Play();
            StartCoroutine(ISuperJump());
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //kolizja z obiektem "enemy"
        if (other.gameObject.tag == "enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            if (state == State.falling && enemy.transform.position.y < this.transform.position.y - .5f)
            {
                //kiedy zaatakujemy enemy od gory
                enemy.JumpedOn();
                Jump(); //podskok po zabiciu enemy
            }
            else
            {
                if (!immortal)
                {
                    //kiedy wpadniemy na enemy z boku
                    if (state != State.hurt)
                    {
                        state = State.hurt;
                        //odrzucenie w kierunku przeciwnym do enemy
                        if (other.gameObject.transform.position.x > transform.position.x)
                        {
                            rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                        }
                        else
                        {
                            rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                        }
                    }
                    Damage(1);
                }
            }
        }
    }

    private void Movement()
    {
        float horizontalDirection = Input.GetAxisRaw("Horizontal");
        if (horizontalDirection < 0)
        {
            rb.velocity = new Vector2(-runningSpeed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }
        else if (horizontalDirection > 0)
        {
            rb.velocity = new Vector2(runningSpeed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }

        if (Input.GetButtonDown("Jump") && (coll.IsTouchingLayers(ground)|| coll.IsTouchingLayers(paperplane)))
        {
            Jump();
        }
    }

    private void Jump()
    {
        soundJump.Play();
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jumping;
    }

    private void AnimationState()
    {
        if (state == State.jumping)
        {
            if (rb.velocity.y < 2f)
            {
                state = State.falling;
            }
        }
        else if (state == State.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }
        else if (state == State.hurt)
        {
            StartCoroutine(Recover());
        }
        else if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            state = State.running;
        }
        else
        {
            state = State.idle;
        }
    }

    IEnumerator Recover()
    {
        yield return new WaitForSeconds(0.5f);
        state = State.idle;
    }

    private void UpdateStatusBar()
    {
        tHP.text = "";
        for (int i = 0; i < health; i++)
        {
            tHP.text += "♥ ";
        }
        tPoints.text = ects.ToString();
        tMultiplier.text = "x" + pointMultiplier.ToString();
    }

    public void Damage(int hp)
    {
        health -= hp;
        UpdateStatusBar();
        StartCoroutine(IDamage());
    }

    IEnumerator IDamage()
    {
        //po otrzymaniu obrazen czekamy jedna sekunde, w tym czasie odtwarza sie animacja
        yield return new WaitForSeconds(1);
        if (health <= 0)
        {
            //jezeli gracz stracil wszystkie zycia, to umiera
            //ustawiamy punkt respawnu na punkt zapisany w PlayerPrefs; jezeli gra nie byla do tej pory zapisana, to zostawiamy domyslny punkt
            spawnPoint = new Vector2(PlayerPrefs.GetFloat("playerPosX", spawnPoint.x), PlayerPrefs.GetFloat("playerPosY", spawnPoint.y));
            //odnawiamy punkty zycia, ale zabieramy jeden ECTS
            health = 5;
            --ects;
            UpdateStatusBar();
            transform.position = spawnPoint;

            cp.LoadGame();
        }
    }

    IEnumerator IMultiplier()
    {
        pointMultiplier = 2; //ustawienie mnoznika punktow
        UpdateStatusBar(); //aktualizacja UI

        //mnoznik punktow jest aktywny okreslona ilosc sekund (zmienna "powerUpDuration")
        yield return new WaitForSeconds(powerUpDuration);

        pointMultiplier = 1; //powrot do poprzedniej wartosci
        UpdateStatusBar(); //aktualizacja UI
    }

    IEnumerator ISpeedUp()
    {
        runningSpeed = 11f; //zmiana szybkosci poruszania

        //przyspieszenie jest aktywne przez okreslona ilosc sekund (zmienna "powerUpDuration")
        yield return new WaitForSeconds(powerUpDuration);

        runningSpeed = 7f; //powrot do poprzedniej wartosci
    }

    IEnumerator IImmortal()
    {
        //czasowe ustawienie niesmiertelosci gracza
        immortal = true;
        yield return new WaitForSeconds(powerUpDuration);

        immortal = false; //powrot do poprzedniego stanu
    }

    IEnumerator ISuperJump()
    {
        //czasowe wlaczenie wyzszego skoku
        jumpForce = 25f;
        yield return new WaitForSeconds(powerUpDuration);

        jumpForce = 18f; //powrot do poprzedniej wartosci
    }

    public void setState(int s)
    {
        state = (State)s;
    }

    public int getHealth()
    {
        return health;
    }
    public int getECTS()
    {
        return ects;
    }

    public bool getImmortalInfo()
    {
        return immortal;
    }

    //metody odtwarzające dźwięki używane w eventach animacji
    private void FootstepSound()
    {
        soundFootstep.Play();
    }

    private void HurtSound()
    {
        soundHurt.Play();
    }

}
