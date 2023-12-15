using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class BackgroundSoundController : MonoBehaviour
{
    [SerializeField] EventReference backgroundSound;
    EventInstance backgroundSoundInstance;
    void Start()
    {
        backgroundSoundInstance = AudioController.Instance.CreateInstance(backgroundSound);
        backgroundSoundInstance.start();
    }

    private void OnDestroy()
    {
        backgroundSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}