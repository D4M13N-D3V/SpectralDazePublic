using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreExit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("ShopManager").SendMessage("ExitShop");
        }
    }
}
