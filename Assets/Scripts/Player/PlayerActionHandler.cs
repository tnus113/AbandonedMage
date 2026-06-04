using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionHandler : MonoBehaviour, IDataPersistence
{
	public Transform firepoint;
	[Header("Unlocked Skills")]
	public List<SkillData> equippedSkills;

	[Header("All Skills Database")]
	[SerializeField] private List<SkillData> allSkills;

	private Dictionary<string, float> skillCooldown = new Dictionary<string, float>();

	private void Start()
	{
		if (VoiceManager.Instance != null)
		{
			VoiceManager.Instance.OnCommandRecognized += HandleCommand;
		}
	}

	public void LoadData(GameData data)
	{
		equippedSkills.Clear();
		foreach (string name in data.equippedSkillNames)
		{
			SkillData skill = allSkills.Find(s => s.skillName == name);
			if (skill != null)
			{
				equippedSkills.Add(skill);
			}
		}
	}

	public void SaveData(ref GameData data)
	{
		data.equippedSkillNames.Clear();
		foreach (SkillData skill in equippedSkills)
		{
			if (skill != null && !string.IsNullOrEmpty(skill.skillName))
			{
				data.equippedSkillNames.Add(skill.skillName);
			}
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			PlayerCommand cmd = PlayerCommand.Attack;
			HandleCommand(cmd);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			PlayerCommand cmd = PlayerCommand.WindShield;
			HandleCommand(cmd);
		}
	}

	private void HandleCommand(PlayerCommand command)
	{
		SkillData skillToCast = null;
		foreach (SkillData skill in equippedSkills)
		{
			if (skill != null && skill.commandType == command)
			{
				skillToCast = skill;
				break;
			}
		}
		if (skillToCast != null)
		{
			TryCastSkill(skillToCast);
		}
	}

	private void TryCastSkill(SkillData skill)
	{
		if (skillCooldown.ContainsKey(skill.commandType.ToString()))
		{
			if (Time.time < skillCooldown[skill.commandType.ToString()])
			{
				Debug.Log("chieu chua hoi!");
				return;
			}
		}
		skillCooldown[skill.commandType.ToString()] = Time.time + skill.cooldownTime;
		if (skill.skillPrefab != null && firepoint != null)
		{
			SoundManager.Instance.PlaySound3D(skill.soundEffectName, firepoint.position);
			GameObject spawnedSkill = Instantiate(skill.skillPrefab, firepoint.position, firepoint.rotation);
			if (skill.commandType == PlayerCommand.WindShield)
			{
				spawnedSkill.transform.SetParent(this.transform);
				spawnedSkill.transform.localPosition = new Vector3(0f, 1f, 0f);
			}
		}
	}

	private void OnDestroy()
	{
		if (VoiceManager.Instance != null)
		{
			VoiceManager.Instance.OnCommandRecognized -= HandleCommand;
		}
	}

	public void EquipSkill(SkillData skill)
	{
		if (skill != null && !equippedSkills.Contains(skill))
		{
			equippedSkills.Add(skill);
		}
	}

	public void UnlockSkill(string skillName)
	{
		SkillData skill = allSkills.Find(s => s.skillName == skillName);
		if (skill != null)
		{
			EquipSkill(skill);
			DataPersistenceManager.instance?.SaveGame();
		}
		else
		{
			Debug.LogWarning($"Skill {skillName} not found in allSkills database.");
		}
	}
}
