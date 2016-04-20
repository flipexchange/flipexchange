using UnityEngine;
using System.Collections;

public class bossController : MonoBehaviour {

    /*  PATROL LOGIC  */
    public float distance = 5f;
    public float speed = 1f;
    public int firingPeriod = 150;

    Vector3 velocity;
    Transform _transform;
    Vector3 _originalPosition;
    bool isGoingLeft = true;
    private float distFromStart;

    /*  BULLET LOGIC  */
    //Drag in the Bullet Emitter from the Component Inspector.
    public GameObject Bullet_Emitter;

    //Drag in the Bullet Prefab from the Component Inspector.
    public GameObject Bullet;

    //Enter the Speed of the Bullet from the Component Inspector.
    public float Bullet_Forward_Force;

    private int firingCounter;

    public void Start()
    {
        _originalPosition = gameObject.transform.position;
        _transform = GetComponent<Transform>();
        velocity = new Vector3(speed, 0, 0);
        _transform.Translate(velocity.x * Time.deltaTime, 0, 0);
        firingCounter = 0;
    }

    void Update()
    {
        distFromStart = transform.position.x - _originalPosition.x;

        if (isGoingLeft) {
            // If gone too far, switch direction
            if (distFromStart < -distance)
                SwitchDirection();

            _transform.Translate(-velocity.x * Time.deltaTime, 0, 0);
        }
        else {
            // If gone too far, switch direction
            if (distFromStart > distance)
                SwitchDirection();

            _transform.Translate(velocity.x * Time.deltaTime, 0, 0);
        }

        // Firing logic
        firingCounter++;
        if (firingCounter > firingPeriod)
        {
            firingCounter = 0;

            //The Bullet instantiation happens here.
            GameObject Temporary_Bullet_Handler;
            Temporary_Bullet_Handler = Instantiate(Bullet, Bullet_Emitter.transform.position, Bullet_Emitter.transform.rotation) as GameObject;
            Physics2D.IgnoreCollision(Temporary_Bullet_Handler.GetComponent<Collider2D>(), GetComponent<Collider2D>());

            //Retrieve the Rigidbody component from the instantiated Bullet and control it.
            Rigidbody2D Temporary_RigidBody;
            Temporary_RigidBody = Temporary_Bullet_Handler.GetComponent<Rigidbody2D>();

            Vector3 playerPos = GameObject.Find("Player").transform.position;
            Vector3 bulletVec = playerPos - transform.position;

            //Tell the bullet to be "pushed" forward by an amount set by Bullet_Forward_Force.
            Temporary_RigidBody.AddForce(bulletVec * Bullet_Forward_Force);
            
            //Basic Clean Up, set the Bullets to self destruct after 10 Seconds, I am being VERY generous here, normally 3 seconds is plenty.
            Destroy(Temporary_Bullet_Handler, 3.0f);
        }
    }
    void SwitchDirection()
    {
        isGoingLeft = !isGoingLeft;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
