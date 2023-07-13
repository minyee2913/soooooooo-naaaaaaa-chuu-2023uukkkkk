using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody2D rigid;
    private float gravity = 0.2f;

    private float MaxVelocityX = 3;
    private Transform model;
    private GameManager manager;

    private bool isJumping;
    private float jumpCool = 0;

    public bool isAlive;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        model = transform.Find("model");
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void _Start()
    {
        rigid.AddForce(Vector2.right * 5, ForceMode2D.Impulse);
        Jump();

        isAlive = true;
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
            if (transform.position.y > 3.5f)
            {
                //clear veloity Y
                rigid.velocity = new Vector2(rigid.velocity.x, 0);
                //add down force to fall
                rigid.AddForce(Vector2.down * gravity, ForceMode2D.Impulse);
            }

            //if input is right
            if (i > 0.5f)
            {
                transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);
            }
            //if input is left
            else if (i < -0.5f)
            {
                transform.position = new Vector2(transform.position.x + -moveSpeed * Time.deltaTime, transform.position.y);
            }

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
                    rigid.gravityScale = gravity + manager.score * 0.02f;
                }
            }
            else
            {
                //detect key is pressing (spaceBar)
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    //add down force to kick
                    rigid.AddForce(Vector2.down * 15, ForceMode2D.Impulse);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if entered target is enemy & player is not jumping
        if (collision.transform.tag == "enemy" && !isJumping)
        {
            //if enemy's Y pos is less than player's Y pos
            if (collision.transform.position.y + 0.25f < transform.position.y)
            {
                //remove enemy at enemies
                manager.enemies.Remove(collision.transform.GetComponent<Enemy>());
                //destroy enemy's gameObject
                Destroy(collision.transform.gameObject);
                //jump player
                Jump();

                //add score
                manager.score += 10;
            }
        }

        //if entered target is ground
        if (collision.transform.tag == "ground")
        {
            //freeze player's rotation
            rigid.freezeRotation = true;
            //clear velocity
            rigid.velocity = Vector2.zero;
            //set gravity to zero
            rigid.gravityScale = 0;

            //set player dead
            isAlive = false;
        }
    }
}
