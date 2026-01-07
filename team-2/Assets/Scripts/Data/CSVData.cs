using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class CSVData
{

}

/// <summary>
/// 대화 시스템에 필요한 부분은 주체자와 대화 내용이다.
/// 주체자는 name에다가 저장하고, 대화 내용은 text에 저장하여 하나의 대화 컨텐츠를 구성한다.
/// </summary>
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
{/// <summary>
/// CSV파일은 ,로 열이 구분되고 행들은 줄 바꿈 문자에 의해 구분이된다.
/// </summary>
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

        for (int i = 1; i < lines.Length; i++)
        {   // 행에서 열(특성 : 이벤트 인덱스, 이름, 내용)별로 나눠준다.
            var values = Regex.Split(lines[i], SPLIT_RE);
            // 두번째 공간(내용)이 비워져있으면 해당 대화는 끝난 것이다.
            // 여태까지 저장해온 리스트 데이터를 딕셔너리 finalData에다가 넣어준다.
            if (values[2] == "")
            {
                finalData[index] = textDatas;
                textDatas = new List<TextData>();
                //Debug.Log(index + ", " + finalData.Keys.Count);
                continue;
            }
            if (values[0] != "") index = int.Parse(values[0]);
            if (values[1] != "") name = values[1];

            textDatas.Add(new TextData(name, values[2]));
            //Debug.Log(textDatas[textDatas.Count - 1].name + ", " + textDatas[textDatas.Count - 1].text);
        }

        return finalData;
    }
}