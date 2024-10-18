using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDetection : MonoBehaviour
{


    public Zombie zombie;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (this.gameObject.activeSelf && other.TryGetComponent<Health>(out Health health))
            {

                //AudioManager.insta.playSound(UnityEngine.Random.Range(16, 19));  //Play Damage Audio
                health.ChangeHealth(-zombie.Damage);
                this.gameObject.SetActive(false);
            }
        }

        if (other.CompareTag("car") && CommonReferences.Instance._pState==PlayerState.CAR)
        {

            if (CommonReferences.Instance.myPlayer.TryGetComponent<Health>(out Health health))
            {

                //AudioManager.insta.playSound(UnityEngine.Random.Range(16, 19));  //Play Damage Audio
                health.ChangeHealth(-zombie.Damage);
                this.gameObject.SetActive(false);
            }
        }
    }

}
