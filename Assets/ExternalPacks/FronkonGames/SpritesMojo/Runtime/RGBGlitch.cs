///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Fronkon Games @FronkonGames <fronkongames@gmail.com>. All rights reserved.
//
// THIS FILE CAN NOT BE HOSTED IN PUBLIC REPOSITORIES.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;

namespace FronkonGames.SpritesMojo
{
  /// <summary>
  /// Simulates a failure in the color channels.
  /// </summary>
  public static class RGBGlitch
  {
    /// <summary>
    /// Channel displacement force, default 0.1 [0.0 - 1.0].
    /// </summary>
    /// <returns>Value.</returns>
    public static readonly FloatRangeVariable Amplitude = new FloatRangeVariable("_Amplitude", 0.1f, 0.0f, 1.0f);

    /// <summary>
    /// Channel displacement velocity, default 0.5 [0.0 - 2.0].
    /// </summary>
    /// <returns>Value.</returns>
    public static readonly FloatPositiveVariable Speed = new FloatPositiveVariable("_Speed", 0.5f);

    /// <summary>
    /// Create a sprite with a NEW material with the effect.
    /// </summary>
    /// <param name="name">Sprite name.</param>
    /// <param name="parent">Sprite parent.</param>
    /// <returns>New GameObject.</returns>
    public static GameObject CreateSprite(string name = default, Transform parent = null) => SpriteMojo.CreateSprite(CreateMaterial(), name, parent);

    /// <summary>
    /// Create a NEW material with the effect.
    /// </summary>
    /// <returns>New material.</returns>
    public static Material CreateMaterial()
    {
      Material material = SpriteMojo.CreateMaterial("Shaders/RGBGlitch/RGBGlitchSprite");
      Reset(material);

      return material;
    }

    /// <summary>
    /// Reset the effect values of a sprite.
    /// </summary>
    /// <param name="sprite">Sprite.</param>
    public static void Reset(SpriteRenderer sprite) => Reset(sprite.material);

    /// <summary>
    /// Reset the effect values of a material.
    /// </summary>
    /// <param name="material">Material.</param>
    public static void Reset(Material material)
    {
      Amplitude.Reset(material);
      Speed.Reset(material);
    }
  }
}