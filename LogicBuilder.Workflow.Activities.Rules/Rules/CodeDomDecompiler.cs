using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace LogicBuilder.Workflow.Activities.Rules
{
    public static class RuleDecompiler
    {
        #region Decompile literals

        public static void DecompileObjectLiteral(StringBuilder decompilation, object primitiveValue)
        {
            if (primitiveValue == null)
            {
                decompilation.Append("null");
            }
            else
            {
                Type primitiveType = primitiveValue.GetType();

                if (primitiveType == typeof(string))
                    DecompileStringLiteral(decompilation, (string)primitiveValue);
                else if (primitiveType == typeof(char))
                    DecompileCharacterLiteral(decompilation, (char)primitiveValue);
                else if (primitiveType == typeof(long))
                    DecompileSuffixedIntegerLiteral(decompilation, primitiveValue, "L");
                else if (primitiveType == typeof(uint))
                    DecompileSuffixedIntegerLiteral(decompilation, primitiveValue, "U");
                else if (primitiveType == typeof(ulong))
                    DecompileSuffixedIntegerLiteral(decompilation, primitiveValue, "UL");
                else if (primitiveType == typeof(float))
                    DecompileFloatingPointLiteral(decompilation, primitiveValue, 'f');
                else if (primitiveType == typeof(double))
                    DecompileFloatingPointLiteral(decompilation, primitiveValue, 'd');
                else if (primitiveType == typeof(decimal))
                    DecompileFloatingPointLiteral(decompilation, primitiveValue, 'm');
                else
                    decompilation.Append(primitiveValue.ToString());
            }
        }

        private static void DecompileFloatingPointLiteral(StringBuilder decompilation, object value, char suffix)
        {
            // Make sure decimal point isn't converted to a comma in European locales.
            string svalue = Convert.ToString(value, CultureInfo.InvariantCulture);
            decompilation.Append(svalue);

            if (suffix == 'd')
            {
                // Don't append 'd' suffixes, they're ugly.  Only if the string-ified value contains
                // no decimal and no exponent do we need to append a ".0" to make it a double (as
                // opposed to an integer).

                bool hasDecimal = svalue.IndexOf('.') >= 0;
                bool hasExponent = svalue.IndexOfAny(new char[] { 'e', 'E' }) >= 0;

                if (!hasDecimal && !hasExponent)
                    decompilation.Append(".0");
            }
            else
            {
                decompilation.Append(suffix);
            }
        }

        private static void DecompileSuffixedIntegerLiteral(StringBuilder decompilation, object value, string suffix)
        {
            decompilation.Append(value.ToString());
            decompilation.Append(suffix);
        }

        private static void DecompileStringLiteral(StringBuilder decompilation, string strValue)
        {
            decompilation.Append("\"");
            for (int i = 0; i < strValue.Length; ++i)
            {
                char c = strValue[i];

                // is this character a surrogate pair?
                if ((char.IsHighSurrogate(c)) && (i + 1 < strValue.Length) && (char.IsLowSurrogate(strValue[i + 1])))
                {
                    // yes, so leave the two characters unchanged
                    decompilation.Append(c);
                    ++i;
                    decompilation.Append(strValue[i]);
                }
                else
                    AppendCharacter(decompilation, c, '"');
            }
            decompilation.Append("\"");
        }

        private static void DecompileCharacterLiteral(StringBuilder decompilation, char charValue)
        {
            decompilation.Append("'");
            AppendCharacter(decompilation, charValue, '\'');
            decompilation.Append("'");
        }

        private static void AppendCharacter(StringBuilder decompilation, char charValue, char quoteCharacter)
        {
            if (charValue == quoteCharacter)
            {
                decompilation.Append("\\");
                decompilation.Append(quoteCharacter);
            }
            else if (charValue == '\\')
            {
                decompilation.Append("\\\\");
            }
            else if ((charValue >= ' ' && charValue < '\u007f') || char.IsLetterOrDigit(charValue) || char.IsPunctuation(charValue))
            {
                decompilation.Append(charValue);
            }
            else
            {
                string escapeSequence = null;
                switch (charValue)
                {
                    case '\0':
                        escapeSequence = "\\0";
                        break;
                    case '\n':
                        escapeSequence = "\\n";
                        break;
                    case '\r':
                        escapeSequence = "\\r";
                        break;
                    case '\b':
                        escapeSequence = "\\b";
                        break;
                    case '\a':
                        escapeSequence = "\\a";
                        break;
                    case '\t':
                        escapeSequence = "\\t";
                        break;
                    case '\f':
                        escapeSequence = "\\f";
                        break;
                    case '\v':
                        escapeSequence = "\\v";
                        break;
                }

                if (escapeSequence != null)
                {
                    decompilation.Append(escapeSequence);
                }
                else
                {
                    decompilation.Append("\\u");

                    UInt16 cv = (UInt16)charValue;
                    for (int i = 12; i >= 0; i -= 4)
                    {
                        int mask = 0xF << i;
                        byte c = (byte)((cv & mask) >> i);
                        decompilation.Append("0123456789ABCDEF"[c]);
                    }
                }
            }
        }

        #endregion

        #region Type decompilation

        public static string DecompileType(Type type)
        {
            if (type == null)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            DecompileType_Helper(sb, type);
            return sb.ToString();
        }

        private static void DecompileType_Helper(StringBuilder decompilation, Type type)
        {
            int i;

            if (type.HasElementType)
            {
                DecompileType_Helper(decompilation, type.GetElementType());

                if (type.IsArray)
                {
                    decompilation.Append("[");
                    decompilation.Append(',', type.GetArrayRank() - 1);
                    decompilation.Append("]");
                }
                else if (type.IsByRef)
                {
                    decompilation.Append('&');
                }
                else if (type.IsPointer)
                {
                    decompilation.Append('*');
                }
            }
            else
            {
                string typeName = type.FullName;
                if (typeName == null) // Full name may be null for an unbound generic.
                    typeName = type.Name;

                typeName = UnmangleTypeName(typeName);
                decompilation.Append(typeName);

                if (type.IsGenericType)
                {
                    decompilation.Append("<");

                    Type[] typeArgs = type.GetGenericArguments();

                    DecompileType_Helper(decompilation, typeArgs[0]); // decompile the first type arg
                    for (i = 1; i < typeArgs.Length; ++i)
                    {
                        decompilation.Append(", ");
                        DecompileType_Helper(decompilation, typeArgs[i]);
                    }

                    decompilation.Append(">");
                }
            }
        }

        internal static void DecompileType(StringBuilder decompilation, CodeTypeReference typeRef)
        {
            // Remove any back-tick decorations on generic types, if present.
            string baseType = UnmangleTypeName(typeRef.BaseType);
            decompilation.Append(baseType);

            if (typeRef.TypeArguments != null && typeRef.TypeArguments.Count > 0)
            {
                decompilation.Append("<");

                bool first = true;
                foreach (CodeTypeReference argTypeRef in typeRef.TypeArguments)
                {
                    if (!first)
                        decompilation.Append(", ");
                    first = false;

                    DecompileType(decompilation, argTypeRef);
                }

                decompilation.Append(">");
            }

            if (typeRef.ArrayRank > 0)
            {
                do
                {
                    decompilation.Append("[");
                    for (int i = 1; i < typeRef.ArrayRank; ++i)
                        decompilation.Append(",");
                    decompilation.Append("]");

                    typeRef = typeRef.ArrayElementType;
                } while (typeRef.ArrayRank > 0);
            }
        }

        private static Dictionary<string, string> knownTypeMap = InitializeKnownTypeMap();

        private static Dictionary<string, string> InitializeKnownTypeMap()
        {
            Dictionary<string, string> map = new Dictionary<string, string>
            {
                { "System.Char", "char" },
                { "System.Byte", "byte" },
                { "System.SByte", "sbyte" },
                { "System.Int16", "short" },
                { "System.UInt16", "ushort" },
                { "System.Int32", "int" },
                { "System.UInt32", "uint" },
                { "System.Int64", "long" },
                { "System.UInt64", "ulong" },
                { "System.Single", "float" },
                { "System.Double", "double" },
                { "System.Decimal", "decimal" },
                { "System.Boolean", "bool" },
                { "System.String", "string" },
                { "System.Object", "object" },
                { "System.Void", "void" }
            };
            return map;
        }

        private static string TryReplaceKnownTypes(string typeName)
        {
            if (!knownTypeMap.TryGetValue(typeName, out string newTypeName))
                newTypeName = typeName;
            return newTypeName;
        }

        private static string UnmangleTypeName(string typeName)
        {
            int tickIndex = typeName.IndexOf('`');
            if (tickIndex > 0)
                typeName = typeName.Substring(0, tickIndex);

            // Replace the '+' for a nested type with a '.'
            typeName = typeName.Replace('+', '.');

            typeName = TryReplaceKnownTypes(typeName);

            return typeName;
        }

        #endregion

        #region Method decompilation

        internal static string DecompileMethod(MethodInfo method)
        {
            if (method == null)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            DecompileType_Helper(sb, method.DeclaringType);
            sb.Append('.');
            if (knownOperatorMap.TryGetValue(method.Name, out string operatorName))
                sb.Append(operatorName);
            else
                sb.Append(method.Name);
            sb.Append('(');
            ParameterInfo[] parms = method.GetParameters();
            for (int i = 0; i < parms.Length; ++i)
            {
                DecompileType_Helper(sb, parms[i].ParameterType);
                if (i != parms.Length - 1)
                    sb.Append(", ");
            }
            sb.Append(')');
            return sb.ToString();
        }

        private static Dictionary<string, string> knownOperatorMap = InitializeKnownOperatorMap();

        private static Dictionary<string, string> InitializeKnownOperatorMap()
        {
            Dictionary<string, string> map = new Dictionary<string, string>(27)
            {

                // unary operators
                { "op_UnaryPlus", "operator +" },
                { "op_UnaryNegation", "operator -" },
                { "op_OnesComplement", "operator ~" },
                { "op_LogicalNot", "operator !" },
                { "op_Increment", "operator ++" },
                { "op_Decrement", "operator --" },
                { "op_True", "operator true" },
                { "op_False", "operator false" },
                { "op_Implicit", "implicit operator" },
                { "op_Explicit", "explicit operator" },

                // binary operators
                { "op_Equality", "operator ==" },
                { "op_Inequality", "operator !=" },
                { "op_GreaterThan", "operator >" },
                { "op_GreaterThanOrEqual", "operator >=" },
                { "op_LessThan", "operator <" },
                { "op_LessThanOrEqual", "operator <=" },
                { "op_Addition", "operator +" },
                { "op_Subtraction", "operator -" },
                { "op_Multiply", "operator *" },
                { "op_Division", "operator /" },
                { "op_IntegerDivision", "operator \\" },
                { "op_Modulus", "operator %" },
                { "op_LeftShift", "operator <<" },
                { "op_RightShift", "operator >>" },
                { "op_BitwiseAnd", "operator &" },
                { "op_BitwiseOr", "operator |" },
                { "op_ExclusiveOr", "operator ^" }
            };
            return map;
        }
        #endregion

        #region Operator Precedence

        // These operations are sorted in order of precedence, lowest-to-highest
        private enum Operation
        {
            RootExpression,
            LogicalOr,          // ||
            LogicalAnd,         // &&
            BitwiseOr,          // |
            BitwiseAnd,         // &
            Equality,           // ==  !=
            Comparitive,        // <  <=  >  >=
            Additive,           // +  -
            Multiplicative,     // *  /  %
            Unary,              // -  !  (cast)
            Postfix,            // field/property ref and method call
            NoParentheses       // Highest
        }

        private delegate Operation ComputePrecedence(CodeExpression expresssion);

        private static Dictionary<Type, ComputePrecedence> precedenceMap = InitializePrecedenceMap();


        private static Dictionary<Type, ComputePrecedence> InitializePrecedenceMap()
        {
            Dictionary<Type, ComputePrecedence> map = new Dictionary<Type, ComputePrecedence>(7)
            {
                { typeof(CodeBinaryOperatorExpression), GetBinaryPrecedence },
                { typeof(CodeCastExpression), GetCastPrecedence },
                { typeof(CodeFieldReferenceExpression), GetPostfixPrecedence },
                { typeof(CodePropertyReferenceExpression), GetPostfixPrecedence },
                { typeof(CodeMethodInvokeExpression), GetPostfixPrecedence },
                { typeof(CodeObjectCreateExpression), GetPostfixPrecedence },
                { typeof(CodeArrayCreateExpression), GetPostfixPrecedence }
            };
            return map;
        }

        private static Operation GetPostfixPrecedence(CodeExpression expression)
        {
            return Operation.Postfix;
        }

        private static Operation GetCastPrecedence(CodeExpression expression)
        {
            return Operation.Unary;
        }

        private static Operation GetBinaryPrecedence(CodeExpression expression)
        {
            CodeBinaryOperatorExpression binaryExpr = (CodeBinaryOperatorExpression)expression;

            Operation operation = Operation.NoParentheses;
            switch (binaryExpr.Operator)
            {
                case CodeBinaryOperatorType.Multiply:
                case CodeBinaryOperatorType.Divide:
                case CodeBinaryOperatorType.Modulus:
                    operation = Operation.Multiplicative;
                    break;

                case CodeBinaryOperatorType.Subtract:
                case CodeBinaryOperatorType.Add:
                    operation = Operation.Additive;
                    break;

                case CodeBinaryOperatorType.LessThan:
                case CodeBinaryOperatorType.LessThanOrEqual:
                case CodeBinaryOperatorType.GreaterThan:
                case CodeBinaryOperatorType.GreaterThanOrEqual:
                    operation = Operation.Comparitive;
                    break;

                case CodeBinaryOperatorType.IdentityEquality:
                case CodeBinaryOperatorType.ValueEquality:
                case CodeBinaryOperatorType.IdentityInequality:
                    operation = Operation.Equality;
                    break;

                case CodeBinaryOperatorType.BitwiseAnd:
                    operation = Operation.BitwiseAnd;
                    break;

                case CodeBinaryOperatorType.BitwiseOr:
                    operation = Operation.BitwiseOr;
                    break;

                case CodeBinaryOperatorType.BooleanAnd:
                    operation = Operation.LogicalAnd;
                    break;

                case CodeBinaryOperatorType.BooleanOr:
                    operation = Operation.LogicalOr;
                    break;

                default:
                    string message = string.Format(CultureInfo.CurrentCulture, Messages.BinaryOpNotSupported, binaryExpr.Operator.ToString());
                    NotSupportedException exception = new NotSupportedException(message);
                    exception.Data[RuleUserDataKeys.ErrorObject] = binaryExpr;
                    throw exception;
            }

            return operation;
        }

        private static Operation GetPrecedence(CodeExpression expression)
        {
            // Assume the operation needs no parentheses.
            Operation operation = Operation.NoParentheses;

            if (precedenceMap.TryGetValue(expression.GetType(), out ComputePrecedence computePrecedence))
                operation = computePrecedence(expression);

            return operation;
        }

        internal static bool MustParenthesize(CodeExpression childExpr, CodeExpression parentExpr)
        {
            // No parent... we're at the root, so no need to parenthesize the root.
            if (parentExpr == null)
                return false;

            Operation childOperation = GetPrecedence(childExpr);
            Operation parentOperation = GetPrecedence(parentExpr);

            if (parentOperation == childOperation)
            {
                if (parentExpr is CodeBinaryOperatorExpression parentBinary)
                {
                    if (childExpr == parentBinary.Right)
                    {
                        // Something like 2 - (3 - 4) needs parentheses.
                        return true;
                    }
                    else
                    {
                        // Something like (2 - 3) - 4 doesn't need parentheses.
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else if (parentOperation > childOperation)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
