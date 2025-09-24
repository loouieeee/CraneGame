using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected float xMoveSpeed;
    [SerializeField] protected float yMoveSpeed;
    [SerializeField] protected float zMoveSpeed;
    [SerializeField] protected float rotateSpeed;

    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected Collider collider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    public void SetZeroVelocity()
    {
        rb.linearVelocity = Vector3.zero;
    }

    public void SetRotateSpeed(float _rotatespeed)
    {
        rotateSpeed = _rotatespeed;
    }

    public void SetXMoveSpeed(float _xmovespeed)
    {
        xMoveSpeed = _xmovespeed;
    }

    public void SetYMoveSpeed(float _ymovespeed)
    {
        yMoveSpeed = _ymovespeed;
    }

    public void SetZMoveSpeed(float _zmovespeed)
    {
        zMoveSpeed = _zmovespeed;
    }
        
    // Update is called once per frame
    void Update()
    {
        
    }
}
