using UnityEngine;

public class LineTest : MonoBehaviour
{
    LineRenderer lr;
    Gradient gradient;

    void Awake()
    {

    }

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        //SetupLaser();
    }
    void SetupLaser()
    {
        //lr.material = laserMat; // Sprites/Default
        lr.useWorldSpace = true;

        lr.widthCurve = new AnimationCurve(
            new Keyframe(0f, 0.02f),
            new Keyframe(0.5f, 0.08f),
            new Keyframe(1f, 0.04f)
        );

        Gradient g = new Gradient();
        g.SetKeys(
            new GradientColorKey[]
            {
            new GradientColorKey(Color.red, 0f),
            new GradientColorKey(Color.red, 1f)
            },
            new GradientAlphaKey[]
            {
            new GradientAlphaKey(0.05f, 0f),
            new GradientAlphaKey(0.8f, 0.5f),
            new GradientAlphaKey(0.05f, 1f)
            }
        );

        lr.colorGradient = g;
    }

}
