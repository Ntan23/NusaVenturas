using UnityEngine;
using UnityEngine.UI;

public class CategoryButtonClick : MonoBehaviour
{
    public Color selectedColor;
    public CategoryButtonBlueprint[] buttons;

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
            }

            if(!buttons[i].isClicked) 
            { 
                buttons[i].button.GetComponent<Image>().color = Color.white;
                buttons[i].target.SetActive(false);
            }

        }
    }
}
