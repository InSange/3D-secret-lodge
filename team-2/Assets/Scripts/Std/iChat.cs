using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iChat : MonoBehaviour
{
    public static void ChatOpen()
    {

    }

    /*public void narration(GameObject OBJ = null)
    {
        if (OBJ.CompareTag("Door"))
        {
            nameText.text = "도우미";
            talkText.text = "신비로운 힘에 의해서 문이 열리지 않는다. 유물을 찾아보자.";
            talkPanel.SetActive(true);
            StartCoroutine(TextPanelOut());
        }
    }

    public void PlayerText(GameObject scanObject)
    {
        if (scanObject.CompareTag("Broken_Door") && isAction == false)
        {
            isAction = true;
            nameText.text = "Player";
            talkText.text = "문이 열리지 않는다...";
        }
        else
        {
            isAction = false;
        }
        talkPanel.SetActive(isAction);
    }

    public void StartMessage()
    {
        nameText.text = "NPC_CAT";
        talkText.text = "안녕? 나랑 대화하지 않을래?";
        talkPanel.SetActive(true);
        StartCoroutine(TextPanelOut());
    }

    public void TutorialMessage()
    {
        GameManager.instance.player.isLoading = true;
        nameText.text = "Player";
        talkText.text = "등산을 하던 와중에 길을 잃어버리고 비도 오기 시작한다.";
        talkPanel.SetActive(true);
        Invoke("nextTutorialMessage", 2.0f);
    }

    void nextTutorialMessage()
    {
        talkText.text = "비를 피할 곳을 찾아보자.";
        StartCoroutine(TextPanelOut());
    }

    public void SetTextPanel(GameObject scanObject)
    {
        string content = npc_Cat.GetContent(talkId, contentNum);
        Debug.Log("content : " + content);
        if (content == null)
        {
            Debug.Log("콘텐트없어");
            if (talkId >= 2)
            {
                if (isInformationAction == false)
                {
                    isInformationAction = true;
                    SetInformationPanel();
                }
                else
                {
                    GetInformation(informationId, informationNum);
                }
                return;
            }
            contentNum = 0;
            talkId = talkId >= 2 ? 2 : talkId + 1;
            isAction = false;
        }
        else
        {
            Debug.Log("콘텐트있어");
            isAction = true;
            scanOBJ = scanObject;
            nameText.text = scanOBJ.name;
            talkText.text = content;
            contentNum++;
        }

        talkPanel.SetActive(isAction);
    }*/
}
