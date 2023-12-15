using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class Awareness : MonoBehaviour
{
    EnemyMovement enemyMovement;
    [SerializeField] EventReference runningSound;
    EventInstance runningInstance;

    private void Awake()
    {
        enemyMovement = GetComponentInParent<EnemyMovement>();
        runningInstance = AudioController.Instance.CreateInstance(runningSound);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(K.PlayerLayer))
        {
            runningInstance.start();
            enemyMovement.IsAware = true;
        }
    }

    private void OnDestroy()
    {
        runningInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}