using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab6
{
    public class Commands
    {
        private bool isFirstValue;
        private int GetMin(int value1, int value2)
        {
            int result = -1;

            if (value1 == -1 && value2 > value1)
                result = value2;
            else if (value2 == -1 && value1 > value2)
                result = value1;
            else if (value1 > -1 && value2 > -1)
            {
                result = value1 <= value2 ? value1 : value2;
            }

            return result;
        }

        private bool IsCharacterFromAlphabet(char ch)
        {
            if (char.IsLetterOrDigit(ch) || ch == '+' || ch == '-' || ch == '*' || ch == '/' || ch == '(' || ch == ')')
                return true;
            return false;
        }

        private bool isNumber(string source)
        {
            bool status = true;

            if (source.Length == 0)
                status = false;
            else
            {
                foreach(char ch in source)
                {
                    if (!char.IsDigit(ch))
                    {
                        status = false;
                        break;
                    }
                }
            }

            return status;
        }

        private bool isId(string source)
        {
            bool status = true;

            if (source.Length == 0)
                status = false;
            else
            {
                bool isFirstCharacter = true;
                foreach (char ch in source)
                {
                    if (isFirstCharacter && !char.IsLetter(ch))
                    {
                        status = false;
                        break;
                    }
                    else if (!char.IsLetterOrDigit(ch))
                    {
                        status = false;
                        break;
                    }
                }
            }

            return status;
        }

        private bool expression(string source)
        {
            if (isFirstValue)
                isFirstValue = false;
            else
                StaticData.mainForm.ResultsTextBox.Text += " - ";

            StaticData.mainForm.ResultsTextBox.Text += "E";

            int openBracketPos = source.IndexOf("(");
            int closeBracketPos = source.LastIndexOf(")");
            string transformedSource;

            if (openBracketPos > -1 && closeBracketPos > -1)
                transformedSource = source.Remove(openBracketPos, closeBracketPos - openBracketPos);
            else if ((openBracketPos > -1 && closeBracketPos == -1) || (openBracketPos == -1 && closeBracketPos > -1))
                return false;
            else
                transformedSource = source;

            int posFirstAddSign = transformedSource.IndexOf("+");
            int posFirstSubSign = transformedSource.IndexOf("-");
            int posSplit = GetMin(posFirstAddSign, posFirstSubSign);

            bool status;

            if (posSplit == -1)
            {
                status = term(source);
                StaticData.mainForm.ResultsTextBox.Text += " - A - eps";
            }
            else
            {
                posSplit = posSplit < openBracketPos ? posSplit : posSplit + closeBracketPos - openBracketPos;

                string substring1 = source.Substring(0, posSplit);
                string substring2 = source.Substring(posSplit + 1, source.Length - 1 - posSplit);

                status = term(substring1);

                StaticData.mainForm.ResultsTextBox.Text += posFirstAddSign == posSplit ? " - +" : " - -";

                status = arithmetic1(substring2);
            }

            return status;
        }

        private bool term(string source)
        {
            StaticData.mainForm.ResultsTextBox.Text += " - T";

            int openBracketPos = source.IndexOf("(");
            int closeBracketPos = source.LastIndexOf(")");
            string transformedSource;

            if (openBracketPos > -1 && closeBracketPos > -1)
                transformedSource = source.Remove(openBracketPos, closeBracketPos - openBracketPos);
            else if ((openBracketPos > -1 && closeBracketPos == -1) || (openBracketPos == -1 && closeBracketPos > -1))
                return false;
            else
                transformedSource = source;

            int posFirstMulSign = transformedSource.IndexOf("*");
            int posFirstDivSign = transformedSource.IndexOf("/");
            int posSplit = GetMin(posFirstMulSign, posFirstDivSign);

            bool status;

            if (posSplit == -1)
            {
                status = symbols(source);
                StaticData.mainForm.ResultsTextBox.Text += " - B - eps";
            }
            else
            {
                posSplit = posSplit < openBracketPos ? posSplit : posSplit + closeBracketPos - openBracketPos;

                string substring1 = source.Substring(0, posSplit);
                string substring2 = source.Substring(posSplit + 1, source.Length - 1 - posSplit);

                status = symbols(substring1);
                StaticData.mainForm.ResultsTextBox.Text += posFirstMulSign == posSplit ? " - *" : " - /";
                status = arithmetic2(substring2);
            }

            return status;
        }

        private bool arithmetic1(string source)
        {
            StaticData.mainForm.ResultsTextBox.Text += " - A";

            bool status = true;

            if (source.Length != 0)
            {
                int openBracketPos = source.IndexOf("(");
                int closeBracketPos = source.LastIndexOf(")");
                string transformedSource;

                if (openBracketPos > -1 && closeBracketPos > -1)
                    transformedSource = source.Remove(openBracketPos, closeBracketPos - openBracketPos);
                else if ((openBracketPos > -1 && closeBracketPos == -1) || (openBracketPos == -1 && closeBracketPos > -1))
                    return false;
                else
                    transformedSource = source;

                int posFirstAddSign = transformedSource.IndexOf("+");
                int posFirstSubSign = transformedSource.IndexOf("-");
                int posSplit = GetMin(posFirstAddSign, posFirstSubSign);

                if (posSplit == -1)
                {
                    status = term(source);
                    StaticData.mainForm.ResultsTextBox.Text += " - A - eps";
                }
                else
                {
                    posSplit = posSplit < openBracketPos ? posSplit : posSplit + closeBracketPos - openBracketPos;

                    string substring1 = source.Substring(0, posSplit);
                    string substring2 = source.Substring(posSplit + 1, source.Length - 1 - posSplit);

                    status = term(substring1);
                    StaticData.mainForm.ResultsTextBox.Text += posFirstAddSign == posSplit ? " - +" : " - -";
                    status = arithmetic1(substring2);
                }
            }

            return status;
        }

        private bool arithmetic2(string source)
        {
            StaticData.mainForm.ResultsTextBox.Text += " - B";

            bool status = true;

            if (source.Length != 0)
            {
                int openBracketPos = source.IndexOf("(");
                int closeBracketPos = source.LastIndexOf(")");
                string transformedSource;

                if (openBracketPos > -1 && closeBracketPos > -1)
                    transformedSource = source.Remove(openBracketPos, closeBracketPos - openBracketPos);
                else if ((openBracketPos > -1 && closeBracketPos == -1) || (openBracketPos == -1 && closeBracketPos > -1))
                    return false;
                else
                    transformedSource = source;

                int posFirstMulSign = transformedSource.IndexOf("*");
                int posFirstDivSign = transformedSource.IndexOf("/");
                int posSplit = GetMin(posFirstMulSign, posFirstDivSign);

                if (posSplit == -1)
                {
                    status = symbols(source);
                    StaticData.mainForm.ResultsTextBox.Text += " - B - eps";
                }
                else
                {
                    posSplit = posSplit < openBracketPos ? posSplit : posSplit + closeBracketPos - openBracketPos;

                    string substring1 = source.Substring(0, posSplit);
                    string substring2 = source.Substring(posSplit + 1, source.Length - 1 - posSplit);

                    status = symbols(substring1);
                    StaticData.mainForm.ResultsTextBox.Text += posFirstMulSign == posSplit ? " - *" : " - /";
                    status = arithmetic2(substring2);
                }
            }

            return status;
        }

        private bool symbols(string source)
        {
            StaticData.mainForm.ResultsTextBox.Text += " - O";

            bool status = true;

            if (source.StartsWith("(") && source.EndsWith(")"))
            {
                status = expression(source.Substring(1, source.Length - 2));
            }
            else
            {
                status = isNumber(source) || isId(source);
                if (isNumber(source))
                    StaticData.mainForm.ResultsTextBox.Text += " - num";
                else if (isId(source))
                    StaticData.mainForm.ResultsTextBox.Text += " - id";
            }

            return status;
        }

        private int checkExpression(string source)
        {
            int status = 0;

            source.Replace(" ", "");

            foreach(char ch in source)
            {
                status++;
                if (!IsCharacterFromAlphabet(ch))
                {
                    return status;
                }
            }

            status = expression(source) ? -1 : -2;

            return status;
        }

        public void CommandCreate()
        {
            if (StaticData.unsaved)
            {
                StaticData.currentData = StaticData.mainForm.TextBox.Text;
                var saveBeforeCloseWindow = new SaveBeforeCloseForm();
                saveBeforeCloseWindow.ShowDialog();
            }

            StaticData.dialogService.FilePath = "";
            StaticData.currentData = "";
            StaticData.mainForm.TextBox.Text = StaticData.currentData;
            StaticData.mainForm.Heading = "Language Processor - unnamed";
        }

        public void CommandOpen()
        {
            if (StaticData.unsaved)
            {
                StaticData.currentData = StaticData.mainForm.TextBox.Text;
                var saveBeforeCloseWindow = new SaveBeforeCloseForm();
                saveBeforeCloseWindow.ShowDialog();
            }

            StaticData.dialogService.OpenFileDialog();
            StaticData.currentData = StaticData.fileService.ReadFile(StaticData.dialogService.FilePath);

            StaticData.mainForm.TextBox.Text = StaticData.currentData;

            StaticData.mainForm.Heading = "Language Processor";
            if (StaticData.dialogService.FilePath != null || StaticData.dialogService.FilePath != "")
                StaticData.mainForm.Heading += " - " + StaticData.dialogService.FilePath;
            else
                StaticData.mainForm.Heading += " - unnamed";

            StaticData.unsaved = false;
        }

        public void CommandSave()
        {
            StaticData.currentData = StaticData.mainForm.TextBox.Text;

            if (StaticData.dialogService.FilePath == null)
            {
                StaticData.dialogService.SaveFileDialog();
                StaticData.fileService.SaveFile(StaticData.dialogService.FilePath, StaticData.currentData);
            }
            else
            {
                StaticData.fileService.SaveFile(StaticData.dialogService.FilePath, StaticData.currentData);
            }

            StaticData.unsaved = false;
            StaticData.mainForm.Heading = "Language Processor - " + StaticData.dialogService.FilePath;
        }

        public void CommandSaveAs()
        {
            StaticData.currentData = StaticData.mainForm.TextBox.Text;
            StaticData.dialogService.SaveFileDialog();
            StaticData.fileService.SaveFile(StaticData.dialogService.FilePath, StaticData.currentData);
            StaticData.mainForm.Heading = "Language Processor - " + StaticData.dialogService.FilePath;
            StaticData.unsaved = false;
        }

        public void CommandUndo()
        {
            if (StaticData.undoStack.Count > 0)
            {
                StaticData.redoStack.Push(StaticData.mainForm.TextBox.Text);
                string newValue = StaticData.undoStack.Pop();
                StaticData.mainForm.TextBox.Text = newValue;
            }
        }

        public void CommandRedo()
        {
            if (StaticData.redoStack.Count > 0)
            {
                StaticData.undoStack.Push(StaticData.mainForm.TextBox.Text);
                string newValue = StaticData.redoStack.Pop();
                StaticData.mainForm.TextBox.Text = newValue;
            }
        }

        public void CommandCopy()
        {
            if (StaticData.mainForm.TextBox.SelectionLength > 0)
                StaticData.mainForm.TextBox.Copy();
        }
        public void CommandPaste()
        {
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == true)
            {
                if (StaticData.mainForm.TextBox.SelectionLength > 0)
                {
                    StaticData.mainForm.TextBox.SelectionStart = StaticData.mainForm.TextBox.SelectionStart + StaticData.mainForm.TextBox.SelectionLength;
                }
                StaticData.mainForm.TextBox.Paste();
            }
        }

        public void CommandCut()
        {
            if (StaticData.mainForm.TextBox.SelectedText != "")
                StaticData.mainForm.TextBox.Cut();
        }

        public void CommandDelete()
        {
            int StartPosDel = StaticData.mainForm.TextBox.SelectionStart;
            int LenSelection = StaticData.mainForm.TextBox.SelectionLength;
            StaticData.mainForm.TextBox.Text = StaticData.mainForm.TextBox.Text.Remove(StartPosDel, LenSelection);
        }

        public void CommandSelectAll()
        {
            StaticData.mainForm.TextBox.SelectAll();
        }

        public void CommandHelp()
        {
            Help.ShowHelp(null, "../../heeelp/help1.html");
        }

        public void CommandCheck()
        {
            string[] strings = StaticData.mainForm.TextBox.Text.Split('\n');

            for (int i = 0; i < strings.Length; i++)
            {
                strings[i] = strings[i].TrimEnd('\r');
            }

            StaticData.mainForm.ResultsTextBox.Text = "";

            for (int i = 0; i < strings.Length; i++)
            {
                StaticData.mainForm.ResultsTextBox.Text += "Line " + (i + 1) + ": ";

                isFirstValue = true;

                int lineStatus = checkExpression(strings[i]);

                StaticData.mainForm.ResultsTextBox.Text += Environment.NewLine;

            }

        }
    }
}
