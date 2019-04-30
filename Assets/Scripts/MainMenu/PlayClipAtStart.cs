using UnityEngine;

public class PlayClipAtStart : MonoBehaviour
{
    [SerializeField] private AudioClip _clip;


    private void Start()
    {
        AudioSource.PlayClipAtPoint(_clip,Camera.main?.transform.position ?? Vector3.zero);
    }
}