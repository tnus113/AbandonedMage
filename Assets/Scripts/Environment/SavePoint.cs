using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
	public ParticleSystem saveEffect;
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			DataPersistenceManager.instance.SaveGame();
			saveEffect.Play();
			SoundManager.Instance.PlaySound3D("Checkpoint", transform.position);
		}
	}
}
