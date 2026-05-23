using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Flamethrower : MonoBehaviour
{
    public ParticleSystem flamethrower;

	public void StartFlamethrower()
    {
        SoundManager.Instance.PlaySound3D("Flamethrower", transform.position);
		flamethrower.Play();
	}
    public void StopFlamethrower()
    {
        flamethrower.Stop();
	}
}
