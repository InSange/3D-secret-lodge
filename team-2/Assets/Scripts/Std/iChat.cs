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
            nameText.text = "�����";
            talkText.text = "�ź�ο� ���� ���ؼ� ���� ������ �ʴ´�. ������ ã�ƺ���.";
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
            talkText.text = "���� ������ �ʴ´�...";
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
        talkText.text = "�ȳ�? ���� ��ȭ���� ������?";
        talkPanel.SetActive(true);
        StartCoroutine(TextPanelOut());
    }

    public void TutorialMessage()
    {
        GameManager.instance.player.isLoading = true;
        nameText.text = "Player";
        talkText.text = "����� �ϴ� ���߿� ���� �Ҿ������ �� ���� �����Ѵ�.";
        talkPanel.SetActive(true);
        Invoke("nextTutorialMessage", 2.0f);
    }

    void nextTutorialMessage()
    {
        talkText.text = "�� ���� ���� ã�ƺ���.";
        StartCoroutine(TextPanelOut());
    }

    public void SetTextPanel(GameObject scanObject)
    {
        string content = npc_Cat.GetContent(talkId, contentNum);
        Debug.Log("content : " + content);
        if (content == null)
        {
            Debug.Log("����Ʈ����");
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
            Debug.Log("����Ʈ�־�");
            isAction = true;
            scanOBJ = scanObject;
            nameText.text = scanOBJ.name;
            talkText.text = content;
            contentNum++;
        }

        talkPanel.SetActive(isAction);
    }*/
}
