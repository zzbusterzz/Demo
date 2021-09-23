using UnityEngine;
using UnityEngine.UI;

public class MenuUIControl : MonoBehaviour
{
    public GameObject menuUI;
    public Button[] menuButtons;//0 button is resume button which we can disable and enable

    public void Start()
    {
        for(int i = 0; i < menuButtons.Length; i++)
        {
            GameObject btn = menuButtons[i].gameObject;
            Debug.Log(btn.name);
            menuButtons[i].onClick.AddListener(()=> OnUIButtonClick(btn));
        }

        ToggleMenuUI(true);
    }

    public void UpdateResumeButtonState(bool enable)
    {
        menuButtons[0].gameObject.SetActive(enable);
    }

    public bool GetResumeButtonState()
    {
        return menuButtons[0].gameObject.activeInHierarchy;
    }

    private void OnUIButtonClick(GameObject button)
    {
        switch (button.name)
        {
            case "New Game":
                //Call New Game here and reset game
                GameControl.instance.NewGame();
            break;

            case "Resume":
                //Resume the current game
                GameControl.instance.ResumeGame();
                break;

            case "Quit":
                Application.Quit();
                break;
        }            
    }

    public void ToggleMenuUI(bool enableUI)
    {
        menuUI.SetActive(enableUI);
    }
}
