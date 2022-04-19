using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MarsFPSKit
{
    namespace UI
    {
        [CreateAssetMenu(menuName = "MarsFPSKit/Options/Controls/Rebinding Controls")]
        public class Kit_OptionsRebindingControls : Kit_OptionBase
        {
            [SerializeField] public string rebindType = "";

            private Kit_OptionsButtonScript buttonScript;

            public override string GetDisplayName()
            {
                return rebindType;
            }

            public override string GetHoverText()
            {
                return "Click to rebind the key for " + rebindType;
            }

            public override OptionType GetOptionType()
            {
                return OptionType.Button;
            }

            public override void OnCreate(TextMeshProUGUI txt, Button button)
            {
                buttonScript = button.GetComponent<Kit_OptionsButtonScript>();
                TextMeshProUGUI bTxt = button.GetComponentInChildren<TextMeshProUGUI>();
                bTxt.text = PlayerPrefs.GetString(rebindType, "Undefined");
            }

            public override void OnButtonChange(TextMeshProUGUI txt, Button button)
            {
                buttonScript.StartRebinding(rebindType);
            }
        }
    }
}