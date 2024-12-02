using Spine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeFactory : MonoBehaviour
{
    [System.Serializable]
    public class IngredientUI
    {
        public GameObject panel;
        public Image ingredientImage;
        public TMP_Text quantityText;
        public Image backgroundImage; // 배경 이미지 컴포넌트 (90, 30, 30) / 재료가 충분함(투명도0) / 충분하지 않음(투명도110)
    }

    public GameObject resultPanel;//제작 후 결과물 표시
    public Image resultImage;//제작 결과물 이미지

    public IngredientUI[] ingredientSlots; // 인스펙터에서 할당, 4개의 재료 슬롯
    public InventoryManager inventoryManager; // 인스펙터에서 할당
    public ClickInputSystem clickInputSystem;  // 클릭 입력 시스템 참조
    private ItemRecipe currentRecipe;

    [Header("효과음")]
    public bool isCraftSuccess = false;


    public static RecipeFactory Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        // 클릭 입력 시스템에서 레시피 클릭 이벤트 구독
        clickInputSystem.OnRecipeClicked += (gameObject) =>
        {
            ItemSlot slot = gameObject.GetComponent<ItemSlot>();
            if (slot != null && slot.itemRecipe != null)
            {
                UpdateCraftingUI(slot.itemRecipe);
            }
        };
    }
    // 제작소 UI 업데이트
    public void UpdateCraftingUI(ItemRecipe recipe)
    {
        currentRecipe = recipe;
        int i = 0;
        foreach (var ingredient in recipe.ingredients)
        {
            if (i < ingredientSlots.Length)
            {
                ingredientSlots[i].panel.SetActive(true);
                ingredientSlots[i].ingredientImage.sprite = ingredient.item.itemImage;
                ingredientSlots[i].quantityText.text = ingredient.quantity.ToString();

                // 인벤토리에서 아이템 수량 확인
                int inventoryCount = InventoryManager.Instance.GetItemCount(ingredient.item);
                // 수량에 따른 배경색 설정
                if (inventoryCount >= ingredient.quantity)
                {
                    ingredientSlots[i].backgroundImage.color = new Color(0, 0, 0, 0); // 투명
                }
                else
                {
                    ingredientSlots[i].backgroundImage.color = new Color(0.352f, 0.117f, 0.117f, 0.431f); 
                    // R:90 G:30 B:30 A:110
                }

                i++;
            }
        }

        // 나머지 슬롯 비활성화
        for (; i < ingredientSlots.Length; i++)
        {
            ingredientSlots[i].panel.SetActive(false);
        }
    }
    // 제작 버튼 클릭 이벤트
    public void CraftItem()
    {
        if (currentRecipe == null || !InventoryManager.Instance.CheckMaterials(currentRecipe))
        {
            Debug.Log("재료가 충분하지 않음");
            return;
        }

        // 재료 소모 및 아이템 생성
        foreach (var ingredient in currentRecipe.ingredients)
        {
            InventoryManager.Instance.RemoveItem(ingredient.item, ingredient.quantity);
        }

        // 결과 아이템을 인벤토리에 추가
        if (currentRecipe.resultItem != null)
        {
            InventoryManager.Instance.AddItem(currentRecipe.resultItem);
            resultImage.sprite = currentRecipe.resultItem.itemImage; // 결과 아이템 이미지 업데이트
            resultPanel.SetActive(true); // 결과 패널 활성화
            isCraftSuccess = true;
            Debug.Log("제작 성공: " + currentRecipe.resultItem.itemName);
        }
        else
        {
            resultPanel.SetActive(false);
        }

        // 모든 재료 슬롯 비활성화
        foreach (var ingredientSlot in ingredientSlots)
        {
            ingredientSlot.panel.SetActive(false);
        }
    }
    //모든 제작 슬롯 비활성화
    public void AllSetfalse()
    {
        for (int i = 0; i < ingredientSlots.Length; i++)
        {
            ingredientSlots[i].panel.SetActive(false);
        }
        resultPanel.SetActive(false);
    }


}
