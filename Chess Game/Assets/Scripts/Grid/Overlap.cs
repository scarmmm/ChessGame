using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overlap : MonoBehaviour
{
    Pawn pawn;
    private void Start()
    {
        pawn = FindObjectOfType<Pawn>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ChessBoard"))
        {
            return;
        }
       
        if (pawn == null || pawn.lastSelectedPiece == null)
        {
            return;
        }

        // THE HERE IS THE KEY
        // THIS WILL TERMINATE THE FUNCTION OF THE OTHER PIECE SINCE IT WAS NOT THE LAST SELECTED PIECE
        if (gameObject != pawn.lastSelectedPiece)
        {
            Debug.Log("Function terminated");
            return; 
        }

        if (other.gameObject == pawn.lastSelectedPiece)
        {
            return;
        }

        // Check if the other object's tag is NOT the same as this object's tag
        if (!other.CompareTag(gameObject.tag) && !other.CompareTag("ChessBoard"))
        {
            Debug.Log($"Last selected piece is: {pawn.lastSelectedPiece.name}");
            Debug.Log($"{gameObject.name} overlaps with {other.name}, which has a different tag: {other.tag}");

            // Destroy the other object
            Debug.Log($"The object destroyed is: {other.gameObject.name}");
            other.gameObject.tag = pawn.eliminatedPlayer;
            Destroy(other.gameObject, 0.1f);

            // Move the selected piece to the position of the destroyed piece
            Vector3 otherPiecePos = other.gameObject.transform.position;
            gameObject.transform.position = otherPiecePos;
        }
        else
        {
            Debug.Log($"Ignored overlap with {other.name} because it has the same tag: {gameObject.tag}");
        }
    }


    private void OnTriggerStay(Collider other)
    {
        // Continue checking for differences in tags
        if (!other.CompareTag(gameObject.tag))
        {
      
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(gameObject.tag))
        {
           
        }
    }
}
