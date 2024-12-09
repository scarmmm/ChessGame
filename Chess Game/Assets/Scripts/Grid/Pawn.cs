using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static GameManager;

public class Pawn : MonoBehaviour
{
    [SerializeField] private int _playerNumber; // Player number for the pawn
    [SerializeField] public CharacterController controller; // Reference to the CharacterController component
    [SerializeField] public Grid _grid;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private GameObject _pawn1;
    [SerializeField] private GameObject _pawn2;
    [SerializeField] private GameObject _rook1;
    [SerializeField] private GameObject _rook2;
    [SerializeField] private GameObject _bishop1;
    [SerializeField] private GameObject _bishop2;
    [SerializeField] private GameObject _knight1;
    [SerializeField] private GameObject _knight2;
    [SerializeField] private GameObject _queen1;
    [SerializeField] private GameObject _queen2;
    [SerializeField] private GameObject _king1;
    [SerializeField] private GameObject _king2;
    [SerializeField] private GameObject _position;
    [SerializeField] private float rotationSpeed = 5.0f;
    [SerializeField] private InputReader inputReader;
    public GameObject selectedPawn;
    [SerializeField] public GameObject lastSelectedPiece;
    private List<GameObject> pawns = new List<GameObject>(); // List to store pawns
    private List<GameObject> rooks = new List<GameObject>(); // List to store rooks
    private List<GameObject> pawns2 = new List<GameObject>();
    private List<GameObject> rooks2 = new List<GameObject>();
    private List<GameObject> bishops = new List<GameObject>(); // List to store bishops
    private List<GameObject> bishops2 = new List<GameObject>();
    private List<GameObject> knights = new List<GameObject>();
    private List<GameObject> knights2 = new List<GameObject>();
    private List<GameObject> queens = new List<GameObject>();
    private List<GameObject> queens2 = new List<GameObject>();
    private List<GameObject> kings = new List<GameObject>();
    private List<GameObject> kings2 = new List<GameObject>();
    private List<GameObject> Team1 = new List<GameObject>();
    private List<GameObject> Team2 = new List<GameObject>();
    private Vector3 playerVelocity;
    private Transform cameraMain;
    bool player1KingCaptured = false;
    bool player2KingCaptured = false;
    public string eliminatedPlayer = "Eliminated";
    ///public string playerTag1 = "Player1";
    Camera Camera;
    // Start is called before the first frame update
    void Start()
    {

        cameraMain = Camera.main.transform;
        GameStates state = GameStates.SelectPiece;
        //Cursor.visible = false;
        //Vector3 worldPosition = _grid.GetCellCenterWorld(new Vector3Int(0, 0, 0)); // Get the world position of the cell at (0, 0)
        // Debug.Log("World Position: " + worldPosition);
        // worldPosition.y = 0;
        //transform.position = worldPosition; // Set the position of the pawn to the world position
        // Instantiate(_pawn, worldPosition, Quaternion.identity);
        //player 1 pawns
        for (int i = 0; i < 8; i++)
        {
            Vector3 worldPosition2 = _grid.GetCellCenterWorld(new Vector3Int(0, i, 0));
            Vector3Int gridPosition = _grid.WorldToCell(worldPosition2);
            worldPosition2.y = 0.101f;
            transform.position = worldPosition2; // Set the position of the pawn to the world position
            GameObject pawnInstance = Instantiate(_pawn1, worldPosition2, Quaternion.identity);
            pawnInstance.transform.localScale = new Vector3(15f, 15f, 15f);
            BoxCollider boxCollider = pawnInstance.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            boxCollider.size = new Vector3(.06f, .06f, .06f);
            Rigidbody rigidbody = pawnInstance.AddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            pawnInstance.AddComponent<Overlap>();
            Targeter targeter = pawnInstance.AddComponent<Targeter>();
            targeter.renderer = pawnInstance.GetComponent<Renderer>();
            PieceIdentity id = pawnInstance.AddComponent<PieceIdentity>();
            PawnMove pawnComponent = pawnInstance.AddComponent<PawnMove>();
            id.pieceType = ChessPieceType.Player1Pawn;
            pawnInstance.tag = "Player1";
            pawnInstance.layer = LayerMask.NameToLayer("Chessboard"); //for the raycast
            pawnInstance.transform.rotation = Quaternion.Euler(-90, 0, 0);

            pawns.Add(pawnInstance);
            Team1.Add(pawnInstance);

        }
        //player 1 rooks
        for (int i = 0; i < 9; i += 7)
        {
            Vector3 worldPosition2 = _grid.GetCellCenterWorld(new Vector3Int(1, i, 0));
            Vector3Int gridPosition = _grid.WorldToCell(worldPosition2);
            worldPosition2.y = 0.128f;
            transform.position = worldPosition2; // Set the position of the rook to the world position
            GameObject rookInstance = Instantiate(_rook1, worldPosition2, Quaternion.identity);
            BoxCollider boxCollider = rookInstance.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            Targeter targeter = rookInstance.AddComponent<Targeter>();
            targeter.renderer = rookInstance.GetComponent<Renderer>();
            Rigidbody rigidbody = rookInstance.AddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rookInstance.transform.localScale = new Vector3(15f, 15f, 15f);
            boxCollider.size = new Vector3(.06f, .06f, .06f);
            PieceIdentity id = rookInstance.AddComponent<PieceIdentity>();
            id.pieceType = ChessPieceType.Player1Rook;
            rookInstance.tag = "Player1";
            rookInstance.layer = LayerMask.NameToLayer("Chessboard"); //for the raycast
            rookInstance.transform.rotation = Quaternion.Euler(-90, 0, 0);
            rooks.Add(rookInstance);
            Team1.Add(rookInstance);
        }
        //player 1 bishops
        for (int i = 2; i < 6; i += 3)
        {
            Vector3 worldPosition2 = _grid.GetCellCenterWorld(new Vector3Int(1, i, 0));
            Vector3Int gridPosition = _grid.WorldToCell(worldPosition2);
            worldPosition2.y = .191f;
            transform.position = worldPosition2; // Set the position of the rook to the world position
            GameObject bishopInstance = Instantiate(_bishop1, worldPosition2, Quaternion.identity);
            BoxCollider boxCollider = bishopInstance.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            Targeter targeter = bishopInstance.AddComponent<Targeter>();
            targeter.renderer = bishopInstance.GetComponent<Renderer>();
            bishopInstance.transform.localScale = new Vector3(15f, 15f, 15f);
            boxCollider.size = new Vector3(.06f, .06f, .06f);
            PieceIdentity id = bishopInstance.AddComponent<PieceIdentity>();
            id.pieceType = ChessPieceType.Player1Bishop;
            bishopInstance.tag = "Player1";
            bishopInstance.layer = LayerMask.NameToLayer("Chessboard"); //for the raycast
            bishopInstance.transform.rotation = Quaternion.Euler(-90, 0, 0);
            bishops.Add(bishopInstance);
            Team1.Add(bishopInstance);
        }
        //player 1 knights
        for (int i = 1; i < 8; i += 5)
        {
            Vector3 worldPosition2 = _grid.GetCellCenterWorld(new Vector3Int(1, i, 0));
            Vector3Int gridPosition = _grid.WorldToCell(worldPosition2);
            worldPosition2.y = .212f;
            transform.position = worldPosition2; // Set the position of the rook to the world position
            GameObject knightInstance = Instantiate(_knight1, worldPosition2, Quaternion.identity);
            BoxCollider boxCollider = knightInstance.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            Targeter targeter = knightInstance.AddComponent<Targeter>();
            targeter.renderer = knightInstance.GetComponent<Renderer>();
            knightInstance.transform.localScale = new Vector3(15f, 15f, 15f);
            boxCollider.size = new Vector3(.06f, .06f, .06f);
            PieceIdentity id = knightInstance.AddComponent<PieceIdentity>();
            knightInstance.AddComponent<Overlap>();
            Rigidbody rigidbody = knightInstance.AddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            id.pieceType = ChessPieceType.Player1Knight;
            knightInstance.tag = "Player1";
            knightInstance.layer = LayerMask.NameToLayer("Chessboard"); //for the raycast
            knightInstance.transform.rotation = Quaternion.Euler(-90, -90, 0);
            knights.Add(knightInstance);
            Team1.Add(knightInstance);
        }
        //player 1 queen
        for (int i = 1; i < 2; i++)
        {
            Vector3 worldPosition2 = _grid.GetCellCenterWorld(new Vector3Int(1, 3, 0));
            Vector3Int gridPosition = _grid.WorldToCell(worldPosition2);
            worldPosition2.y = 0.202f;
            transform.position = worldPosition2; // Set the position of the rook to the world position
            GameObject queenInstance = Instantiate(_queen1, worldPosition2, Quaternion.identity);
            BoxCollider boxCollider = queenInstance.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            Targeter targeter = queenInstance.AddComponent<Targeter>();
            targeter.renderer = queenInstance.GetComponent<Renderer>();
            queenInstance.transform.localScale = new Vector3(15f, 15f, 15f);
            boxCollider.size = new Vector3(.06f, .06f, .06f);
            PieceIdentity id = queenInstance.AddComponent<PieceIdentity>();
            Rigidbody rigidbody = queenInstance.AddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            id.pieceType = ChessPieceType.Player1Queen;
            queenInstance.tag = "Player1";
            queenInstance.layer = LayerMask.NameToLayer("Chessboard"); //for the raycast
            queenInstance.transform.rotation = Quaternion.Euler(-90, 90, 0);
            queens.Add(queenInstance);
            Team1.Add(queenInstance);
        }
        //player 1 king 
        for (int i = 1; i < 2; i++)
        {
            Vector3 worldPosition2 = _grid.GetCellCenterWorld(new Vector3Int(1, 4, 0));
            Vector3Int gridPosition = _grid.WorldToCell(worldPosition2);
            worldPosition2.y = 0.258f;
            transform.position = worldPosition2; // Set the position of the rook to the world position
            GameObject kingInstance = Instantiate(_king1, worldPosition2, Quaternion.identity);
            BoxCollider boxCollider = kingInstance.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            Targeter targeter = kingInstance.AddComponent<Targeter>();
            targeter.renderer = kingInstance.GetComponent<Renderer>();
            kingInstance.AddComponent<Overlap>();
            kingInstance.transform.localScale = new Vector3(15f, 15f, 15f);
            boxCollider.size = new Vector3(.06f, .06f, .06f);
            PieceIdentity id = kingInstance.AddComponent<PieceIdentity>();
            Rigidbody rigidbody = kingInstance.AddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            id.pieceType = ChessPieceType.Player1King;
            kingInstance.tag = "Player1";
            kingInstance.layer = LayerMask.NameToLayer("Chessboard"); //for the raycast
            kingInstance.transform.rotation = Quaternion.Euler(-90, 90, 0);
            kings.Add(kingInstance);
            Team1.Add(kingInstance);
        }



        //player 2 pawns
        for (int i = 0; i < 8; i++)
        {
            Vector3 worldPosition2 = _grid.GetCellCenterWorld(new Vector3Int(-5, i, 0));
            Vector3Int gridPosition = _grid.WorldToCell(worldPosition2);
            worldPosition2.y = -0.068f;
            transform.position = worldPosition2; // Set the position of the pawn to the world position
            GameObject pawnInstance = Instantiate(_pawn2, worldPosition2, Quaternion.identity);
            pawnInstance.transform.localScale = new Vector3(15f, 15f, 15f);
            BoxCollider boxCollider = pawnInstance.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            boxCollider.size = new Vector3(.06f, .06f, .06f);
            Targeter targeter = pawnInstance.AddComponent<Targeter>();
            targeter.renderer = pawnInstance.GetComponent<Renderer>();
            PieceIdentity id = pawnInstance.AddComponent<PieceIdentity>();
            PawnMove pawnComponent = pawnInstance.AddComponent<PawnMove>();
            Rigidbody rigidbody = pawnInstance.AddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            pawnInstance.AddComponent<Overlap>();
            id.pieceType = ChessPieceType.Player2Pawn;
            pawnInstance.tag = "Player2";
            pawnInstance.layer = LayerMask.NameToLayer("Chessboard"); //for the raycast
            pawnInstance.transform.rotation = Quaternion.Euler(-90, 0, 0);
            pawns2.Add(pawnInstance);
            Team2.Add(pawnInstance);
        }
        //player 2 rooks
        for (int i = 0; i < 9; i += 7)
        {
            Vector3 worldPosition2 = _grid.GetCellCenterWorld(new Vector3Int(-6, i, 0));
            Vector3Int gridPosition = _grid.WorldToCell(worldPosition2);
            worldPosition2.y = 0.137f;
            transform.position = worldPosition2; // Set the position of the rook to the world position
            GameObject rookInstance = Instantiate(_rook2, worldPosition2, Quaternion.identity);
            BoxCollider boxCollider = rookInstance.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            Targeter targeter = rookInstance.AddComponent<Targeter>();
            targeter.renderer = rookInstance.GetComponent<Renderer>();
            rookInstance.transform.localScale = new Vector3(15f, 15f, 15f);
            boxCollider.size = new Vector3(.06f, .06f, .06f);
            PieceIdentity id = rookInstance.AddComponent<PieceIdentity>();
            Rigidbody rigidbody = rookInstance.AddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rookInstance.AddComponent<Overlap>();
            id.pieceType = ChessPieceType.Player2Rook;
            rookInstance.tag = "Player2";
            rookInstance.layer = LayerMask.NameToLayer("Chessboard"); //for the raycast
            rookInstance.transform.rotation = Quaternion.Euler(-90, 0, 0);
            rooks2.Add(rookInstance);
            Team2.Add(rookInstance);
        }
        for (int i = 2; i < 6; i += 3)
        {
            Vector3 worldPosition2 = _grid.GetCellCenterWorld(new Vector3Int(-6, i, 0));
            Vector3Int gridPosition = _grid.WorldToCell(worldPosition2);
            worldPosition2.y = 0.172f;
            transform.position = worldPosition2; // Set the position of the rook to the world position
            GameObject bishopInstance = Instantiate(_bishop2, worldPosition2, Quaternion.identity);
            BoxCollider boxCollider = bishopInstance.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            Targeter targeter = bishopInstance.AddComponent<Targeter>();
            bishopInstance.AddComponent<Overlap>();
            targeter.renderer = bishopInstance.GetComponent<Renderer>();
            bishopInstance.transform.localScale = new Vector3(15f, 15f, 15f);
            boxCollider.size = new Vector3(.06f, .06f, .06f);
            PieceIdentity id = bishopInstance.AddComponent<PieceIdentity>();
            Rigidbody rigidbody = bishopInstance.AddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            id.pieceType = ChessPieceType.Player2Bishop;
            bishopInstance.tag = "Player2";
            bishopInstance.layer = LayerMask.NameToLayer("Chessboard"); //for the raycast
            bishopInstance.transform.rotation = Quaternion.Euler(-90, 0, 0);
            bishops2.Add(bishopInstance);
            Team2.Add(bishopInstance);
        }
        //player 2 knights
        for (int i = 1; i < 8; i += 5)
        {
            Vector3 worldPosition2 = _grid.GetCellCenterWorld(new Vector3Int(-6, i, 0));
            Vector3Int gridPosition = _grid.WorldToCell(worldPosition2);
            worldPosition2.y = 0.176f;
            transform.position = worldPosition2; // Set the position of the rook to the world position
            GameObject knightInstance = Instantiate(_knight2, worldPosition2, Quaternion.identity);
            BoxCollider boxCollider = knightInstance.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            Targeter targeter = knightInstance.AddComponent<Targeter>();
            targeter.renderer = knightInstance.GetComponent<Renderer>();
            knightInstance.AddComponent<Overlap>();
            knightInstance.transform.localScale = new Vector3(15f, 15f, 15f);
            boxCollider.size = new Vector3(.06f, .06f, .06f);
            PieceIdentity id = knightInstance.AddComponent<PieceIdentity>();
            Rigidbody rigidbody = knightInstance.AddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            id.pieceType = ChessPieceType.Player2Knight;
            knightInstance.tag = "Player2";
            knightInstance.layer = LayerMask.NameToLayer("Chessboard"); //for the raycast
            knightInstance.transform.rotation = Quaternion.Euler(-90, -90, 0);
            queens.Add(knightInstance);
            Team2.Add(knightInstance);
        }
        //player 2 queen
        for (int i = 1; i < 2; i++)
        {
            Vector3 worldPosition2 = _grid.GetCellCenterWorld(new Vector3Int(-6, 3, 0));
            Vector3Int gridPosition = _grid.WorldToCell(worldPosition2);
            worldPosition2.y = .227f;
            transform.position = worldPosition2; // Set the position of the rook to the world position
            GameObject queenInstance = Instantiate(_queen2, worldPosition2, Quaternion.identity);
            BoxCollider boxCollider = queenInstance.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            Targeter targeter = queenInstance.AddComponent<Targeter>();
            targeter.renderer = queenInstance.GetComponent<Renderer>();
            queenInstance.transform.localScale = new Vector3(15f, 15f, 15f);
            boxCollider.size = new Vector3(.06f, .06f, .06f);
            PieceIdentity id = queenInstance.AddComponent<PieceIdentity>();
            Rigidbody rigidbody = queenInstance.AddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            id.pieceType = ChessPieceType.Player2Queen;
            queenInstance.tag = "Player2";
            queenInstance.layer = LayerMask.NameToLayer("Chessboard"); //for the raycast
            queenInstance.transform.rotation = Quaternion.Euler(-90, 90, 0);
            queens2.Add(queenInstance);
            Team2.Add(queenInstance);
        }
        //player 2 king 
        for (int i = 1; i < 2; i++)
        {
            Vector3 worldPosition2 = _grid.GetCellCenterWorld(new Vector3Int(-6, 4, 0));
            Vector3Int gridPosition = _grid.WorldToCell(worldPosition2);
            worldPosition2.y = .259f;
            transform.position = worldPosition2; // Set the position of the rook to the world position
            GameObject kingInstance = Instantiate(_king2, worldPosition2, Quaternion.identity);
            BoxCollider boxCollider = kingInstance.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            Targeter targeter = kingInstance.AddComponent<Targeter>();
            targeter.renderer = kingInstance.GetComponent<Renderer>();
            kingInstance.transform.localScale = new Vector3(15f, 15f, 15f);
            boxCollider.size = new Vector3(.06f, .06f, .06f);
            PieceIdentity id = kingInstance.AddComponent<PieceIdentity>();
            Rigidbody rigidbody = kingInstance.AddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            id.pieceType = ChessPieceType.Player2King;
            kingInstance.tag = "Player2";
            kingInstance.layer = LayerMask.NameToLayer("Chessboard"); //for the raycast
            kingInstance.transform.rotation = Quaternion.Euler(-90, 90, 0);
            kings2.Add(kingInstance);
            Team2.Add(kingInstance);
        }

    }

