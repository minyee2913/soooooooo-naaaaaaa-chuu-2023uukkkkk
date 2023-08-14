using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    
    private Player player_;
    public float speed;

    private float moveTime;
    private Vector2 dir;

    [SerializeField] public Gun gun;
    private Animator animator_;

    private float shootDelay;
    private float shootTimer;

    private float Right, Left;
    

    public bool IsAlive;

    private void Awake() {
        
        animator_ = GetComponent<Animator>();
        player_ = GameManager.player;
        
        //randomize followMin & posDiffer per enemy
        // followMin = Random.Range(2f, 5f);
        // posDiffer = Random.Range(-6f, 6f);
        
        Right = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x;
        Left = -Right;
        
        moveTime = Random.Range(0f, 1.7f);
        dir = Random.Range(0, 2) >= 1 ? Vector2.right : Vector2.left;
        
        shootDelay = Random.Range(1, 2.5f);
        shootTimer = 0;

        IsAlive = true;
    }
    

    void Update()
    {
        if (player_.isAlive && IsAlive) {
            // dir x가 0보다 크다면 Right인 거임

            bool moved = false;
            if (dir.x > 0) {
                if (transform.position.x + dir.x * speed * Time.deltaTime < Right) {
                    transform.Translate((Vector3) (dir) * speed * Time.deltaTime); //move right
                    transform.localScale = new Vector3(-1, 1, 1);

                    moved = true;
                }
                else {
                    transform.localScale = new Vector3(1, 1, 1);
                }

            }else if (dir.x < 0) {
                if (transform.position.x + dir.x * speed * Time.deltaTime > Left) {
                    transform.Translate((Vector3) (dir) * speed * Time.deltaTime); //move left
                    transform.localScale = new Vector3(1, 1, 1);

                    moved = true;
                }
                else {
                    transform.localScale = new Vector3(-1, 1, 1);
                }

            }

            animator_.SetBool("isMoving", moved);

            if (transform.position.x < -6) transform.position = new Vector2(-6, transform.position.y);
            else if (transform.position.x > 7) transform.position = new Vector2(7, transform.position.y);

            moveUpdate();
            Shoot();
            timerUpdate();
            GunLook();
        }
    }


    private void GunLook() {
        if(player_.transform.position.x < transform.position.x * dir.x)  transform.localScale = new Vector3(dir.x, 1, 1);
    }
    public void Kicked()
    {
        IsAlive = false;

        StartCoroutine(_kicked());
    }

    IEnumerator _kicked()
    {
        Vector3 rot = transform.rotation.eulerAngles;
        for (int i = 0; i < 10; i++)
        {
            transform.localScale = new Vector2(1 + (i * 0.02f), 1 - (i * 0.04f));
            transform.localRotation = Quaternion.Euler(new Vector3(rot.x, rot.y, -2.5f * i));
            yield return new WaitForSeconds(0.01f);
        }
    }

    void timerUpdate()
    {
        moveTime -= Time.deltaTime;
        shootTimer += Time.deltaTime;
    }
    void moveUpdate() {
        if (moveTime <= 0) {
            moveTime = Random.Range(0f, 1.7f);
            dir = Random.Range(0, 2) >= 1 ? Vector2.right : Vector2.left;
        }
    }
    void Shoot() {
        if (shootDelay <= shootTimer) {
            shootTimer = 0;
            if (gun != null) gun.Attack();
        }
    }

    private void OnDestroy() { Destroy(gun); }
}