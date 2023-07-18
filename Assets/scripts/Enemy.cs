using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    
    private Player player_;
    public float speed;

    private float followMin;
    private float posDiffer;

    private float followCool = 0;

    [SerializeField] private Gun gun;

    private float shootDelay;
    private float shootTimer;

    private void Awake()
    {
        
        player_ = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        
        followMin = Random.Range(2f, 5f);
        posDiffer = Random.Range(-6f, 6f);
        shootDelay = Random.Range(1, 2.5f);
        shootTimer = 0;
    }

    void Update()
    {

        if (followCool < 1.5f)
        {
            followCool += Time.deltaTime;

            if (Vector2.Distance(player_.transform.position, transform.position) > followMin)
            {
                if (player_.transform.position.x + posDiffer > transform.position.x)
                {
                    transform.Translate(Vector2.right * speed * Time.deltaTime);
                }
                else if (player_.transform.position.x + posDiffer < transform.position.x)
                {
                    transform.Translate(Vector2.right * -speed * Time.deltaTime);
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