using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class FloatingText : MonoBehaviour
{
    public TextMeshProUGUI floatTextPrint;
    
    [SerializeField]private float _moveSpeed = 1f;
    [SerializeField]private float _biggerSpeed = 1f;//위로 움직이는 속도값
    [SerializeField]private float _destroyTime = 1f;
    private float _time;
    private Vector3 _vector;
    private Vector3 _originalScale;
    private void Awake()
    {
        floatTextPrint = this.GetComponent<TextMeshProUGUI>();
        _originalScale = floatTextPrint.transform.localScale;
    }

    private void OnEnable()
    {
        floatTextPrint.transform.localScale = _originalScale;
        gameObject.GetComponent<RectTransform>().SetAsFirstSibling();
        _time = 0;
    }
    void Update()
    {
        _vector.Set(floatTextPrint.transform.position.x, floatTextPrint.transform.position.y + (_moveSpeed + Time.deltaTime), floatTextPrint.transform.position.z);
        floatTextPrint.transform.position = _vector;
        floatTextPrint.transform.localScale += Vector3.one * (Time.deltaTime * _biggerSpeed);
        _time += Time.deltaTime;
        if(_time >=_destroyTime)
        {
            gameObject.SetActive(false);
        }
    }
    public void SetText(string Text)
    {
        floatTextPrint.text =  string.Format(" {0}",Text);
    }
}
