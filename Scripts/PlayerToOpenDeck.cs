using UnityEngine;

public class PlayerToOpenDeck : CardBaseState
{
    public override void EnterState(GameStateManager game){
        Debug.Log("Player to open deck");
        game.instructionPrompt(1);
    }

    public override void UpdateState(GameStateManager game){
        
        if(game.GetGameObjectOnMouseClick())
        {
            GameObject card = game.GetGameObjectOnMouseClick();
            game.finalPos = card.transform.position + new Vector3(2.2f, 2f, 2f);
            if(card.tag == "Deck" && game.Hashmap[game.deck[game.deck.Count - 1]] == card){
                card.GetComponent<Selectable>().faceUp = true;
                game.previouslySelected = card;
                game.switchState(game.PlayerToSelectCard);
            }
        }
            
    }

    public override void ChangeState(GameStateManager game){

    }
}
