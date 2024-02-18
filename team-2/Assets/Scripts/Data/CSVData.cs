using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class CSVData
{

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

        for (int i = 1; i < lines.Length; i++)
        {   // �࿡�� ��(Ư�� : �̺�Ʈ �ε���, �̸�, ����)���� �����ش�.
            var values = Regex.Split(lines[i], SPLIT_RE);

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