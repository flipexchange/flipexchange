using UnityEngine;
using System.Collections;

public class platformController : MonoBehaviour
{
    /*  PATROL LOGIC  */
    protected Vector3 velocity;
    private Transform _transform;
    public float distance = 5f;
    public float speed = 1f;
    Vector3 _originalPosition;
    bool isGoingLeft = true;
    private float distFromStart;

    public void Start()
    {
        _originalPosition = gameObject.transform.position;
        _transform = GetComponent<Transform>();
        velocity = new Vector3(speed, 0, 0);
        _transform.Translate(velocity.x * Time.deltaTime, 0, 0);
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
    }
    /*void FixedUpdate() {
        if (isGoingLeft)
        {
            // If gone too far, switch direction
            if (distFromStart < -distance)
                SwitchDirection();

            GetComponent<Rigidbody2D>().MovePosition(transform.position - velocity * Time.fixedDeltaTime);
        }
        else
        {
            // If gone too far, switch direction
            if (distFromStart > distance)
                SwitchDirection();

            GetComponent<Rigidbody2D>().MovePosition(transform.position + velocity * Time.fixedDeltaTime);
        }
    }*/
    void SwitchDirection()
    {
        isGoingLeft = !isGoingLeft;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("CHILD");
        col.transform.parent = transform;
    }
    void OnCollisionExit2D(Collision2D col)
    {
        Debug.Log("ORPHAN");
        col.transform.parent = null;
    }
}