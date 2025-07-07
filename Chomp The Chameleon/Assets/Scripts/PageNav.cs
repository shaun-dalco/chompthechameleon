using UnityEngine;

public class PageNav : MonoBehaviour
{
    public GameObject next;
    public GameObject back;

    public GameObject page1;
    public GameObject page2;

    public void NextPage() {
        next.SetActive(false);
        back.SetActive(true);

        page1.SetActive(false);
        page2.SetActive(true);
    }

    public void BackPage() {
        next.SetActive(true);
        back.SetActive(false);

        page1.SetActive(true);
        page2.SetActive(false);
    }
}