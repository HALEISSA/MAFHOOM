using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    [SerializeField] private float hoverScale = 1.08f;
    [SerializeField] private float speed = 8f;

    private bool isHovering;

    private Image img;
    [SerializeField] private Color hoverColor = new Color(1f, 1f, 1f, 1f);
    private Color originalColor;

    private void Awake()
    {
        originalScale = transform.localScale;

        img = GetComponent<Image>();
        if (img != null)
            originalColor = img.color;
    }

    private void Update()
    {
        Vector3 targetScale = isHovering ? originalScale * hoverScale : originalScale;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * speed);

        if (img != null)
        {
            Color targetColor = isHovering ? hoverColor : originalColor;
            img.color = Color.Lerp(img.color, targetColor, Time.deltaTime * speed);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }
}