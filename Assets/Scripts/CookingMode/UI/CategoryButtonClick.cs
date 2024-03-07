using UnityEngine;
using UnityEngine.UI;

public class CategoryButtonClick : MonoBehaviour
{
    [SerializeField] private Color selectedColor;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private CategoryButtonBlueprint[] buttons;

    public void ClickButton(int index)
    {
        buttons[index].isClicked = true;
        
        for(int i = 0; i < buttons.Length; i++)
        {
            if(i != index) buttons[i].isClicked = false;

            if(buttons[i].isClicked) 
            {
                buttons[i].button.GetComponent<Image>().color = selectedColor;
                buttons[i].target.SetActive(true);
                scrollRect.content = buttons[i].target.GetComponent<RectTransform>();
            }

            if(!buttons[i].isClicked) 
            { 
                buttons[i].button.GetComponent<Image>().color = Color.white;
                buttons[i].target.SetActive(false);
            }

        }
    }
}
