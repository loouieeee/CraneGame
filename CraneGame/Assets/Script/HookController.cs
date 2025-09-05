using UnityEngine;

public class HookController : MonoBehaviour
{
    //public static HookController instance;

    public float xInput;
    public float yInput;
    public float xMoveSpeed = 50;
    public float yMoveSpeed = 30;

    public Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        rb.linearVelocity = new Vector3 (-xInput * xMoveSpeed,-yMoveSpeed, 0);
    }

    void ZeroVelocity()
    {
        rb.angularVelocity= Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("stop"))
        {


        }

    }
}
