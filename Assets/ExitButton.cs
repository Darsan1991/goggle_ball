using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExitButton : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] private bool _disableOnIos;

    private void Awake()
    {
        if (_disableOnIos)
        {
            gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Application.Quit();
    }
}
