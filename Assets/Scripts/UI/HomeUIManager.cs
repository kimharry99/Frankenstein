﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeUIManager : SingletonBehaviour<HomeUIManager>
{
    [Header("Time UI")]
    public GameObject textDay;
    public GameObject textTime;

    [Header("Energy UI")]
    public GameObject sliderEnergy;

    [Header("Sub Panels")]
    public GameObject panelSetting;
    public GameObject panelHome;
    public GameObject panelExploration;
    public GameObject panelResearch;
    public GameObject panelDisassemble;
    public GameObject panelCrafting;
    public GameObject panelAssemble;
    public GameObject panelStorage;

    // for debugging
    public void ClickedSendTime()
    {
        GameManager.Inst.SendTime(1);
    }

    #region HomePanel methods
    public void ButtonSettingClicked()
    {
        panelHome.SetActive(false);
        panelSetting.SetActive(true);
    }

    public void ButtonExploreClicked()
    {
        SceneManager.LoadScene(1);

    }

    public void ButtonResearchClicked()
    {
        panelHome.SetActive(false);
        panelResearch.SetActive(true);
    }
    public void ButtonDisassembleClicked()
    {
        panelHome.SetActive(false);
        panelDisassemble.SetActive(true);
    }

    public void ButtonCraftClicked()
    {
        panelHome.SetActive(false);
        panelCrafting.SetActive(true);
    }

    public void ButtonAssembleClicked()
    {
        panelHome.SetActive(false);
        panelAssemble.SetActive(true);
    }

    public void ButtonStorageClicked()
    {
        panelHome.SetActive(false);
        panelStorage.SetActive(true);
    }
    #endregion

    // Parameter panel means a panel to be closed.
    public void ButtonCloseClicked(GameObject panel)
    {
        panel.SetActive(false);
        panelHome.SetActive(true);
    }

    #region CraftingPanel methods
    public void ButtonItemClicked()
    {
        print("ButtonItemClicked");
    }

    public void ButtonCreateClicked()
    {
        print("ButtonCreateClicked");
    }
    #endregion

    #region AssemblePanel methods
    public void ButtonDeadBodyClicked()
    {
        print("ButtonDeadBodyClicked");
    }

    public void ButtonEquipClicked()
    {
        print("ButtonEquipClicked");
    }
    #endregion
}
