using UnityEngine;
using UnityEngine.UI;

using FronkonGames.SpritesMojo;
using Outline = FronkonGames.SpritesMojo.Outline;

public partial class Deck : MonoBehaviour
{
  [Space(10.0f), SerializeField]
  private RectTransform outlineCanvas;

  [SerializeField] private Slider sliderOutlineSize;

  private void ChangeOutline(Material material)
  {
    Outline.Size.Set(material, 30, timeToUpdate);
    Outline.Mode.Set(material, (OutlineMode)Random.Range(0, 3));

    float hue = Random.Range(0.0f, 1.0f);
    Outline.Color0.Set(material, Color.HSVToRGB(hue, 1.0f, 1.0f), timeToUpdate);
    Outline.Color1.Set(material, Color.HSVToRGB(1.0f - hue, 1.0f, 1.0f), timeToUpdate);

    if (patterns.Length > 0)
      Outline.Texture.Set(material, patterns[Random.Range(0, patterns.Length)]);

    Outline.TextureVelocity.Set(material, new Vector2(Random.Range(-randomRange, randomRange), Random.Range(-randomRange, randomRange)) * 0.01f, timeToUpdate);
    Outline.Vertical.Set(material, Random.Range(0.0f, 1.0f) >= 0.5f);
  }

  private void UpdateCanvasOutline()
  {
    sliderAmount.gameObject.SetActive(false);
    outlineCanvas.gameObject.SetActive(true);

    UpdateSlider(sliderOutlineSize, Outline.Size, 30, selectedCard.Material);
  }
}
