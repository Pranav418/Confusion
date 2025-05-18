using UnityEngine;

public class PlayerPlays7 : CardBaseState
{
    GameObject Opponentcard;
    GameObject playerCard;
    float moving = 1f;
    bool opponentCardChosen = false;
    bool playerCardChosen = false;
    public override void EnterState(GameStateManager game){
        Debug.Log("Player to play 7");
        game.instructionPrompt(6);
        moving = 1f;
    }

    public override void UpdateState(GameStateManager game){
        
        if(playerCardChosen && opponentCardChosen){
            if(moving > 0){
                moving -= Time.deltaTime;
                Opponentcard.transform.position = Vector3.Lerp(game.startingPos, game.finalPos, (1-moving));
                playerCard.transform.position = Vector3.Lerp(game.finalPos, game.startingPos, (1-moving));
            }
            else{
                if(Opponentcard.tag != "P2"){              // not P2 or P1, rotate 90 degrees
                    Opponentcard.transform.Rotate(0, 0, 90);
                    playerCard.transform.Rotate(0, 0, 90);
                }   
                    
                
                playerCard.tag = Opponentcard.tag;
                Opponentcard.tag = "P1";
                opponentCardChosen = false;
                playerCardChosen = false;
                game.switchState(game.AIToPlay);
            }
            
        }
        
        
        if(game.GetGameObjectOnMouseClick() && !opponentCardChosen && game.GetGameObjectOnMouseClick().tag != "Deck")
        {
            if(game.GetGameObjectOnMouseClick().tag != "P1"){
                Opponentcard = game.GetGameObjectOnMouseClick();        
                Debug.Log("Opponent Card Chosen");
                game.startingPos = Opponentcard.transform.position;
                opponentCardChosen = true;
            }   
        }

        if(game.GetGameObjectOnMouseClick() && !playerCardChosen && game.GetGameObjectOnMouseClick().tag == "P1")
        {
            playerCard = game.GetGameObjectOnMouseClick();
            game.finalPos = playerCard.transform.position;
            playerCardChosen = true;
        }
    }

    public override void ChangeState(GameStateManager game){

    }
}
