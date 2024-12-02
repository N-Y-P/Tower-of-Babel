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
        public Image backgroundImage; // ��� �̹��� ������Ʈ (90, 30, 30) / ��ᰡ �����(����0) / ������� ����(����110)
    }

    public GameObject resultPanel;//���� �� ����� ǥ��
    public Image resultImage;//���� ����� �̹���

    public IngredientUI[] ingredientSlots; // �ν����Ϳ��� �Ҵ�, 4���� ��� ����
    public InventoryManager inventoryManager; // �ν����Ϳ��� �Ҵ�
    public ClickInputSystem clickInputSystem;  // Ŭ�� �Է� �ý��� ����
    private ItemRecipe currentRecipe;

    [Header("ȿ����")]
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
        // Ŭ�� �Է� �ý��ۿ��� ������ Ŭ�� �̺�Ʈ ����
        clickInputSystem.OnRecipeClicked += (gameObject) =>
        {
            ItemSlot slot = gameObject.GetComponent<ItemSlot>();
            if (slot != null && slot.itemRecipe != null)
            {
                UpdateCraftingUI(slot.itemRecipe);
            }
        };
    }
    // ���ۼ� UI ������Ʈ
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

                // �κ��丮���� ������ ���� Ȯ��
                int inventoryCount = InventoryManager.Instance.GetItemCount(ingredient.item);
                // ������ ���� ���� ����
                if (inventoryCount >= ingredient.quantity)
                {
                    ingredientSlots[i].backgroundImage.color = new Color(0, 0, 0, 0); // ����
                }
                else
                {
                    ingredientSlots[i].backgroundImage.color = new Color(0.352f, 0.117f, 0.117f, 0.431f); 
                    // R:90 G:30 B:30 A:110
                }

                i++;
            }
        }

        // ������ ���� ��Ȱ��ȭ
        for (; i < ingredientSlots.Length; i++)
        {
            ingredientSlots[i].panel.SetActive(false);
        }
    }
    // ���� ��ư Ŭ�� �̺�Ʈ
    public void CraftItem()
    {
        if (currentRecipe == null || !InventoryManager.Instance.CheckMaterials(currentRecipe))
        {
            Debug.Log("��ᰡ ������� ����");
            return;
        }

        // ��� �Ҹ� �� ������ ����
        foreach (var ingredient in currentRecipe.ingredients)
        {
            InventoryManager.Instance.RemoveItem(ingredient.item, ingredient.quantity);
        }

        // ��� �������� �κ��丮�� �߰�
        if (currentRecipe.resultItem != null)
        {
            InventoryManager.Instance.AddItem(currentRecipe.resultItem);
            resultImage.sprite = currentRecipe.resultItem.itemImage; // ��� ������ �̹��� ������Ʈ
            resultPanel.SetActive(true); // ��� �г� Ȱ��ȭ
            isCraftSuccess = true;
            Debug.Log("���� ����: " + currentRecipe.resultItem.itemName);
        }
        else
        {
            resultPanel.SetActive(false);
        }

        // ��� ��� ���� ��Ȱ��ȭ
        foreach (var ingredientSlot in ingredientSlots)
        {
            ingredientSlot.panel.SetActive(false);
        }
    }
    //��� ���� ���� ��Ȱ��ȭ
    public void AllSetfalse()
    {
        for (int i = 0; i < ingredientSlots.Length; i++)
        {
            ingredientSlots[i].panel.SetActive(false);
        }
        resultPanel.SetActive(false);
    }


}
