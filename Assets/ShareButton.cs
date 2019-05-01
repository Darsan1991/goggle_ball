using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShareButton : MonoBehaviour,IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        var url = Application.platform == RuntimePlatform.Android ? $"https://play.google.com/store/apps/details?id={Application.identifier}" :
            $"http://itunes.apple.com/app/id{GameSettings.Default.IosAppId}";
        NativeShare.Share($"Hey, Check out this Awesome Game\n{url}");
    }
}
