using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueEvent : MonoBehaviour
{
    [SerializeField] EventDialogue _event;
    [SerializeField] bool isOneTime;

    // Camera
    [SerializeField] GameObject eventCamera;

    public void SetEvent(EventDialogue e)
    {
        _event = e;
    }

    public EventDialogue GetEvent()
    {
        return _event;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(eventCamera != null)
            {
                GameManager.Instance.canInput = false;
                PlayableDirector pd = eventCamera.GetComponent<PlayableDirector>();
                pd.stopped += OffCamera;
                eventCamera.SetActive(true);
            }
            else
            {
                UIManager.Instance.StartDialogue(_event);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }

    void OffCamera(PlayableDirector pd)
    {
        eventCamera.SetActive(false);
        UIManager.Instance.StartDialogue(_event);
    }
}
