using UnityEngine;

public class SelectionSystem {

    private static SelectionSystem _Instance;
    public static SelectionSystem Instance { get => _Instance == null ? (_Instance = new SelectionSystem()) : _Instance; }

    public BaseObject SelectedTile{ get; private set; }
    public bool IsAnyTileSelected { get => SelectedTile != null; }

    public void ReleaseObject()
    {
        SelectedTile = null;
    }

    public void Action()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        SelectedTile = Utils.Inputs.ScreenToObject;
    }
}
