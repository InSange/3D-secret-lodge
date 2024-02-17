using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

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

public struct TextData
{
    public string name;
    public string text;

    public TextData(string name, string text)
    {
        this.name = name;
        this.text = text;
    }
}

public class CSVReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static Dictionary<int, List<TextData>> LoadCSVData(string path)
    {   // ������ ���ҽ� �ҷ�����
        TextAsset data = Resources.Load(path) as TextAsset;
        if (!data) return null;
        // row���� �����ش�.
        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        // �����ؼ� �����͸� ������ ��ųʸ�
        Dictionary<int, List<TextData>> finalData = new Dictionary<int, List<TextData>>();
        List<TextData> textDatas = new List<TextData>();
        string name = "";
        string text = "";
        int index = 0;

        for (int i =1; i < lines.Length; i++)
        {   // �࿡�� ��(Ư�� : �̺�Ʈ �ε���, �̸�, ����)���� �����ش�.
            var values = Regex.Split(lines[i], SPLIT_RE);

            if(values[2] == "")
            {
                finalData[index] = textDatas;
                Debug.Log(index + ", " + finalData.Keys.Count);
                continue;
            }
            if (values[0] != "") index = int.Parse(values[0]);
            if (values[1] != "") name = values[1];

            textDatas.Add(new TextData(name, values[2]));
            Debug.Log(textDatas[textDatas.Count - 1].name + ", " + textDatas[textDatas.Count - 1].text);
        }

        foreach(var d in finalData.Keys)
        {
            for(int i = 0; i < finalData[d].Count; i++)
            {
                Debug.Log(finalData[d][i].name + " : " + finalData[d][i].text);
            }
        }

        return finalData;
    }
}