namespace AvaloniaUI.Models
{
    public class CheckBoxState
    {
        public CheckBoxState(string checkBoxText, bool checkBoxValue)
        {
            CheckBoxText = checkBoxText;
            CheckBoxValue = checkBoxValue;
        }
        
        public string CheckBoxText { get; set; }
        public bool CheckBoxValue { get; set; }
    }
}