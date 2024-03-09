using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPC_TYPE
{
    Cat = 0,
    Boy = 200,
    Man = 300,
    Dwarf = 400,
    Robin = 500,
    Woman = 600
}

public class NPC : MonoBehaviour
{
    [SerializeField] NPC_TYPE type;

    public void Talk()
    {
        switch (type)
        {
            case NPC_TYPE.Cat:
                UIManager.Instance.CatTalk();
                break;
            default:
                int npcTalkNumber = (int)type + Random.Range(0, 3);
                UIManager.Instance.StartDialogue((EventDialogue)npcTalkNumber);
                break;
        }
    }

    public void SetType(NPC_TYPE t)
    {
        type = t;
    }
}
