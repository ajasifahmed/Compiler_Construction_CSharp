using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace ConsoleApp2
{
    class LA
    {
        List<string> dt = new List<string>();
        List<string> operators = new List<string>();
        List<string> punctuators = new List<string>();
        List<string> RelationalOperator = new List<string>();
        List<Tokens> token = new List<Tokens>();
        Dictionary<string, string> keyWords = new Dictionary<string, string>();
        public LA()
        {
            initDataTypes();
            init_keyWords();
            init_operators();
            init_punctuators();
            init_RelationalOperator();
        }
        void initDataTypes()
        {
            dt.Add("String");
            dt.Add("Number");
        }
        void init_keyWords()
        {
            keyWords.Add("for", "for");
            keyWords.Add("if", "if");
            keyWords.Add("while", "while");
            //keyWords.Add("void", "void");
            keyWords.Add("return", "return");
            keyWords.Add("else", "else");
            keyWords.Add("break", "break");
            keyWords.Add("continue", "continue");
            keyWords.Add("true", "true");
            keyWords.Add("false", "false");
            keyWords.Add("public", "public");
            keyWords.Add("class", "class");
            keyWords.Add("static", "static");
            keyWords.Add("this", "this");
            keyWords.Add("abstract", "abstract");
            keyWords.Add("new", "new");
            keyWords.Add("private", "private");
            keyWords.Add("interface", "interface"); 
        }
        void init_operators()
        {
            operators.Add("+");
            operators.Add("-");
            operators.Add("/");
            operators.Add("%");
            operators.Add("=");
        }
        void init_RelationalOperator()
        {
            RelationalOperator.Add("<");
            RelationalOperator.Add(">");
            RelationalOperator.Add("!");
        }
        void init_punctuators()
        {
            punctuators.Add("{");
            punctuators.Add("}");
            punctuators.Add("(");
            punctuators.Add(")");
            punctuators.Add("[");
            punctuators.Add("]");
            punctuators.Add(";");
            punctuators.Add(",");
        }

        //for identifier
        bool isIdentifier(string value)
        {
            string pattern = @"^(?:((?!\d)\w+(?:\.(?!\d)\w+)*)\.)?((?!\d)\w+)$";
            //string pattern = "^[_a-zA-Z][a-zA-Z0-9]*[a-zA-Z0-9]|[a-zA-Z]";
            return Regex.IsMatch(value, pattern);
        }
        //for numer constant
        bool isNumberConstant(string value)
        {
            string pattern = @"^([0-9]*\.?)[0-9]+$";
            return Regex.IsMatch(value, pattern);
        }
        //for string
        bool isString(string value)
        {
            string pattern = @"[a-zA-Z]*";
            return Regex.IsMatch(value, pattern);

        }
        //for keyword
        bool isKeyword(string value)
        {
            if (keyWords.ContainsKey(value))
            {
                return true;

            }
            else { return false; };
        }
        //for punctuators
        bool isPunctuator(string value)
        {
            if (punctuators.Contains(value))
            {
                return true;
            }
            else return false;
        }
        //Operators
        bool isOperator(string value)
        {
            if (operators.Contains(value))
            {
                return true;
            }
            else return false;
        }
        //Realational Operators
        bool isRelOperator(string ch)
        {
            if (RelationalOperator.Contains(ch))
            {
                return true;
            }
            else
                return false;
        }
        // word Break
        bool wordBreaker(string value)
        {
            if (value == " " || isPunctuator(value) == true || value == "\n" || isOperator(value) || value == "\"" || value == "/" || isRelOperator(value))
            {
                return true;
            }
            else return false;
        }
        // Breaking
        public void WordBreak(string file)
        {
            Tokens _token;
            int line = 0;
            string temp = "";
            string tFile = file;
            char current = ' ';

            for (int i = 0; i < tFile.Length; i++)
            {
                current = tFile[i];
                if (wordBreaker(current.ToString()))
                { //string starting check
                    if (current.ToString() == "\"")
                    {

 // then check if temp if not empty. then print all words before current.Bcz new string hass sarted.
                        if (temp != "") 
                        {
                            if (isKeyword(temp))
                            {
                                _token = new Tokens("KeyWords", temp, line);
                                token.Add(_token);
                                temp = "";
                                current = ' ';
                                //go to line:200
                            }
                            //similarly
                            else if (dt.Contains(temp))
                            {
                                _token = new Tokens("DATA TYPE", temp, line);
                                token.Add(_token);
                                temp = "";
                                current = ' ';
                            }
                            else if (isIdentifier(temp))
                            {
                                _token = new Tokens("ID", temp, line);
                                token.Add(_token);
                                temp = "";
                                current = ' ';

                            }
                            else if (isNumberConstant(temp))
                            {

                                _token = new Tokens("NumberConstant", temp, line);
                                token.Add(_token);
                                _token = new Tokens("Punctuator", current.ToString(), line);
                                token.Add(_token);
                                temp = "";
                                current = ' ';
                            }
                            else
                            {
                                _token = new Tokens("Invalid Lexeme", temp, line);
                                token.Add(_token);
                                _token = new Tokens("Punctuator", current.ToString(), line);
                                token.Add(_token);
                                temp = "";
                                current = ' ';
                            }

                        }

                        // if temp is empty then this is the fitst (")

                        string s = "";
                        int pointer;
                        pointer = i;
                        pointer++;
                        int Length = tFile.Length;

                        // checkiing whole file for 2nd (")

                        for (int j = pointer; j < Length; j++)
                        {
                            if (tFile[j] == '\n' || tFile[j] == '"')
                            {
                                if (tFile[j] == '"')
                                {
                                  
                                        //because it (\")it is a part of string.
                                    if (tFile[j - 1] == '\\')
                                    {  
                                        if (j == Length - 1)
                                        {//end of file and 2nd (") is missing.So invalid string.
                                            s = s.Insert((s.Length - 1), "\"");
                                            s = s.TrimEnd('\\');
                                            _token = new Tokens("Invalid String Constant", s, line);
                                            token.Add(_token);
                                            pointer = j;
                                            break;//end of loop.

                                        }
                                        else
                                        {// so we can print that (").Otherwise it will not print (") which is the part of string.
                                            s.Insert(s.Length - 1, "\"");
                                            //go to line number:285
                                        }
                                    }
                                    else
                                    {
                                        if (s.Length == 0)
                                        {
                                            _token = new Tokens("Empty String", "\"\"", line);
                                            token.Add(_token);
                                            pointer = j;
                                            break;//end of loop
                                        }
                                        else
                                        {
                                            s = s.TrimEnd('\\');
                                            _token = new Tokens("String Constant", s, line);
                                            token.Add(_token);
                                            pointer = j;
                                            break; // end of loop
                                        }
                                    }
                                }
                               
                                // incase of line break
                                else
                                {
                                    s = s.Insert(0, "\"");//inserting (") so we can display(")
                                    s = s.Trim();
                                    _token = new Tokens("Invalid String Constant", s, line);
                                    token.Add(_token);
                                    line++;
                                    pointer = j;
                                    break;//end of loop 
                                    //go to line 285
                                }
                            }
                            else
                            {
                                if (j == Length - 1)// file ended 2nd (") is missing.
                                {
                                    _token = new Tokens("Invalid String Constant", tFile[j].ToString(), line);
                                    token.Add(_token);
                                    pointer = j;
                                    break;//end of loop
                                    //go to line:285
                                }
                                else
                                { s += tFile[j]; }
                            }

                        }


                        i = pointer;
                        temp = "";
                        s = "";
                        current = ' ';
                        //go to line:141
                    }
                        /*check for comments*/
                    else if (current.ToString() == "/")
                    {
                        if (temp != "")//comments started print all words before current
                        {
                            if (isKeyword(temp))
                            {
                                _token = new Tokens("KeyWords", temp, line);
                                token.Add(_token);
                                temp = "";
                                current = ' ';//go to line:
                            }

                            else if (dt.Contains(temp))
                            {
                                _token = new Tokens("DATA TYPE", temp, line);
                                token.Add(_token);
                                temp = "";
                                current = ' ';

                            }
                            else if (isIdentifier(temp))
                            {
                                _token = new Tokens("ID", temp, line);
                                token.Add(_token);
                                temp = "";
                                current = ' ';

                            }
                            else if (isNumberConstant(temp))
                            {

                                _token = new Tokens("NumberConstant", temp, line);
                                token.Add(_token);
                                _token = new Tokens("Punctuator", current.ToString(), line);
                                token.Add(_token);
                                temp = "";
                                current = ' ';
                            }
                            else
                            {
                                _token = new Tokens("Invalid Lexeme", temp, line);
                                token.Add(_token);
                                _token = new Tokens("Punctuator", current.ToString(), line);
                                token.Add(_token);
                                temp = "";
                                current = ' ';
                            }
                        }
                        string s = "";
                        int pointer;
                        pointer = i;
                        int Length = tFile.Length;


                        if (pointer == Length - 1)
                        {
                            _token = new Tokens("InValidLexeme", "/", line);// /*.
                            token.Add(_token);
                        }
                        else if (tFile[pointer + 1].ToString() != "/" && tFile[pointer + 1] != '*')
                        {
                            _token = new Tokens("InValidLexeme", "/", line);
                            token.Add(_token);

                        }
                        else if (tFile[pointer + 1].ToString() == "*")
                        {
                            pointer++;
                            if (pointer == Length - 1)
                            {
                                _token = new Tokens("Empty M.Line Comment", "", line);
                                token.Add(_token);

                            }
                            pointer++;
                            for (int j = pointer; j < Length; j++)
                            {
                                if (tFile[j] != '*')
                                {
                                    if (tFile[j] == '\n')
                                    {
                                        pointer = j;
                                        line++;
                                    }
                                    else
                                    {
                                        pointer = j;
                                    }

                                }
                                else
                                {
                                    if (pointer == Length - 1)
                                    {
                                        _token = new Tokens("M.Line Comment til end", "", line);
                                        token.Add(_token);
                                        break;
                                    }
                                    else
                                    {
                                        if (tFile[j + 1] == '/')//  */
                                        {
                                            pointer = j + 1;
                                            break;
                                        }
                                        else
                                        {
                                            if (tFile[j + 1] == '\n')
                                            {
                                                line++;
                                                j++;
                                            }
                                            else
                                            {
                                                pointer = j;
                                            }
                                        }
                                    }

                                }

                            }//for

                        }
                        else
                        {
                            pointer++;
                            if (pointer == Length - 1)
                            {
                                _token = new Tokens("Empty Line Comment", "", line);
                                token.Add(_token);
                            }
                            for (int j = pointer; j < Length; j++)
                            {
                                if (tFile[j] != '\n')
                                {
                                    pointer = j;
                                }
                                else
                                {
                                    line++;
                                    pointer = j;
                                    break;
                                }

                            }

                        }
                        i = pointer;



                    }
                    else if (isPunctuator(current.ToString()))
                    {
                        if (temp == "")
                        {
                            _token = new Tokens("Punctuator", current.ToString(), line);
                            token.Add(_token);
                            temp = "";
                            current = ' ';
                        }
                        else //
                        {
                            if (isKeyword(temp))
                            {
                                _token = new Tokens("KeyWords", temp, line);
                                token.Add(_token);
                                _token = new Tokens("Punctuator", current.ToString(), line);
                                token.Add(_token);
                                temp = "";
                                current = ' ';
                            }

                            else if (dt.Contains(temp))
                            {
                                _token = new Tokens("DATA TYPE", temp, line);
                                token.Add(_token);
                                _token = new Tokens("Punctuator", current.ToString(), line);
                                token.Add(_token);
                                temp = "";
                                current = ' ';

                            }
                            else if (isIdentifier(temp))
                            {
                                _token = new Tokens("ID", temp, line);
                                token.Add(_token);
                                _token = new Tokens("Punctuator", current.ToString(), line);
                                token.Add(_token);
                                temp = "";
                                current = ' ';

                            }
                            else if (isNumberConstant(temp))
                            {
                                _token = new Tokens("NumberConstant", temp, line);
                                token.Add(_token);
                                _token = new Tokens("Punctuator", current.ToString(), line);
                                token.Add(_token);
                                temp = "";
                                current = ' '; //go to line 141 (foorloop)
                            }
                            else
                            {
                                _token = new Tokens("Invalid Lexeme", temp, line);
                                token.Add(_token);
                                _token = new Tokens("Punctuator", current.ToString(), line);
                                token.Add(_token);
                                temp = "";
                                current = ' ';
                            }


                        }

                    }
                    else if (isRelOperator(current.ToString()))
                    {
                        if (temp == "")
                        {
                            if (i == tFile.Length - 1)
                            {
                                _token = new Tokens("RelationalOperator", current.ToString(), line);
                                token.Add(_token);
                                temp = "";
                                current = ' ';
                            }
                            else if (tFile[i + 1].ToString() == "=")
                            {
                                _token = new Tokens("RelationalOperator", current.ToString() + tFile[i + 1], line);
                                token.Add(_token);
                                temp = "";
                                current = ' ';
                                i++;
                            }
                            else
                            {
                                if (current.ToString() == "!")
                                {
                                    _token = new Tokens("InvalidLexeme", current.ToString(), line);
                                    token.Add(_token);
                                    temp = "";
                                    current = ' ';
                                }
                                else
                                {
                                    _token = new Tokens("RelationalOperator", current.ToString(), line);
                                    token.Add(_token);
                                    temp = "";
                                    current = ' ';
                                }
                            }


                        }
                        else
                        {
                            if (isKeyword(temp))
                            {
                                _token = new Tokens("KeyWords", temp, line);
                                token.Add(_token);
                                if (i == tFile.Length - 1)
                                {
                                    _token = new Tokens("RelationalOperator", current.ToString(), line);
                                    token.Add(_token);
                                    temp = "";
                                    current = ' ';
                                }
                                else if (tFile[i + 1].ToString() == "=")
                                {
                                    _token = new Tokens("RelationalOperator", current.ToString() + tFile[i + 1], line);
                                    token.Add(_token);
                                    temp = "";
                                    current = ' ';
                                    i++;
                                }
                                else
                                {
                                    if (current.ToString() == "!")
                                    {
                                        _token = new Tokens("InvalidLexeme", current.ToString(), line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                    }
                                    else
                                    {
                                        _token = new Tokens("RelationalOperator", current.ToString(), line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                    }
                                }
                            }
                            else if (dt.Contains(temp))
                            {
                                _token = new Tokens("DATA TYPE", temp, line);
                                token.Add(_token);
                                if (i == tFile.Length - 1)
                                {
                                    _token = new Tokens("RelationalOperator", current.ToString(), line);
                                    token.Add(_token);
                                    temp = "";
                                    current = ' ';
                                }
                                else if (tFile[i + 1].ToString() == "=")
                                {
                                    _token = new Tokens("RelationalOperator", current.ToString() + tFile[i + 1], line);
                                    token.Add(_token);
                                    temp = "";
                                    current = ' ';
                                    i++;
                                }
                                else
                                {
                                    if (current.ToString() == "!")
                                    {
                                        _token = new Tokens("InvalidLexeme", current.ToString(), line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                    }
                                    else
                                    {
                                        _token = new Tokens("RelationalOperator", current.ToString(), line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                    }
                                }

                            }
                            else if (isIdentifier(temp))
                            {
                                _token = new Tokens("ID", temp, line);
                                token.Add(_token);
                                if (i == tFile.Length - 1)
                                {
                                    _token = new Tokens("RelationalOperator", current.ToString(), line);
                                    token.Add(_token);
                                    temp = "";
                                    current = ' ';
                                }
                                else if (tFile[i + 1].ToString() == "=")
                                {
                                    _token = new Tokens("RelationalOperator", current.ToString() + tFile[i + 1], line);
                                    token.Add(_token);
                                    temp = "";
                                    current = ' ';
                                    i++;
                                }
                                else
                                {
                                    if (current.ToString() == "!")
                                    {
                                        _token = new Tokens("InvalidLexeme", current.ToString(), line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                    }
                                    else
                                    {
                                        _token = new Tokens("RelationalOperator", current.ToString(), line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                    }
                                }

                            }

                            else if (isNumberConstant(temp))
                            {
                                _token = new Tokens("Number constant", temp, line);
                                token.Add(_token);
                                if (i == tFile.Length - 1)
                                {
                                    _token = new Tokens("RelationalOperator", current.ToString(), line);
                                    token.Add(_token);
                                    temp = "";
                                    current = ' ';
                                }
                                else if (tFile[i + 1].ToString() == "=")
                                {
                                    _token = new Tokens("RelationalOperator", current.ToString() + tFile[i + 1], line);
                                    token.Add(_token);
                                    temp = "";
                                    current = ' ';
                                    i++;
                                }
                                else
                                {
                                    if (current.ToString() == "!")
                                    {
                                        _token = new Tokens("InvalidLexeme", current.ToString(), line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                    }
                                    else
                                    {
                                        _token = new Tokens("RelationalOperator", current.ToString(), line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                    }
                                }
                            }
                            else
                            {
                                _token = new Tokens("Invalid Lexeme", temp, line);
                                token.Add(_token);
                                if (i == tFile.Length - 1)
                                {
                                    _token = new Tokens("RelationalOperator", current.ToString(), line);
                                    token.Add(_token);
                                    temp = "";
                                    current = ' ';
                                }
                                else if (tFile[i + 1].ToString() == "=")
                                {
                                    _token = new Tokens("RelationalOperator", current.ToString() + tFile[i + 1], line);
                                    token.Add(_token);
                                    temp = "";
                                    current = ' ';
                                    i++;
                                }
                                else
                                {
                                    if (current.ToString() == "!")
                                    {
                                        _token = new Tokens("InvalidLexeme", current.ToString(), line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                    }
                                    else
                                    {
                                        _token = new Tokens("RelationalOperator", current.ToString(), line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                    }
                                }
                            }


                        }

                    }
                    else if (isOperator(current.ToString()))
                    {
                        if (temp == "")
                        {

                            if (i == tFile.Length - 1)
                            {

                                _token = new Tokens("Operator", current.ToString(), line);
                                token.Add(_token);
                                temp = "";
                                current = ' ';
                            }
                            else
                            {
                                if (current.ToString() == "=" && tFile[i + 1].ToString() == "=")
                                {
                                    _token = new Tokens("RelationalOperator", current.ToString() + "=", line);
                                    token.Add(_token);
                                    temp = "";
                                    current = ' ';
                                    i += 1;
                                }
                                else if (current.ToString() == "+" && tFile[i + 1].ToString() == "+")
                                {

                                    _token = new Tokens("IncrementOperator", current.ToString() + "+", line);
                                    token.Add(_token);
                                    temp = "";
                                    current = ' ';
                                    i += 1;
                                }
                                else if (current.ToString() == "-" && tFile[i + 1].ToString() == "-")
                                {

                                    _token = new Tokens("DecrementOperator", current.ToString() + "-", line);
                                    token.Add(_token);
                                    temp = "";
                                    current = ' ';
                                    i += 1;
                                }
                                else if (current.ToString() == "+" && tFile[i + 1].ToString() == "=")
                                {

                                    _token = new Tokens("ArithmaticCompoundOperation", current.ToString() + "=", line);
                                    token.Add(_token);
                                    temp = "";
                                    current = ' ';
                                    i += 1;
                                }
                                else if (current.ToString() == "*" && tFile[i + 1].ToString() == "=")
                                {

                                    _token = new Tokens("ArithmaticCompoundOperation", current.ToString() + "=", line);
                                    token.Add(_token);
                                    temp = "";
                                    current = ' ';
                                    i += 1;
                                }
                                else if (current.ToString() == "-" && tFile[i + 1].ToString() == "=")
                                {

                                    _token = new Tokens("ArithmaticCompoundOperation", current.ToString() + "=", line);
                                    token.Add(_token);
                                    temp = "";
                                    current = ' ';
                                    i += 1;
                                }
                                else if (current.ToString() == "/" && tFile[i + 1].ToString() == "=")
                                {

                                    _token = new Tokens("ArithmaticCompoundOperation", current.ToString() + "=", line);
                                    token.Add(_token);
                                    temp = "";
                                    current = ' ';
                                    i += 1;
                                }
                                else
                                {
                                    _token = new Tokens("Operator", current.ToString(), line);
                                    token.Add(_token);
                                    temp = "";
                                    current = ' ';

                                }
                            }
                            //_token = new Tokens("Operator", current.ToString(), line);
                            //token.Add(_token);
                            //temp = "";
                            //current = ' ';

                        }
                        else
                        {

                            if (isKeyword(temp))
                            {
                                _token = new Tokens("KeyWords", temp, line);
                                token.Add(_token);

                                temp = "";
                                if (i == tFile.Length - 1)
                                {
                                    Console.WriteLine("single operat m");
                                    Console.WriteLine(i + "" + tFile.Length);
                                    _token = new Tokens("Operator", current.ToString(), line);
                                    token.Add(_token);
                                    temp = "";
                                    current = ' ';
                                }
                                else
                                {
                                    if (current.ToString() == "=" && tFile[i + 1].ToString() == "=")
                                    {
                                        _token = new Tokens("RelationalOperator", current.ToString() + "=", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else if (current.ToString() == "+" && tFile[i + 1].ToString() == "+")
                                    {

                                        _token = new Tokens("IncrementOperator", current.ToString() + "+", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else if (current.ToString() == "-" && tFile[i + 1].ToString() == "-")
                                    {

                                        _token = new Tokens("DecrementOperator", current.ToString() + "-", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else if (current.ToString() == "+" && tFile[i + 1].ToString() == "=")
                                    {

                                        _token = new Tokens("ArithmaticCompoundOperation", current.ToString() + "=", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else if (current.ToString() == "*" && tFile[i + 1].ToString() == "=")
                                    {

                                        _token = new Tokens("ArithmaticCompoundOperation", current.ToString() + "=", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else if (current.ToString() == "-" && tFile[i + 1].ToString() == "=")
                                    {

                                        _token = new Tokens("ArithmaticCompoundOperation", current.ToString() + "=", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else if (current.ToString() == "/" && tFile[i + 1].ToString() == "=")
                                    {

                                        _token = new Tokens("ArithmaticCompoundOperation", current.ToString() + "=", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else
                                    {
                                        _token = new Tokens("Operator", current.ToString(), line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';

                                    }

                                }
                            }
                            else if (dt.Contains(temp))
                            {
                                _token = new Tokens("DATA TYPE", temp, line);
                                token.Add(_token);

                                temp = "";
                                if (i == tFile.Length - 1)
                                {
                                    Console.WriteLine("single operat m");
                                    Console.WriteLine(i + "" + tFile.Length);
                                    _token = new Tokens("Operator", current.ToString(), line);
                                    token.Add(_token);
                                    temp = "";
                                    current = ' ';
                                }
                                else
                                {
                                    if (current.ToString() == "=" && tFile[i + 1].ToString() == "=")
                                    {
                                        _token = new Tokens("RelationalOperator", current.ToString() + "=", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else if (current.ToString() == "+" && tFile[i + 1].ToString() == "+")
                                    {

                                        _token = new Tokens("IncrementOperator", current.ToString() + "+", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else if (current.ToString() == "-" && tFile[i + 1].ToString() == "-")
                                    {

                                        _token = new Tokens("DecrementOperator", current.ToString() + "-", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else if (current.ToString() == "+" && tFile[i + 1].ToString() == "=")
                                    {

                                        _token = new Tokens("ArithmaticCompoundOperation", current.ToString() + "=", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else if (current.ToString() == "*" && tFile[i + 1].ToString() == "=")
                                    {

                                        _token = new Tokens("ArithmaticCompoundOperation", current.ToString() + "=", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else if (current.ToString() == "-" && tFile[i + 1].ToString() == "=")
                                    {

                                        _token = new Tokens("ArithmaticCompoundOperation", current.ToString() + "=", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else if (current.ToString() == "/" && tFile[i + 1].ToString() == "=")
                                    {

                                        _token = new Tokens("ArithmaticCompoundOperation", current.ToString() + "=", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else
                                    {
                                        _token = new Tokens("Operator", current.ToString(), line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';

                                    }

                                }
                                //  current = ' ';


                            }
                            else if (isIdentifier(temp))
                            {
                                _token = new Tokens("ID", temp, line);
                                token.Add(_token);

                                temp = "";
                                if (i == tFile.Length - 1)
                                {

                                    _token = new Tokens("Operator", current.ToString(), line);
                                    token.Add(_token);
                                    temp = "";
                                    current = ' ';
                                }
                                else
                                {
                                    if (current.ToString() == "=" && tFile[i + 1].ToString() == "=")
                                    {
                                        _token = new Tokens("RelationalOperator", current.ToString() + "=", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else if (current.ToString() == "+" && tFile[i + 1].ToString() == "+")
                                    {

                                        _token = new Tokens("IncrementOperator", current.ToString() + "+", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;//go to loop
                                    }
                                    else if (current.ToString() == "-" && tFile[i + 1].ToString() == "-")
                                    {

                                        _token = new Tokens("DecrementOperator", current.ToString() + "-", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else if (current.ToString() == "+" && tFile[i + 1].ToString() == "=")
                                    {

                                        _token = new Tokens("ArithmaticCompoundOperation", current.ToString() + "=", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else if (current.ToString() == "*" && tFile[i + 1].ToString() == "=")
                                    {

                                        _token = new Tokens("ArithmaticCompoundOperation", current.ToString() + "=", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else if (current.ToString() == "-" && tFile[i + 1].ToString() == "=")
                                    {

                                        _token = new Tokens("ArithmaticCompoundOperation", current.ToString() + "=", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else if (current.ToString() == "/" && tFile[i + 1].ToString() == "=")
                                    {

                                        _token = new Tokens("ArithmaticCompoundOperation", current.ToString() + "=", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else
                                    {
                                        _token = new Tokens("Operator", current.ToString(), line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';//go to line 141(foor lopp)

                                    }

                                }

                            }

                            else if (isNumberConstant(temp))
                            {
                                _token = new Tokens("Number constant", temp, line);
                                token.Add(_token);
                                
                                
                                temp = "";
                                
                                if (i == tFile.Length - 1)
                                {
                                    
                                    
                                    _token = new Tokens("Operator", current.ToString(), line);
                                    token.Add(_token);
                                    temp = "";
                                    current = ' ';
                                }
                                else
                                {
                                    if (current.ToString() == "=" && tFile[i + 1].ToString() == "=")
                                    {
                                        _token = new Tokens("RelationalOperator", current.ToString() + "=", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else if (current.ToString() == "+" && tFile[i + 1].ToString() == "+")
                                    {

                                        _token = new Tokens("IncrementOperator", current.ToString() + "+", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else if (current.ToString() == "-" && tFile[i + 1].ToString() == "-")
                                    {

                                        _token = new Tokens("DecrementOperator", current.ToString() + "-", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else if (current.ToString() == "+" && tFile[i + 1].ToString() == "=")
                                    {

                                        _token = new Tokens("ArithmaticCompoundOperation", current.ToString() + "=", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else if (current.ToString() == "*" && tFile[i + 1].ToString() == "=")
                                    {

                                        _token = new Tokens("ArithmaticCompoundOperation", current.ToString() + "=", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else if (current.ToString() == "-" && tFile[i + 1].ToString() == "=")
                                    {

                                        _token = new Tokens("ArithmaticCompoundOperation", current.ToString() + "=", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else if (current.ToString() == "/" && tFile[i + 1].ToString() == "=")
                                    {

                                        _token = new Tokens("ArithmaticCompoundOperation", current.ToString() + "=", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else
                                    {
                                        _token = new Tokens("Operator", current.ToString(), line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';

                                    }

                                }
                            }
                            else
                            {
                                _token = new Tokens("Invalid Lexeme", temp, line);
                                token.Add(_token);
                                //_token = new Tokens("Operators", current.ToString(), line);
                                //token.Add(_token);
                                temp = "";
                                // current = ' ';
                                if (i == tFile.Length - 1)
                                {
                                    Console.WriteLine("single operat m");
                                    Console.WriteLine(i + "" + tFile.Length);
                                    _token = new Tokens("Operator", current.ToString(), line);
                                    token.Add(_token);
                                    temp = "";
                                    current = ' ';
                                }
                                else
                                {
                                    if (current.ToString() == "=" && tFile[i + 1].ToString() == "=")
                                    {
                                        _token = new Tokens("RelationalOperator", current.ToString() + "=", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else if (current.ToString() == "+" && tFile[i + 1].ToString() == "+")
                                    {

                                        _token = new Tokens("IncrementOperator", current.ToString() + "+", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else if (current.ToString() == "-" && tFile[i + 1].ToString() == "-")
                                    {

                                        _token = new Tokens("DecrementOperator", current.ToString() + "-", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else if (current.ToString() == "+" && tFile[i + 1].ToString() == "=")
                                    {

                                        _token = new Tokens("ArithmaticCompoundOperation", current.ToString() + "=", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else if (current.ToString() == "*" && tFile[i + 1].ToString() == "=")
                                    {

                                        _token = new Tokens("ArithmaticCompoundOperation", current.ToString() + "=", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else if (current.ToString() == "-" && tFile[i + 1].ToString() == "=")
                                    {

                                        _token = new Tokens("ArithmaticCompoundOperation", current.ToString() + "=", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else if (current.ToString() == "/" && tFile[i + 1].ToString() == "=")
                                    {

                                        _token = new Tokens("ArithmaticCompoundOperation", current.ToString() + "=", line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';
                                        i += 1;
                                    }
                                    else
                                    {
                                        _token = new Tokens("Operator", current.ToString(), line);
                                        token.Add(_token);
                                        temp = "";
                                        current = ' ';

                                    }

                                }
                            }


                        }

                    }
                    else if (current == ' ')//space
                    {
                        if (temp == "")
                        {
                            current = ' ';//go to line:141
                        }
                        else
                        {
                            if (isKeyword(temp))
                            {
                                _token = new Tokens("KeyWords", temp, line);
                                token.Add(_token);
                                temp = "";
                                current = ' ';
                            }


                            else if (dt.Contains(temp))
                            {
                                _token = new Tokens("DATA TYPE", temp, line);
                                token.Add(_token);
                                temp = "";
                                current = ' ';//go to foorloop

                            }
                            else if (isIdentifier(temp))
                            {
                                _token = new Tokens("ID", temp, line);
                                token.Add(_token);
                                temp = "";
                                current = ' ';

                            }

                            else if (isNumberConstant(temp))
                            {

                                _token = new Tokens("Number constant", temp, line);
                                token.Add(_token);
                                temp = "";
                                current = ' ';
                            }
                            else
                            {
                                _token = new Tokens("Invalid Lexeme", temp, line);
                                token.Add(_token);
                                temp = "";
                                current = ' ';
                            }


                        }
                    }
                    else
                    {
                        if (temp == "")
                        {
                            current = ' ';

                        }
                        else
                        {
                            string tempp = temp.Trim();

                            if (isKeyword(tempp))
                            {
                                _token = new Tokens("KeyWords", tempp, line);
                                token.Add(_token);
                                temp = "";
                                current = ' ';
                            }


                            else if (dt.Contains(tempp))
                            {
                                _token = new Tokens("DATA TYPE", tempp, line);
                                token.Add(_token);
                                temp = "";
                                current = ' ';
                            }
                            else if (isIdentifier(tempp))
                            {
                                _token = new Tokens("ID", tempp, line);
                                token.Add(_token);
                                temp = "";
                                current = ' ';
                            }

                            else if (isNumberConstant(tempp))
                            {
                                _token = new Tokens("Number constant", tempp, line);
                                token.Add(_token);
                                temp = "";
                                current = ' ';
                            }

                            else
                            {
                                if (tempp.Length != 0)
                                {
                                    _token = new Tokens("Invalid Lexeme", tempp, line);
                                    token.Add(_token);
                                    temp = "";
                                    current = ' ';
                                }
                                else
                                {
                                    current = ' ';
                                    temp = "";
                                };

                            }

                            tempp = "";
                        }

                        line++;
                    }
                }
                else
                {
                    temp += tFile[i];
                };
            }
        }
        public void printTokens()
        {
            string myTokens = @"C:\Users\Admin\Desktop\myTokensFile.txt";
            List<string> s = new List<string>();
            token.ForEach(item =>
            {
                Console.WriteLine("( " + item.CV + "," + item.CP + "," + item.lineNo + " )");
                s.Add("( " + item.CV + "," + item.CP + "," + item.lineNo + " )");
                

            });
            foreach (var item in s)
            {
                File.WriteAllLines(myTokens, s);
            }
        }
    }
}
