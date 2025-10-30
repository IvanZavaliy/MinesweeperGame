using UnityEngine;

namespace DefaultNamespace
{
    public class Cell
    {
        private int xCoord;
        private int yCoord;
        private bool isBomb;
        private bool isOpened = false;
        private bool isFlagged = false;
        private GameObject cellInstance;
        
        public bool IsBomb { get => isBomb; set => isBomb = value; }
        public int XCoord { get => xCoord; }
        public int YCoord { get => yCoord; }
        public GameObject CellInstance { get => cellInstance; set => cellInstance = value; }
        
        public Cell(int xCoord, int yCoord)
        {
            this.xCoord = xCoord;
            this.yCoord = yCoord;
        }
        
        public OpenCellResult OpenCell()
        {
            if (isOpened || isFlagged) return OpenCellResult.None;

            if (isBomb) return OpenCellResult.GameOver;
            
            isOpened = true;
            return OpenCellResult.Opened;
        }

        public SetBombFlagResult SetbombFlag()
        {
            if (isOpened) return SetBombFlagResult.None;

            if (isFlagged)
            {
                isFlagged = false;
                return SetBombFlagResult.Unsetted;
            }
            
            isFlagged = true;
            return SetBombFlagResult.Setted;
        }
    }
}