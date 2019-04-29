using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreTileUI : MonoBehaviour
{
    public event Action<StoreTileUI> Clicked; 

    [SerializeField] private SlowBallProduct _slowBall;

    public SlowBallProduct SlowBall => _slowBall;

    public void OnClick()
    {
        Clicked?.Invoke(this);
    }
}