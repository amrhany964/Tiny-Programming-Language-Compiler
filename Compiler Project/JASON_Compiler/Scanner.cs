using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


public enum Token_Class
{
    Int, Float, String, Read, Write, Repeat, Until, If, ElseIf, Else, Then, Return, Endl,
    PlusOp, MinusOp, MultiplyOp, DivideOp, LessThanOp, GreaterThanOp, EqualOp, NotEqualOp, AndOp, OrOp,
    AssignmentOp, LParanthesis, RParanthesis, LCurl, RCurl, Semicolon, Comma,
    Identifier, Number, StringElem, Comment
}
namespace JASON_Compiler
{


    public class Token
    {
        public string lex;
        public Token_Class token_type;
    }

    public class Scanner
    {
        public List<Token> Tokens = new List<Token>();
        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();/////////
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();

        public Scanner()
        {
            ReservedWords.Add("int", Token_Class.Int);
            ReservedWords.Add("float", Token_Class.Float);
            ReservedWords.Add("string", Token_Class.String);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("write", Token_Class.Write);
            ReservedWords.Add("repeat", Token_Class.Repeat);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("if", Token_Class.If);
            ReservedWords.Add("elseif", Token_Class.ElseIf);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("return", Token_Class.Return);
            ReservedWords.Add("endl", Token_Class.Endl);



            // map[&&]=Andop;
            Operators.Add("&&", Token_Class.AndOp);
            Operators.Add("||", Token_Class.OrOp);


            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("(", Token_Class.LParanthesis);
            Operators.Add(")", Token_Class.RParanthesis);
            Operators.Add("{", Token_Class.LCurl);
            Operators.Add("}", Token_Class.RCurl);


            Operators.Add("=", Token_Class.EqualOp);
            Operators.Add(":=", Token_Class.AssignmentOp);
            Operators.Add("<", Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("<>", Token_Class.NotEqualOp);


            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("/", Token_Class.DivideOp);



        }

        public void StartScanning(string SourceCode)
        {
            for (int i = 0; i < SourceCode.Length; i++)
            {
                int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar.ToString();

                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n')
                    continue;

                if ((CurrentChar >= 'A' && CurrentChar <= 'Z') || (CurrentChar >= 'a' && CurrentChar <= 'z')) //if you read a character
                { // A - Z a - z
                    j++;
                    if (j < SourceCode.Length)
                    {
                        CurrentChar = SourceCode[j];
                    }
                    while (j < SourceCode.Length && ((CurrentChar >= 'A' && CurrentChar <= 'Z') || (CurrentChar >= 'a' && CurrentChar <= 'z') || (CurrentChar >= '0' && CurrentChar <= '9')))
                    {
                        CurrentLexeme += CurrentChar;//A5
                        j++;
                        if (j < SourceCode.Length)
                        {
                            CurrentChar = SourceCode[j];
                        }
                    }
                    i = j - 1;
                    FindTokenClass(CurrentLexeme);
                }
                else if( (CurrentChar >= '0' && CurrentChar <= '9') || CurrentChar == '.')
                {
                    j++;
                    if (j < SourceCode.Length)
                    {
                        CurrentChar = SourceCode[j];
                    }
                    while (j < SourceCode.Length && ((CurrentChar == '.') || (CurrentChar >= '0' && CurrentChar <= '9')))
                    {
                        CurrentLexeme += CurrentChar;  //  1234.7
                        j++;
                        if (j < SourceCode.Length)
                        {
                            CurrentChar = SourceCode[j];
                        }
                    }
                    i = j - 1;
                    FindTokenClass(CurrentLexeme);
                }
                else if (CurrentChar == '{' || CurrentChar == '}' || CurrentChar == '(' || CurrentChar == ')' || CurrentChar == ';' || CurrentChar == ',')
                {
                    FindTokenClass(CurrentLexeme);
                }
                else if (CurrentChar == '/')
                {
                    j++;
                    if (j < SourceCode.Length)
                    {
                        CurrentChar = SourceCode[j];
                    }
                    if (CurrentChar == '*')
                    {
                        CurrentLexeme += CurrentChar;
                        j++;
                        if (j < SourceCode.Length)
                        {
                            CurrentChar = SourceCode[j];
                        }
                        while (j < SourceCode.Length && !(CurrentChar == '*' && j + 1 < SourceCode.Length && SourceCode[j + 1] == '/'))
                        {
                            CurrentLexeme += CurrentChar;
                            j++;
                            if (j < SourceCode.Length)
                            {
                                CurrentChar = SourceCode[j];
                            }
                        }
                        if (j < SourceCode.Length)
                        {
                            CurrentLexeme += "*";
                            CurrentLexeme += "/";
                            i = j + 1;
                            FindTokenClass(CurrentLexeme);
                        }
                        else
                        {
                            i = j;
                            FindTokenClass(CurrentLexeme);
                        }
                    }
                    else
                    {

                        FindTokenClass(CurrentLexeme);
                    }
                }
                else if (CurrentChar == '+' || CurrentChar == '-' || CurrentChar == '*')
                {
                    FindTokenClass(CurrentLexeme);
                }
                else if (CurrentChar == '<' || CurrentChar == '>' || CurrentChar == '=' || CurrentChar == ':')
                {
                    j++;
                    if (j < SourceCode.Length)
                    {
                        CurrentChar = SourceCode[j];
                    }
                    if ((CurrentLexeme == "<" && CurrentChar == '>') || (CurrentLexeme == ":" && CurrentChar == '=')) // <>   :=
                    {
                        CurrentLexeme += CurrentChar;
                        i = j;
                    }
                    FindTokenClass(CurrentLexeme);
                }
                else if (CurrentChar == '&' || CurrentChar == '|') //&
                {
                    j++;
                    if (j < SourceCode.Length)
                    {
                        if ((CurrentChar == '&' && SourceCode[j] == '&') || (CurrentChar == '|' && SourceCode[j] == '|'))
                        {
                            CurrentChar = SourceCode[j];
                            CurrentLexeme += CurrentChar;
                            i = j;
                            FindTokenClass(CurrentLexeme);
                        }
                        else
                        {
                            FindTokenClass(CurrentLexeme);
                            i = j - 1;
                        }
                    }
                    else FindTokenClass(CurrentLexeme);
                }
                else if (CurrentChar == '"') //   "" 
                {
                    j++;
                    //   bool flag = false;
                    if (j < SourceCode.Length)
                    {
                        CurrentChar = SourceCode[j];

                    }
                    else
                    {
                        FindTokenClass(CurrentLexeme);
                        continue;
                    }

                    if (SourceCode[j] == '"')
                    {
                        CurrentLexeme += '"';
                        FindTokenClass(CurrentLexeme);
                        i = j;
                        continue;
                    }
                    ;

                    while (j < SourceCode.Length && CurrentChar != '"')
                    {
                        //  flag = true;
                        CurrentLexeme += CurrentChar;
                        j++;
                        if (j < SourceCode.Length)
                        {
                            CurrentChar = SourceCode[j];
                        }
                    }
                    if (CurrentChar == '"')
                    {
                        CurrentLexeme += CurrentChar;
                        i = j;
                        FindTokenClass(CurrentLexeme);
                    }
                }
                else
                {
                    FindTokenClass(CurrentLexeme);
                }
            }

            JASON_Compiler.TokenStream = Tokens;
        }

        void FindTokenClass(string Lex) // A5
        {
            Token_Class TC;
            Token Tok = new Token();
            Tok.lex = Lex;
            //Is it a reserved word?
            if (isReservedWord(Lex))
            {
                TC = ReservedWords[Lex];
                Tok.token_type = TC;
                Tokens.Add(Tok);
                return;
            }

            //Is it an identifier?
            if (isIdentifier(Lex))
            {
                TC = Token_Class.Identifier;
                Tok.token_type = TC;
                Tokens.Add(Tok);
                return;
            }


            //Is it a Constant?
            if (isConstant(Lex))
            {
                TC = Token_Class.Number;
                Tok.token_type = TC;
                Tokens.Add(Tok);
                return;
            }

            //Is it an operator?
            if (isOperator(Lex))
            {
                TC = Operators[Lex]; //map[)]=bracket
                Tok.token_type = TC;
                Tokens.Add(Tok);
                return;
            }

            //Is it a stringElem?
            if (isStringElem(Lex)) // "  "
            {
                TC = Token_Class.StringElem;
                Tok.token_type = TC;
                Tokens.Add(Tok);
                return;
            }

            //Is it a comment?
            if (isComment(Lex))
            {
                TC = Token_Class.Comment;
                Tok.token_type = TC;
                Tokens.Add(Tok);
                return;
            }

            //Is it an undefined?


            Errors.Error_List.Add(Lex);

        }



        bool isIdentifier(string lex)
        {
            bool isValid = true;
            // Check if the lex is an identifier or not.
            Regex reg = new Regex("^[a-zA-Z][a-zA-Z0-9]*$");
            if (!reg.IsMatch(lex))
            {
                isValid = false;
            }
            return isValid;
        }
        bool isConstant(string lex)
        {
            bool isValid = true;
            // Check if the lex is a constant (Number) or not.
            Regex reg = new Regex("^[0-9]+(\\.[0-9]+)?$");
            if (!reg.IsMatch(lex))
            {
                isValid = false;
            }
            return isValid;
        }
        bool isStringElem(string lex)
        {
            bool isValid = true;
            // Check if the lex is a String Element or not.
            Regex reg = new Regex("^\".*\"$");
            if (!reg.IsMatch(lex))
            {
                isValid = false;
            }
            return isValid;
        }
        bool isComment(string lex)
        {
            bool isValid = true;
            // Check if the lex is a Comment or not.
            Regex reg = new Regex(@"^/\*.*\*/$");
            if (!reg.IsMatch(lex))
            {
                isValid = false;
            }
            return isValid;
        }
        bool isOperator(string lex)
        {
            bool isValid = true;
            // Check if the lex is an Operator or not.
            if (!Operators.ContainsKey(lex))
            {
                isValid = false;
            }
            return isValid;
        }
        bool isReservedWord(string lex)
        {
            bool isValid = true;
            // Check if the lex is a Reserved Word or not.
            if (!ReservedWords.ContainsKey(lex))
            {
                isValid = false;
            }
            return isValid;
        }
    }
}