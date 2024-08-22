using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public BallBehavior Ball1;
    public BallBehavior Ball2;

    private float offset;

    private void Awake()
    {
        offset = transform.position.y - Ball1.transform.position.y;      
    }

    private void Update()
    {
        // TODO Modify later on so that it works with the lowest y position of the ball that is currently highest up.
        Vector3 _currentPosition1 = transform.position;
        Vector3 _currentPosition2 = transform.position;
        _currentPosition1.y = Ball1.LowestY + offset;
        _currentPosition2.y = Ball2.LowestY + offset;

        // The the y position of ball 1 is lower than the y position of ball 2, follow ball 2, and vice versa.
        if (_currentPosition1.y < _currentPosition2.y ) transform.position = _currentPosition2;
        else if (_currentPosition1.y > _currentPosition2.y ) transform.position = _currentPosition1;
    }
}
