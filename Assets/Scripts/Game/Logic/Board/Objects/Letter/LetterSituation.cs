public enum LetterSituation
{
    DEALLOCATED, // This is the default area, where the tile is not allocated to any area
    PLACED, // This is the area where the tile is placed
    HIGHLIGHTED, // This is the area where the tile is highlighted
    SELECTABLE,  // This is the area where the tile is selectable by the player
    UNSELECTABLE // This is the area where the tile is not selectable by the player (e.g. occupied by another tile)
}
