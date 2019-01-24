using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SpectralDaze.Store;
public class StoreEntrance : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(LoadShop());
        }
    }

    IEnumerator LoadShop()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Shop_Scene", LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        GameObject.FindGameObjectWithTag("ShopManager").GetComponent<StoreManager>().UsedEntrance = this;
    }

    public void UnloadShop()
    {
        StartCoroutine(UnLoadShop());
    }

    IEnumerator UnLoadShop()
    {
        AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync("Shop_Scene");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
