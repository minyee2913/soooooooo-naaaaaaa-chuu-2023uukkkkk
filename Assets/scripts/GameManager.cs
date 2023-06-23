using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject enemy;
    [HideInInspector]
    public List<Enemy> enemies = new();
    private Vector2 spawnPos = new(8, -3.18f);

    //amazing
    private float spawningDelay = 0;

    public int score = 0;
    public int lv = 1;
    public int xp = 0;

    private Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        GameStart();
    }

    public void GameStart()
    {
        for (int i = 0; i < enemies.Count; i++) {
            var e = enemies[i];

            enemies.Remove(e);
            Destroy(e.gameObject);
        }

        player.transform.position = new Vector2(-5, -3.3f);
        player._Start();
    }

    private void Update()
    {
        if (enemies.Count < 4)
        {
            spawningDelay += Time.deltaTime;
            if (spawningDelay > 1)
            {
                GameObject enem = Instantiate(enemy, new Vector2(spawnPos.x * Random.Range(-1, 1), spawnPos.y), Quaternion.identity);

                enemies.Add(enem.GetComponent<Enemy>());

                spawningDelay = 0;
            }
        }

        if (!player.isAlive)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameStart();
            }
        }
    }
}
