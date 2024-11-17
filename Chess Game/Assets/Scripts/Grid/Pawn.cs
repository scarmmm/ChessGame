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
    private Vector3 playerVelocity;
    private Transform cameraMain;
    Camera Camera;
    // Start is called before the first frame update
    void Start()
    {
        cameraMain = Camera.main.transform;
        //Cursor.visible = false;
        Vector3 worldPosition = _grid.GetCellCenterWorld(new Vector3Int(0, 0, 0)); // Get the world position of the cell at (0, 0)
        Debug.Log("World Position: " + worldPosition);
        worldPosition.y = 0;
        transform.position = worldPosition; // Set the position of the pawn to the world position
        Instantiate(_pawn, worldPosition, Quaternion.identity);
        for (int i = 1; i < 8; i++) 
        {
            Vector3 worldPosition2 = _grid.GetCellCenterWorld(new Vector3Int(0, i, 0));
            worldPosition2.y = 0;
            transform.position = worldPosition2; // Set the position of the pawn to the world position
            GameObject pawnInstance = Instantiate(_pawn, worldPosition2, Quaternion.identity);
            BoxCollider boxCollider = pawnInstance.AddComponent<BoxCollider>();
            Targeter targeter = pawnInstance.AddComponent<Targeter>();
            targeter.renderer = pawnInstance.GetComponent<Renderer>();
            pawnInstance.tag = "Pawn";
            pawnInstance.layer = LayerMask.NameToLayer("Chessboard"); //for the raycast
            pawns.Add(pawnInstance);
            
        }
        
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
            if (Input.GetMouseButtonDown(0))
            {
                // this if statement is to select the piece 
                if (selectedPawn == null)
                {
                    // see if we got a hit and the tag is "pawn"
                    if (hit.collider != null && hit.collider.gameObject.CompareTag("Pawn"))
                    {
                        selectedPawn = hit.collider.gameObject; // Select the pawn
                        Debug.Log("Selected Pawn: " + selectedPawn.name);
                        Debug.Log("Selected Pawn Position: " + selectedPawn.transform.position);
                        Debug.Log("Selected Pawn Grid Position: " + gridPosition);
                    }
                    else 
                    {
          
                    }
                }
                else
                {
                    // move the selected pawn to the new position
                    if (isValidPosition(worldPosition2))
                        {
                        Debug.Log("Moving selected pawn to: " + worldPosition2);
                        selectedPawn.transform.position = worldPosition2;
                        selectedPawn = null; // deselect the pawn after moving
                        }
                }
            }
        }

    }
    private bool isValidPosition(Vector3 pos) 
    {
        bool isValid = false;
        if (selectedPawn.CompareTag("Pawn"))
        {
           if (pos.x == selectedPawn.transform.position.x + 1 || pos.x == selectedPawn.transform.position.x - 1)
            {
                Debug.Log("Valid Position: " + pos);
                return isValid = true;
            }

            return isValid;
        }
        //will add more logic for other pieces but should be just as simple as this

        return isValid;
    }


    private void GetCharacterPosition()
    {
        Vector3 characterPosition = transform.position;
        Debug.Log("Character Position: " + characterPosition);
    }
}
