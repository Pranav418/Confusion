using UnityEngine;

public class PlayerPlaysQueen : CardBaseState
{
    GameObject card;
    bool cardsShown = false;
    
    public override void EnterState(GameStateManager game){
        Debug.Log("Player to play Queen");
        game.instructionPrompt(7);

    }

    public override void UpdateState(GameStateManager game){
        
        if(cardsShown){
            if(game.getMouseClick()){
                showPlayersCards(card, false);
                cardsShown = false;
                game.switchState(game.AIToPlay);
            }

        }

        else if(game.GetGameObjectOnMouseClick())
        {
            card = game.GetGameObjectOnMouseClick();
            if(card.tag == "P1"){
                showPlayersCards(card, true);
                Debug.Log("Cards Shown");
                cardsShown = true;
            }   
        }

        
    }

    public override void ChangeState(GameStateManager game){

    }    

    void showPlayersCards(GameObject card, bool visibility)
    {
        GameObject[] playersCards = GameObject.FindGameObjectsWithTag(card.tag);
        Debug.Log(playersCards.Length);
        foreach(GameObject playerCard in playersCards)
            playerCard.GetComponent<Selectable>().faceUp = visibility;
    }
}
