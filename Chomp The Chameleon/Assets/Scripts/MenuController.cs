using UnityEngine;

public class MenuController : MonoBehaviour
{
    [System.Serializable]
    public class MenuGroup
    {
        public string name;
        public GameObject panel;
    }

    public MenuGroup[] menus;

    public void ShowMenu(string menuName)
    {
        foreach (var menu in menus)
        {
            menu.panel.SetActive(menu.name == menuName);
        }
    }

    public void HideAllMenus()
    {
        foreach (var menu in menus)
        {
            menu.panel.SetActive(false);
        }
    }
}
