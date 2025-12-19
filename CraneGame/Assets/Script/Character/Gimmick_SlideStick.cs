using UnityEngine;
using System.Collections;
public class Gimmick_SlideStick : Gimmick
{
    [SerializeField] private bool isFacingLeft;
    [SerializeField] private bool isMoving;
    [SerializeField] private float movingTime;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        isMoving = true;
        StartCoroutine(SlideStickMovement());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SlideStickMovement()
    {
        //int dir = isFacingLeft ? 1 : -1;

        //while (isMoving)
        //{
        //    rb.linearVelocity = new Vector3(-dir * xMoveSpeed, 0, 0);
        //    yield return new WaitForSeconds(movingTime);
        //    SetZeroVelocity();

        //    rb.linearVelocity = new Vector3(dir * xMoveSpeed, 0, 0);
        //    yield return new WaitForSeconds(movingTime);
        //    SetZeroVelocity();
        //}

        int dir = isFacingLeft ? 1 : -1;

        while (isMoving)
        {
            // 获取物体自身的右方向（transform.right）
            Vector3 localRight = transform.up;

            // 向自身左/右移动
            rb.linearVelocity = -dir * localRight * xMoveSpeed;
            yield return new WaitForSeconds(movingTime);
            SetZeroVelocity();

            rb.linearVelocity = dir * localRight * xMoveSpeed;
            yield return new WaitForSeconds(movingTime);
            SetZeroVelocity();
        }
    }
    public void SetFacingRight()
    {
        transform.Rotate(0, 180, 0);
    }

    public void SetFacingUp()
    {
        transform.Rotate(0, 90, 0);
    }
    public void SetFacingDown()
    {
        transform.Rotate(0, -90, 0);
    }

    public void SetMoveTime(float _time)
    {
        movingTime = _time;
    }

}
