using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody2D rigid;
    private float gravity = 0.1f;

    private float MaxVelocityX = 3;
    private GameManager manager;

    private bool isJumping;
    private float jumpCool = 0;

    public bool isAlive;

    public bool slowMotion = false;
    public bool slowSoundOut = false;

    public ParticleSystem crashParticle1;
    public ParticleSystem crashParticle2;

    public Slider slowTimeBar;

    float slowTime = 0;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void _Start()
    {
        isAlive = true;

        //clear velocity
        rigid.velocity = Vector2.zero;
        //set gravity to zero
        rigid.gravityScale = 0;
        rigid.AddForce(Vector2.right * 5, ForceMode2D.Impulse);
        slowMotion = false;
        Jump();

        jumpCool = 1;
    }

    void Jump()
    {
        //clear velocity
        rigid.velocity = Vector2.zero;

        //set gravityScale not to affect jumping force
        rigid.gravityScale = 0.4f;

        isJumping = true; //set jumping state
        rigid.AddForce(Vector2.up * 15, ForceMode2D.Impulse); //add up force to jump

        //add spin force
        rigid.AddTorque(1, ForceMode2D.Impulse);
    }

    void Update()
    {
        if (isAlive)
        {
            float i = Input.GetAxis("Horizontal");

            //if velocity X escapes max value
            if (rigid.velocity.x > MaxVelocityX)
            {
                rigid.velocity = new Vector2(MaxVelocityX, rigid.velocity.y);//set velocity value to max value
            }
            else if (rigid.velocity.x < -MaxVelocityX) //same
            {
                rigid.velocity = new Vector2(-MaxVelocityX, rigid.velocity.y);//set velocity value to max value
            }

            //if position Y is over than 3.5
            if (transform.position.y > 2.5f)
            {
                //clear veloity Y
                rigid.velocity = new Vector2(rigid.velocity.x, 0);
                //add down force to fall
                rigid.AddForce(Vector2.down * gravity, ForceMode2D.Impulse);

                transform.position = new Vector2(transform.position.x, 2.4f);

                manager.soundManager.Play("effect.slowIn");
                slowTime = 0;
                slowMotion = true;
                manager.vcam.SlowOn();
                slowTimeBar.gameObject.SetActive(true);
            }

            //if input is right

            float appliedSpeed = moveSpeed;
            if (slowMotion) appliedSpeed += 8;

            if (i > 0f)
            {
                transform.position = new Vector2(transform.position.x + appliedSpeed * Time.deltaTime, transform.position.y);
            }
            //if input is left
            else if (i < 0f)
            {
                transform.position = new Vector2(transform.position.x + -appliedSpeed * Time.deltaTime, transform.position.y);
            }

            if (transform.position.x < -7) transform.position = new Vector2(-7, transform.position.y);
            else if (transform.position.x > 8) transform.position = new Vector2(8, transform.position.y);

            if (isJumping)
            {
                //count jumpCool regardless of update cycle
                jumpCool += Time.deltaTime;

                //if jump cool is end
                if (jumpCool > 0.1f)
                {
                    //clear jump data
                    jumpCool = 0;
                    isJumping = false;
                    rigid.gravityScale = gravity;
                }
            }
            else
            {
                //detect key is pressing (spaceBar)
                if (Input.GetKeyDown(KeyCode.Space)) {
                    //add down force to kick
                    rigid.AddForce(Vector2.down * 15, ForceMode2D.Impulse);

                    if (slowMotion) {
                        manager.soundManager.Play("effect.slowOut");
                        if (manager.soundManager._tracks[2].time < 1.3f)
                            manager.soundManager._tracks[2].time = 1.3f;

                        slowMotion = false;
                        manager.vcam.SlowOut();
                        slowTimeBar.gameObject.SetActive(false);
                    }
                }
            }

            if (slowMotion)
            {
                Time.timeScale = 0.3f;

                slowTime += Time.unscaledDeltaTime;

                if (slowTime < 0.5f && slowSoundOut) slowSoundOut = false;

                if (slowTime >= 1.2f && !slowSoundOut)
                {
                    manager.soundManager.Play("effect.slowOut");
                    manager.soundManager._tracks[2].time = 0f;
                    slowSoundOut = true;
                }

                if (slowTime > 3f)
                {
                    slowMotion = false;
                    manager.vcam.SlowOut();
                    slowTimeBar.gameObject.SetActive(false);
                }
            } else
            {
                Time.timeScale = 1;
            }

            slowTimeBar.value = 1 - (slowTime / 3f);
        }
        else {
            rigid.gravityScale = 0;
            rigid.velocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.transform.tag)
        {
            case "enemy":
                if (!isJumping) {
                    var enemy = collision.transform.GetComponent<Enemy>();
                    manager.enemies.Remove(enemy);

                    enemy.Kicked();
                    Destroy(collision.transform.gameObject, 0.3f);

                    var particle1 = Instantiate(crashParticle1, transform.position + new Vector3(0, -0.5f), Quaternion.identity);
                    Destroy(particle1.gameObject, 1);

                    var particle2 = Instantiate(crashParticle2, transform.position + new Vector3(0, -0.5f), Quaternion.identity);
                    Destroy(particle2.gameObject, 1);

                    manager.soundManager.Play("effect.kick");

                    Jump();

                    manager.score += 10;
                    manager.xp += 5;
                }
                break;
            case "bullet":

            case "ground":
                rigid.freezeRotation = true;
                rigid.velocity = Vector2.zero;

                rigid.gravityScale = 0;

                isAlive = false;

                manager.soundManager.Play("effect.death");

                manager.uidocs.ShowTitle();

                manager.lastScore = manager.score;
                if (manager.score > PlayerPrefs.GetInt("bestScore")) PlayerPrefs.SetInt("bestScore", manager.score);

                break;
                    
        }
    }
}
