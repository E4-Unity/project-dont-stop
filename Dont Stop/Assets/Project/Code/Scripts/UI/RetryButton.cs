using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    public void Retry()
    {
        SceneLoadingManager.Instance.ChangeScene(SceneManager.GetActiveScene().buildIndex);
    }
}
