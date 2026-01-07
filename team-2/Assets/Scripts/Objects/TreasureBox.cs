using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBox : MonoBehaviour
{/// <summary>
/// 보물찾기 맵에 쓰이는 보물상자 오브젝트의 컴포넌트이다.
/// 해당 보물 상자가 아티팩트를 가지고 있는지 판별하기 위해 bool형과 담고있는 오브젝트를 변수로 담아주었다.
/// </summary>
    [SerializeField] bool haveArtifact; // 두번째 페이즈가 이벤트함수로 처리되면서 사용하지 않음.
    [SerializeField] GameObject boxContent;
    /// <summary>
    /// 플레이어가 박스를 열면! 박스 안에 있던 오브젝트를 활성화 시켜준다!
    /// </summary>
    public void OpenBox()
    {
        this.gameObject.SetActive(false);
        if(boxContent != null) boxContent.SetActive(true);
    }
    /// <summary>
    /// 상자가 가지고 있는 오브젝트 설정!
    /// </summary>
    /// <param name="flag"></param>
    /// <param name="obj"></param>
    public void SetHaveArtifact(bool flag, GameObject obj)
    {
        haveArtifact = flag;
        boxContent = obj;
    }
}
