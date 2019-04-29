using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float _speed=10;
    [SerializeField] private float _radius;
    [SerializeField] private float _randomizeAngle=50f;
    private bool _active;


    public float Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }

    public Vector2 Velocity { get; private set; }

    public Vector2 CircleCenter { get; set; }
    public float CircleRadius { get; set; }

    public bool Active
    {
        get { return _active; }
        set
        {
            if (_active == value)
            {
                return;
            }

            _active = value;
            Velocity = Random.insideUnitCircle * Speed;
        }
    }


    private void FixedUpdate()
    {
        if(!Active)
            return;

        Velocity = Speed * Velocity.normalized;

        var targetPosition = transform.position + (Vector3)Velocity * Time.fixedDeltaTime;

        if (((Vector2) targetPosition - CircleCenter).magnitude >= CircleRadius-_radius)
        {
            
            //(transform.position + d*Velocity - CircleCenter).magnitude = CircleRadius-_radius
            var normal = (CircleCenter - (Vector2)transform.position).normalized;
            var hitAngle = Vector2.Angle(normal, -Velocity.normalized);

            var reflectAngle = Mathf.Clamp(hitAngle + Random.Range(-_randomizeAngle / 2, _randomizeAngle / 2),10,hitAngle>50?20 : 75f);
            var reflectDirection = Quaternion.AngleAxis(Vector3.Cross(normal,-Velocity.normalized).z>0? -reflectAngle : reflectAngle, Vector3.forward) * normal;
            Debug.Log($"Velocity Dir:{Velocity.normalized} Reflect Dir:{reflectDirection}");

            Velocity = Speed * reflectDirection.normalized;
        }
        else
        {
            transform.position = targetPosition;
        }
    }

//    private Vector2 GetCutPoint(Vector2 velocity)
//    {
//        //x**2 + y**2 = CircleRadius
//        //y = mx+c
//        //x**2 + (mx+c)**2 = CircleRadius
//        //x**2+m**2*x**2+2mxc+c**2 = CircleRadius
//        //(1+m**2)x**2 + 2mxc +c**2-CircleRadius = 0
//    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position,_radius);
    }

    private void Update()
    {
       
    }
}
