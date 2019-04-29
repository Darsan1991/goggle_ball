using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AspectFillImage))]
public class AspectFillRandomImage : MonoBehaviour
{
    [SerializeField]private List<Sprite> _images = new List<Sprite>();

    private void Awake()
    {
        GetComponent<AspectFillImage>().Sprite = _images.GetRandom();
    }
}
