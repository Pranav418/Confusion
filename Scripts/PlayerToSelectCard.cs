using UnityEngine;

public class PlayerToSelectCard : CardBaseState
{
    GameObject card;
    float moving = 1f;
    bool cardSelected = false;
    public override void EnterState(GameStateManager game){
        moving = 1f;
        Debug.Log("Player to select card");
        game.instructionPrompt(2);

    }

    public override void UpdateState(GameStateManager game){
        if(game.GetGameObjectOnMouseClick())
        {
            card = game.GetGameObjectOnMouseClick();
            game.currentCard = card;
            game.startingPos = card.transform.position;
            if(card.tag == "Deck" && game.Hashmap[game.deck[game.deck.Count - 1]] == card){
                card.GetComponent<Selectable>().faceUp = false;
                card.transform.position += new Vector3(2.2f, 2f, 0);
                game.deck.RemoveAt(game.deck.Count - 1);
                GameObject[] delete = GameObject.FindGameObjectsWithTag("Destroy");
                if(delete.Length != 0){
                    foreach(GameObject item in delete)
                        item.SetActive(false);
                }
                card.tag = "Destroy";
                game.turn = (game.turn + 1) % 4;
                game.switchState(game.AIToPlay);
            }

            else if(card.tag == "P1"){
                cardSelected = true;
                game.playSound();
                game.currentCard.GetComponent<Selectable>().faceUp = true;
            }
        }
        // animation
        if(cardSelected){
            if(moving > 0){
                moving -= Time.deltaTime;
                game.currentCard.transform.position = Vector3.Lerp(game.startingPos, game.finalPos, (1-moving));
                game.previouslySelected.transform.position = Vector3.Lerp(game.finalPos - new Vector3(2.2f, 2f, 2f), game.startingPos, (1-moving));
            }
            else{
                game.previouslySelected.GetComponent<Selectable>().faceUp = false;
                game.deck.RemoveAt(game.deck.Count - 1);
                GameObject[] delete = GameObject.FindGameObjectsWithTag("Destroy");
                if(delete.Length != 0){
                    foreach(GameObject item in delete)
                        item.SetActive(false);
                }
                game.currentCard.tag = "Destroy";
                game.previouslySelected.tag = "P1";
                game.turn = (game.turn + 1) % 4;
                cardSelected = false;

                switch (card.name[1])
                {
                    case 'K':
                        game.switchState(game.PlayerPlaysKing);
                        break;
                    case 'Q':
                        game.switchState(game.PlayerPlaysQueen);
                        break;
                    case 'J':
                        game.switchState(game.PlayerPlaysJack);
                        break;
                    case '7':
                        game.switchState(game.PlayerPlays7);
                        break;
                    default:
                        game.switchState(game.PlayerPlaysNormalCard);
                        break;
                }  
            }
        }
    }

    public override void ChangeState(GameStateManager game){

    }
}