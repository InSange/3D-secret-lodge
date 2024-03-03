using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GameData
{
    // ���� �� ȿ������
    public float bgm, sfx;
    public bool tutorial;   // Clear StartMap
    public bool visitedHall;    // check visited in hall
    public bool clearJumpMap;   // check jumpMap clear
    public bool clearMaze;  // check jumpMap clear
    public bool clearTreasure;  // check treasureMap clear
    public bool clearTrap;  // check trapMap clear
}