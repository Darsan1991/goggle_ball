using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MainMenu;
using MyGame;
using UnityEngine;

public class SplashPanel : MonoBehaviour
{
    [SerializeField] private RectTransform _target;
    [SerializeField] private RectTransform _ball;
    [SerializeField] private Animator _googleAnim;
    [SerializeField] private Animator _ballAnim;
    [SerializeField] private Animator _characterAnim;


    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.2f);
        yield return SplashAnim();
    }

    private IEnumerator SplashAnim()
    {
        yield return RotateTimes(_target);
        yield return BallMoveAnim(_ball, new[]
        {
            new Vector2(_ball.GetSizeInScreenSpace().x/2, _googleAnim.transform.position.y), (Vector2) _googleAnim.transform.position
        });
        _ball.gameObject.SetActive(false);
        _googleAnim.gameObject.SetActive(true);
        yield return new WaitUntil(() => _googleAnim.GetCurrentAnimatorStateInfo(0).normalizedTime>0.93f);
        
        _ball.transform.position = new Vector3(Screen.width+300,_ballAnim.transform.position.y,_ball.transform.position.z);
        _ball.gameObject.SetActive(true);
        yield return BallMoveAnim(_ball, new[] {(Vector2) _ballAnim.transform.position});
        _ball.gameObject.SetActive(false);
        _ballAnim.gameObject.SetActive(true);
        yield return new WaitUntil(() => _ballAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.93f);
        _characterAnim.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        UIManager.Instance.MenuPanel.Show();
    }

    private IEnumerator BallMoveAnim(RectTransform ball,IEnumerable<Vector2> points)
    {
        var list = points.ToList();
        var speed = 600f;
        for (var i = 0; i < list.Count; i++)
        {
            var j = i;
            StartCoroutine(Rotate(ball, -2.5f ,untilAction: () => i == j));
            while ((list[i]-(Vector2)ball.position).sqrMagnitude>1f)
            {
                ball.position = Vector2.MoveTowards(ball.position,list[i],speed*Time.deltaTime);
                yield return null;
            }
        }
    }

    private IEnumerator RotateTimes(Transform target,int rotateCount = 4)
    {
        var speed = 1.5f/rotateCount;

        var normalized = 0f;
        var startAngle = target.eulerAngles.z;
        while (normalized<1f)
        {
            yield return null;
            normalized = Mathf.MoveTowards(normalized, 1, Time.deltaTime * speed);
            var eulerAngles = target.eulerAngles;
            eulerAngles.z = Mathf.Lerp(startAngle,startAngle -rotateCount * 360, normalized);
            target.eulerAngles = eulerAngles;
        }
        
        target.eulerAngles = new Vector3(target.eulerAngles.x,target.eulerAngles.y,startAngle-rotateCount*360);

    }

    private IEnumerator Rotate(Transform target,float speed=1,Func<bool> untilAction=null)
    {
        while (untilAction?.Invoke()??true)
        {
            target.Rotate(Vector3.forward,speed*360*Time.deltaTime);
            yield return null;
        }
    }
}
