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
        StartCoroutine(SlideStickMovement());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SlideStickMovement()
    {
        int dir = isFacingLeft ? 1 : -1;

        while (isMoving)
        {
            rb.linearVelocity = new Vector3(-dir * xMoveSpeed, 0, 0);
            yield return new WaitForSeconds(movingTime);
            SetZeroVelocity();

            rb.linearVelocity = new Vector3(dir * xMoveSpeed, 0, 0);
            yield return new WaitForSeconds(movingTime);
            SetZeroVelocity();
        }
    }


}
