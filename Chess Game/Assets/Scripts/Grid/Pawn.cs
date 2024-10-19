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
    [SerializeField] private GameObject _pawnPrefab;
    [SerializeField] private GameObject _position;
    [SerializeField] private float rotationSpeed = 5.0f;
    [SerializeField] private InputReader inputReader;
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
        Instantiate(_pawnPrefab, worldPosition, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
        //GetCharacterPosition();
        setPosition();
    }
    private void UpdateMovement()
    {
        Vector3 move = new Vector3(inputReader.MovementValue.x, 0, inputReader.MovementValue.y);
        move = cameraMain.forward * move.z + cameraMain.right * move.x;
        move.y = 0;
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero) //check if the character is moving
        {
            Quaternion targetRotation = Quaternion.LookRotation(move); //store the calculated move direction
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed); //rotate the character to the move (target) direction
        }
        else
        {

        }

        controller.Move(playerVelocity * Time.deltaTime);
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
            Vector3 worldPosition2 = _grid.GetCellCenterWorld(gridPosition); // Get the center of the cell at the grid position
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Grid Position that we clicked on: " + gridPosition);
                Debug.Log("World Position of that click is: " + worldPosition2);
                Instantiate(_pawnPrefab, worldPosition2, Quaternion.identity); //instantiate the pawn at the grid position
               // _pawnPrefab.transform.position = worldPosition2; //set the position of the pawn to the world position
          
            }
        }

    }
    private void GetCharacterPosition()
    {
        Vector3 characterPosition = transform.position;
        Debug.Log("Character Position: " + characterPosition);
    }
}
