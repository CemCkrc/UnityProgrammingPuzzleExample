using UnityEngine;
using System.Collections;

//It is an old headbobbing script

public class PlayerHeadBobbing : MonoBehaviour
{

    public float bobbingSpeed = 0.1f;
    public float bobbingHeight = 0.2f;
    public float midpoint;

    private float timer = 0.0f;

    void Update()
    {
        float waveslice = 0.0f;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 conversion = transform.localPosition;

        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            timer = 0.0f;
        }
        else
        {
            waveslice = Mathf.Sin(timer);
            timer = timer + bobbingSpeed;
            if (timer > Mathf.PI * 2)
            {
                timer = timer - (Mathf.PI * 2);
            }
        }
        if (waveslice != 0)
        {
            float translateChange = waveslice * bobbingHeight;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange = totalAxes * translateChange;
            conversion.y = midpoint + translateChange;
        } 
        else
        {
            conversion.y = midpoint;
        }

        transform.localPosition = conversion;
    }
}