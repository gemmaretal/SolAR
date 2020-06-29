using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    private Touch touch;
    private Vector2 touchposition;
    private Quaternion rotatex;
    private Quaternion rotatey;
    private float rotatespeed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Moved)
            {
                rotatex = Quaternion.Euler(0f, -touch.deltaPosition.x * rotatespeed, 0f);
                rotatey = Quaternion.Euler(-touch.deltaPosition.y * rotatespeed, 0f,0f);
                transform.rotation = rotatex * transform.rotation;
                transform.rotation = rotatey * transform.rotation;

            }

        }
    }
}
