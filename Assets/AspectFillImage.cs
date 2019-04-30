using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[ExecuteInEditMode]
public class AspectFillImage : MonoBehaviour
{
    [HideInInspector][SerializeField] private Image _image;
    [SerializeField] private bool _update;

    private RectTransform RectTransform => (RectTransform) transform;

    public Sprite Sprite
    {
        get { return _image.sprite;}
        set
        {
            _image.sprite = value;
            Refresh();
        }
    }

    private IEnumerator Start()
    {
        Refresh();
        yield return new WaitForEndOfFrame();
        Refresh();
        for (var i = 0; i < 2; i++)
        {
            yield return null;
            Refresh();
        }
    }

    private void Update()
    {
        if(!_update && Application.isPlaying)
            return;
        if(transform.hasChanged)
        Refresh();
    }

    [ContextMenu("Refresh")]
    public void Refresh()
    {
        if(_image == null)
            return;

        _image.transform.localScale = Vector3.one;
        _image.rectTransform.anchorMax = Vector2.one*0.5f;
        _image.rectTransform.anchorMin = Vector2.one*0.5f;
        _image.rectTransform.anchoredPosition3D = Vector3.zero;
        _image.preserveAspect = true;

        if (_image.sprite == null)
        {
            return;
        }
        var imgAspect = _image.sprite.bounds.extents.y / _image.sprite.bounds.extents.x;
        var aspect = RectTransform.rect.size.y / RectTransform.rect.size.x;
        _image.rectTransform.sizeDelta = imgAspect>=aspect?new Vector2(RectTransform.rect.size.x,imgAspect* RectTransform.rect.size.x) 
            : new Vector2(RectTransform.rect.size.y/imgAspect,RectTransform.rect.size.y);
    }


    private void OnValidate()
    {
        if (_image !=null && !_image.transform.IsChildOf(transform))
        {
            _image = null;
        }
    }
}