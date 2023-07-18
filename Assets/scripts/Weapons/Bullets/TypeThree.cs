using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeThree : Types
{
    private GameObject[] bullets;
    private int spreadBulletCnt;
    private GameObject spreadBullet;

    public TypeThree(float speed, int time) : base(speed, time)
    {
    }

    public void setBullet(GameObject bullet) {
        spreadBullet = bullet;
    }

        void Start() {
        
        spreadBulletCnt = 5;
        bullets = new GameObject[spreadBulletCnt];
        for (int i = 0; i < spreadBulletCnt; i++)
        {
            bullets[i] = Instantiate(spreadBullet);
            bullets[i].SetActive(false);
        }
        
    }

    void setGeneral() {
        spreadBulletCnt = 0;
    }
    protected override void skill() {
        
    }

    private void spread() {
        
    }
}
