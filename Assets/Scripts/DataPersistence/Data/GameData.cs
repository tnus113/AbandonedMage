using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;
    public List<string> equippedSkillNames;
    public string lastSceneName;

	public GameData()
    {
        playerPosition = new Vector3(426.55f, 15.36f, 285.32f);
        equippedSkillNames = new List<string>(){};
        lastSceneName = "GameScene";
	}
}
