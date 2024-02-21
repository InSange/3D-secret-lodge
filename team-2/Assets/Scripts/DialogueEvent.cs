using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEvent : MonoBehaviour
{
    [SerializeField] EventDialogue _event;
    [SerializeField] bool isOneTime;

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
            UIManager.Instance.StartDialogue(_event);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
}
