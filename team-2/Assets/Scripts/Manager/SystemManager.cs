using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemManager : MonoBehaviour
{
    // 기본 캔버스
    [SerializeField] GameObject canvas;

    // 시스템 매니저는 각종 UI 및 음향을 담당.
    public GameObject scanOBJ;
    public NPC npc_Cat;

    public void MainMenuButton()
    {
        GameManager.instance.PauseFunc();
      //  Destroy(GameManager.instance.player.gameObject);
     //   mainMenu_Panel.SetActive(true);
    //    MainMenu.gameObject.SetActive(true);
        Cursor.visible = true;
    }

    /*public void SetInformationPanel()
    {
        isSelectInformation = true;
        Cursor.visible = true;
        infoPanel.SetActive(true);
    }

    public void GetInformation(int information_id, int information_num)
    {
        string information = npc_Cat.GetInformation(information_id, information_num);

        if(information == null)
        {
            isInformationAction = false;
            informationNum = 0;
            informationId = -1;
            isAction = false;
            contentNum = 0;

            talkPanel.SetActive(isAction);
        }
        else
        {
            talkText.text = information;
            informationNum++;
        }
    }

    public void MazeInformation()
    {
        Cursor.visible = false;
        informationId = 0;
        infoPanel.SetActive(false);
        isSelectInformation = false;
        GetInformation(informationId, informationNum);
    }

    public void JumpInformation()
    {
        Cursor.visible = false;
        informationId = 1;
        infoPanel.SetActive(false);
        isSelectInformation = false;
        GetInformation(informationId, informationNum);
    }

    public void TreasureInformation()
    {
        Cursor.visible = false;
        informationId = 2;
        infoPanel.SetActive(false);
        isSelectInformation = false;
        GetInformation(informationId, informationNum);
    }

    public void RCPInformation()
    {
        Cursor.visible = false;
        informationId = 3;
        infoPanel.SetActive(false);
        isSelectInformation = false;
        GetInformation(informationId, informationNum);
    }

    public void BossInformation()
    {
        Cursor.visible = false;
        informationId = 4;
        infoPanel.SetActive(false);
        isSelectInformation = false;
        GetInformation(informationId, informationNum);
    }

    IEnumerator TextPanelOut()
    {
        GameManager.instance.player.isLoading = true;
        yield return new WaitForSeconds(2.0f);
        talkPanel.SetActive(false);
        GameManager.instance.player.isLoading = false;
    }*/
}
