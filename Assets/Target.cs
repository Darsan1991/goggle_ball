using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private List<RadiusAndPoint> _radiusAndPoints = new List<RadiusAndPoint>();
    [SerializeField] private float _maxRadius;
    [SerializeField] private Ball _ball;


    public Ball Ball => _ball;

    public int GetPointsForDistance(float distance) => 
        _radiusAndPoints.FirstOrDefault(point => point.radius >= distance).points;

    public int GetCurrentPoints()
    {
        return GetPointsForDistance(((Vector2) _ball.transform.position - (Vector2) transform.position).magnitude);
    }

    private void Awake()
    {
        _ball.CircleCenter = transform.position;
        _ball.CircleRadius = _maxRadius;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (var radiusAndPoint in _radiusAndPoints)
        {
            Gizmos.DrawWireSphere(transform.position,radiusAndPoint.radius);
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,_maxRadius);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            _ball.Active = !_ball.Active;
        }
    }

    public void ResetTarget()
    {
        _ball.transform.position = transform.position;
        _ball.transform.rotation = Quaternion.identity;
    }

    public void Throw()
    {
        Ball.Active = true;
    }

    public void Stop()
    {
        Ball.Active = false;
    }

    [System.Serializable]
    public struct RadiusAndPoint
    {
        public float radius;
        public int points;
    }
}
