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
    }
}