using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsManagement : MonoBehaviour
{
    [SerializeField] float resetTime;
    [SerializeField] float currentTime;
           
    [Header("Power Objects")]
    [SerializeField] GameObject healthPickupObj;    
    [SerializeField] GameObject fuelPickUpObj;


    [SerializeField] LayerMask groundLayer;

   
    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
          
            ResetPowers();
        }
    }
    internal void ResetPowers()
    {
        currentTime = resetTime;


        int selectedLevel = PlayerPrefs.GetInt("selectedLevel", 0);
        if (!healthPickupObj.activeSelf)
        {
            healthPickupObj.SetActive(true);

            Vector3 poz = new Vector3(Random.Range(140f, 280), 20, Random.Range(150, 250));

            if (selectedLevel == 0)
            {
                poz = new Vector3(Random.Range(140f, 280), 20, Random.Range(150, 250));
            }
            else
            {
                poz = new Vector3(Random.Range(135f, 350f), 20, Random.Range(85, 360));
            }

            if (Physics.Raycast(poz, Vector3.down, out RaycastHit hit, groundLayer))
            {
                poz = hit.point;
                poz.y += 1f;
                healthPickupObj.transform.position = poz;
            }
            else
            {
                poz.y = 1f;
                healthPickupObj.transform.position = poz;
            }
        }

        if (!fuelPickUpObj.activeSelf)
        {
            fuelPickUpObj.SetActive(true);

            Vector3 poz = new Vector3(Random.Range(140f, 280), 20, Random.Range(150, 250));

            if (selectedLevel == 0)
            {
                poz = new Vector3(Random.Range(140f, 280), 20, Random.Range(150, 250));
            }
            else
            {
                poz = new Vector3(Random.Range(135f, 350f), 20, Random.Range(85, 360));
            }

            if (Physics.Raycast(poz, Vector3.down, out RaycastHit hit, groundLayer))
            {
                poz = hit.point;
                poz.y += 1f;
                fuelPickUpObj.transform.position = poz;
            }
            else
            {
                poz.y = 1f;
                fuelPickUpObj.transform.position = poz;
            }
        }
    }
}
