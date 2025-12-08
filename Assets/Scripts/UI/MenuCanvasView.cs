using System.Collections;
using TMPro;
using UnityEngine;

namespace UI
{
    public class MenuCanvasView : MonoBehaviour
    {
        [Header("Input nickname elements")]
        [SerializeField] private TextMeshProUGUI nicknameText;
        [SerializeField] private TextMeshProUGUI informationText;
        
        [Header("UI Containers (Vertical Layout Groups)")]
        [SerializeField] private Transform rankColumnContainer;
        [SerializeField] private Transform nicknameColumnContainer;
        [SerializeField] private Transform timeColumnContainer;
        [SerializeField] private Transform attemptsColumnContainer;
        
        [Header("Rows prefabs")]
        public GameObject numberTextPrefab;  
        public GameObject nicknameTextPrefab;
        
        private Coroutine _activeCoroutine;
        
        public void UpdateNicknameText(string nickname)
        {
            nicknameText.text = nickname;
        }

        public void ShowInformationText(string content, Color textColor, float fadeDuration = 2.0f)
        {
            if (_activeCoroutine != null)
            {
                StopCoroutine(_activeCoroutine);
            }

            _activeCoroutine = StartCoroutine(ShowAndFadeRoutine(content, textColor, fadeDuration));
        }

        private IEnumerator ShowAndFadeRoutine(string content, Color targetColor, float duration)
        {
            informationText.text = content;
            targetColor.a = 1f;
            informationText.color = targetColor;
            informationText.gameObject.SetActive(true);
            
            yield return new WaitForSeconds(0.5f);
            
            float timer = 0f;
            
            while (timer < duration)
            {
                timer += Time.deltaTime;
                
                float newAlpha = Mathf.Lerp(1f, 0f, timer / duration);
                informationText.color = new Color(targetColor.r, targetColor.g, targetColor.b, newAlpha);
                
                yield return null;
            }
            
            informationText.color = new Color(targetColor.r, targetColor.g, targetColor.b, 0f);
            informationText.gameObject.SetActive(false);
        }
        
        public void CreateRow(int rank, string nickname, int time, int attempts)
        {
            CreateTextObject(rankColumnContainer, numberTextPrefab, rank + ".");
            CreateTextObject(nicknameColumnContainer, nicknameTextPrefab, nickname);
            CreateTextObject(timeColumnContainer, numberTextPrefab,
                time == int.MaxValue ? "-" : time + "s");
            CreateTextObject(attemptsColumnContainer, numberTextPrefab, attempts.ToString());
        }
        
        private void CreateTextObject(Transform container, GameObject prefab, string content)
        {
            GameObject newObject = Instantiate(prefab, container);
            TextMeshProUGUI text = newObject.GetComponent<TextMeshProUGUI>();
        
            text.text = content;
        }
        
        public void ClearLeaderboard()
        {
            ClearContainer(rankColumnContainer);
            ClearContainer(nicknameColumnContainer);
            ClearContainer(timeColumnContainer);
            ClearContainer(attemptsColumnContainer);
        }
        
        private void ClearContainer(Transform container)
        {
            foreach (Transform child in container)
            {
                Destroy(child.gameObject);
            }
        }
    }
}