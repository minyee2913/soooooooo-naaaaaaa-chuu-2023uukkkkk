
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Gun : MonoBehaviour
{
    private Transform Player;

    private GameObject[] bullets;


    [SerializeField] private GameObject[] BulletTypes;
    public GameObject bullet;

    [SerializeField] private Transform muzzlePos;
    
    private int idx;

    public Quaternion _angle;
    
    [Range(1, 45)]
    [SerializeField] private int limit;
    private void Start() {
        Player = GameObject.FindGameObjectWithTag("Player").gameObject.transform; // 플레이어 게임 오브젝트 get
        
        int type = Random.Range(1, 3); // 총알 타입 지정
        idx = 0;
        bullet = BulletTypes[type-1];
        Types myType = bullet.GetComponent<Types>();
        
        myType.init(10, (int)(5 * type * 0.5));
        
        bullets = new GameObject[limit]; // 총알 배열 초기화
        for (int i = 0; i < limit; i++) { bullets[i] = clone(myType); bullets[i].SetActive(false); } // 최적화하기 위한 총알 배열 넣기
        
    }

    private GameObject clone(Types types) {
        GameObject bullet = Instantiate(this.bullet);
        bullet.transform.localScale = new Vector2(0.4f, 0.2f);

        bullet.GetComponent<Types>().init(types.getSpeed(), types.getTime());
        return bullet;
    }

    private void Update() {
        LookAt();
    }

    void LookAt() {
        Vector2 dir =  transform.position - Player.position;
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        _angle =  Quaternion.AngleAxis(-angle, Vector3.forward);
    }
    public void Attack()
    {
        idx %= limit;
        bullets[idx].transform.position = muzzlePos.position;
        bullets[idx].transform.rotation = _angle;
        bullets[idx].GetComponent<Types>().setTarget(new Vector2(Player.position.x, Player.position.y));
        bullets[idx++].SetActive(true);

    }

/*    public void DestroyBullet()
    {
        Destroy(bullet);
    }*/

    private void OnDestroy()
    {
        for(int i = 0; i < bullets.Length; i++) {
            Destroy(bullets[i]);
        }
    }
}
