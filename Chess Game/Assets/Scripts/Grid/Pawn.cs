using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class Pawn : MonoBehaviour
{
    [SerializeField] private int _playerNumber; // Player number for the pawn
    [SerializeField] public CharacterController controller; // Reference to the CharacterController component
    [SerializeField] public Grid _grid;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private GameObject _pawn;
    [SerializeField] private GameObject _rook;
    [SerializeField] private GameObject _position;
    [SerializeField] private float rotationSpeed = 5.0f;
    [SerializeField] private InputReader inputReader;
    private GameObject selectedPawn;
    private List<GameObject> pawns = new List<GameObject>(); // List to store pawns
    private List<GameObject> rooks = new List<GameObject>(); // List to store rooks
    private List<GameObject> pawns2 = new List<GameObject>(); // List to store pawns for player 2
    private List<GameObject> rooks2 = new List<GameObject>(); // List to store rooks for player 2
    private Vector3 playerVelocity;
    private Transform cameraMain;
    Camera Camera;
    // Start is called before the first frame update
    void Start()
    {
        cameraMain = Camera.main.transform;
        //Cursor.visible = false;
        Vector3 worldPosition = _grid.GetCellCenterWorld(new Vector3Int(0, 0, 0)); // Get the world position of the cell at (0, 0)
       // Debug.Log("World Position: " + worldPosition);
        worldPosition.y = 0;
        transform.position = worldPosition; // Set the position of the pawn to the world position
        Instantiate(_pawn, worldPosition, Quaternion.identity);
        //player 1 pawns
        for (int i = 1; i < 8; i++) 
        {
            Vector3 worldPosition2 = _grid.GetCellCenterWorld(new Vector3Int(0, i, 0));
            Vector3Int gridPosition = _grid.WorldToCell(worldPosition2);
            worldPosition2.y = 0;
            transform.position = worldPosition2; // Set the position of the pawn to the world position
            GameObject pawnInstance = Instantiate(_pawn, worldPosition2, Quaternion.identity);
            BoxCollider boxCollider = pawnInstance.AddComponent<BoxCollider>();
            Targeter targeter = pawnInstance.AddComponent<Targeter>();
            targeter.renderer = pawnInstance.GetComponent<Renderer>();
            PieceIdentity id = pawnInstance.AddComponent<PieceIdentity>();
            id.pieceType = ChessPieceType.Player1Pawn;
            //pawnInstance.tag = "Pawn";  
            pawnInstance.layer = LayerMask.NameToLayer("Chessboard"); //for the raycast
            pawns.Add(pawnInstance);
            
        }
        //player 1 rooks
        for (int i = 0; i < 9; i += 7)
        {
            Vector3 worldPosition2 = _grid.GetCellCenterWorld(new Vector3Int(1, i, 0));
            Vector3Int gridPosition = _grid.WorldToCell(worldPosition2);
            worldPosition2.y = 0;
            transform.position = worldPosition2; // Set the position of the rook to the world position
            GameObject rookInstance = Instantiate(_rook, worldPosition2, Quaternion.identity);
            BoxCollider boxCollider = rookInstance.AddComponent<BoxCollider>();
            Targeter targeter = rookInstance.AddComponent<Targeter>();
            targeter.renderer = rookInstance.GetComponent<Renderer>();
            rookInstance.transform.localScale = new Vector3(3f, 3f, 3f);
            PieceIdentity id = rookInstance.AddComponent<PieceIdentity>();
            id.pieceType = ChessPieceType.Player1Rook;
            rookInstance.layer = LayerMask.NameToLayer("Chessboard"); //for the raycast
            rooks.Add(rookInstance);
        }

        //player 2 pawns
        for (int i = 0; i < 8; i++)
        {
            Vector3 worldPosition2 = _grid.GetCellCenterWorld(new Vector3Int(-5, i, 0));
            Vector3Int gridPosition = _grid.WorldToCell(worldPosition2);
            worldPosition2.y = 0;
            transform.position = worldPosition2; // Set the position of the pawn to the world position
            GameObject pawnInstance = Instantiate(_pawn, worldPosition2, Quaternion.identity);
            BoxCollider boxCollider = pawnInstance.AddComponent<BoxCollider>();
            Targeter targeter = pawnInstance.AddComponent<Targeter>();
            targeter.renderer = pawnInstance.GetComponent<Renderer>();
           // pawnInstance.tag = "Pawn";
            //pawnInstance.tag = "Player2";   
            pawnInstance.layer = LayerMask.NameToLayer("Chessboard"); //for the raycast
            pawns2.Add(pawnInstance);
        }
        //player 2 rooks


    }

    // Update is called once per frame
    void Update()
    {
        setPosition();
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
            Vector3 worldMousePosition = hit.point; //get the world position of the mouse
            Vector3Int gridPosition = _grid.WorldToCell(worldMousePosition); //convert the world position to a grid position(this works)
            Vector3 worldPosition2 = _grid.GetCellCenterWorld(gridPosition); // Get the center of the cell at the grid position and move the peice to that position
            float posy = gridPosition.y;
            if (Input.GetMouseButtonDown(0))
            {
                // this if statement is to select the piece 
                if (selectedPawn == null)
                {
                    // see if we got a hit and the tag is "pawn"
                    PieceIdentity piece = hit.collider.gameObject.GetComponent<PieceIdentity>();
                    if (hit.collider != null && (piece.pieceType == ChessPieceType.Player1Pawn || piece.pieceType == ChessPieceType.Player2Pawn))
                    {
                        selectedPawn = hit.collider.gameObject; // Select the pawn
                        Debug.Log("Selected Pawn Grid Position: " + gridPosition);
                  
                    }
                    else 
                    {
          
                    }
                }
                else
                {
                        // move the selected pawn to the new position, if the position is valid
                    Vector3Int grid = getGridPosition(selectedPawn);
                    if (isValidPosition(grid,gridPosition))
                        {
                        selectedPawn.transform.position = _grid.GetCellCenterWorld(gridPosition);
                        //Vector3Int worldPositionGrid = _grid.WorldToCell(worldPosition2);
                        //Debug.Log("Move Valid:" + worldPositionGrid);
                        //selectedPawn.transform.position = worldPosition2;
                        selectedPawn = null; // deselect the pawn after moving
                        }
                    selectedPawn = null;
                }
            }
        }

    }
    private bool isValidPosition(Vector3Int gridPosition, Vector3Int hitPosition) 
    {
        bool isValid = false;
        PieceIdentity piece = selectedPawn.GetComponent<PieceIdentity>();
        if (piece.pieceType == ChessPieceType.Player1Pawn || piece.pieceType == ChessPieceType.Player2Pawn)
        {
            Vector3Int selectedPawnGridPos = getGridPosition(selectedPawn);
            // Check if target is adjacent on X and remains on the same Y
            if ((hitPosition.x == selectedPawnGridPos.x + 1 ||
                 hitPosition.x == selectedPawnGridPos.x - 1) &&
                hitPosition.y == selectedPawnGridPos.y)
            {
               Debug.Log("Valid Position: " + selectedPawnGridPos);
                return isValid = true;
            }
            else {Debug.Log("Invalid Position: " + selectedPawnGridPos); }

            return isValid;
        }
        if (selectedPawn.CompareTag("Rook"))
        {
            Vector3Int selectedRookGridPos = getGridPosition(selectedPawn);
            // Check if target is adjacent on X and remains on the same Y
            if ((hitPosition.x == selectedRookGridPos.x + 1 ||
                 hitPosition.x == selectedRookGridPos.x - 1))
            {
                Debug.Log("Valid Position: " + selectedRookGridPos);
                return isValid = true;
            }
            else { Debug.Log("Invalid Position: " + selectedRookGridPos); }

            return isValid;
        }
        //will add more logic for other pieces but should be just as simple as this

        return isValid;
    }

    Vector3Int getGridPosition (GameObject pawn)
    {
        Vector3Int gridPosition = _grid.WorldToCell(pawn.transform.position);
        Debug.Log("getGridPosition(): " + gridPosition);
        return gridPosition;
    }
    private void GetCharacterPosition()
    {
        Vector3 characterPosition = transform.position;
        Debug.Log("Character Position: " + characterPosition);
    }
}
