using UnityEngine;

public class FighterMovement : MonoBehaviour
{
    public float speed = 12f;

    private Rigidbody rigidbody;
    private float movementInputValue;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
	}

    private void OnEnable()
    {
        rigidbody.isKinematic = false;
    }

    private void OnDisable()
    {
        rigidbody.isKinematic = true;
    }

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        // Adjust the rigidbodies position
        Move();
    }

    public void Horizontal(float forwardBackward)
    {
        movementInputValue = forwardBackward;
    }

    private void Move()
    {
        // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
        Vector3 movement = transform.right * movementInputValue * speed * Time.deltaTime;

        // Apply this movement to the rigidbody's position.
        rigidbody.MovePosition(rigidbody.position + movement);
    }

}
