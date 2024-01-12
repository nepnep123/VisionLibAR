using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

namespace Visometry.VisionLib.SDK.Examples
{
    /// <summary>
    ///  The InvariantCultureTextField makes it possible to set the text
    ///  of a Text component using different parameter types.
    /// </summary>
    /// @ingroup Examples
    [RequireComponent(typeof(Text))]
    [AddComponentMenu("VisionLib/Examples/Invariant Culture TextField")]
    public class InvariantCultureTextField : MonoBehaviour
    {
        /// <summary>
        ///  Format specifies for the conversion to a string.
        /// </summary>
        /// <remarks>
        ///  See the documentation for "Standard Numeric Format Strings" in C# for
        ///  further details.
        /// </remarks>
        public string formatSpecifier;

        private Text textComponent;
        private string text = string.Empty;

        private void UpdateTextComponent()
        {
            if (this.textComponent)
            {
                this.textComponent.text = this.text;
            }
        }

        private void Awake()
        {
            this.textComponent = GetComponent<Text>();
            this.UpdateTextComponent();
        }

        /// <summary>
        ///  Sets the text using a string.
        /// </summary>
        public void SetText(string value)
        {
            this.text = value;
            this.UpdateTextComponent();
        }

        /// <summary>
        ///  Sets the text using an integer.
        /// </summary>
        public void SetText(int value)
        {
            this.text = value.ToString(this.formatSpecifier, CultureInfo.InvariantCulture);
            this.UpdateTextComponent();
        }

        /// <summary>
        ///  Sets the text using a floating point number.
        /// </summary>
        public void SetText(float value)
        {
            this.text = value.ToString(this.formatSpecifier, CultureInfo.InvariantCulture);
            this.UpdateTextComponent();
        }

        /// <summary>
        ///  Sets the text using a boolean.
        /// </summary>
        public void SetText(bool value)
        {
            this.text = value.ToString();
            this.UpdateTextComponent();
        }
    }
}
