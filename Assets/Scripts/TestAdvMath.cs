using UnityEngine;

public class TestAdvMath:MonoBehaviour
{
    [SerializeField] private LineRenderer _renderer;


    private void Awake()
    {
        _renderer.positionCount = 0;

        var pointsCount = 101;

        _renderer.positionCount = pointsCount;

        for (var i = 0; i < pointsCount; i++)
        {
            var y = AdvMath.EaseIn((float)i/(pointsCount-1));
            _renderer.SetPosition(i,new Vector3(5*(float)i / (pointsCount - 1),y*5));           
        }
    }
}