    // Update is called once per frame
    void Update()
    {
        setPosition();
        if (kings.Count == 0 || kings2.Count == 0 || kings[0] == null || kings2[0] == null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }


    }
    private void setPosition()
    {
        //this code will actively track the mouse position and convert it to a grid position
        //screen position function did not work had to use raycast to get the world space position

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int layerMask = LayerMask.GetMask("Chessboard");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider != null || hit.collider != gameObject.CompareTag("ChessBoard"))
            {
                Vector3 worldMousePosition = hit.point; //get the world position of the mouse
                Vector3Int gridPosition = _grid.WorldToCell(worldMousePosition); //convert the world position to a grid position(this works)
                Vector3 worldPosition2 = _grid.GetCellCenterWorld(gridPosition); // Get the center of the cell at the grid position and move the peice to that position
                float posy = gridPosition.y;
                if (Input.GetMouseButtonDown(0))
                {
                    // this if statement is to select the piece 

                    if (selectedPawn == null)
                    {
                        Debug.Log("Game State: " + GameManager.instance.State);
                        if (GameManager.instance.State == GameStates.PlayerTurn1)
                        {
                            PlayerTurn(hit, gridPosition, "Player1");

                        }
                        else if (GameManager.instance.State == GameStates.PlayerTurn2)
                        {
                            PlayerTurn(hit, gridPosition, "Player2");

                        }

                    }
                    else
                    {
                        // move the selected pawn to the new position, if the position is valid
                        Vector3Int grid = getGridPosition(selectedPawn);
                        if (isValidPosition(grid, gridPosition))
                        {
                            selectedPawn.transform.position = _grid.GetCellCenterWorld(gridPosition);
                            //keep y axis at zero 
                            selectedPawn.transform.position = new Vector3(selectedPawn.transform.position.x, transform.position.y, selectedPawn.transform.position.z);

                            if (selectedPawn != null)
                            {
                                lastSelectedPiece = selectedPawn;
                            }
                            selectedPawn = null; // deselect the pawn after moving
                            GameManager.instance.UpdateGameState(GameManager.instance.State == GameStates.PlayerTurn1 ? GameStates.PlayerTurn2 : GameStates.PlayerTurn1);

                        }
                        if (selectedPawn != null)
                        {
                            lastSelectedPiece = selectedPawn;
                        }

                        selectedPawn = null;
                        //Debug.Log("Last Selected Piece: " + lastSelectedPiece);
                    }
                }
            }
        }
    }
    private void PlayerTurn(RaycastHit hit, Vector3Int gridPosition, string player)
    {
        PieceIdentity piece = hit.collider.gameObject.GetComponent<PieceIdentity>();

        if (piece == null)
        {
            Debug.Log("Invalid selection: No piece selected.");
            return; // Do nothing if no piece is clicked
        }

        if (selectedPawn == null) // Select piece
        {
            if (piece != null && piece.pieceType.ToString().Contains(player))
            {
                selectedPawn = hit.collider.gameObject;
            }
            else
            {
                Debug.Log("Invalid selection: Not your piece.");
            }
        }

    }

    private bool isValidPosition(Vector3Int gridPosition, Vector3Int hitPosition)
    {
        bool isValid = false;
        bool isBlocked = false;
        bool isPathBlocked = false;
        isBlocked = IsBlocked(selectedPawn, hitPosition);
        isPathBlocked = pathIsBlocked(selectedPawn, hitPosition);
        Debug.Log("Is Blocked: " + isBlocked);
        PieceIdentity piece = selectedPawn.GetComponent<PieceIdentity>();
        Debug.Log("Selected Position" + gridPosition);
        if (piece.pieceType == ChessPieceType.Player1Pawn || piece.pieceType == ChessPieceType.Player2Pawn)
        {
            Vector3Int selectedPawnGridPos = getGridPosition(selectedPawn);
            PawnMove pawnComponent = selectedPawn.GetComponent<PawnMove>();
            bool isDiagnol = pieceIsDiagnol();
            Debug.Log("Is Diagnol: " + isDiagnol);
            if (isDiagnol == true)
            {
                //replace with abs value
                if ((hitPosition.x == selectedPawnGridPos.x + 1 && hitPosition.y == selectedPawnGridPos.y + 1) ||
                    (hitPosition.x == selectedPawnGridPos.x - 1 && hitPosition.y == selectedPawnGridPos.y - 1))
                {
                    Debug.Log("Valid Position: " + hitPosition);
                    return isValid = true;
                }
                else if (hitPosition.x == selectedPawnGridPos.x - 1 && hitPosition.y == selectedPawnGridPos.y + 1)
                {
                    Debug.Log("Valid Position: " + hitPosition);
                    return isValid = true;
                }
                else
                {
                    Debug.Log("Invalid Position: " + hitPosition);
                    return isValid;
                }

            }

            else if (pawnComponent != null && pawnComponent.isFirstMove)
            {
                if (Mathf.Abs(hitPosition.x - selectedPawnGridPos.x) == 2 && hitPosition.y == selectedPawnGridPos.y && !isBlocked)
                {
                    pawnComponent.isFirstMove = false;
                    return isValid = true;
                }
                else if (Mathf.Abs(hitPosition.x - selectedPawnGridPos.x) == 1 && hitPosition.y == selectedPawnGridPos.y && !isBlocked)
                {
                    Debug.Log("Valid Position: " + hitPosition);
                    return isValid = true;
                }
                else
                {
                    Debug.Log("Invalid Position: " + hitPosition);
                    return isValid;
                }
            }
            else if ((hitPosition.x == selectedPawnGridPos.x + 1 ||
                 hitPosition.x == selectedPawnGridPos.x - 1) &&
                hitPosition.y == selectedPawnGridPos.y)
            {
                if (isBlocked == false)
                {
                    Debug.Log("Valid Position: " + hitPosition);
                    return isValid = true;
                }
                else
                {
                    Debug.Log("Invalid Position: " + hitPosition);
                    return isValid;
                }
                //Debug.Log("Valid Position: " + hitPosition);
                //return isValid = true;
            }
            else
            {

                Debug.Log("Invalid Position: " + hitPosition);
            }

            return isValid;
        }

        if (piece.pieceType == ChessPieceType.Player1Rook || piece.pieceType == ChessPieceType.Player2Rook)
        {
            Vector3Int selectedRookGridPos = getGridPosition(selectedPawn);
            if (hitPosition.y > 7 || hitPosition.y < 0)
            {
                Debug.Log("Invalid Position: " + hitPosition);
                return isValid;
            }
            if (hitPosition.x < -6 || hitPosition.x > 1)
            {
                Debug.Log("Invalid Position: " + hitPosition);
                return isValid;
            }
            // Check if target is adjacent on X and remains on the same Y
            if (selectedRookGridPos.x != hitPosition.x && selectedRookGridPos.y != hitPosition.y)
            {
                Debug.Log("Invalid Position: " + hitPosition);
                //return isValid = true;
            }
            else if (isBlocked == false && isPathBlocked == false)
            {
                Debug.Log("Valid Position: " + hitPosition);
                isValid = true;
            }

            return isValid;
        }

        if (piece.pieceType == ChessPieceType.Player1Bishop || piece.pieceType == ChessPieceType.Player2Bishop)
        {
            Vector3Int selectedBishopGridPos = getGridPosition(selectedPawn);
            if (hitPosition.y > 7 || hitPosition.y < 0)
            {
                Debug.Log("Invalid Position: " + hitPosition);
            }
            if (hitPosition.x < -6 || hitPosition.x > 1)
            {
                Debug.Log("Invalid Position: " + hitPosition);
            }

            if ((hitPosition.x == gridPosition.x || hitPosition.y == gridPosition.x))
            {
                Debug.Log("Invalid Position: " + hitPosition);
                isValid = false;
            }
            else if (isBlocked == false && isPathBlocked == false)
            {
                Debug.Log("Valid Position: " + hitPosition);
                isValid = true;
            }
            return isValid;
        }

        if (piece.pieceType == ChessPieceType.Player1Knight || piece.pieceType == ChessPieceType.Player2Knight)
        {
            Debug.Log("Entered Knight Logic");
            if (hitPosition.y > 7 || hitPosition.y < 0)
            {
                Debug.Log("Invalid Position: " + hitPosition);
                return isValid;
            }
            if (hitPosition.x < -6 || hitPosition.x > 1)
            {
                Debug.Log("Invalid Position: " + hitPosition);
                return isValid;
            }
            if (Mathf.Abs(hitPosition.x - gridPosition.x) == 2 && Mathf.Abs(hitPosition.y - gridPosition.y) == 1)
            {
                Debug.Log("Valid Position2: " + hitPosition);
                isValid = true;
                return isValid;
            }
            if (Mathf.Abs(hitPosition.x - gridPosition.x) == 1 && Mathf.Abs(hitPosition.y - gridPosition.y) == 2)
            {
                Debug.Log("Valid Position2: " + hitPosition);
                isValid = true;
                return isValid;
            }
            else
            {
                Debug.Log("Invalid Position: " + hitPosition);
                isValid = false;
            }
            return isValid;
        }

        if (piece.pieceType == ChessPieceType.Player1Queen || piece.pieceType == ChessPieceType.Player2Queen)
        {

            Debug.Log("Entered Queen Logic");
            if (hitPosition.y > 7 || hitPosition.y < 0)
            {
                Debug.Log("Invalid Position: " + hitPosition);
                return isValid;
            }
            if (hitPosition.x < -6 || hitPosition.x > 1)
            {
                Debug.Log("Invalid Position: " + hitPosition);
                return isValid;
            }
            else if (isBlocked == false && isPathBlocked == false)
            {
                isValid = true;
                return isValid;

            }
        }

        if (piece.pieceType == ChessPieceType.Player1King || piece.pieceType == ChessPieceType.Player2King)
        {
            Debug.Log("Entered King Logic");
            if (hitPosition.y > 7 || hitPosition.y < 0)
            {
                Debug.Log("Invalid Position: " + hitPosition);
                return isValid;
            }
            if (hitPosition.x < -6 || hitPosition.x > 1)
            {
                Debug.Log("Invalid Position: " + hitPosition);
                return isValid;
            }
            if ((Mathf.Abs(hitPosition.x - gridPosition.x) == 1 || Mathf.Abs(hitPosition.y - gridPosition.y) == 1) && isBlocked == false)
            {
                Debug.Log("Valid Position2: " + hitPosition);
                isValid = true;
                return isValid;
            }


        }
        return isValid;
    }

    Vector3Int getGridPosition(GameObject pawn)
    {
        Vector3Int gridPosition = _grid.WorldToCell(pawn.transform.position);
        //Debug.Log("getGridPosition(): " + gridPosition);
        return gridPosition;
    }


    private bool IsBlocked(GameObject piece, Vector3 destination)
    {
        bool isBlocked = false;
        Vector3 currentPosition = getGridPosition(selectedPawn);
        PieceIdentity pieceIdentity = piece.GetComponent<PieceIdentity>();
        //chess if a team one piece is blocking the move
        if (pieceIdentity.pieceType == ChessPieceType.Player1Pawn || pieceIdentity.pieceType == ChessPieceType.Player1Bishop ||
            pieceIdentity.pieceType == ChessPieceType.Player1King || pieceIdentity.pieceType == ChessPieceType.Player1Knight ||
            pieceIdentity.pieceType == ChessPieceType.Player1Bishop || pieceIdentity.pieceType == ChessPieceType.Player1Rook)
        {
            for (int i = 0; i < Team1.Count; i++)
            {
                //check if our team peices are already occupied
                Vector3Int pieceGridLocation = getGridPosition(Team1[i]);
                //Debug.Log("Piece Grid Location: " + pieceGridLocation);
                GameObject pieceGO = Team2[i];

                if (pieceGridLocation.x == destination.x && pieceGridLocation.y == destination.y && pieceGO.active)
                {
                    isBlocked = true;
                    //  Debug.Log("Pawn is blocked");
                    return isBlocked;
                }
            }
        }
        if (pieceIdentity.pieceType == ChessPieceType.Player2Pawn || pieceIdentity.pieceType == ChessPieceType.Player2Bishop ||
            pieceIdentity.pieceType == ChessPieceType.Player2King || pieceIdentity.pieceType == ChessPieceType.Player2Knight ||
            pieceIdentity.pieceType == ChessPieceType.Player2Bishop || pieceIdentity.pieceType == ChessPieceType.Player2Rook)
        {
            for (int i = 0; i < Team2.Count; i++)
            {
                //check if our team peices are already occupied
                Vector3Int pieceGridLocation = getGridPosition(Team2[i]);
                GameObject pieceGO = Team2[i];    
                //Debug.Log("Piece Grid Location: " + pieceGridLocation);

                if (pieceGridLocation.x == destination.x && pieceGridLocation.y == destination.y && pieceGO.active)
                {
                    isBlocked = true;
                    //  Debug.Log("Pawn is blocked");
                    return isBlocked;
                }
            }
        }

        return isBlocked;
    }

    private bool pieceIsDiagnol()
    {
        bool isDiagnol = false;
        PieceIdentity pieceIdentity = selectedPawn.GetComponent<PieceIdentity>();
        Vector3Int selectedPiecePosition = getGridPosition(selectedPawn);
        if (pieceIdentity.pieceType == ChessPieceType.Player1Pawn)
        {
            List<Vector3> adjacent = new List<Vector3>();
            Debug.Log("Entered Diagnol logic");

            for (int i = 0; i < Team2.Count; i++)
            {
                Vector3Int pieceGridLocation = getGridPosition(Team2[i]);
                // Debug.Log("Piece Grid (team2) Location: " + pieceGridLocation);
                // Debug.Log("Selected Piece Position: " + selectedPiecePosition);
                if ((pieceGridLocation.x == selectedPiecePosition.x - 1 && pieceGridLocation.y == selectedPiecePosition.y + 1)
                    || (pieceGridLocation.x == selectedPiecePosition.x - 1 && pieceGridLocation.y == selectedPiecePosition.y - 1))
                {
                    adjacent.Add(pieceGridLocation);
                    isDiagnol = true;
                }
            }
            return isDiagnol;
        }
        return isDiagnol;
    }

    private bool pathIsBlocked(GameObject selectedPawn, Vector3Int hit)
    {
        bool pathIsBlocked = false;
        Vector3Int currentPiecePosition = getGridPosition(selectedPawn);
        PieceIdentity pieceIdentity = selectedPawn.GetComponent<PieceIdentity>();
        Vector3Int selectedPiecePosition = getGridPosition(selectedPawn);
        if (pieceIdentity.pieceType == ChessPieceType.Player1Rook || pieceIdentity.pieceType == ChessPieceType.Player2Rook)
        {
            if (hit.y == currentPiecePosition.y)
            {
                int minX = Mathf.Min(currentPiecePosition.x, hit.x);
                int maxX = Mathf.Max(currentPiecePosition.x, hit.x);


                for (int x = minX + 1; x < maxX; x++)
                {
                    Vector3Int positionToCheck = new Vector3Int(x, currentPiecePosition.y, currentPiecePosition.z);
                    for (int i = 0; i < Team1.Count; i++)
                    {
                        Vector3Int pieceGridLocation = getGridPosition(Team1[i]);
                        if (pieceGridLocation == positionToCheck)
                        {
                            pathIsBlocked = true;
                            return pathIsBlocked;
                        }
                    }
                }
            }
            if (hit.x == currentPiecePosition.x)
            {
                int minY = Mathf.Min(currentPiecePosition.y, hit.y);
                int maxY = Mathf.Max(currentPiecePosition.y, hit.y);

                for (int y = minY + 1; y < maxY; y++)
                {
                    Vector3Int positionToCheck = new Vector3Int(currentPiecePosition.x, y, currentPiecePosition.z);
                    for (int i = 0; i < Team1.Count; i++)
                    {
                        Vector3Int pieceGridLocation = getGridPosition(Team1[i]);
                        if (pieceGridLocation == positionToCheck)
                        {
                            pathIsBlocked = true;
                            return pathIsBlocked;
                        }
                    }
                }
            }
        }
        if (pieceIdentity.pieceType == ChessPieceType.Player1Bishop || pieceIdentity.pieceType == ChessPieceType.Player2Bishop)
        {
            if (hit.x != currentPiecePosition.x && hit.y != currentPiecePosition.y)
            {
                int minX = Mathf.Min(currentPiecePosition.x, hit.x);
                int maxX = Mathf.Max(currentPiecePosition.x, hit.x);


                for (int x = minX + 1; x < maxX; x++)
                {
                    Vector3Int positionToCheck = new Vector3Int(x, currentPiecePosition.y, currentPiecePosition.z);
                    for (int i = 0; i < Team1.Count; i++)
                    {
                        Vector3Int pieceGridLocation = getGridPosition(Team1[i]);
                        if (pieceGridLocation == positionToCheck)
                        {
                            pathIsBlocked = true;
                            return pathIsBlocked;
                        }
                    }
                }
            }

        }
        return pathIsBlocked;
    }
}
  
   





