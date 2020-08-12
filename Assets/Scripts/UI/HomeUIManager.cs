﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeUIManager : SingletonBehaviour<HomeUIManager>
{
    [Header("Sub Panels")]
    public GameObject panelHome;
    public GameObject panelResearch;
    public GameObject panelDisassemble;
    public GameObject panelCrafting;
    public GameObject panelAssemble;
    public GameObject panelChest;
    public GameObject panelNotice;

    [Header("Inventory")]
    public Button[] imageChestSlot;
    public Sprite emptyImage;

    public Chest chest;

    [Header("Disassemble UI")]
    public Image[] imageDisassembleUsing;
    public Image[] imageDisassembleHolding;
    public GameObject[] imageCheck;
    public Text textDisassembleEnergy;
    public Scrollbar scrollbarDisassemble;
    public int[] indexHoldingChest;
    public int[] indexUsingHolding;

    [Header("Notice")]
    public Text textNotice;

    #region chest methods
    public void UpdateChest(int slotNumber)
    {
        if (chest.slotItem[slotNumber] != null)
            imageChestSlot[slotNumber].image.sprite = chest.slotItem[slotNumber].itemImage;
        else
            imageChestSlot[slotNumber].image.sprite = emptyImage;
    }

    public void InitialUpdateChest()
    {
        chest = StorageManager.Inst.chests[0];
        for (int i = 0; i < chest.slotItem.Length; i++)
        {
            if (chest.slotItem[i] != null)
                imageChestSlot[i].image.sprite = chest.slotItem[i].itemImage;
            else
                imageChestSlot[i].image.sprite = emptyImage;
        }
    }
    public void SortItem(int num)
    {
        chest = StorageManager.Inst.chests[num];
        for (int i = 0; i < chest.slotItem.Length; i++)
        {
            if (chest.slotItem[i] != null)
                imageChestSlot[i].image.sprite = chest.slotItem[i].itemImage;
            else
                imageChestSlot[i].image.sprite = emptyImage;
        }
    }
    #endregion

    #region HomePanel methods

    public void ButtonExploreClicked()
    {
        GameManager.Inst.StartExploration();
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

        UpdateDisassemble();
        scrollbarDisassemble.value = 1;
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

    public void ButtonChestClicked()
    {
        panelHome.SetActive(false);
        panelChest.SetActive(true);
    } 
    // Parameter panel means a panel to be closed.
    public void ButtonCloseClicked(GameObject panel)
    {
        panel.SetActive(false);
        panelHome.SetActive(true);
    }

    #endregion


    public void PanelNoticeClicked()
    {
        panelNotice.SetActive(false);
    }

    #region DisassemblePanel methods
    public int disassembleEnergy = 0;
    public void UpdateDisassemble()
    {
        int i = 0, j = 0;

        for (; i < chest.slotItem.Length; i++)
        {
            if (chest.slotItem[i] != null && chest.slotItem[i].type == 0)
            {
                imageDisassembleHolding[j].sprite = chest.slotItem[i].itemImage; // Update Corpse at the jth Holding Slot
                indexHoldingChest[j] = i; // Record the Index of Corpse (Holding Slot to Chest Slot)

                imageCheck[j].SetActive(false);

                if (j < 6) // Reset the jth Using Slot
                {
                    imageDisassembleUsing[j].sprite = emptyImage;
                    indexUsingHolding[j] = -1;
                }

                j++;
            }

            if (j > 29) // Prevent the Overflow (Now It's not Needed)
                break;
        }
        // Done Updating Corpse

        while (j <= 29) // Reset the Remaining Slots
        {
            imageDisassembleHolding[j].sprite = emptyImage;
            indexHoldingChest[j] = -1;

            imageCheck[j].SetActive(false);

            if (j < 6)
            {
                imageDisassembleUsing[j].sprite = emptyImage;
                indexUsingHolding[j] = -1;
            }

            j++;
        }

        disassembleEnergy = 0;
        textDisassembleEnergy.text = "추출 에너지 [ " + disassembleEnergy.ToString() + " ]";
    }

    public void DisassembleButtonHoldingClicked(int slotNumber)
    {
        if (imageCheck[slotNumber].activeSelf == false && imageDisassembleHolding[slotNumber].sprite != emptyImage) // The Case that Slot not been Selected Before
        {
            int i = 0;
            while (i < 6 && imageDisassembleUsing[i].sprite != emptyImage) // Find Empty Slot in 'Panel Using Items'
                i++;

            if (i < 6)
            {
                imageCheck[slotNumber].SetActive(true); // Mark the Selected Slot

                imageDisassembleUsing[i].sprite = chest.slotItem[indexHoldingChest[slotNumber]].itemImage; // Update Corpse at the Using Slot
                indexUsingHolding[i] = slotNumber; // Record the Index of Corpse (Using Slot to Holding Slot)

                disassembleEnergy += chest.slotItem[indexHoldingChest[slotNumber]].energyPotential;
                textDisassembleEnergy.text = "추출 에너지 [ " + disassembleEnergy.ToString() + " ]";

                Debug.Log(slotNumber + "select;" +
                          "    +" + chest.slotItem[indexHoldingChest[slotNumber]].energyPotential + "energy" + "" +
                          "\ntotal:" + disassembleEnergy.ToString());
            }

            else
            {
                panelNotice.SetActive(true);
                textNotice.text = "분해 슬롯이 가득 찼습니다.";
            }
        }

        else if (imageCheck[slotNumber].activeSelf == true) // The Case that Slot has been Selected Before
        {
            imageCheck[slotNumber].SetActive(false); // Unmark the Selected Slot


            int i = 0;
            for (; i < 6; i++) // Find the Corpse's Index in Using Slot
            {
                if (indexUsingHolding[i] == slotNumber)
                    break;
            }

            imageDisassembleUsing[i].sprite = emptyImage; // Remove Corpse in the Using Slot
            indexUsingHolding[i] = -1; // Initialize

            disassembleEnergy -= chest.slotItem[indexHoldingChest[slotNumber]].energyPotential;
            textDisassembleEnergy.text = "추출 에너지 [ " + disassembleEnergy.ToString() + " ]";

            Debug.Log(slotNumber + "select cancel;" +
                      "    -" + chest.slotItem[indexHoldingChest[slotNumber]].energyPotential + "energy" +
                      "\ntotal:" + disassembleEnergy.ToString());
        }
    }

    public void DisassembleButtonUsingClicked(int slotNumber)
    {
        if (imageDisassembleUsing[slotNumber].sprite != emptyImage)
        {
            int indexHolding = indexUsingHolding[slotNumber];

            imageCheck[indexHolding].SetActive(false); // Unmark the Holding Slot where Selected Corpse is

            imageDisassembleUsing[slotNumber].sprite = emptyImage; // Remove Corpse in the Using Slot
            indexUsingHolding[slotNumber] = -1; // Initialize

            disassembleEnergy -= chest.slotItem[indexHoldingChest[indexHolding]].energyPotential;
            textDisassembleEnergy.text = "추출 에너지 [ " + disassembleEnergy.ToString() + " ]";
        }
    }

    public void DisassembleButtonCreateClicked()
    {
        GameManager.Inst.DisassembleItem();
    }
    #endregion

    #region CraftingPanel methods
    public void CraftingButtonItemClicked()
    {
        print("ButtonItemClicked");
    }

    public void CraftingButtonCreateClicked()
    {
        print("ButtonCreateClicked");
    }
    #endregion

    #region AssemblePanel methods
    //public void ButtonDeadBodyClicked()
    //{
    //    print("ButtonDeadBodyClicked");
    //}

    //public void ButtonEquipClicked()
    //{
    //    print("ButtonEquipClicked");
    //}
    // chest에서 BodyPart에 해당하는 아이템의 이미지를 BodyAssemble 패널에 업데이트한다. 
    public void UpdateBodyAsswemblyHoldingImages()
    {

    }

    // 해당 slot의 아이템이 선택되었는지 표시한다. 다른 slot의 아이템이 선택 해제되었음을 표시한다.
    public void DisplayIsSelected(int slotNumber)
    {

    }
    #endregion
}
