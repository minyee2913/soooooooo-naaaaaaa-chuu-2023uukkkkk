using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    //enemy object
    public GameObject enemy;
    public GameObject GunEnemy;

    
    //spawned enemies
    [HideInInspector]
    public List<Enemy> enemies = new();

    //player spawn pos
    private float spawnY = -3.18f;

    //enemy spawning delay
    private float spawningDelay = 0;

    public SoundManager soundManager;

    public int score = 0;
    public int lv = 1;
    public int xp = 0;
    public int maxXp = 20;

    public static Player player;
    public UIdocs uidocs;
    public Vcam vcam;

    public ParticleSystem lvUpParticle;

    public GameObject resetPanel;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        uidocs = GameObject.Find("UI").GetComponent<UIdocs>();
        vcam = GameObject.Find("vcam").GetComponent<Vcam>();
    }

    private void Start()
    {
        GameStart();
    }

    public void GameStart()
    {
        //remove all spawned enemies
        for (int i = 0; i < enemies.Count; i++) {
            var e = enemies[i];

            // Debug.Log(e);

            enemies.Remove(e);

            //e.GetComponent<Enemy>().gun.DestroyBullet();

            Destroy(e.gameObject);
        }

        //move player to spawn pos
        player.transform.position = new Vector2(-5, -2.5f);
        player._Start();

        lv = 1;
        maxXp = 20;
        xp = 1;
        score = 0;
    }

    public void GamePause()
    {
        resetPanel.SetActive(true);
        Time.timeScale=0;
    }
    public void Cancel()
    {
        resetPanel.SetActive(false);
        Time.timeScale=1;
    }
    public void ResetAccount()
    {
        Cancel();
        lv = 1;
        maxXp = 20;
        xp = 1;
        GameStart();
    }

    private void Update()
    {
        if(player.isAlive)
        {
            if (Input.GetKeyDown(KeyCode.Q)) GamePause();

            //if enemy count less then 12
            if (enemies.Count < 12)
            {
                //count spawning delay regardless of update cycle;
                spawningDelay += Time.deltaTime;

                float delay = 0.7f;
                if (enemies.Count > 5) delay = 1.5f;

                if (spawningDelay > delay)
                {

                    //copy enemy prefab to gameObject
                    GameObject enem;
                    //if (Random.Range(0, 100) > 50) enem = Instantiate(enemy, new Vector2(spawnPos.x * Random.Range(-1, 1), spawnPos.y), Quaternion.identity);
                    enem = Instantiate(GunEnemy, new Vector2(Random.Range(-7, 8), spawnY), Quaternion.identity);

                    //add enemy at spawned enemies list
                    enemies.Add(enem.GetComponent<Enemy>());

                    spawningDelay = 0;
                }
            }

            if (xp >= maxXp)
            {
                xp = 0;
                lv++;
                maxXp = lv * 20;

                soundManager.Play("effect.lvup");

                var particle = Instantiate(lvUpParticle, player.transform.position, Quaternion.identity);
                particle.transform.SetParent(player.transform, false);

                particle.transform.localPosition = Vector2.zero;
                particle.transform.localScale = new Vector2(0.8f, 0.8f);
                Destroy(particle.gameObject, 1);

                uidocs.LvUp();
            }

            

        } else
        {
            //detect key pressing (spacebar)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameStart(); //start game
            }
        }
    }
}
