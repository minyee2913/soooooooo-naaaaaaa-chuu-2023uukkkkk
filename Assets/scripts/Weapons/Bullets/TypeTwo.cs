using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeTwo : Types
{
    private float Right, Left, Top, Bottom;
    private int bounseCnt;
    public void Start()
    {
        Right = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x;
        Left = -Right;
        Top = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y;
        Bottom = -Top;
    }

    public void OnEnable() { bounseCnt = 3; }
    protected override void skill() {
        if(!InStage()) reverseDirection();
        else if(bounseCnt <= 0) this.gameObject.SetActive(false);
    }
    void reverseDirection() {
        bounseCnt--;
        // transform.localEulerAngles = new Vector3(0, 0, (transform.eulerAngles.z + 180) % 360); // 나ㅣ중에 쿼터니언 값으로 바꾸자
        transform.rotation = Quaternion.Euler(0, 0, (transform.eulerAngles.z + 180) % 360 + Random.Range(-40, 40)); // 나ㅣ중에 쿼터니언 값으로 바꾸자
    }
    
    bool InStage() {
        Vector2 pos = transform.position;
        return (Right > pos.x && pos.x > Left &&
                Top > pos.y && pos.y > Bottom);
        
    }
    
    public TypeTwo(float speed, int time) : base(speed, time)
    {
    }
}
