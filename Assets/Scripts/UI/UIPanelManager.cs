using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

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

    private string currentActivePanelName;

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
                    currentActivePanelName = panel.name;
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

        currentActivePanelName = null;
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

    public string GetCurrentActivePanel()
    {
        return currentActivePanelName;
    }

    public void Quit()
    {
#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
        }
#else
        Application.Quit();
#endif
    }
}
