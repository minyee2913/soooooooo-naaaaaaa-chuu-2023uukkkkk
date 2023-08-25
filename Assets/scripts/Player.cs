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

    public Sprite flow;
    public Sprite strike;
    private SpriteRenderer _renderer;

    public Slider slowTimeBar;

    float slowTime = 0;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _renderer = GetComponent<SpriteRenderer>();
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

                _renderer.sprite = flow;

                manager.soundManager.Play("effect.slowIn");
                slowTime = 0;
                slowMotion = true;
                manager.vcam.SlowOn();
                slowTimeBar.gameObject.SetActive(true);
            }


            //{
            //  YOU CAN DO IT (itch)
            //      GUYS
            //}

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
                    _renderer.sprite = strike;

                    //{
                    //  YOU CAN DO IT (nee)
                    //      GUYS
                    //}
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
            //{
            //  YOU CAN DO IT (sang)
            //      GUYS
            //}
        }
    }
}
