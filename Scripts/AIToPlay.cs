using UnityEngine;
using System.Linq;

public class AIToPlay : CardBaseState
{
    string[] player = {"P4", "P1", "P2", "P3"};
    public GameObject[] AiCards;
    float moving = 1f;
    // GameObject AicurrentCard;
    
    public override void EnterState(GameStateManager game){
        Debug.Log("AI to play");
        game.instructionPrompt(3);
        AiCards = GameObject.FindGameObjectsWithTag(player[game.turn]);
        moving = 1f;

        int highest_index = -1;
        int highest_value = -1;
        for(int i = 0; i < AiCards.Length; i++)
        {
            if(game.Value(AiCards[i]) > highest_value)
            {
                highest_value = game.Value(AiCards[i]);
                highest_index = i;
            }
        }
    //    AicurrentCard = AiCards[highest_index];
       game.currentCard = AiCards[highest_index];
       game.currentCard.GetComponent<Selectable>().faceUp = true;
       game.previouslySelected = game.Hashmap[game.deck.Last()];
       game.startingPos = game.currentCard.transform.position;
       game.finalPos = game.previouslySelected.transform.position;
       game.playSound();

    }

    public override void UpdateState(GameStateManager game){
        if(moving > 0){
            moving -= Time.deltaTime;
            game.currentCard.transform.position = Vector3.Lerp(game.startingPos, game.finalPos + new Vector3(2.2f, 2f, 2f), (1-moving));
            game.previouslySelected.transform.position = Vector3.Lerp(game.finalPos - new Vector3(2.2f, 2f, 2f), game.startingPos, (1-moving));
        }
        else{
            if(game.turn == 0 || game.turn == 3)
                 game.previouslySelected.transform.Rotate(0, 0, 90);
            game.deck.RemoveAt(game.deck.Count - 1);
            GameObject[] delete = GameObject.FindGameObjectsWithTag("Destroy");
            if(delete.Length != 0){
                foreach(GameObject item in delete)
                    item.SetActive(false);
            }
                    
            game.currentCard.tag = "Destroy";
            game.previouslySelected.tag = player[game.turn];

            
            if(game.turn == 0){
                game.turn = (game.turn + 1) % 4;
                game.switchState(game.PlayerToOpenDeck);
            }
                
            else{
                game.turn = (game.turn + 1) % 4;
                game.switchState(game.AIToPlay);    
            }
            
        }
    }

    public override void ChangeState(GameStateManager game){

    }
}
