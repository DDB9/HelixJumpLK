using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public BallBehavior Ball;

    private float offset;

    private void Awake()
    {
        offset = transform.position.y - Ball.transform.position.y;      
    }

    private void Update()
    {
        // TODO Modify later on so that it works with the lowest y position of the 2 balls.
        Vector3 _currentPosition = transform.position;
        _currentPosition.y = Ball.LowestY + offset;
        transform.position = _currentPosition;
    }
}
