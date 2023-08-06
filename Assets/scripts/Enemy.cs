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

    private float shootDelay;
    private float shootTimer;

    private float Right, Left;

    private void Awake() {
        
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
    }
    

    void Update()
    {
        if (player_.isAlive) {
            // dir x가 0보다 크다면 Right인 거임
            if (dir.x > 0) {
                if (transform.position.x + dir.x * speed * Time.deltaTime < Right) {
                    transform.Translate((Vector3) (dir) * speed * Time.deltaTime); //move right
                    transform.localScale = new Vector3(-1, 1, 1); 
                }
                else {transform.localScale = new Vector3(1, 1, 1); }
            }else if (dir.x < 0) {
                if (transform.position.x + dir.x * speed * Time.deltaTime > Left) {
                    transform.Translate((Vector3) (dir) * speed * Time.deltaTime); //move left
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else { transform.localScale = new Vector3(-1, 1, 1); }
            }

            moveUpdate();
            Shoot();
            timerUpdate();
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