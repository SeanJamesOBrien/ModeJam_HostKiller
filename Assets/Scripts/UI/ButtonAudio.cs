using FMODUnity;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.VFX;

[RequireComponent(typeof(Button))]
public class ButtonAudio : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] EventReference hoverSound;
    [SerializeField] EventReference clickSound;
    Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        if (button.interactable)
        {
            AudioController.Instance.PlayOneShot(clickSound, transform.position);
        }
    }

    //Detect if the Cursor starts to pass over the GameObject
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (button.interactable)
        {
            AudioController.Instance.PlayOneShot(hoverSound, transform.position);
        }
    }
}
