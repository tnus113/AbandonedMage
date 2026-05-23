using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;
    public List<string> equippedSkillNames;

	public GameData()
    {
        playerPosition = new Vector3(457.7801f, 14.49278f, 296.9763f);
        equippedSkillNames = new List<string>(){};
	}
}
