using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSFX : MonoBehaviour
{
    public void CloseAttack()
    {
        SoundManager.Instance.PlaySound3D("CloseAttack", transform.position);
	}
}
