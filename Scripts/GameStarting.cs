using UnityEngine;

public class GameStarting : CardBaseState
{
    public override void EnterState(GameStateManager game){
        Debug.Log("Playering viewing cards");
        game.instructionPrompt(0);
    }

    public override void UpdateState(GameStateManager game){
        
        if(game.getMouseClick())
        {
            GameObject[] playersCards = GameObject.FindGameObjectsWithTag("P1");
            foreach(GameObject card in playersCards)
                card.GetComponent<Selectable>().faceUp = false;
            game.switchState(game.PlayerToOpenDeck);
            
        }
            
    }

    public override void ChangeState(GameStateManager game){

    }
}
