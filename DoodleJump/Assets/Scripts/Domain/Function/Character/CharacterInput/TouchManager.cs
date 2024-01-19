using UnityEngine;

public class TouchManager : MonoBehaviour
{
    private float fingerActionSensitivity = Screen.width * 0.05f;
    private float fingerBeginX;
    private float fingerBeginY;
    private float fingerCurrentX;
    private float fingerCurrentY;
    private float fingerSegmentX;
    private float fingerSegmentY;
    //
    private int fingerTouchState;
    //
    private int FINGER_STATE_NULL = 0;
    private int FINGER_STATE_TOUCH = 1;
    private int FINGER_STATE_ADD = 2;
    // Use this for initialization
    void Start()
    {
        fingerActionSensitivity = Screen.width * 0.05f;

        fingerBeginX = 0;
        fingerBeginY = 0;
        fingerCurrentX = 0;
        fingerCurrentY = 0;
        fingerSegmentX = 0;
        fingerSegmentY = 0;

        fingerTouchState = FINGER_STATE_NULL;
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            if (fingerTouchState == FINGER_STATE_NULL)
            {
                fingerTouchState = FINGER_STATE_TOUCH;
                fingerBeginX = Input.mousePosition.x;
                fingerBeginY = Input.mousePosition.y;
            }

        }

        if (fingerTouchState == FINGER_STATE_TOUCH)
        {
            fingerCurrentX = Input.mousePosition.x;
            fingerCurrentY = Input.mousePosition.y;
            fingerSegmentX = fingerCurrentX - fingerBeginX;
            fingerSegmentY = fingerCurrentY - fingerBeginY;

        }


        if (fingerTouchState == FINGER_STATE_TOUCH)
        {
            float fingerDistance = fingerSegmentX * fingerSegmentX + fingerSegmentY * fingerSegmentY;

            if (fingerDistance > (fingerActionSensitivity * fingerActionSensitivity))
            {
                toAddFingerAction();
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            fingerTouchState = FINGER_STATE_NULL;
        }
    }

    private void toAddFingerAction()
    {

        fingerTouchState = FINGER_STATE_ADD;

        if (Mathf.Abs(fingerSegmentX) > Mathf.Abs(fingerSegmentY))
        {
            fingerSegmentY = 0;
        }
        else
        {
            fingerSegmentX = 0;
        }

        if (fingerSegmentX == 0)
        {
            if (fingerSegmentY > 0)
            {
                // Debug.Log("up");
            }
            else
            {
                // Debug.Log("down");
            }
        }
        else if (fingerSegmentY == 0)
        {
            if (fingerSegmentX > 0)
            {

            }
            else
            {
                // SwitchPanel.laftAction();
            }
        }

    }
}