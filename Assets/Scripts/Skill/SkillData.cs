using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Data/Skill Data")]
public class SkillData : ScriptableObject
{
    public GameObject skillPrefab;
	public string skillName;
    public string soundEffectName;
	public PlayerCommand commandType;
	public float damage;
    public float cooldownTime;
}
