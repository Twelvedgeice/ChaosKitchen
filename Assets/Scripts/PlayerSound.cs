using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private Player player;
    private float footTimer;
    private float footTimerMax = 0.1f;

    private float volme = 5f;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        footTimer -= Time.deltaTime;
        if(footTimer < 0f)
        {
            footTimer = footTimerMax;
            if (player.isWalking())
            {
                SoundManager.Instance.PlayFootstepSound(player.transform.position, volme);
            }
        }
    }
}
