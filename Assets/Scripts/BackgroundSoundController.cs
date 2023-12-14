using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class BackgroundSoundController : MonoBehaviour
{
    [SerializeField] EventReference backgroundSound;
    void Start()
    {
        EventInstance backgroundSoundInstance = AudioController.Instance.CreateInstance(backgroundSound);
        backgroundSoundInstance.start();
    }
}