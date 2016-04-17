using UnityEngine;
using System.Collections;

public class enemyController : MonoBehaviour
{
    /*  PATROL LOGIC  */
    protected Vector3 velocity;
    public Transform _transform;
    public float distance = 5f;
    public float speed = 1f;
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

        if (isGoingLeft)
        {
            // If gone too far, switch direction
            if (distFromStart < -distance)
                SwitchDirection();

            _transform.Translate(-velocity.x * Time.deltaTime, 0, 0);
        }
        else
        {
            // If gone too far, switch direction
            if (distFromStart > distance)
                SwitchDirection();

            _transform.Translate(velocity.x * Time.deltaTime, 0, 0);
        }
        firingCounter++;
        if (firingCounter > 75)
        {
            firingCounter = 0;

            //The Bullet instantiation happens here.
            GameObject Temporary_Bullet_Handler;
            Temporary_Bullet_Handler = Instantiate(Bullet, Bullet_Emitter.transform.position, Bullet_Emitter.transform.rotation) as GameObject;

            //Sometimes bullets may appear rotated incorrectly due to the way its pivot was set from the original modeling package.
            //This is EASILY corrected here, you might have to rotate it from a different axis and or angle based on your particular mesh.
            //Temporary_Bullet_Handler.transform.Rotate(Vector3.left * 90);

            //Retrieve the Rigidbody component from the instantiated Bullet and control it.
            Rigidbody2D Temporary_RigidBody;
            Temporary_RigidBody = Temporary_Bullet_Handler.GetComponent<Rigidbody2D>();

            //Tell the bullet to be "pushed" forward by an amount set by Bullet_Forward_Force.
            if(isGoingLeft)
                Temporary_RigidBody.AddForce(Vector2.left * Bullet_Forward_Force);
            else
                Temporary_RigidBody.AddForce(Vector2.right * Bullet_Forward_Force);

            //Basic Clean Up, set the Bullets to self destruct after 10 Seconds, I am being VERY generous here, normally 3 seconds is plenty.
            Destroy(Temporary_Bullet_Handler, 10.0f);
        }
    }
    void SwitchDirection()
    {
        isGoingLeft = !isGoingLeft;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        //TODO: Change facing direction, animation, etc
    }
}
