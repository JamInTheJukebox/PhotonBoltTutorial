using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_Controller : MonoBehaviour
{
    #region Singleton

    private static GUI_Controller _instance = null;

    public static GUI_Controller current
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GUI_Controller>();
            return _instance;
        }

    }
    #endregion

    private void Start()
    {
        ShowHealth(false);
    }

    [SerializeField]
    private UI_Healthbar _Healthbar = null;

    public void ShowHealth(bool active)
    {
        _Healthbar.gameObject.SetActive(active);
    }

    public void UpdateLife(int current, int total)
    {
        _Healthbar.UpdateLife(current, total);
    }
}
