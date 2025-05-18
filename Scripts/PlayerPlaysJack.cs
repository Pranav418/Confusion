using UnityEngine;
using System.Collections.Generic;

public class PlayerPlaysJack : CardBaseState
{
    public GameObject[] AiCards;

    public override void EnterState(GameStateManager game){
        Debug.Log("Choose Opponents cards to shuffle");
        game.instructionPrompt(4);
    }

    public override void UpdateState(GameStateManager game){
                 
        if(game.GetGameObjectOnMouseClick())
        {
            GameObject card = game.GetGameObjectOnMouseClick();
            if(card.tag != "P1"){
                shuffleOtherPlayerCards(card);
                Debug.Log("Cards Shuffled");
                game.switchState(game.AIToPlay); 
            }   
        }
    }

    public override void ChangeState(GameStateManager game){

    }

    void shuffleOtherPlayerCards(GameObject card)
    {
        // int[] order = {0,1,2,3};
        List<int> order = new List<int>();
        for(int i = 0; i < 4; i++)
            order.Add(i);
        List<Vector3> positions = new List<Vector3>();
        ShuffleArray(order);

        GameObject[] playersCards = GameObject.FindGameObjectsWithTag(card.tag);
        foreach (GameObject playerCard in playersCards){
            positions.Add(playerCard.transform.position);
        }

        for(int i = 0; i < 4; i++)
            playersCards[i].transform.position = positions[order[i]];
        
        
    }
    
    void ShuffleArray(List<int> array)
    {
        System.Random random = new System.Random();

        for (int i = array.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);

            // Swap elements at indices i and j
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }
}
