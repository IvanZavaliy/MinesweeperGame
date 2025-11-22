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
        public bool IsOpened { set => isOpened = value; }
        public bool IsFlagged { get => isFlagged; }
        public int XCoord { get => xCoord; }
        public int YCoord { get => yCoord; }
        public GameObject CellInstance { get => cellInstance; set => cellInstance = value; }
        
        public Cell(int xCoord, int yCoord)
        {
            this.xCoord = xCoord;
            this.yCoord = yCoord;
        }
        
        public CellStatusResult CellStatus()
        {
            if (isOpened || isFlagged) return CellStatusResult.None;

            if (isBomb) return CellStatusResult.GameOver;
            
            if (!isOpened) return CellStatusResult.Closed;
            
            return CellStatusResult.Opened;
        }

        public SetBombFlagResult SetBombFlag()
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