using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    
    private Player player_;
    public float speed;

    //min range to follow
    private float followMin;

    //following base
    private float posDiffer;

    //following delay
    private float followCool = 0;

    [SerializeField] private Gun gun;

    private float shootDelay;
    private float shootTimer;

    private void Awake()
    {
        
        player_ = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            //randomize followMin & posDiffer per enemy
        followMin = Random.Range(2f, 5f);
        posDiffer = Random.Range(-6f, 6f);
        shootDelay = Random.Range(1, 2.5f);
        shootTimer = 0;
    }

    void Update()
    {
        //if followCool is less than 1.5s
        if (followCool < 1.5f)
        {
            //count followCount regardless of update cycle;
            followCool += Time.deltaTime;

            //if distance is over than followMin
            if (Vector2.Distance(player_.transform.position, transform.position) > followMin)
            {
                if (transform.position.x >= -10 && transform.position.x <= 10)
                {
                    if (player_.transform.position.x + posDiffer > transform.position.x) //if is moving to right
                    {
                        transform.Translate(Vector2.right * speed * Time.deltaTime); //move right
                    }
                    else if (player_.transform.position.x + posDiffer < transform.position.x) //if is moving to left
                    {
                        transform.Translate(Vector2.right * -speed * Time.deltaTime); //move left
                    }
                } else
                {
                    transform.position = new Vector2(Random.Range(-10, 10), transform.position.y);
                }
            }
        }
        
        Shoot();
    }

    void Shoot()
    {
        if (shootDelay > (shootTimer += Time.deltaTime)) return;
        shootTimer = 0;
        gun.Attack();   
    }

    private void OnDestroy() {
        Destroy(gun);
    }
}