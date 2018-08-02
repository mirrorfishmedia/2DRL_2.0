using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class LinkedTeleporter : MonoBehaviour {


    public Transform linkedPosition;
    public SoundEffect teleportSoundEffect;
    public float teleportCooldown = .1f;

    public LinkedTeleporter linkedTeleporter;
    public float nextTime;
    


    private void OnCollisionEnter2D(Collision2D collision)
    {
        DoTeleport(collision.gameObject);



    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        DoTeleport(col.gameObject);
    }

    void DoTeleport(GameObject teleportThis)
    {
        if (Time.time > nextTime)
        {
            
            GameMan.gm.soundMan.PlaySoundEffect(teleportSoundEffect);
            nextTime = Time.time + teleportCooldown;
            linkedTeleporter.nextTime = nextTime;

            AILerp aiLerp = teleportThis.GetComponent<AILerp>();
            if (aiLerp != null)
            {
                aiLerp.Teleport(linkedPosition.transform.position, true);
            }

            teleportThis.transform.position = linkedPosition.transform.position;
            Debug.Log("teleport " + teleportThis.name);

        }
    }

}
