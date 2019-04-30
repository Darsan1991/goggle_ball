using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class NumberImage : MonoBehaviour
{
    [SerializeField]private List<Sprite> _numberSprites = new List<Sprite>();

    private int _number;

    public int Number
    {
        get { return _number; }
        set
        {
            if(value>=_numberSprites.Count || value<0)
                throw new NotImplementedException();

            _number = value;
            GetComponent<Image>().sprite = _numberSprites[value];
        }
    }
}
