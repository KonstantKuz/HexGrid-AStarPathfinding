using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    private HexCell currentCell;
    private HexCell start;
    private HexCell end;

    public HexCell Start
    {
        get { return start; }
    }
    public HexCell End
    {
        get { return end; }
    }

    private void OnTriggerEnter(Collider other)
    {
        currentCell = other.GetComponent<HexCell>();
        SetSelectedColorToCurrentCell();
    }

    private void SetSelectedColorToCurrentCell()
    {
        if (currentCell != null && currentCell.IsOpened)
        {
            currentCell.SetColor(currentCell.CellColors.selectedCellColor);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        currentCell = other.GetComponent<HexCell>();
    }
    
    private void OnTriggerExit(Collider other)
    {
        currentCell = other.GetComponent<HexCell>();
        TrySetNormalColorToCurrentCell();
    }

    private void TrySetNormalColorToCurrentCell()
    {
        if (currentCell != null && currentCell.IsOpened && currentCell != start && currentCell != end)
        {
            currentCell.SetColor(currentCell.CellColors.normalCellColor);
        }
    }

    private void Update()
    {
        FollowCursor();

        HandleInputOnCurrentCell();
    }

    private void FollowCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            transform.position = hit.point;
        }
    }

    private void HandleInputOnCurrentCell()
    {
        if (currentCell == null)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            SetCurrentCellAsStart();
        }

        if (Input.GetMouseButtonDown(1))
        {
            SetCurrentCellAsEnd();
        }

        if (!Input.GetMouseButton(2))
        {
            return;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            SetCurrentCellOpened();
        }
        else
        {
            SetCurrentCellClosed();
        }
    }

    private void SetCurrentCellAsStart()
    {
        start?.SetColor(currentCell.CellColors.normalCellColor);
        start = currentCell;
        currentCell.SetColor(currentCell.CellColors.startCellColor);
        EventHolder.OnSomeCellChanged();
    }

    private void SetCurrentCellAsEnd()
    {
        end?.SetColor(currentCell.CellColors.normalCellColor);
        end = currentCell;
        currentCell.SetColor(currentCell.CellColors.endCellColor);
        EventHolder.OnSomeCellChanged();
    }

    private void SetCurrentCellOpened()
    {
        currentCell.SetCellStatus(true);
    }

    private void SetCurrentCellClosed()
    {
        currentCell.SetCellStatus(false);
    }
}
