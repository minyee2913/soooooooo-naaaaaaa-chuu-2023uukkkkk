
using UnityEngine;
using UnityEngine.UIElements;

public class Types : MonoBehaviour
{
    [Range(1, 100)]
    private float speed;
    
    
    private int time;
    private Vector3 target;
    private Vector2 myDir;
    private Rigidbody2D rb;
    
    // region constructor
    public Types(float speed, int time) {
        this.speed = speed;
        Invoke("Unable", time);
    }


    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void init(float speed, int time) {
        this.speed = speed;
        this.time = time;
    }
    // endregion constructor

    public void setTarget(Vector2 target)
    {
        myDir = target - (Vector2) transform.position;


    }
    
    // region unity methods
    
    public void OnEnable() {
        Invoke("Unable", time);
        
    }


    private void Update() {
        move();
        skill();
        
    }

    // endregion unity methods
    
    
    protected void Unable()
    {
        this.gameObject.SetActive(false);
    }

    protected virtual void skill() {
        
    }

    protected void move() {
        // transform.position = Vector2.SmoothDamp(gameObject.transform.position, target.transform.position, ref moveDir,  1/speed * 5);
        // transform.position = Vector2.MoveTowards(gameObject.transform.position, target.transform.position, speed);
        // rb.AddForce((target - transform.position).normalized * speed * Time.deltaTime);
        transform.Translate(Vector2.up * -speed * Time.deltaTime );
    }

    
    
    // region getter methods
    public int getTime()
    {
        return time;
    }

    public float getSpeed()
    {
        return speed;
    }
    // endregion getter methods
}
