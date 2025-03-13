using System.Windows.Forms;
using System;

namespace Lab1
{
    public class Scanner
    {
        private string[] keywords = { "type", "record", "end", "integer", "real", "char", "boolean", "string", "var" };
        private string[] operators = { "=", ":", ",", ";" };
        private string[] separators = { " ", "\t", "\n", "\r" };

        public void Analyze(string text, DataGridView dataGridView, RichTextBox richTextBox)
        {
            int lineNumber = 1;
            int position = 0;
            int start = 0;
            int length = text.Length;
            bool lastWasKeyword = false;

            while (start < length)
            {
                char currentChar = text[start];

                if (IsSeparator(currentChar))
                {
                    if (currentChar == '\n')
                    {
                        lineNumber++;
                        position = 0;
                        start++;
                        continue;
                    }
                    else if (currentChar == ' ')
                    {
                        int spaceStart = position;
                        while (start < length && text[start] == ' ')
                        {
                            start++;
                            position++;
                        }

                        // Если пробел идет между двумя ключевыми словами, засчитываем его
                        if (lastWasKeyword)
                        {
                            AddTokenToDataGridView(dataGridView, " ", "(пробел)", lineNumber, spaceStart, position);
                            lastWasKeyword = false;  // сбрасываем флаг после обработки пробела
                        }

                        continue;
                    }
                }

                if (IsOperator(currentChar))
                {
                    string token = currentChar.ToString();
                    int endPosition = position + 1;
                    AddTokenToDataGridView(dataGridView, token, "Оператор", lineNumber, position, endPosition);
                    start++;
                    position++;
                    lastWasKeyword = false;
                    continue;
                }

                if (char.IsLetter(currentChar))
                {
                    int end = start;
                    while (end < length && (char.IsLetterOrDigit(text[end]) || text[end] == '_'))
                    {
                        end++;
                    }

                    string token = text.Substring(start, end - start);
                    string tokenType = IsKeyword(token) ? "Ключевое слово" : "Идентификатор";
                    int endPosition = position + token.Length;
                    AddTokenToDataGridView(dataGridView, token, tokenType, lineNumber, position, endPosition);

                    start = end;
                    position = endPosition;
                    lastWasKeyword = tokenType == "Ключевое слово";
                    continue;
                }

                if (char.IsDigit(currentChar))
                {
                    int end = start;
                    bool isReal = false;

                    while (end < length && (char.IsDigit(text[end]) || text[end] == '.' || text[end] == ','))
                    {
                        if (text[end] == '.' || text[end] == ',')
                        {
                            if (isReal || end + 1 >= length || !char.IsDigit(text[end + 1]))
                                break;
                            isReal = true;
                        }
                        end++;
                    }

                    string token = text.Substring(start, end - start);
                    int endPosition = position + token.Length;
                    string tokenType = isReal ? "Вещественное число" : "Целое без знака";

                    AddTokenToDataGridView(dataGridView, token, tokenType, lineNumber, position, endPosition);

                    start = end;
                    position = endPosition;
                    lastWasKeyword = false;
                    continue;
                }

                // Ошибка: недопустимый символ
                int endPositionError = position + 1;
                AddTokenToDataGridView(dataGridView, currentChar.ToString(), "Ошибка", lineNumber, position, endPositionError);

                // Подсветка ошибки в RichTextBox
                HighlightError(richTextBox, start, 1);

                start++;
                position++;
                lastWasKeyword = false;
            }
        }


        private bool IsSeparator(char ch)
        {
            return Array.Exists(separators, s => s[0] == ch);
        }

        private bool IsOperator(char ch)
        {
            return Array.Exists(operators, op => op[0] == ch);
        }

        private bool IsKeyword(string token)
        {
            return Array.Exists(keywords, kw => kw == token);
        }

        private string GetNextWord(string text, int start)
        {
            int end = start;
            while (end < text.Length && (char.IsLetterOrDigit(text[end]) || text[end] == '_'))
            {
                end++;
            }
            return text.Substring(start, end - start);
        }

        private void AddTokenToDataGridView(DataGridView dataGridView, string token, string tokenType, int lineNumber, int startPos, int endPos)
        {
            string positionRange = $"с {startPos} по {endPos - 1} символ";
            dataGridView.Rows.Add(GetTokenCode(token, tokenType), tokenType, token, lineNumber, positionRange);
        }

        private int GetTokenCode(string token, string tokenType)
        {
            switch (tokenType)
            {
                case "Ключевое слово":
                    switch (token)
                    {
                        case "type": return 1;
                        case "record": return 2;
                        case "end": return 3;
                        case "integer": return 4;
                        case "real": return 5;
                        case "char": return 6;
                        case "boolean": return 7;
                        case "string": return 8;
                        case "var": return 9;
                        default: return 0;
                    }
                case "Идентификатор": return 10;
                case "(пробел)": return 11;
                case "Оператор":
                    switch (token)
                    {
                        case "=": return 12;
                        case ":": return 13;
                        case ",": return 14;
                        case ";": return 15;
                        default: return 0;
                    }
                case "Целое без знака": return 16;
                case "Вещественное число": return 17;
                case "Ошибка": return -1;
                default: return 0;
            }
        }

        private void HighlightError(RichTextBox richTextBox, int start, int length)
        {
            richTextBox.SelectionStart = start;
            richTextBox.SelectionLength = length;
            richTextBox.SelectionBackColor = System.Drawing.Color.Plum;
        }
    }
}
