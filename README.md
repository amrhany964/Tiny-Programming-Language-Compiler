# 🧩 Lexical Analysis Phase – TINY / JSON Compiler

## 📘 Overview
This project implements the **Lexical Analysis (Scanner)** phase for the TINY / JSON compiler.  
The scanner reads the source code, detects lexical patterns using **Deterministic Finite Automata (DFA)** and **Regular Expressions (Regex)**,  
and generates a **Lexeme-to-Token Table**.

---

## ⚙️ Features
- Detection of all reserved keywords and operators  
- Support for integer, float, and string literals  
- Comment recognition and ignoring during tokenization  
- Automatic error detection for invalid lexemes  
- GUI Interface displaying:
  - Input code area  
  - Error list  
  - Lexeme–Token table  

---

## 🧮 Regular Expressions and DFAs
All tokens were defined using **Regular Expressions** and recognized by its corresponding **DFA** state machine..

📘 Reference: [Regular Expressions – Wikipedia](<https://en.wikipedia.org/wiki/Regular_expression>)  
📄 Regex and dfa file: [regex & dfa](<https://github.com/amrhany964/Tiny-Programming-Language-Compiler/blob/main/docs/REGEX%20and%20DFAs%20for%20Tiny%20Language.pdf>)


---

## 💡 Tokens Implemented

| Category | Tokens |
|-----------|---------|
| **Data Types & I/O** | `Int`, `Float`, `String`, `Read`, `Write` |
| **Control Flow** | `Repeat`, `Until`, `If`, `ElseIf`, `Else`, `Then`, `Return` |
| **Operators** | `PlusOp (+)`, `MinusOp (-)`, `MultiplyOp (*)`, `DivideOp (/)`, `LessThanOp (<)`, `GreaterThanOp (>)`, `EqualOp (=)`, `NotEqualOp (!=)`, `AndOp (&&)`, `OrOp (||)` |
| **Assignment & Punctuation** | `AssignmentOp (:=)`, `Semicolon (;)`, `Comma (,)` |
| **Parentheses & Braces** | `LParanthesis ( )`, `RParanthesis ( )`, `LCurl {`, `RCurl }` |
| **Others** | `Identifier`, `Number`, `StringElem`, `Comment`, `Endl` |

---

## 🖥️ Example Run
Here’s an example of the scanner analyzing a TINY program:  

🖼️ *Screenshot:*  
<img width="2369" height="1166" alt="Lexical analysis" src="https://github.com/user-attachments/assets/a4605f50-b1c0-4b33-bb8f-09a192161f68" />


---

## 🧠 Token Table Example
| Lexeme | Token | Line |
|---------|--------|------|
| `int` | `Int` | 1 |
| `x` | `Identifier` | 1 |
| `:=` | `AssignmentOp` | 2 |
| `5` | `Number` | 2 |
| `/* comment */` | `Comment` | 3 |

---

## 🧩 Notes
- Implemented in **C#**  
- Based on **Finite Automata** and **Regex** matching  
- GUI built for testing and visualizing scanner output  
- Designed as part of the **Compiler Project – Lexical Phase**

---

## 👨‍💻 Authors
**Amr Hany**  
**Ahmed Atta**  
**Mariam Sherif**  
**Farah Younis**  
**Jana Waleed**  
**Mariam Hassan**  
Faculty of Computers and Information science– Ain Shams University  
