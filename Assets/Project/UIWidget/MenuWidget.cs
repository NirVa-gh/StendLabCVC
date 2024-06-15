using System.Collections;
using System.Collections.Generic;
using character;
using Unity.VisualScripting;
using UnityEngine;
namespace UIWidgets
{
    public class MenuWidget : UIWidget
    {
        private void FixedUpdate()
        {
            if (this.m_IsShowing) 
            {  
                if (GameObject.FindWithTag("Player").GetComponent<NewCharacterCoontroller>().canMove)
                {
                    GameObject.FindWithTag("Player").GetComponent<NewCharacterCoontroller>().stopController( GameObject.FindWithTag("Player").GetComponent<NewCharacterCoontroller>().virtualCamera);
                }
                
            }
            else
            {
                if (GameObject.FindWithTag("Player").GetComponent<NewCharacterCoontroller>().canMove)
                {
                      GameObject.FindWithTag("Player").GetComponent<NewCharacterCoontroller>().startController( GameObject.FindWithTag("Player").GetComponent<NewCharacterCoontroller>().virtualCamera);
                }
              
            }
        }
    }
}

