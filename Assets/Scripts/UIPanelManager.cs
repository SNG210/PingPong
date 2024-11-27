using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UiPanelManager : MonoBehaviour
{
    public static UiPanelManager Instance;

    [System.Serializable]
    public struct Panel
    {
        public string name;        
        public GameObject panel;
        public UnityEvent onActivate; 
        public bool unlockCursor;     
    }

    [Header("Panels Configuration")]
    [SerializeField] private List<Panel> panels = new List<Panel>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ActivatePanel("MainMenu");
    }

    public void ActivatePanel(string panelName)
    {
        bool panelFound = false;

        foreach (Panel panel in panels)
        {
            if (panel.panel != null)
            {
                if (panel.name == panelName)
                {
                    panel.panel.SetActive(true);
                    InvokePanelEvent(panel); 
                    SetCursorState(panel.unlockCursor); 
                    panelFound = true;
                }
                else
                {
                    panel.panel.SetActive(false);
                }
            }
        }

        if (!panelFound)
        {
            Debug.LogWarning($"Panel with name '{panelName}' not found!");
        }
    }

    public void DeactivateAllPanels()
    {
        foreach (Panel panel in panels)
        {
            if (panel.panel != null)
            {
                panel.panel.SetActive(false);
            }
        }
    }

    private void InvokePanelEvent(Panel panel)
    {
        panel.onActivate?.Invoke();
    }

    private void SetCursorState(bool unlockCursor)
    {
        Cursor.lockState = unlockCursor ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = unlockCursor;
    }


    public void Quit()
    {
        Application.Quit();
    }

}
