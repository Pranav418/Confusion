using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    CardBaseState currentState;
    public PlayerToOpenDeck PlayerToOpenDeck = new PlayerToOpenDeck();
    public AIToPlay AIToPlay = new AIToPlay();
    public PlayerToSelectCard PlayerToSelectCard = new PlayerToSelectCard();
    public PlayerPlaysKing PlayerPlaysKing = new PlayerPlaysKing();
    public PlayerPlaysQueen PlayerPlaysQueen = new PlayerPlaysQueen();
    public PlayerPlaysJack PlayerPlaysJack = new PlayerPlaysJack();
    public PlayerPlays7 PlayerPlays7 = new PlayerPlays7();
    public PlayerPlaysNormalCard PlayerPlaysNormalCard = new PlayerPlaysNormalCard();
    public GameStarting GameStarting = new GameStarting();

    public int index = 0;
    public GameObject cardPrefab;
    public GameObject previouslySelected;
    public GameObject currentCard;
    public List<string> deck;
    public static string[] suits = new string[] {"C", "D", "H", "S"};
    public static string[] values = new string[] {"A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"};
    public Dictionary<string, GameObject> Hashmap = new Dictionary<string, GameObject>();
    public Vector3 startingPos;
    public Vector3 finalPos;
    public int turn = 1;

    public CardsLeftPrompt cardsLeftPrompt;
    public PauseMenu pauseMenu;
    public GameOver gameOver;
    public InstructionsScript instructionsScript;
    public AudioSource source;
    public AudioClip clip;

    void Start()
    {
        currentState = GameStarting;
        deck = GenerateDeck();
        Shuffle(deck);
        
        MindFDeal();
        currentState.EnterState(this);
    }

    public void playSound()
    {
        source.PlayOneShot(clip);
    }
    void Update()
    {
        if(!isGameOver() && !PauseMenu.isPaused){

            showCardPrompt();
            currentState.UpdateState(this);

        }
        
    }

    public static List<string> GenerateDeck()
    {
        List<string> newDeck = new List<string>();
        foreach (string s in suits)
            foreach (string v in values)
                newDeck.Add(s+v);

        return newDeck;
    }

    void Shuffle<T>(List<T> list)  
    {  
        System.Random random = new System.Random();

        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);

            // Swap elements at indices i and j
            T temp = list[i];  
            list[i] = list[j];  
            list[j] = temp; 
        }  
    }

    public void showCardPrompt()
    {
        cardsLeftPrompt.showPrompt(deck.Count - 16);
    }

    public void instructionPrompt(int instruction_number)
    {
        instructionsScript.prompt(instruction_number);
    }

    public void GameOver()
    {
        int[] scores = {0, 0, 0, 0};
        GameObject[] player; 
        for(int i = 1; i <= 4; i++){
            player = GameObject.FindGameObjectsWithTag("P" + i.ToString());

            foreach(GameObject card in player){
                card.GetComponent<Selectable>().faceUp = true;
                scores[i-1] += Value(card);
            }
        }
        int lowest_index = -1;
        int lowest_value = 5000;
        for(int i = 0; i < 4; i++){
            if(scores[i] < lowest_value){
                lowest_value = scores[i];
                lowest_index = i;
            }
        }
        string winner = "P" + (lowest_index+1).ToString();


        gameOver.SetUp(winner, scores);
    }

    public bool isGameOver()
    {
        if(deck.Count == 16)
        {
            // print("GameOver");
            GameOver();
            return true;
        }
        return false;
    }
    public struct playerCardStartPosition{
        public float x;
        public float y;

        public playerCardStartPosition(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }

    void dealPlayersCards(string playerNumber, playerCardStartPosition P, bool rotate, bool visibility = false)
    {
        string card = deck[index];
        
        for(int i = 0; i < 4; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, new Vector3(P.x, P.y - i*(float)2.5, 0) , Quaternion.identity);
            if(rotate){
                newCard.transform.Rotate(0, 0, 90);
            }    
            else{
                newCard.transform.position += new Vector3(i*(float)2.5, i*(float)2.5, 0);
            }
            newCard.name = card;
            Hashmap[newCard.name] = newCard;
            newCard.GetComponent<Selectable>().faceUp = visibility;
            newCard.tag = "P" + playerNumber;
            index++;
            card = deck[index];
        }
    }

    void RiggedCard(char val)                              // To modify players initial hand if required
    {
        int index = -1;
        for(int i = 0; i < deck.Count; i++){
            if(deck[i][1] == val){
                index = i;
            }
                
        }
        string temp = deck[index];
        deck[index] = deck[0];
        deck[0] = temp;  
    }

    void MindFDeal()
    {
        float yOffset = 0;
        float zOffset = 0.03f;
        playerCardStartPosition P1 = new playerCardStartPosition(-4f, 8f);
        playerCardStartPosition P2 = new playerCardStartPosition(-4f, -6f);
        playerCardStartPosition P3 = new playerCardStartPosition(7f, 5f);
        playerCardStartPosition P4 = new playerCardStartPosition(-7.5f, 5f);
        RiggedCard('K');
        
        dealPlayersCards("1", P1, false, true);
        dealPlayersCards("2", P2, false);
        dealPlayersCards("3", P3, true);
        dealPlayersCards("4", P4, true);
        int T = 16;
        foreach(string card in deck)
        {
            
            
            if(T > 0)
            {
                T--;
                continue;
            }

            GameObject newCard = Instantiate(cardPrefab, new Vector3(0.17f , 0.64f - yOffset, 0 - zOffset) , Quaternion.identity);
            newCard.name = card;
            newCard.tag = "Deck";
            Hashmap[newCard.name] = newCard;
            // newCard.GetComponent<Selectable>().faceUp = true;
            yOffset += 0.04f;
            zOffset +=0.03f;
        }
    }

    public void switchState(CardBaseState state){
        currentState = state;
        state.EnterState(this);
    }

    public bool getMouseClick()
    {
        if(Input.GetMouseButtonDown(0))
            return true;
        return false;
    }

    public GameObject GetGameObjectOnMouseClick()
    {
        
        if(Input.GetMouseButtonDown(0)){
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            
            string cardType;
            if(hit)
            {
                // print(hit.collider.gameObject.name);
                // print(hit.collider.gameObject.tag);
                cardType = hit.collider.gameObject.tag;
                return hit.collider.gameObject;
            }
        }
        return null;
    }

    public int Value(GameObject Card)
    {
        char C = Card.name[1];
        if(Card.name.Length == 3)
            return 10;
        if(C == 'A')
            return 0;
        if(C == 'J' || C == 'Q' || C == 'K')
            return 1000;
        
        return C - '0';
    }

    public float elapsedTime = 0;
    
    public void MoveCard(GameObject X, Vector3 destination, float time, Vector3 offset, bool rotate = false)
    {

        Vector3 startingPos  = X.transform.position;
        Vector3 finalPos = destination + offset;
        
        while (elapsedTime < time)
        {
            X.transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
        }
        if(rotate)
            X.transform.Rotate(0, 0, 90);
        return;        
    }


}
