using UnityEngine;

public class Gimmick_Sickle : Gimmick
{
    [SerializeField] private bool isRotate;
    [SerializeField] private bool isRotateClockwise;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }


    // Update is called once per frame
    void Update()
    {
        RotateMovement();
    }

    public void RotateMovement()
    {
        int dir = isRotateClockwise ? 1 : -1;
        if (isRotate)
        {
            transform.Rotate(0, dir * rotateSpeed * Time.deltaTime,0);
        }
    }
    public void SetIsRotateClockwise()
    {
        isRotateClockwise = true;
    }
    public void SetIsRotate()
    {
        isRotate = true;
    }
}
