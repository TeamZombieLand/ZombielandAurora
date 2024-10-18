using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundDetection : MonoBehaviour
{
    [SerializeField] float timer;
    [SerializeField] float currenttimer;
    [SerializeField] Material material;
    private void Start()
    {
        material = this.GetComponent<Renderer>().material;
    }
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            StopCoroutine("GlowEffect");
            StartCoroutine("GlowEffect");
        }
    }

    IEnumerator GlowEffect()
    {
        currenttimer = timer;
        while (currenttimer > 0)
        {
            material.SetFloat("EdgeSize", currenttimer);
            currenttimer -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        while (currenttimer <= timer)
        {
            material.SetFloat("EdgeSize", currenttimer);
            currenttimer += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

    }
}
