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
        rigid.velocity = Vector2.zero;

        rigid.gravityScale = 0.4f;

        isJumping = true;
        rigid.AddForce(Vector2.up * 15, ForceMode2D.Impulse);

        rigid.AddTorque(1, ForceMode2D.Impulse);
    }

    void Update()
    {
        if (isAlive)
        {
            float i = Input.GetAxis("Horizontal");

            if (rigid.velocity.x > MaxVelocityX)
            {
                rigid.velocity = new Vector2(MaxVelocityX, rigid.velocity.y);
            }
            else if (rigid.velocity.x < -MaxVelocityX)
            {
                rigid.velocity = new Vector2(-MaxVelocityX, rigid.velocity.y);
            }

            if (transform.position.y > 3.5f)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, 0);
                rigid.AddForce(Vector2.down * gravity, ForceMode2D.Impulse);
            }

            if (i > 0.5f)
            {
                transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);
            }
            else if (i < -0.5f)
            {
                transform.position = new Vector2(transform.position.x + -moveSpeed * Time.deltaTime, transform.position.y);
            }

            if (isJumping)
            {
                jumpCool += Time.deltaTime;

                if (jumpCool > 0.1f)
                {
                    jumpCool = 0;
                    isJumping = false;
                    rigid.gravityScale = gravity + manager.score * 0.02f;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    rigid.AddForce(Vector2.down * 15, ForceMode2D.Impulse);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "enemy" && !isJumping)
        {
            if (collision.transform.position.y + 0.25f < transform.position.y)
            {
                manager.enemies.Remove(collision.transform.GetComponent<Enemy>());
                Destroy(collision.transform.gameObject);
                Jump();

                manager.score += 10;
            }
        }

        if (collision.transform.tag == "ground")
        {
            rigid.freezeRotation = true;
            rigid.velocity = Vector2.zero;
            rigid.gravityScale = 0;

            isAlive = false;
        }
    }
}
