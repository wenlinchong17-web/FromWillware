using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;

    public GameObject notificationPrefab;
    public Transform notificationRoot;

    private void Awake()
    {
        Instance = this;
    }

    public void Show(string message)
    {
        GameObject obj =
            Instantiate(notificationPrefab, notificationRoot);

        NotificationItem item =
            obj.GetComponent<NotificationItem>();

        item.Init(message);
    }
}