using TMPro;
using UnityEngine;

namespace UI
{
    public class MenuCanvasView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nicknameText;
        
        public void UpdateNicknameText(string nickname)
        {
            nicknameText.text = nickname;
        }
    }
}
