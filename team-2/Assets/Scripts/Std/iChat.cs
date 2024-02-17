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
    {   // 데이터 리소스 불러오기
        TextAsset data = Resources.Load(path) as TextAsset;
        if (!data) return null;
        // row별로 나눠준다.
        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        // 저장해서 데이터를 보내줄 딕셔너리
        Dictionary<int, List<TextData>> finalData = new Dictionary<int, List<TextData>>();
        List<TextData> textDatas = new List<TextData>();
        string name = "";
        string text = "";
        int index = 0;

        for (int i =1; i < lines.Length; i++)
        {   // 행에서 열(특성 : 이벤트 인덱스, 이름, 내용)별로 나눠준다.
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