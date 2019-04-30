using UnityEngine;
using UnityEngine.EventSystems;

public class PlaySoundOnClick:MonoBehaviour,IPointerClickHandler
{
    [Tooltip("Leave Empty to Play Default")]
    [SerializeField] private AudioClip _overrideClip;


    public void OnPointerClick(PointerEventData eventData)
    {
        if (AudioManager.IsSoundEnable)
        {
            AudioSource.PlayClipAtPoint(_overrideClip!=null ? _overrideClip : AudioManager.DefaultClickClip,Camera.main?.transform.position ?? Vector3.zero);
        }
    }
}