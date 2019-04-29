using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RectEnvironment:MonoBehaviour, ICycleTile
{
    private const float WORLD_SIZE = Constants.WORLD_SIZE;
    public const bool VERTICAL_DIRECTION = Constants.VERTICAL_DIRECTION;

    [HideInInspector]
    [SerializeField]
    private Vector3 _startPoint;
    [HideInInspector]
    [SerializeField]
    private Vector3 _endPoint;

    [Range(0,1f)]
    [SerializeField] private float _probability;

    [SerializeField] private  int _id;


    public float Probability => _probability;

    public int Id => _id;



    public Vector3 StartPointLocalPosition
    {
        get
        {
            return _startPoint;

        }
        set
        {
            _startPoint = value;
           
        }
    }

    public Vector3 EndPointLocalPosition
    {
        get { return _endPoint; }
        set
        {
            _endPoint = value;
        }
    }

   
    public float Size => (transform.TransformPoint(EndPointLocalPosition) - transform.TransformPoint(StartPointLocalPosition)).magnitude;
    public Vector2 Position => transform.TransformPoint(StartPointLocalPosition);


    private void OnDrawGizmos()
    {
        var leftTop = VERTICAL_DIRECTION? transform.TransformPoint(EndPointLocalPosition) - Vector3.right * WORLD_SIZE / 2 
            : transform.TransformPoint(StartPointLocalPosition)+Vector3.up*WORLD_SIZE/2;
        var rightTop =VERTICAL_DIRECTION?transform.TransformPoint(EndPointLocalPosition) + Vector3.right * WORLD_SIZE / 2
            : transform.TransformPoint(EndPointLocalPosition) + Vector3.up * WORLD_SIZE / 2;
        var leftBottom = VERTICAL_DIRECTION?transform.TransformPoint(StartPointLocalPosition) - Vector3.right * WORLD_SIZE / 2:
            transform.TransformPoint(StartPointLocalPosition) - Vector3.up * WORLD_SIZE / 2; 
        var rightBottom = VERTICAL_DIRECTION?transform.TransformPoint(StartPointLocalPosition) + Vector3.right * WORLD_SIZE / 2:
            transform.TransformPoint(EndPointLocalPosition) - Vector3.up * WORLD_SIZE / 2;

        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(leftTop, 0.2f);
        Gizmos.DrawWireSphere(rightTop, 0.2f);
        Gizmos.DrawWireSphere(leftBottom, 0.2f);
        Gizmos.DrawWireSphere(rightBottom, 0.2f);


        Gizmos.color = Color.white;
        Gizmos.DrawLine(leftTop, rightTop);
        Gizmos.DrawLine(rightTop, rightBottom);
        Gizmos.DrawLine(rightBottom, leftBottom);
        Gizmos.DrawLine(leftTop, leftBottom);



    }

   

    public void SetPosition(Vector2 position)
    {
        transform.position = (transform.position - transform.TransformPoint(StartPointLocalPosition)) + (Vector3)position;
    }
}