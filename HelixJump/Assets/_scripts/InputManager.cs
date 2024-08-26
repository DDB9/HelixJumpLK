using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GameObject HelixOne, HelixTwo;
    public float SwipeSensitivity;

    private Touch userInput;
    private Vector2 lastTouchPosition;

    private void LateUpdate()
    {
        // If the user has used one or more fingers to touch the screen...
        if (Input.touchCount > 0)
        {
            // Use only the information from the first finger that touched the screen.
            userInput = Input.GetTouch(0);

            // Rotate the helix around its y-axis using the user input.
            if (userInput.phase == TouchPhase.Moved || userInput.phase == TouchPhase.Ended)
            {
                // Apply the rotation conform the user's swipe input (delta position).
                HelixOne.transform.Rotate(0, -userInput.deltaPosition.x * SwipeSensitivity * Time.deltaTime, 0);
                HelixTwo.transform.Rotate(0, -userInput.deltaPosition.x * SwipeSensitivity * Time.deltaTime, 0);
            }
        }

        #if UNITY_STANDALONE_WIN
        // Input code if the game plays in windows instead of android.
        if (Input.GetMouseButton(0))
        {
            Vector2 _touchPosition = Input.mousePosition;

            if (_touchPosition == Vector2.zero)
            {
                lastTouchPosition = _touchPosition;
            }
            float _touchDelta = lastTouchPosition.x - _touchPosition.x;
            lastTouchPosition = _touchPosition;

            HelixOne.transform.Rotate(Vector3.up * _touchDelta);
            HelixTwo.transform.Rotate(Vector3.up * _touchDelta);
        }

        if (Input.GetMouseButtonUp(0))
        {
            lastTouchPosition = Vector2.zero;
        }
        #endif
    }
}
