using UnityEngine;
using UnityEngine.SceneManagement;

public class Gateway : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.ChangeScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
