using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject goldCoin, healthGlobe, staminaGlobe;

    public void DropItems() {
        int randNum = Random.Range(1, 5);

        if (randNum == 1){
            int randomAmountOfGold = Random.Range(1, 3);
            for (int i=0; i< randomAmountOfGold; i++){
                Instantiate(goldCoin, transform.position, Quaternion.identity);
            }
        }
        
        if (randNum == 2) {
            Instantiate(healthGlobe, transform.position, Quaternion.identity);
        }

        if (randNum == 3) {
            Instantiate(staminaGlobe, transform.position, Quaternion.identity);
        }

    }
}
